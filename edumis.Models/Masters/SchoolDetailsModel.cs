using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Masters;

[Table("tbms_schooldetails")]
public class SchoolDetailsModel : BaseEntity<int>
{   
    [Column(name: "branchid", TypeName = "varchar(50)")]
    public string BranchId { get; set; } = default!;

    [Column(name: "udisecode", TypeName = "varchar(50)")]
    public string? UDISECode { get; set; } = default!;

    [Column("shift")]
    public int? Shift {  get; set; }

    [Column("gender")]
    public int? Gender { get; set; }

    [Column("estbyear")]
    public int? EstbYear { get; set; }

    [Column("nomenclature")]
    public int? Nomenclature { get; set; }

    [Column("policestation")]
    public string? PoliceStation {  get; set; }
    
    [Column(name: "hospital", TypeName = "varchar(250)")]
    public string? Hospital {  get; set; }
    
    [Column(name: "assembly", TypeName = "varchar(150)")]
    public string? Assembly {  get; set; }
    
    [Column(name: "constituency", TypeName = "varchar(50)")]
    public string? Constituency { get; set; }

    [Column(name: "streams", TypeName = "integer[]")] 
    public int[]? Streams { get; set; }

    [Column(name: "address", TypeName = "varchar(500)")]
    public string? Address {  get; set; }

    public BranchesModel BranchNavigation { get; set; } = default!;
}
