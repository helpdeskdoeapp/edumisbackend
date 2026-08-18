namespace edumis.Models.Web.DTO;

public class MarqueeDetailsResponseDTO
{
    public int RecordId {  get; set; }
    public string Title { get; set; } = default!;
    public string? ExternalLink { get; set; }
    public bool ShowNewIcon { get; set; }
    public bool IsValid { get; set; }
}
