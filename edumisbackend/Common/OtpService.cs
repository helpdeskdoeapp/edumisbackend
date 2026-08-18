using edumis.Common;
using edumis.DataAccess.IRepositories;
using edumis.DataAccess.IRepositories.ICommunication;
using edumis.Models;
using edumis.Models.Communication;

namespace edumisbackend.Common;


internal class OtpService(IUnitOfWork uow, IConfiguration config): IOtpService {
    
    private readonly ISMSSettingsRepo settings = uow.SMSSettingsRepo;
    private readonly ISMSTemplatesRepo templates = uow.SMSTemplatesRepo;
    private readonly IOTPSentRepo sentOtps = uow.OTPSentRepo;
    private readonly int expiryTime = config.GetValue<int>("OtpExpiryTime"); 
    
    public async Task<ResponseModel<string>> SendOtpAsync(string? mobile, string? purpose) {
        if(mobile == null || mobile.Trim().Length != 10)
            return ResponseModel<string>.Failure("Invalid mobile number",StatusCodes.Status406NotAcceptable);
        
        var smsSettings = await settings.GetFirstOrDefault(x => x.IsValid);
        if (smsSettings == null)
            return ResponseModel<string>.Failure("SMS Settings Not Found!", StatusCodes.Status404NotFound);

        var smsTemplate = await templates.GetFirstOrDefault(x => x.SMSType == (int)SMS_PURPOSE.OTP_SMS && x.IsValid == true);
        if (smsTemplate == null)
            return ResponseModel<string>.Failure("SMS Template Not Found!", StatusCodes.Status404NotFound);

        // check if already has 3 or more pending sms in last 5 minutes
        var fiveMinutesBefore = DateTime.Now.AddMinutes(-5);
        var previousOtps = await sentOtps.GetAll(x => x.SentTo == mobile && x.Purpose == purpose && x.SentDate > fiveMinutesBefore);
        if (previousOtps.Count() >= 3) 
            return ResponseModel<string>.Failure("Wait 5 minutes before sending another OTP request.", StatusCodes.Status403Forbidden);
            
        var random = new Random();
        long otpText = random.Next(100001, 999999);
        var message = $"OTP : {otpText} --Directorate of Education";
        var returnSmsStatus = SMS.sendOTPMSG(smsSettings.UserID, smsSettings.Password, smsSettings.SenderId, mobile, message, smsSettings.SecureKey, smsTemplate.TemplateId, smsSettings.SMSURL);
        
        if (!returnSmsStatus.Contains("402"))//402,MsgID = 070920211631027377372edudel-Am (Success Message returned)
            return ResponseModel<string>.Failure("Failed To Send SMS!");
        
        var modelToSave = new OTPSentModel() {
            SentTo = mobile,
            Purpose = purpose,
            OTP = otpText.ToString(),
            ValidUpTo = DateTime.Now.AddMinutes(expiryTime)
        };
        await sentOtps.Add(modelToSave);
        await sentOtps.Save();

        return ResponseModel<string>.Success("",
            $"OTP sent to the mobile number xxxxxxx{mobile[7..]}. OTP is Valid for {expiryTime} minutes only."
        );
        
    }
    
    
    
    public async Task<ResponseModel<string>> ValidateOtpAsync(string? mobile, string otp, string? purpose = null) {
        if(mobile == null || mobile.Trim().Length != 10)
            return ResponseModel<string>.Failure("Invalid mobile number",StatusCodes.Status406NotAcceptable);
        
        otp = otp.Trim();
        if (otp.Length != 6)
            return ResponseModel<string>.Failure("Invalid OTP!",StatusCodes.Status406NotAcceptable);

        var lastOtp = await sentOtps.GetFirstOrDefaultByOrder(x => x.SentDate, x => x.SentTo == mobile && x.Purpose == purpose, true);
        if (lastOtp == null)
            return ResponseModel<string>.Failure("Failed to verify OTP!", StatusCodes.Status404NotFound );

        if (lastOtp.ValidUpTo.Subtract(DateTime.Now).Minutes < 0)
            return ResponseModel<string>.Failure("OTP Expired!");

        return lastOtp.OTP != otp ? ResponseModel<string>.Failure("OTP Mismatch!", StatusCodes.Status403Forbidden ) 
            : ResponseModel<string>.Success("OTP Valid!");
    }
}