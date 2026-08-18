using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Inspection;

public class IssueRelatedAttachmentsModel : BaseEntity<long>
{    
    [Column("issueid")]
    public Guid IssueId { get; set; }

    [Column("serialno")]
    public int SerialNo { get; set; }

    [Column(name: "title", TypeName = "varchar(500)")]
    public string? Title { get; set; } = default!;

    [Column(name: "filename", TypeName = "varchar(500)")]
    public string? FileName { get; set; } = default!;

    [Column(name: "contenttype", TypeName = "varchar(100)")]
    public string? ContentType { get; set; } = default!;

    [Column(name: "extension", TypeName = "varchar(50)")]
    public string? Extension { get; set; } = default!;

    [Column(name: "filepath", TypeName = "varchar(500)")]
    public string? FilePath { get; set; } = default!;

    public IssueDetailModel InspectionIssueNavigation { get; set; } = default!;
}
