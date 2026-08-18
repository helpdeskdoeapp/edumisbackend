namespace edumis.Models.MISC.DTO;

public class SwachhBharatImagesResponseDTO
{
    public string BranchId { get; set; } = default!;   
    public DateOnly ForDate { get; set; }
    public string ImageUrl { get; set; } = default!;
    public string ImageName { get; set; } = default!;  
    public string? ImageContentType { get; set; }
    public string? ImageFileExtn { get; set; }
    public bool IsCurrent { get; set; }
}
