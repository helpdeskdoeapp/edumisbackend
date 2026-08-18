using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Inspection;

public class IssueRelatedCommentsModel : BaseEntity<long>
{
    [Column("issueid")]
    public Guid IssueId { get; set; }

    [Column("serialno")]
    public int SerialNo { get; set; }

    [Column("comment_type")]
    public int CommentType { get; set; }

    [Column(name: "comment", TypeName = "text")]
    public string? Comment { get; set; } = default!;

    public IssueDetailModel InspectionIssueNavigation { get; set; } = default!;
}
