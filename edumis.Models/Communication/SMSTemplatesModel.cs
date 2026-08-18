using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Communication;

[Table("tbcom_smstemplates")]
public class SMSTemplatesModel : BaseEntity<int>
{  
    [Column(name: "templateid", TypeName = "varchar(250)")]
    public string TemplateId { get; set; } = default!;

    [Column(name: "message", TypeName = "text")]
    public string? Message { get; set; } = default!;

    [Column(name: "smstype")]
    public int SMSType {  get; set; }

    [Column(name: "isvalid")]
    public bool IsValid { get; set; }
}
