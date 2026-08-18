using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Global;

[Table("tbalm_visitor_logs")]
public class AlumniVisitorCounterModel
{
    [Column(name: "rowid")]
    public long RowId { get; set; }

    [Column(name: "visitdatetime")]
    public DateTime VisitDateTime { get; set; } = DateTime.UtcNow;

    [Column(name: "ipaddress", TypeName = "varchar(50)")]
    public string? IPAddress { get; set; } = default!;

    [Column(name: "useragent", TypeName = "varchar(512)")]
    public string? UserAgent { get; set; }

    [Column(name: "country", TypeName = "varchar(250)")]
    public string? Country { get; set; }
}
