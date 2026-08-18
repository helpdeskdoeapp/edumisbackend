using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Events;

[Table("tbweb_events")]
public class EventsModel : BaseEntity<long>
{
    [Column(name: "financialyear", TypeName = "varchar(10)")]
    public string FinancialYear { get; set; } = default!;

    [Column(name: "title", TypeName = "text")]
    public string Title { get; set; } = default!;

    [Column(name: "description", TypeName = "text")]
    public string? Description { get; set; }
        
    [Column(name: "venue", TypeName = "text")]
    public string Venue { get; set; } = default!;

    [Column(name: "category")]
    public int Category { get; set; }

    [Column(name: "startdate", TypeName = "date")]
    public DateOnly StartDate { get; set; }

    [Column(name: "enddate", TypeName = "date")]
    public DateOnly EndDate { get; set; }

    [Column(name: "starttime", TypeName = "time")]
    public TimeOnly StartTime { get; set; }

    [Column(name: "endtime", TypeName = "time")]
    public TimeOnly EndTime { get; set; }

    [Column(name: "organizedby", TypeName = "varchar(500)")]
    public string? OrganizedBy { get; set; } = default!;

    [Column(name: "branchid", TypeName = "varchar(50)")]
    public string? BranchId { get; set; }

    [Column(name: "videolink", TypeName = "text")]
    public string? VideoLink { get; set; }

    [Column(name: "externallink", TypeName = "text")]
    public string? ExternalLink { get; set; }    

    [Column(name: "banner_filepath", TypeName = "varchar(500)")]
    public string? BannerFilePath { get; set; }

    [Column(name: "banner_filename", TypeName = "varchar(100)")]
    public string? BannerFileName { get; set; }

    [Column(name: "banner_file_extn", TypeName = "varchar(30)")]
    public string? BannerFileExtn { get; set; }

    [Column(name: "banner_file_content_type", TypeName = "varchar(50)")]
    public string? BannerFileContentType { get; set; }

    [Column(name: "isvalid")]
    public bool IsValid { get; set; } = true;

    [Column(name: "alumni_event")]
    public bool? AlumniEvent { get; set; }
}

