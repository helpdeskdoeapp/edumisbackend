using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Masters;

[Table("tbmsdesignations")]
public class DesignationModel : BaseEntity<int>
{
    [Column(name: "title", TypeName = "varchar(500)")]
    public string Title { get; set; } = default!;

    [Column(name: "designationgroup")]
    public int DesignationGroup { get; set; }
       
    [Column(name: "isgazetted")]
    public bool IsGazetted { get; set; }

    [Column(name: "isactive")]
    public bool IsActive { get; set; }
}
