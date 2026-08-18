using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Global;

[Table("tbglcodevalues")]
public class CodeValuesModel : BaseEntity<int>
{
    [Column(name: "codevalue")]
    public int CodeValue { get; set; }
       
    [Column(name: "codevaldescription", TypeName = "varchar(500)")]
    public string CodeValDescription { get; set; } = default!;

    [Column(name: "parentcode")]
    public int? ParentCode { get; set; }
       
    [Column(name: "isactive")]
    public bool IsActive { get; set; }

    [Column(name: "code")]
    public int Code { get; set; }

    public CodesModel CodesNavigation { get; set; } = default!;
}
