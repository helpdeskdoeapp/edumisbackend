using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Global;

[Table("tbglexceptionlogs")]
public class ExceptionLogs
{  
    [Column(name: "rowid")]
    public long RowId { get; set; }

    [Column(name: "origin", TypeName = "text")]
    public string? Origin { get; set; }

    [Column(name: "errormessage", TypeName = "text")]
    public string? ErrorMessage { get; set; }

    [Column(name: "stacktrace", TypeName = "text")]
    public string? StackTrace { get; set; }

    [Column(name: "innermessage", TypeName = "text")]
    public string? InnerMessage { get; set; }

    [Column(name: "createddate")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
