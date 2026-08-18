using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Masters.DTO;

public record PostsDTO
(
   [Required][MaxLength(15)] string PostCode,
   [Required][MaxLength(150)] string PostTitle,
   [Required] bool IsGazetted,
   string? OrderNo,
   DateTime? OrderDate,
   [Required] bool IsValid,
   [Required][MaxLength(100)] string LoggedInUserId
);

public record PostsDetailsDTO
(
    string PostCode,
    string PostTitle,
    bool IsGazetted,
    string? OrderNo,
    DateTime? OrderDate,
    bool IsValid
);
