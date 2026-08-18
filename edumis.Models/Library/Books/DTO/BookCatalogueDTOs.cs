using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Library.Books.DTO;

public record BookCatalogueRequestDTO(       
    string? Location,
    string? Shelf,   
    [Required] int Condition,
    string? ConditionNotes,
    int? DamageType,    
    [Required] int Status
);

public record BookCatalogueUpdateRequestDTO(
    [Required] Guid BookId,
    [Required] string[] AccessionNumber,   
    string? Location,
    string? Shelf   
);

public class BookCatalogueDetailsDTO
{
    public Guid BookId { get; set; }   
    public string AccessionNumber { get; set; } = default!;
    public string? Location { get; set; }
    public string? Shelf { get; set; }    
    public int Condition { get; set; }
    public string ConditionDesc { get; set; } = default!;
    public string? ConditionNotes { get; set; }    
    public int? DamageType { get; set; }
    public string DamageTypeDesc { get; set; } = default!;   
    public int Status { get; set; }
    public string StatusDesc { get; set; } = default!;
}
