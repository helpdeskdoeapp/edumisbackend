using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Global;

[Table("tbglcodes")]
public class CodesModel : BaseEntity<int>
{
    [Column(name: "code")]
    public int Code { get; set; }
      
    [Column(name: "codedescription", TypeName = "varchar(500)")]
    public string CodeDescription { get; set; } = default!;

    [Column(name: "isactive")]
    public bool IsActive { get; set; }    

    public ICollection<CodeValuesModel>? CodeValuesList { get; private set; } = default!;
}
