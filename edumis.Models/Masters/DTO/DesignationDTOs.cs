using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Masters.DTO;

public record NewDesignationDTO(
   [Required][MaxLength(100)] string Title,
   [Required] int DesignationGroup,
   [Required] bool IsGazetted  
   );

public record DesignationUpdateRequestDTO(
   [Required] int DesignationId,
   [Required][MaxLength(100)] string Title,
   [Required] int DesignationGroup,
   [Required] bool IsGazetted    
   );

public record DesignationDetailsDTO(
    int DesignationId,
    string Title,
    int DesignationGroup,
    string DesignationGroupDesc,
    bool IsGazetted,
    bool IsActive
);
