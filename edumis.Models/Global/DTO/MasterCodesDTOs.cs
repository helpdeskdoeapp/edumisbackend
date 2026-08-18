using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Global.DTO;

public class MasterCodeDetailsResponseDTO
{
    public int Code { get; set; }
    public string CodeDescription { get; set; } = default!;
    public bool IsActive { get; set; }
    public ICollection<MasterCodeValueDetailsDTO>? SubCodes { get; set; }
}

public class MasterCodeValueDetailsDTO
{
    public int Code { get; set; }
    public int SubCode { get; set; }
    public string SubCodeDescription { get; set; } = default!;
    public int? ParentCode { get; set; }
    public bool IsActive { get; set; }
}

public record MasterCodeRequestDTO(    
    [Required][MaxLength(120, ErrorMessage = "Invalid Input Length. Only 120 characters are allowed.")] string CodeDescription    
);

public record MasterCodeUpdateRequestDTO(
    [Required] int Code,
    [Required][MaxLength(120, ErrorMessage = "Invalid Input Length. Only 120 characters are allowed.")] string CodeDescription
);

public record MasterSubCodeRequestDTO(
     [Required] int Code,    
     [Required][MaxLength(120, ErrorMessage = "Invalid Input Length. Only 120 characters are allowed.")] string SubCodeDescription,
     int? ParentCode       
);

public record MasterSubCodeUpdateRequestDTO(    
     [Required] int SubCode,
     [Required][MaxLength(120, ErrorMessage = "Invalid Input Length. Only 120 characters are allowed.")] string SubCodeDescription,
     int? ParentCode
);
