using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.SMC;

[Table("tbsmc_devicetokens")]
public class MemberDeviceTokensModel : BaseEntity<long>
{    
    [Column(name: "memberid")]
    public Guid MemberId { get; set; }

    [Column(name: "serialno")]
    public int SerialNo { get; set; }
   
    [Column(name: "devicename", TypeName = "varchar(250)")]
    public string DeviceName { get; set; } = default!;
        
    [Column(name: "macaddress", TypeName = "varchar(100)")]
    public string? MacAddress { get; set; } = default!;

    [Column(name: "token", TypeName = "text")]
    public string? Token { get; set; } = default!;    
}
