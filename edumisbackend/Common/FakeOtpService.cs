using edumis.DataAccess.IRepositories;

namespace edumisbackend.Common;


internal class FakeOtpService(IUnitOfWork uow, IConfiguration config): IOtpService {
    
    public Task<ResponseModel<string>> SendOtpAsync(string? mobile, string? purpose) {
        if(mobile == null || mobile.Trim().Length != 10)
            return Task.FromResult(ResponseModel<string>.Failure("Invalid mobile number",StatusCodes.Status406NotAcceptable));
        
        return Task.FromResult(ResponseModel<string>.Success("OTP Sent",
            $"OTP is {mobile[..6]}."));
    }
    
    public Task<ResponseModel<string>> ValidateOtpAsync(string? mobile, string otp, string? purpose = null) {
        ResponseModel<string> response;
        if (mobile == null || mobile.Trim().Length != 10) {
            response = ResponseModel<string>.Failure("Invalid mobile number",StatusCodes.Status406NotAcceptable);
            return Task.FromResult(response);
        }

        response =  mobile[..6] != otp 
            ? ResponseModel<string>.Failure("OTP Mismatch!", StatusCodes.Status403Forbidden ) 
            : ResponseModel<string>.Success("OTP Valid!");
        
        return Task.FromResult(response);
    }
}