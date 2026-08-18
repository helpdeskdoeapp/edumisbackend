using edumis.Common;
using edumis.DataAccess.IRepositories;
using edumis.Models;
using edumis.Models.Communication;
using edumis.Models.Users;
using edumis.Models.Users.DTO;
using edumisbackend.Common;
using edumisbackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace edumisbackend.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IUnitOfWork unitOfWork, IConfiguration configuration,      
    TokenHelper tokenHelper, IHostEnvironment environment
    ) : ControllerBase //IHostEnvironment environment,IHttpClientFactory _httpClientFactory
{   
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginUser([FromBody] LoginDTO UserRequest)
    {
        //if (string.IsNullOrWhiteSpace(UserRequest.CaptchaToken))
        //    return BadRequest(new ResponseModel()
        //    {
        //        Success = false,
        //        Message = "Captcha is required",
        //        ReturnCode = StatusCodes.Status400BadRequest.ToString()
        //    });

        //var secret = configuration["GoogleReCaptcha:SecretKey"];
        //var verifyUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={UserRequest.CaptchaToken}";

        //var client = _httpClientFactory.CreateClient();
        //var response = await client.PostAsync(verifyUrl, null);
        //var content = await response.Content.ReadAsStringAsync();
        //var captchaResult = JsonSerializer.Deserialize<ReCaptchaResponseDTO>(content);

        //if (captchaResult == null || !captchaResult.Success)
        //    return BadRequest(new ResponseModel()
        //    {
        //        Success = false,
        //        Message = "Captcha verification failed!",
        //        ReturnCode = StatusCodes.Status400BadRequest.ToString()
        //    });
       
        var UserDetails = await unitOfWork.Users.GetFirstOrDefault(x =>
                            (x.UniqueId == UserRequest.UserName ||
                            (x.EmailId != null && x.EmailId.ToLower() == UserRequest.UserName.ToLower()))
                            && x.IsValid == true);
        
        if (UserDetails != null)
        {
            if (Utilities.VerifyPassword(UserRequest.Password, UserDetails.Password))
            {
                //var UserData = await unitOfWork.Users.GetUserDetails(UserDetails.UserId);

                var FinalAccessToken = tokenHelper.CreateTempToken(UserDetails.UserId.ToString());

                //var RefreshToken = tokenHelper.GenerateRefreshToken();
                //RefreshTokenModel refreshTokenSaveObj = new RefreshTokenModel()
                //{
                //    RowId = Guid.NewGuid(),
                //    UserId = UserDetails.UserId,
                //    Token = RefreshToken,
                //    ExpiresOnUTC = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JWTAuth:RefreshTokenExpirationDays")),
                //    CreatedBy = "Auto",
                //    ModifiedBy = "Auto"
                //};
                //await unitOfWork.RefreshTokenRepo.Add(refreshTokenSaveObj);

                await unitOfWork.UserActivityLogs.Add(new UserActivityLogsModel()
                {
                    UserId = UserDetails.UserId,
                    Activity = "LOGIN SUCCESS",
                    ActivityDateTime = DateTime.UtcNow,
                    IPAddress = UserRequest.IPAddress ?? string.Empty,
                    UserAgent = UserRequest.UserAgent
                });
                await unitOfWork.Save();

                return Ok(ResponseModel<object>.Success(new
                {
                    //Success = true,
                    //Message = "Authorized",
                    Token = FinalAccessToken,
                    //RefreshToken = RefreshToken,
                    // UserId = UserDetails.UserId.ToString(),
                    IsActive = UserDetails.IsValid,
                    IsAccountLocked = UserDetails.IsAccountLocked != null ? UserDetails.IsAccountLocked : false,
                    IsLoggedIn = UserDetails.IsLoggedIn != null ? UserDetails.IsLoggedIn : false
                }, "Login success."));  
            }
            else
            {
                await unitOfWork.UserActivityLogs.Add(new UserActivityLogsModel()
                {
                    UserId = UserDetails.UserId,
                    Activity = "LOGIN FAILED",
                    ActivityDateTime = DateTime.UtcNow,
                    IPAddress = UserRequest.IPAddress ?? string.Empty,
                    UserAgent = UserRequest.UserAgent
                });
                await unitOfWork.Save();
                return Ok(ResponseModel<string>.Failure("Invalid user credentials!", StatusCodes.Status401Unauthorized));               
            }
        }
        else
        {
            await unitOfWork.UserActivityLogs.Add(new UserActivityLogsModel()
            {
                UserId = new Guid(),
                SecondaryId = UserRequest.UserName,
                Activity = "LOGIN FAILED",
                ActivityDateTime = DateTime.UtcNow,
                IPAddress = UserRequest.IPAddress ?? string.Empty,
                UserAgent = UserRequest.UserAgent
            });
            await unitOfWork.Save();
            return Ok(ResponseModel<string>.Failure("Invalid user credentials!", StatusCodes.Status401Unauthorized));
        }
    }
        
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAccessToken()
    {
        var RefreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(RefreshToken))
            return Ok(ResponseModel<string>.Failure("Invalid Token!", StatusCodes.Status401Unauthorized));
       
        var refreshToken = await unitOfWork.RefreshTokenRepo.GetUserTokenDetails(Utilities.ComputeSha256Hash(RefreshToken));
        if (refreshToken is null || refreshToken.ExpiresOnUTC < DateTime.UtcNow)
            return Ok(ResponseModel<string>.Failure("Token Expired!", StatusCodes.Status401Unauthorized));
        
        var UserData = await unitOfWork.Users.GetUserDetails(refreshToken.UserId);
        if (UserData == null)
            return Ok(ResponseModel<string>.Failure("No user details found!"));
       
        var FinalAccessToken = tokenHelper.CreateToken(UserData);
        var NewRefreshToken = tokenHelper.GenerateRefreshToken();
        var CsrfToken = tokenHelper.GenerateCsrfToken();
        var NewRefreshTokenHash = Utilities.ComputeSha256Hash(NewRefreshToken);

        RefreshTokenModel refreshTokenSaveObj = new RefreshTokenModel()
        {
            RowId = Guid.NewGuid(),
            UserId = refreshToken.UserId,
            Token = NewRefreshTokenHash,
            ExpiresOnUTC = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JWTAuth:RefreshTokenExpirationDays")),
            CreatedBy = "Auto",
            ModifiedBy = "Auto"
        };
        await unitOfWork.RefreshTokenRepo.Add(refreshTokenSaveObj);
        await unitOfWork.Save();

        await unitOfWork.RefreshTokenRepo.RemoveUnusedTokensExcept(refreshToken.UserId, NewRefreshTokenHash);

        // set refreshToken as HttpOnly cookie
        Response.Cookies.Append("refreshToken", NewRefreshToken, new CookieOptions
        {
            HttpOnly = true,       // not accessible from JS
            Secure = true,         // only over HTTPS
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JWTAuth:RefreshTokenExpirationDays")),
           // Domain = configuration["CookieDomain"] //".delhi.gov.in"
        });

        // set csrfToken as HttpOnly cookie
        Response.Cookies.Append("csrfToken", CsrfToken, new CookieOptions
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
            //RefreshToken = NewRefreshToken,
            UserId = UserData.UserId.ToString(),
            IsActive = UserData.IsValid,
            IsAccountLocked = UserData?.IsAccountLocked ?? false, // != null ? UserData.IsAccountLocked : false,
            IsLoggedIn = UserData?.IsLoggedIn ?? false // != null ? UserData.IsLoggedIn : false
        }, "Success"));       
    }

    [HttpGet("accesstoken")]
    [Authorize]
    public async Task<IActionResult> GetAccessToken()
    {
        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? Utilities.DecryptString(TokenParam) : string.Empty;

        var userData = await unitOfWork.Users.GetFirstOrDefault(x => x.UserId == new Guid(UserId));
        if (userData == null)
            return Ok(ResponseModel<string>.Failure("Invalid user credentials!"));
       
        UserDTO? UserDetails = null;
        
        switch(userData.UserType)
        {
            case 201:
                UserDetails = await unitOfWork.Users.GetMasterAdminUserDetails(new Guid(UserId));
                break;
            case 202:
            case 203:
            case 204:
                UserDetails = await unitOfWork.Users.GetBranchAdminUserDetails(new Guid(UserId));
                break;
            case 205:
            case 206:
                UserDetails = await unitOfWork.Users.GetBranchUserDetails(new Guid(UserId));
                break;
        }
        
        if(UserDetails == null)
            return Ok(ResponseModel<string>.Failure("Invalid user credentials!"));

        var FinalAccessToken = tokenHelper.CreateToken(UserDetails);
        var RefreshToken = tokenHelper.GenerateRefreshToken();
        var RefreshTokenHash = Utilities.ComputeSha256Hash(RefreshToken);
        var CsrfToken = tokenHelper.GenerateCsrfToken();

        RefreshTokenModel refreshTokenSaveObj = new RefreshTokenModel()
        {
            RowId = Guid.NewGuid(),
            UserId = UserDetails.UserId,
            Token = RefreshTokenHash,
            ExpiresOnUTC = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JWTAuth:RefreshTokenExpirationDays")),
            CreatedBy = "Auto",
            ModifiedBy = "Auto"
        };
        await unitOfWork.RefreshTokenRepo.Add(refreshTokenSaveObj);       
        await unitOfWork.Save();

        // set refreshToken as HttpOnly cookie
        Response.Cookies.Append("refreshToken", RefreshToken, new CookieOptions
        {
            HttpOnly = true,       // not accessible from JS
            Secure = true,         // only over HTTPS
            SameSite = SameSiteMode.None, //For cross site access in case API and frontend are on different domains
            Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JWTAuth:RefreshTokenExpirationDays")),
            //Domain = configuration["CookieDomain"] //".delhi.gov.in"
        });

        // set csrfToken as HttpOnly cookie
        Response.Cookies.Append("csrfToken", CsrfToken, new CookieOptions
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
            IsAccountLocked = UserDetails.IsAccountLocked != null ? UserDetails.IsAccountLocked : false,
            IsLoggedIn = UserDetails.IsLoggedIn != null ? UserDetails.IsLoggedIn : false
        }, "Login Success"));       
    }

    [HttpPost("revoke-token")]
    [Authorize]
    public async Task<IActionResult> RevokeToken()//[FromBody] RevokeTokenRequest request
    {
        var RefreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(RefreshToken))
            return Ok(ResponseModel<string>.Failure("Invalid Token!", StatusCodes.Status401Unauthorized));
       
        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? Utilities.DecryptString(TokenParam) : string.Empty;

        var RefreshTokenHash = Utilities.ComputeSha256Hash(RefreshToken);

        var userToken = await unitOfWork.RefreshTokenRepo.GetFirstOrDefault(u => u.UserId == new Guid(UserId) && u.Token == RefreshTokenHash);

        if (userToken != null)
        {
            await unitOfWork.RefreshTokenRepo.Remove(userToken);
            await unitOfWork.Save();
        }

        Response.Cookies.Delete("refreshToken");
        Response.Cookies.Delete("csrfToken");

        return Ok(ResponseModel<string>.Success("Success", "You have successfully logged out."));
    }

    [HttpPost("checkuser/{userid}")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckUserExistance([FromRoute] string userid)
    {
        string[] AdminUsers = ["missa", "smcsa", "supersa"];
        if (string.IsNullOrEmpty(userid) || AdminUsers.Contains(userid))
            return Ok(ResponseModel<string>.Failure("Invalid user credentials!"));
       
        var UserData = await unitOfWork.Users.GetFirstOrDefault(x =>
                           (x.UniqueId == userid ||
                           (x.EmailId != null && x.EmailId.ToLower() == userid.ToLower()))
                           && x.IsValid == true);
        if (UserData == null)
            return Ok(ResponseModel<string>.Failure("Invalid user credentials!"));
      
        string? mobileNo = string.Empty;

        switch (UserData.UserType)
        {            
            case 202:
            case 203:
            case 204:
                var BranchAdminUserDetails = await unitOfWork.BranchRepo.GetFirstOrDefault(x=>x.BranchId == UserData.UniqueId);
                if (BranchAdminUserDetails != null)
                    mobileNo = BranchAdminUserDetails.ContactNo;                
                break;
            case 205:
            case 206:
                var BranchUserDetails = await unitOfWork.EmployeesRepo.GetFirstOrDefault(x => x.EmployeeId == UserData.UniqueId);
                if (BranchUserDetails != null)
                    mobileNo = BranchUserDetails.MobileNo;
                break;
        }

        if (!string.IsNullOrEmpty(mobileNo))
        {
            var SMSSettings = await unitOfWork.SMSSettingsRepo.GetFirstOrDefault(x => x.IsValid);
            if (SMSSettings == null)
                return Ok(ResponseModel<string>.NoData("SMS Settings Not Found!"));
          
            var smsTemplate = await unitOfWork.SMSTemplatesRepo.GetFirstOrDefault(x => x.SMSType == (int)SMS_PURPOSE.OTP_SMS && x.IsValid == true);
            if (smsTemplate == null)
                return Ok(ResponseModel<string>.NoData("SMS Template Not Found!"));
           
            Random rnd = new Random();
            long OTPText = rnd.Next(100001, 999999);
            string Message = "OTP : " + OTPText.ToString() + " --Directorate of Education";
            var ReturnSMSStatus = edumis.Common.SMS.sendOTPMSG(SMSSettings.UserID, SMSSettings.Password, SMSSettings.SenderId, mobileNo, Message, SMSSettings.SecureKey, smsTemplate.TemplateId, SMSSettings.SMSURL);

            if (!ReturnSMSStatus.Contains("402"))//402,MsgID = 070920211631027377372edudel-Am (Success Message returned)
                return Ok(ResponseModel<string>.Failure("Failed To Send SMS!", 400));           
            else
            {
                OTPSentModel modelToSave = new OTPSentModel()
                {
                    SentTo = UserData.UserId.ToString(),
                    Purpose = "RESET_PASSWORD",
                    OTP = OTPText.ToString(),
                    ValidUpTo = DateTime.Now.AddMinutes(Convert.ToInt32(configuration["OTPExpiryTime"].ToString()))
                };
                await unitOfWork.OTPSentRepo.Add(modelToSave);
                await unitOfWork.Save();

                return Ok(ResponseModel<string>.Success("Success", $"OTP sent to the mobile number xxxxxxx{mobileNo.Substring(7)}. OTP is Valid for {configuration["OTPExpiryTime"].ToString()} minutes only."));               
            }
        }
        return Ok(ResponseModel<string>.Failure($"Mobile no. not registered. Kindly contact system administrator.", 400));       
    }

    [HttpGet("userdetails")]
    [Authorize]
    public async Task<IActionResult> GetUserDetails()
    {
        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        UserDTO? UserDetails = null;
        var userData = await unitOfWork.Users.GetFirstOrDefault(x => x.UserId == new Guid(UserId));
        switch (userData.UserType)
        {
            case 201:
                UserDetails = await unitOfWork.Users.GetMasterAdminUserDetails(new Guid(UserId));
                break;
            case 202:
            case 203:
            case 204:
                UserDetails = await unitOfWork.Users.GetBranchAdminUserDetails(new Guid(UserId));
                break;
            case 205:
            case 206:
                UserDetails = await unitOfWork.Users.GetBranchUserDetails(new Guid(UserId));
                break;
        }

        if (UserDetails == null)
            return Ok(ResponseModel<string>.Failure("Unauthorized user!", StatusCodes.Status401Unauthorized));

        return Ok(ResponseModel<UserDTO>.Success(UserDetails, "User details retrieved."));        
    }    
}
