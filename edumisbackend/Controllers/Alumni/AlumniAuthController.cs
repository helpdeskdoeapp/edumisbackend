using edumis.Common;
using edumis.DataAccess.IRepositories;
using edumis.Models.Alumni.Members.DTO;
using edumis.Models.Alumni.UserAccounts;
using edumis.Models.Alumni.UserAccounts.DTO;
using edumis.Models.Users;
using edumisbackend.Common;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Alumni;

[Route("api/[controller]")]
[ApiController]
public class AlumniAuthController(IUnitOfWork unitOfWork, IConfiguration configuration, TokenHelper tokenHelper) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AlumniLoginRequestDTO requestDTO)
    {
        var UserDetails = await unitOfWork.AlumniLoginRepo.GetFirstOrDefault(x =>
                            x.EmailID.ToLower() == requestDTO.UserName.ToLower() &&
                            x.IsValid == true);

        if (UserDetails != null)
        {
            if (Utilities.VerifyPassword(requestDTO.Password, UserDetails.Password))
            {
                var FinalAccessToken = tokenHelper.CreateTempToken(UserDetails.AlumniID.ToString());
                
                await unitOfWork.UserActivityLogs.Add(new UserActivityLogsModel()
                {
                    UserId = UserDetails.AlumniID,
                    Activity = "ALUMNI LOGIN SUCCESS",
                    ActivityDateTime = DateTime.UtcNow,
                    IPAddress = requestDTO.IPAddress ?? string.Empty,
                    UserAgent = requestDTO.UserAgent
                });
                await unitOfWork.Save();
                return Ok(ResponseModel<object>.Success(new
                {
                    //Success = true,
                    //Message = "Authorized",
                    Token = FinalAccessToken,                   
                    IsActive = UserDetails.IsValid,
                    IsAccountLocked = UserDetails?.IsAccountLocked ?? false,// != null ? UserDetails.IsAccountLocked : false,
                    IsLoggedIn = UserDetails?.IsLoggedIn ?? false// != null ? UserDetails.IsLoggedIn : false
                }, "Login successful!", StatusCodes.Status200OK));               
            }
            else
            {
                await unitOfWork.UserActivityLogs.Add(new UserActivityLogsModel()
                {
                    UserId = UserDetails.AlumniID,
                    Activity = "LOGIN FAILED",
                    ActivityDateTime = DateTime.UtcNow,
                    IPAddress = requestDTO.IPAddress ?? string.Empty,
                    UserAgent = requestDTO.UserAgent
                });
                await unitOfWork.Save();
                return Ok(ResponseModel<string>.Failure("Invalid User Id or Password.", StatusCodes.Status401Unauthorized));              
            }
        }
        else
        {
            await unitOfWork.UserActivityLogs.Add(new UserActivityLogsModel()
            {
                UserId = new Guid(),
                SecondaryId = requestDTO.UserName,
                Activity = "ALUMNI LOGIN FAILED",
                ActivityDateTime = DateTime.UtcNow,
                IPAddress = requestDTO.IPAddress ?? string.Empty,
                UserAgent = requestDTO.UserAgent
            });
            await unitOfWork.Save();
            return Ok(ResponseModel<string>.Failure("Invalid User Id or Password.", StatusCodes.Status401Unauthorized));
        }
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAccessToken() //[FromBody] string RefreshToken
    {
        var RefreshToken = Request.Cookies["almrefreshToken"];
        if (string.IsNullOrEmpty(RefreshToken))
            return Ok(ResponseModel<string>.Failure("Token Expired!", StatusCodes.Status401Unauthorized));

        var refreshToken = await unitOfWork.AlumniRefreshTokensRepo.GetUserTokenDetails(Utilities.ComputeSha256Hash(RefreshToken));
        if (refreshToken is null || refreshToken.ExpiresOnUTC < DateTime.UtcNow)
            return Ok(ResponseModel<string>.Failure("Token Expired!", StatusCodes.Status401Unauthorized));
      
        var UserData = await unitOfWork.AlumniLoginRepo.GetUserDetails(refreshToken.UserId);
        if (UserData == null)
            return Ok(ResponseModel<string>.NoData("Invalid User Details!"));       
       
        var FinalAccessToken = tokenHelper.CreateToken(UserData);
        var NewRefreshToken = tokenHelper.GenerateRefreshToken();
        var CsrfToken = tokenHelper.GenerateCsrfToken();
        var NewRefreshTokenHash = Utilities.ComputeSha256Hash(NewRefreshToken);

        AlumniRefreshTokenModel refreshTokenSaveObj = new AlumniRefreshTokenModel()
        {
            RowId = Guid.NewGuid(),
            UserId = refreshToken.UserId,
            Token = NewRefreshTokenHash,
            ExpiresOnUTC = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JWTAuth:RefreshTokenExpirationDays")),
            CreatedBy = "Auto",
            ModifiedBy = "Auto"
        };
        await unitOfWork.AlumniRefreshTokensRepo.Add(refreshTokenSaveObj);
        await unitOfWork.Save();

        await unitOfWork.AlumniRefreshTokensRepo.RemoveUnusedTokensExcept(refreshToken.UserId, NewRefreshTokenHash);

        // set refreshToken as HttpOnly cookie
        Response.Cookies.Append("almrefreshToken", NewRefreshToken, new CookieOptions
        {
            HttpOnly = true,       // not accessible from JS
            Secure = true,         // only over HTTPS
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JWTAuth:RefreshTokenExpirationDays"))
        });

        // set csrfToken as HttpOnly cookie
        Response.Cookies.Append("almCsrfToken", CsrfToken, new CookieOptions
        {
            HttpOnly = false,       // accessible from JS
            Secure = true,         // only over HTTPS
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JWTAuth:RefreshTokenExpirationDays"))
        });

        return Ok(ResponseModel<object>.Success(new
        {
            //Success = true,
            //Message = "Authorized",
            Token = FinalAccessToken,
            //RefreshToken = NewRefreshToken,
            //UserId = UserData.UserId.ToString(),
            IsActive = UserData.IsValid,
            IsAccountLocked = UserData?.IsAccountLocked ?? false, // != null ? UserData.IsAccountLocked : false,
            IsLoggedIn = UserData?.IsLoggedIn ?? false, //!= null ? UserData.IsLoggedIn : false
        }, "Success", StatusCodes.Status200OK));
    }

    [HttpGet("accesstoken")]
    [Authorize]
    public async Task<IActionResult> GetAccessToken()
    {
        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? Utilities.DecryptString(TokenParam) : string.Empty;

        var userData = await unitOfWork.AlumniLoginRepo.GetFirstOrDefault(x => x.AlumniID == new Guid(UserId));
        if (userData == null)
            return Ok(ResponseModel<string>.Failure("Invalid user credentials"));

        var UserDetails = await unitOfWork.AlumniLoginRepo.GetUserDetails(userData.AlumniID);
        if (UserDetails == null)
            return Ok(ResponseModel<string>.Failure("Invalid user credentials"));
       
        var FinalAccessToken = tokenHelper.CreateToken(UserDetails);
        var RefreshToken = tokenHelper.GenerateRefreshToken();
        var RefreshTokenHash = Utilities.ComputeSha256Hash(RefreshToken);
        var CsrfToken = tokenHelper.GenerateCsrfToken();

        AlumniRefreshTokenModel refreshTokenSaveObj = new AlumniRefreshTokenModel()
        {
            RowId = Guid.NewGuid(),
            UserId = UserDetails.UserId,
            Token = RefreshTokenHash,
            ExpiresOnUTC = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JWTAuth:RefreshTokenExpirationDays")),
            CreatedBy = "Auto",
            ModifiedBy = "Auto"
        };
        await unitOfWork.AlumniRefreshTokensRepo.Add(refreshTokenSaveObj);
        await unitOfWork.Save();

        // set refreshToken as HttpOnly cookie
        Response.Cookies.Append("almrefreshToken", RefreshToken, new CookieOptions
        {
            HttpOnly = true,       // not accessible from JS
            Secure = true,         // only over HTTPS
            SameSite = SameSiteMode.None, //For cross site access in case API and frontend are on different domains
            Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JWTAuth:RefreshTokenExpirationDays")),
            //Domain = configuration["CookieDomain"] //".delhi.gov.in"
        });

        // set csrfToken as HttpOnly cookie
        Response.Cookies.Append("almCsrfToken", CsrfToken, new CookieOptions
        {
            HttpOnly = false,       // accessible from JS
            Secure = true,//!environment.IsDevelopment(),         // only over HTTPS
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JWTAuth:RefreshTokenExpirationDays")),
            Domain = configuration["FrontendDomain"],
            Path = "/"
        });       

        return Ok(ResponseModel<object>.Success(new
        {
            //Success = true,
            //Message = "Authorized",
            Token = FinalAccessToken,
            //RefreshToken = RefreshToken,
            // UserId = UserDetails.UserId.ToString(),
            IsActive = UserDetails.IsValid,
            IsAccountLocked = UserDetails?.IsAccountLocked ?? false, //!= null ? UserDetails.IsAccountLocked : false,
            IsLoggedIn = UserDetails?.IsLoggedIn ?? false //!= null ? UserDetails.IsLoggedIn : false
        }, "Authorized", StatusCodes.Status200OK));
    }

    [HttpPost("revoke-token")]
    [Authorize]
    public async Task<IActionResult> RevokeToken()//[FromBody] RevokeTokenRequest request
    {
        var RefreshToken = Request.Cookies["almrefreshToken"];
        if (string.IsNullOrEmpty(RefreshToken))
            return Ok(ResponseModel<string>.Failure("Token Expired!", StatusCodes.Status401Unauthorized));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        var userToken = await unitOfWork.AlumniRefreshTokensRepo.GetFirstOrDefault(u => u.UserId == new Guid(UserId) && u.Token == Utilities.ComputeSha256Hash(RefreshToken));

        if (userToken != null)
        {
            await unitOfWork.AlumniRefreshTokensRepo.Remove(userToken);
            await unitOfWork.Save();
        }

        // delete refreshToken cookie
        Response.Cookies.Delete("almrefreshToken");
        Response.Cookies.Delete("almCsrfToken");

        return Ok(ResponseModel<string>.Success("Success", "Token Revoked!"));
    }

    [HttpPost("verify-alumni")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyAlumni([FromBody] string emailId)
    {
        if(string.IsNullOrEmpty(emailId))
            return Ok(ResponseModel<string>.Failure("Invalid Email Id!"));

        var UserData = await unitOfWork.AlumniLoginRepo.GetFirstOrDefault(x =>
                           x.EmailID.ToLower() == emailId.ToLower() &&
                           x.IsValid == true);

        if (UserData == null)
            return Ok(ResponseModel<string>.Failure("Email Id not registered!"));

        return Ok(ResponseModel<string>.Success(string.Empty));
    }

    [HttpPost("update-password")]
    [Authorize]
    public async Task<IActionResult> UpdatePassword([FromBody] AlumniPasswordUpdateRequestDTO requestDTO)
    {
        if (requestDTO == null)
            return Ok(ResponseModel<string>.Failure("Invalid Request!", 400));

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string AlumniId = TokenParam != null ? Utilities.DecryptString(TokenParam) : string.Empty;

        if (string.IsNullOrEmpty(AlumniId))
            return Ok(ResponseModel<string>.Failure("Not Authorized!", 401));

        var alumni = await unitOfWork.AlumniLoginRepo.GetFirstOrDefault(x => x.AlumniID == new Guid(AlumniId));
        if (alumni == null)
            return Ok(ResponseModel<string>.Failure("Failed to fetch alumni details!"));

        if (!Utilities.VerifyPassword(requestDTO.Password, requestDTO.OldPassword))        
            return Ok(ResponseModel<string>.Failure("Incorrect old password.", StatusCodes.Status400BadRequest));        

        if (Utilities.VerifyPassword(requestDTO.Password, requestDTO.Password))        
            return Ok(ResponseModel<string>.Failure("New password cannot be the same as the old password.", StatusCodes.Status400BadRequest));        

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string LoggedInUserId = userId != null ? edumis.Common.Utilities.DecryptString(userId) : string.Empty;

        alumni.SetPassword(Utilities.HashPassword(requestDTO.Password), LoggedInUserId);

        await unitOfWork.UserActivityLogs.Add(new UserActivityLogsModel()
        {
            UserId = new Guid(AlumniId),
            Activity = "PASSWORD CHANGED",
            ActivityDateTime = DateTime.UtcNow,
            IPAddress = requestDTO.IPAddress ?? string.Empty,
            UserAgent = requestDTO.UserAgent
        });
       
        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true,"Password updated successfully!"));
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] AlumniPasswordResetRequestDTO requestDTO)
    {       
        var alumni = await unitOfWork.AlumniLoginRepo.GetFirstOrDefault(x => x.EmailID.ToLower() == requestDTO.EmailId.ToLower());
        if (alumni == null)
            return Ok(ResponseModel<string>.NoData("Alumni not found!"));
              
        alumni.SetPassword(Utilities.HashPassword(requestDTO.Password), alumni.AlumniID.ToString());

        await unitOfWork.UserActivityLogs.Add(new UserActivityLogsModel()
        {
            UserId = alumni.AlumniID,
            Activity = "PASSWORD RESET",
            ActivityDateTime = DateTime.UtcNow,
            IPAddress = requestDTO.IPAddress ?? string.Empty,
            UserAgent = requestDTO.UserAgent
        });

        await unitOfWork.Save();

        return Ok(ResponseModel<bool>.Success(true, "Password reset successfully!"));
    }

    #region Get Details API
    [HttpGet("user-details")]
    [Authorize]
    public async Task<IActionResult> AlumniDetails()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string alumniUserId = userId != null ? Utilities.DecryptString(userId) : string.Empty;

        if (!Guid.TryParse(alumniUserId, out var alumniGuid))
            return Ok(ResponseModel<string>.Failure("Invalid Alumni Id", StatusCodes.Status400BadRequest));

        var returnData = await unitOfWork.AlumniDetailsRepo.GetDetails(alumniGuid);
        if (returnData == null)
            return Ok(ResponseModel<string>.NoData("Alumni details not found!", StatusCodes.Status404NotFound));

        return Ok(ResponseModel<AlumniDetailsDTO>.Success(returnData, "Alumni details fetched successfully", StatusCodes.Status200OK));
    }
    #endregion
}
