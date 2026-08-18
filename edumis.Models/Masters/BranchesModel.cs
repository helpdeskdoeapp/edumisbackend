using edumis.Models.Employees;
using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Masters;

[Table("tbmsbranches")]
public class BranchesModel : BaseEntity<int>
{  
    [Column(name: "branchid", TypeName = "varchar(50)")]
    public string BranchId { get; set; } = default!;

    [Column(name: "buildingid", TypeName = "varchar(50)")]
    public string? BuildingId { get; set; } = default!;
      
    [Column(name: "branchname", TypeName = "varchar(500)")]
    public string BranchName { get; set; } = default!;
       
    [Column(name: "branchtype")]
    public int BranchType { get; set; }

    [Column(name: "parentbranchid", TypeName = "varchar(50)")]
    public string? ParentBranchId { get; set; }
        
    [Column(name: "zoneid", TypeName = "integer")]
    public int? ZoneId { get; set; } 
        
    [Column(name: "districtid", TypeName = "integer")]
    public int? DistrictId { get; set; } 

    [Column(name: "inchargeid", TypeName = "varchar(50)")]
    public string? InchargeId {  get; set; } = default!;

    [Column(name: "emailid", TypeName = "varchar(150)")]
    public string? EmailId { get; set; } = default!;

    [Column(name: "contactno", TypeName = "varchar(20)")]
    public string? ContactNo { get; set; } = default!;

    [Column(name: "address", TypeName = "varchar(500)")]
    public string? Address { get; set; } = default!;
        
    [Column(name: "isactive")]
    public bool IsActive { get; set; }
}
