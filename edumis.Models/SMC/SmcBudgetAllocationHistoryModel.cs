using System.ComponentModel.DataAnnotations.Schema;
using edumis.Models.Global;
using edumis.Models.Masters;

namespace edumis.Models.SMC;

[Table("tbsmc_allocation_history")]
public class SmcBudgetAllocationHistoryModel {
    
    [Column("row_id")]   
    public long RowId { get; set; }
    
    [Column(name: "session", TypeName = "varchar(10)")] 
    public required string Session { get; set; }

    [Column(name: "school_id", TypeName = "varchar(50)")]  
    public required string SchoolId { get; set; }
    
    [Column("amount", TypeName = "numeric")]
    public decimal Amount {  get; set; }
    
    [Column(name: "allocation_type")]
    public int AllocationType { get; set; }

    [Column(name: "allocation_date")]
    public DateTime AllocationDate { get; set; } = DateTime.UtcNow;

    [Column(name: "donor_name", TypeName = "varchar(200)")]
    public string? DonorName { get; set; } = null;

    [Column(name: "donor_pan", TypeName = "varchar(10)")]
    public string? DonorPan { get; set; } = null;

    [Column(name: "donor_mobile", TypeName = "varchar(13)")] 
    public string? DonorMobile { get; set; } = null;
    
    [Column(name: "donor_address", TypeName = "varchar(400)")] 
    public string? DonorAddress { get; set; } = null;
    
    [Column(name: "remarks", TypeName = "varchar(400)")] 
    public string? Remarks { get; set; } = null;
    
    [Column(name: "created_by", TypeName = "varchar(200)")]
    public string? CreatedBy { get; set; }

    [Column(name: "created_date")]
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    
    [ForeignKey(nameof(SchoolId))]
    public BranchesModel? BranchesNavigation { get; set; }

    [ForeignKey(nameof(Session))]
    public SessionInfoModel? SessionNavigation { get; set; }
    
}