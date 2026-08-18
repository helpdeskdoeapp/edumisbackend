using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Leave;

[Table("tbgl_holidays")]
public class HolidayModel: BaseEntity<long> {
    [Column("session")] [Required]
    public required string Session { get; set; }
    
    [Column("school_id")] [Required]
    public required string SchoolId { get; set; }
    
    [Column("type")] [Required]
    public required int Type { get; set; } //  RH, GH etc
    
    [Column("description")] [Required]
    public required string Description { get; set; }
    
    [Column("date")] [Required]
    public required DateOnly Date { get; set; }
    
    [Column("remark")]
    public string? Remark { get; set; }
}