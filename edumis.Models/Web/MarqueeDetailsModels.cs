using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Web;

[Table("tbweb_marquee")]
public class MarqueeDetailsModels : BaseEntity<int>
{   
    [Column(name: "title", TypeName = "text")]
    public string Title { get; set; } = default!;
    
    [Column(name: "externallink", TypeName = "text")]
    public string? ExternalLink { get; set; }       

    [Column(name: "show_new_icon")]
    public bool ShowNewIcon { get; set; } = true;

    [Column(name: "isvalid")]
    public bool IsValid { get; set; } = true;

}
