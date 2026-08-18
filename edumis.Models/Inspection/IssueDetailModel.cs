using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Inspection;

public class IssueDetailModel : BaseEntity<long>
{

    [Column("issueid")]   
    public Guid IssueId { get; set; }

    [Column("title", TypeName = "text")]
    public string Title { get; set; } = default!;

    [Column("description", TypeName = "text")]
    public string Description { get; set; } = default!;

    [Column("category")]
    public int Category { get; set; }

    [Column("status")]
    public int Status { get; set; }   
}
