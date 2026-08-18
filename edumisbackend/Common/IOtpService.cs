namespace edumisbackend.Common;

public interface IOtpService {
    public Task<ResponseModel<string>> SendOtpAsync(string? mobile, string? purpose = null);
    public Task<ResponseModel<string>> ValidateOtpAsync(string? mobile, string otp, string? purpose = null);
}
