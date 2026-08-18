using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Global.DTO;

public record SessionInfoDTO
(
    [Required][MaxLength(10)][MinLength(9)] string ForSession,
    [Required] bool IsValid,
    [Required] bool IsCurrent,
    [Required] bool IsRegistrationOpen,
    DateOnly? RegistrationStartDate,
    DateOnly? RegistrationEndDate,
    DateOnly? LateRegistrationStartDate,
    DateOnly? LateRegistrationEndDate,
    TimeOnly? RegistrationEndTime,
    TimeOnly? LateRegistrationEndTime,
    DateOnly? Reg_AgeAsOnDate,
    TimeOnly? RegistrationStartTime,
    TimeOnly? LateRegistrationStartTime,
    [Required][MaxLength(100)] string LoggedInUserId
);

public record SessionDetailsDTO
(
    string ForSession,
    bool IsValid,
    bool IsCurrent,
    DateOnly? RegistrationStartDate,
    DateOnly? RegistrationEndDate,
    DateOnly? LateRegistrationStartDate,
    DateOnly? LateRegistrationEndDate,
    TimeOnly? RegistrationEndTime,
    TimeOnly? LateRegistrationEndTime,
    DateOnly? Reg_AgeAsOnDate,
    TimeOnly? RegistrationStartTime,
    TimeOnly? LateRegistrationStartTime,
    bool IsRegistrationOpen
);
