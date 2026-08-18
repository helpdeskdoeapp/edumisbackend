using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Users;

[Table("tbgluseractivitylogs")]
public class UserActivityLogsModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [Column(name: "rowid")]
    public long RowId { get; set; }
            
    [Column(name: "userid")]
    public Guid UserId { get; set; }

    [Column(name: "secondaryid", TypeName = "varchar(250)")]
    public string? SecondaryId { get; set; }

    [Column(name: "activity", TypeName = "varchar(250)")]
    public string Activity { get; set; } = default!;
           
    [Column(name: "activitydatetime")]
    public DateTime ActivityDateTime { get; set; } = DateTime.Now;

    [Column(name: "ipaddress", TypeName = "varchar(30)")]
    public string IPAddress { get; set; } = default!;

    [Column(name: "useragent", TypeName = "varchar(250)")]
    public string? UserAgent { get; set; }
}
