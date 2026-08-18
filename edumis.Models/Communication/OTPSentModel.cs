using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Communication;

[Table("tbcom_sentotp")]
public class OTPSentModel : BaseEntity<long>
{
    [Column(name: "sentto", TypeName = "varchar(100)")]
    public string SentTo { get; set; } = default!;

    [Column(name: "purpose", TypeName = "varchar(100)")]
    public string? Purpose { get; set; } = default!;

    [Column(name: "ipaddress", TypeName = "varchar(15)")]
    public string? IPAddress { get; set; } = default!;

    [Column(name: "otp", TypeName = "varchar(10)")]
    public string OTP { get; set; } = default!;

    [Column(name: "sentdate")]
    public DateTime SentDate { get; set; } = DateTime.Now;

    [Column(name: "validupto")]
    public DateTime ValidUpTo { get; set; } = DateTime.Now.AddMinutes(10);
}
