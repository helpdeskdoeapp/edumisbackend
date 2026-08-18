using edumis.Models.Global;
using edumis.Models.Masters;
using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.SMC;

[Table("tbsmc_registeredmembers")]
public class MemberRegistrationsModel : BaseEntity<long>
{
    [Column(name: "memberid")]
    public Guid MemberId { get; set; } = Guid.NewGuid();
        
    [Column(name: "forsession", TypeName = "varchar(10)")]
    public string ForSession { get; set; } = default!;

    [Column(name: "uniqueid", TypeName = "varchar(50)")]    
    public string UniqueId { get; set; } = default!;
       
    [Column(name: "name", TypeName = "varchar(250)")]
    public string Name { get; set; } = default!;

    [Column(name: "gender")]
    public int Gender { get; set; }

    [Column(name: "designationid")]
    public int DesignationId {  get; set; }
       
    [Column(name: "branchid", TypeName = "varchar(50)")]
    public string BranchId {  get; set; } = default!;
       
    [Column(name: "membertype")]
    public int MemberType { get; set; }
        
    [Column(name: "mobileno", TypeName = "varchar(10)")]
    public string MobileNo { get; set; } = default!;
       
    [Column(name: "isactive")]
    public bool IsActive {  get; set; }

    [Column(name: "photo", TypeName = "bytea")]
    public byte[]? Photo { get; set; }

    [ForeignKey(nameof(BranchId))]
    public BranchesModel? BranchesNavigation {  get; set; }

    [ForeignKey(nameof(ForSession))]
    public SessionInfoModel? SessionNavigation { get; set; }
}
