using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using edumis.Models.Global;
using edumis.Models.Masters;

namespace edumis.Models.SMC;

[Table("tbsmc_allocations")]
public class SmcBudgetAllocationModel: BaseEntity<long> {
    
    [Column(name: "session", TypeName = "varchar(10)")] 
    public required string Session { get; set; }

    [Column(name: "school_id", TypeName = "varchar(50)")]  
    public required string SchoolId { get; set; }
    
    [Column("allocation", TypeName = "numeric")]
    public decimal Allocation {  get; set; }

    [Column("consumption", TypeName = "numeric")]
    public decimal Consumption { get; set; } = 0;
    
    [ForeignKey(nameof(SchoolId))]
    public BranchesModel? BranchesNavigation { get; set; }

    [ForeignKey(nameof(Session))]
    public SessionInfoModel? SessionNavigation { get; set; }
}