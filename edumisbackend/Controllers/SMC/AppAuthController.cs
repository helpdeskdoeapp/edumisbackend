using edumis.DataAccess.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using edumis.Common;
using edumis.Models.SMC;
using edumis.Models.SMC.DTO;
using edumisbackend.Common;
using edumisbackend.Helpers;

namespace edumisbackend.Controllers.SMC;

[Route("smc/[controller]")]
[ApiController]
public class AppAuthController(
    IUnitOfWork unitOfWork,
    IConfiguration configuration,
    IOtpService otpService,
    TokenHelper tokenHelper) : ControllerBase {
    #region SMC Member API

    [HttpPost("sendotp/{mobileno}")]
    [AllowAnonymous]
    public async Task<IActionResult> SendOTP([FromRoute] string mobileno) {
        if (mobileno.Length != 10)
            return Ok(ResponseModel<string>.Failure("Invalid Mobile", StatusCodes.Status406NotAcceptable));

        var currentSession = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid && x.IsCurrent);
        if (currentSession == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!",
                StatusCodes.Status204NoContent));

        var MemberDetails = await unitOfWork.SMCMemberRegistrationsRepo.GetFirstOrDefault(x =>
            x.MobileNo == mobileno &&
            x.ForSession == currentSession.ForSession &&
            x.IsActive == true);
        if (MemberDetails == null)
            return Ok(ResponseModel<string>.Unauthorized("Not Authorized!"));

        return Ok(await otpService.SendOtpAsync(mobileno, "VERIFY_MOBILE_SMC"));
    }

    [HttpPost("submit_mobile_verify_otp/{mobile}")]
    [AllowAnonymous]
    public async Task<IActionResult>
        SubmitMobileVerificationOtpText([FromRoute] string mobile, [FromBody] VerifyBranchOtpRequest request)
    {
        if (mobile.Length != 10)
            return Ok(ResponseModel<string>.Failure("Invalid Mobile Number!",
                StatusCodes.Status406NotAcceptable));

        if (request == null || string.IsNullOrEmpty(request.Otp))
            return BadRequest(ResponseModel<string>.Failure("Invalid Input!", StatusCodes.Status406NotAcceptable));

        var currentSession = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid && x.IsCurrent);
        if (currentSession == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!",
                StatusCodes.Status204NoContent));


        var validationResponse = await otpService.ValidateOtpAsync(mobile, request.Otp, "VERIFY_MOBILE_SMC");
        if (!validationResponse.IsSuccess)
            return Ok(validationResponse);

        var allProfiles = (await unitOfWork.SMCMemberRegistrationsRepo.GetAllMembers(mobile)).ToList();
        if (allProfiles.Count == 0)
            return Ok(ResponseModel<string>.Failure("Member Details Not Found.", StatusCodes.Status404NotFound));

        var activeProfiles = allProfiles.FindAll(x => x.IsActive && x.ForSession == currentSession.ForSession);
        if (activeProfiles.Count == 0)
            return Ok(ResponseModel<string>.Failure("No active membership exists for current session.",
                StatusCodes.Status404NotFound));

        var member = activeProfiles.First();
        var accessToken = GenerateAccessToken(member);
        await InjectRefreshToken(new Guid(member.MemberId ?? ""));

        return Ok(ResponseModel<object>.Success(new {
            member.MemberId,
            Token = accessToken,
            Profiles = activeProfiles
                .Select(p => new { p.MemberId, p.Name, p.BranchId, p.BranchName, p.MemberTypeDesc, p.MemberType }).ToList(),
        }));
    }

    [HttpPost("gettoken_for_member/{memberid}")]
    [Authorize]
    public async Task<IActionResult> GetTokenForMember([FromRoute] string memberid) {
        if (string.IsNullOrEmpty(memberid))
            return Ok(ResponseModel<string>.Failure("Invalid Member Id."));

        var currentSession = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid && x.IsCurrent);
        if (currentSession == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!",
                StatusCodes.Status204NoContent));

        var tokenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var authMemberId = tokenId != null ? Utilities.DecryptString(tokenId) : string.Empty;
        await unitOfWork.SmcRefreshTokenRepo.RemoveUnusedTokensExcept(new Guid(authMemberId), null);

        var authMemberDetails = await unitOfWork.SMCMemberRegistrationsRepo.GetMemberDetails(authMemberId);
        if (authMemberDetails is not { IsActive: true })
            return Ok(ResponseModel<string>.Unauthorized());

        var authMobile = authMemberDetails.MobileNo;

        var member = await unitOfWork.SMCMemberRegistrationsRepo.GetMemberDetails(memberid);
        if (member == null || member.MobileNo != authMobile || member.ForSession != currentSession.ForSession ||
            !member.IsActive)
            return Ok(ResponseModel<string>.Failure("Member Details Not Found!", StatusCodes.Status404NotFound));

        var accessToken = GenerateAccessToken(member);
        await InjectRefreshToken(new Guid(member.MemberId ?? ""));

        return Ok(ResponseModel<string>.Success(accessToken));
    }

    [HttpGet("memberdetails")]
    [Authorize]
    public async Task<IActionResult> GetRegisteredMemberDetails() {
        var TokenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string MemberId = TokenId != null ? edumis.Common.Utilities.DecryptString(TokenId) : string.Empty;

        var memberDetails = await unitOfWork.SMCMemberRegistrationsRepo.GetMemberDetails(MemberId);
        if (memberDetails == null)
            return Ok(ResponseModel<string>.Failure("Member Details Not Found.", StatusCodes.Status404NotFound));

        if (!memberDetails.IsActive)
            return Ok(ResponseModel<string>.Failure("No active member found!", StatusCodes.Status404NotFound));

        return Ok(memberDetails);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAccessToken() {
        var token = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token))
            return Ok(ResponseModel<string>.Failure("Invalid Token!", StatusCodes.Status401Unauthorized));

        var currentSession = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid && x.IsCurrent);
        if (currentSession == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!",
                StatusCodes.Status204NoContent));

        var refreshToken = await unitOfWork.SmcRefreshTokenRepo.GetUserTokenDetails(Utilities.ComputeSha256Hash(token));
        if (refreshToken is null || refreshToken.ExpiresOnUTC < DateTime.UtcNow)
            return Ok(ResponseModel<string>.Failure("Token Expired!", StatusCodes.Status401Unauthorized));

        var member = await unitOfWork.SMCMemberRegistrationsRepo.GetMemberDetails(refreshToken.UserId.ToString());
        if (member is null || !member.IsActive || member.ForSession != currentSession.ForSession)
            return Ok(ResponseModel<string>.Unauthorized("Member Not Active!"));

        var accessToken = GenerateAccessToken(member);
        await InjectRefreshToken(refreshToken.UserId);

        return Ok(ResponseModel<object>.Success(accessToken));
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken() {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return Ok(ResponseModel<string>.Success("Success", "No token supplied!"));

        Response.Cookies.Delete("refreshToken");

        var refreshTokenHash = Utilities.ComputeSha256Hash(refreshToken);
        var userToken = await unitOfWork.SmcRefreshTokenRepo.GetFirstOrDefault(u => u.Token == refreshTokenHash);

        if (userToken == null)
            return Ok(ResponseModel<string>.Success("Success", "Invalid token supplied!"));

        await unitOfWork.SmcRefreshTokenRepo.Remove(userToken);
        await unitOfWork.Save();

        return Ok(ResponseModel<string>.Success("Success", "You have successfully logged out."));
    }

    #endregion

    #region Branch APIs

    [HttpPost("validate_branch/{branchid}")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateBranch([FromRoute] string branchid) {
        if (string.IsNullOrEmpty(branchid))
            return Ok(ResponseModel<string>.Failure("Invalid Input!"));

        var branchDetails = await unitOfWork.SMCUserRepo.GetFirstOrDefault(x =>
            x.BranchId == branchid &&
            x.IsValid == true);
        if (branchDetails == null)
            return Ok(ResponseModel<string>.Failure("Not Authorized!", StatusCodes.Status401Unauthorized));


        return Ok(await otpService.SendOtpAsync(branchDetails.MobileNo, "VERIFY_MOBILE_SMC"));
    }
    
    [HttpPost("verify_branch_otp/{branchid}")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyBranchOtpText(
        [FromRoute] string branchid,
        [FromBody] VerifyBranchOtpRequest request)
    {
        if (string.IsNullOrEmpty(branchid))
            return Ok(ResponseModel<string>.Failure("Invalid Input!"));

        if (request == null || string.IsNullOrEmpty(request.Otp))
            return BadRequest(ResponseModel<string>.Failure("Invalid Input!"));

        var account = await unitOfWork.SMCUserRepo.GetFirstOrDefault(x =>
            x.BranchId == branchid &&
            x.IsValid == true);
        if (account == null)
            return Ok(ResponseModel<string>.Failure("Not Authorized!", StatusCodes.Status401Unauthorized));

        var otpValidation = await otpService.ValidateOtpAsync(account.MobileNo, request.Otp, "VERIFY_MOBILE_SMC");
        if (!otpValidation.IsSuccess)
            return Ok(otpValidation);

        var branchUser = await unitOfWork.SMCUserRepo.GetBranchUserDetails(account.UserId.ToString());
        if (branchUser is null)
            return Ok(ResponseModel<string>.Failure("Not Authorized!", StatusCodes.Status401Unauthorized));

        var accessToken = GenerateAccessToken(branchUser);
        await InjectRefreshToken( branchUser.UserId );

        return Ok(ResponseModel<object>.Success(
            new {
                UserName = branchUser.BranchName,
                MemberType = branchUser.BranchType,
                BranchId = account.BranchId,
                BranchType = branchUser.BranchType,
                Token = accessToken,
            }));
    }
    
    [HttpPost("refresh-token-branch")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAccessTokenForBranch() {
        var token = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(token))
            return Ok(ResponseModel<string>.Failure("Invalid Token!", StatusCodes.Status401Unauthorized));

        var currentSession = await unitOfWork.AcademicSessions.GetFirstOrDefault(x => x.IsValid && x.IsCurrent);
        if (currentSession == null)
            return Ok(ResponseModel<string>.Failure("Unable to verify current academic session!",
                StatusCodes.Status204NoContent));
        
        var refreshToken = await unitOfWork.SmcRefreshTokenRepo.GetUserTokenDetails(Utilities.ComputeSha256Hash(token));
        if (refreshToken is null || refreshToken.ExpiresOnUTC < DateTime.UtcNow)
            return Ok(ResponseModel<string>.Failure("Token Expired!", StatusCodes.Status401Unauthorized));

        var branchUser = await unitOfWork.SMCUserRepo.GetBranchUserDetails(refreshToken.UserId.ToString());
        if (branchUser == null)
            return Ok(ResponseModel<string>.Failure("Branch User Details Not Found.", StatusCodes.Status404NotFound));
        
        if (!branchUser.IsValid)
            return Ok(ResponseModel<string>.Failure("No active branch user found!", StatusCodes.Status404NotFound));

        var accessToken = GenerateAccessToken(branchUser);
        await InjectRefreshToken(refreshToken.UserId);

        return Ok(ResponseModel<string>.Success(accessToken));
    }

    [HttpGet("getbranchdetails")]
    [Authorize]
    
    public async Task<IActionResult> GetBranchDetails() {
        var tokenId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var branchUserId = tokenId != null ? edumis.Common.Utilities.DecryptString(tokenId) : string.Empty;

        var branchDetails = await unitOfWork.SMCUserRepo.GetBranchUserDetails(branchUserId);
        if (branchDetails == null)
            return Ok(ResponseModel<string>.Failure("Branch Details Not Found.", StatusCodes.Status404NotFound));

        if (!branchDetails.IsValid)
            return Ok(ResponseModel<string>.Failure("No active branch found!", StatusCodes.Status404NotFound));

        return Ok(branchDetails);
    }

    #endregion

    private async Task InjectRefreshToken(Guid id) {
        var refreshToken = tokenHelper.GenerateRefreshToken();
        var refreshTokenHash = Utilities.ComputeSha256Hash(refreshToken);

        var refreshTokenSaveObj = new SmcRefreshTokenModel {
            RowId = Guid.NewGuid(),
            UserId = id,
            Token = refreshTokenHash,
            ExpiresOnUTC = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JWTAuth:RefreshTokenExpirationDays")),
            CreatedBy = "Auto",
            ModifiedBy = "Auto"
        };
        await unitOfWork.SmcRefreshTokenRepo.Add(refreshTokenSaveObj);
        await unitOfWork.SmcRefreshTokenRepo.RemoveUnusedTokensExcept(id, refreshTokenHash);
        await unitOfWork.Save();

        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(configuration.GetValue<int>("JWTAuth:RefreshTokenExpirationDays")),
        });
    }
    private string GenerateAccessToken(SMCMemberDetailsDTO member) => tokenHelper.GenerateAccessToken(
        new Dictionary<string, string> {
            { ClaimTypes.NameIdentifier, Utilities.EncryptString(member.MemberId??"") },
            { ClaimTypes.PrimaryGroupSid, member.MemberType.ToString() },
            { "BranchId", member.BranchId },
            { "MemberType", member.MemberType.ToString() },
            { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() },
        });

    private string GenerateAccessToken(SMCBranchDetailsDTO branch) => 
        tokenHelper.GenerateAccessToken(new Dictionary<string, string> {
            { ClaimTypes.NameIdentifier, Utilities.EncryptString(branch.UserId.ToString()) },
            { ClaimTypes.PrimaryGroupSid, branch.UserType.ToString() },
            { "BranchId", branch.BranchId },
            { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() }
        });
}