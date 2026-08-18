using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Web.DTO;

public class MarqueeRequestDetailsDTO
{
    public string Title { get; set; } = default!;
    public string? ExternalLink { get; set; }
    public bool ShowNewIcon { get; set; }
}
