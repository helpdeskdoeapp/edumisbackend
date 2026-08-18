using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Communication;

[Table("tbcom_sms_settings")]
public class SMSSettingsModel : BaseEntity<int>
{
    [Column(name: "userid", TypeName = "varchar(250)")]
    public string UserID { get; set; } = default!;

    [Column(name: "password", TypeName = "varchar(250)")]
    public string Password { get; set; } = default!;

    [Column(name: "securekey", TypeName = "varchar(250)")]
    public string SecureKey { get; set; } = default!;

    [Column(name: "senderid", TypeName = "varchar(250)")]
    public string SenderId { get; set; } = default!;

    [Column(name: "appkey", TypeName = "varchar(250)")]
    public string AppKey { get; set; } = default!;

    [Column(name: "smsurl", TypeName = "varchar(500)")]
    public string SMSURL {  get; set; } = default!;

    [Column(name: "isvalid")]
    public bool IsValid {  get; set; }
}
