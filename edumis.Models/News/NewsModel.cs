using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.News;

[Table("tbweb_news")]
public class NewsModel : BaseEntity<long>
{
    [Column(name: "financialyear", TypeName = "varchar(10)")]
    public string FinancialYear { get; set; } = default!;

    [Column(name: "title", TypeName = "text")]
    public string Title { get; set; } = default!;

    [Column(name: "description", TypeName = "text")]
    public string? Description { get; set; }
       
    [Column(name: "videolink", TypeName = "text")]
    public string? VideoLink { get; set; }

    [Column(name: "externallink", TypeName = "text")]
    public string? ExternalLink { get; set; }
           
    [Column(name: "newsdate", TypeName ="date")]
    public DateOnly NewsDate { get; set; }

    [Column(name: "banner_filepath", TypeName = "varchar(500)")]
    public string? BannerFilePath { get; set; }

    [Column(name: "banner_filename", TypeName = "varchar(100)")]
    public string? BannerFileName { get; set; }

    [Column(name: "banner_file_extn", TypeName = "varchar(30)")]
    public string? BannerFileExtn { get; set; }

    [Column(name: "banner_file_content_Type", TypeName = "varchar(50)")]
    public string? BannerFileContentType { get; set; }

    [Column(name: "isvalid")]
    public bool IsValid { get; set; } = true;

    [Column(name: "alumni_news")]
    public bool? AlumniNews { get; set; }
}
