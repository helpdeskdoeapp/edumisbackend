using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Circulars;

[Table("tbweb_circulars")]
public class CircularModel : BaseEntity<long>
{        
    [Column(name: "financialyear", TypeName = "varchar(10)")]
    public string FinancialYear { get; set; } = default!;

    [Column(name: "circulardate", TypeName = "date")]   
    public DateOnly CircularDate { get; set; }
       
    [Column(name: "title", TypeName = "text")]
    public string Title { get; set; } = default!;

    [Column(name: "description", TypeName = "text")]
    public string? Description { get; set; }
      
    [Column(name: "type")]
    public int Type { get; set; }
        
    [Column(name: "isvalid")]
    public bool IsValid { get; set; } = true;

    [Column(name: "filepath", TypeName = "varchar(500)")]
    public string? FilePath { get; set; }

    [Column(name: "filename", TypeName = "varchar(100)")]
    public string? FileName { get; set; }

    [Column(name: "file_extn", TypeName = "varchar(30)")]
    public string? FileExtn { get; set; }

    [Column(name: "file_content_Type", TypeName = "varchar(50)")]
    public string? FileContentType { get; set; }
}
