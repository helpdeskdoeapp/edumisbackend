using edumis.Common;
using edumis.DataAccess.IRepositories;
using edumis.Models.Users;
using edumis.Models.Users.DTO;
using edumisbackend.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace edumisbackend.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet("user-profile")]   
    public async Task<ActionResult<ResponseModel<object>>> GetUserProfile()
    {
        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? edumis.Common.Utilities.DecryptString(TokenParam) : string.Empty;

        if (string.IsNullOrEmpty(UserId))
            return ResponseModel<object>.Failure("Not Authorized!", 401);

        var result = await unitOfWork.Users.GetUserProfile(new Guid(UserId));
        if (result == null)
            return ResponseModel<object>.NoData("No profile found!");

        return ResponseModel<object>.Success(result, "User profile details retrieved successfully!");
    }

    [HttpPost("update-password")]   
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequestDTO updatePasswordRequest)
    {
        if (updatePasswordRequest == null)
            return Ok(ResponseModel<string>.Failure("Invalid Request!", 400));        

        var TokenParam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        string UserId = TokenParam != null ? Utilities.DecryptString(TokenParam) : string.Empty;

        if (string.IsNullOrEmpty(UserId))
            return Ok(ResponseModel<string>.Failure("Not Authorized!", 401));

        var userDetails = await unitOfWork.Users.GetFirstOrDefault(x=>x.UserId == new Guid(UserId));
        if (userDetails == null)
            return Ok(ResponseModel<string>.NoData("Invaild User!"));

        if(!Utilities.VerifyPassword(updatePasswordRequest.CurrentPassword, userDetails.Password))
            return Ok(ResponseModel<string>.Failure("Not Authorized!", 401));

        userDetails.PrevPassword2 = userDetails.PrevPassword1;
        userDetails.PrevPassword1 = userDetails.Password;
        userDetails.Password = Utilities.HashPassword(updatePasswordRequest.NewPassword);
        userDetails.MaxNoOfInvalidLoginAttempt = 5;
        userDetails.LastPwdChangedDate = DateTime.UtcNow;
        userDetails.IsPwdChangeWarningSent = false;        
        userDetails.ModifiedBy = UserId;
        userDetails.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.UserActivityLogs.Add(new UserActivityLogsModel()
        {
            UserId = new Guid(UserId),
            Activity = "PASSWORD UPDATE SUCCESS",
            ActivityDateTime = DateTime.UtcNow,
            IPAddress = updatePasswordRequest.IPAddress,
            UserAgent = updatePasswordRequest.UserAgent
        });
        await unitOfWork.Save();

        return Ok(ResponseModel<string>.Success("Success", "Password updated successfully!"));     
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] PasswordResetRequestDTO passwordResetRequest)
    {
        if (passwordResetRequest == null)
            return Ok(ResponseModel<string>.Failure("Invalid Request!", 400));

        var LastSentOTP = await unitOfWork.OTPSentRepo.GetFirstOrDefaultByOrder(x => x.SentDate, x => x.SentTo == passwordResetRequest.UserId && x.Purpose == "RESET_PASSWORD", true);
        if (LastSentOTP == null)
            return Ok(ResponseModel<string>.NoData("Failed to verify OTP!", 404));
        
        if (LastSentOTP.ValidUpTo.Subtract(DateTime.Now).Minutes < 0)
            return Ok(ResponseModel<string>.Failure("OTP Expired!", 400));
       
        if (LastSentOTP.OTP != passwordResetRequest.OTPText)
            return Ok(ResponseModel<string>.Failure("OTP Mismatch!", 400));

        var userDetails = await unitOfWork.Users.GetFirstOrDefault(x => 
            x.UniqueId == passwordResetRequest.UserId || 
            (x.EmailId != null && x.EmailId.ToLower() == passwordResetRequest.UserId.ToLower()));
        if (userDetails == null)
            return Ok(ResponseModel<string>.NoData("Invaild User!"));

        userDetails.PrevPassword2 = userDetails.PrevPassword1;
        userDetails.PrevPassword1 = userDetails.Password;
        userDetails.Password = Utilities.HashPassword(passwordResetRequest.NewPassword);
        userDetails.MaxNoOfInvalidLoginAttempt = 5;
        userDetails.LastPwdChangedDate = DateTime.UtcNow;
        userDetails.IsPwdChangeWarningSent = false;     
        userDetails.IsAccountLocked = false;
        userDetails.IsLoggedIn = false;
        userDetails.ModifiedDate = DateTime.UtcNow;

        await unitOfWork.UserActivityLogs.Add(new UserActivityLogsModel()
        {
            UserId = userDetails.UserId,
            Activity = "PASSWORD RESET SUCCESS",
            ActivityDateTime = DateTime.UtcNow,
            IPAddress = passwordResetRequest.IPAddress,
            UserAgent = passwordResetRequest.UserAgent
        });
        await unitOfWork.Save();

        return Ok(ResponseModel<string>.Success("Success", "Password reset successfully!"));
    }
}
