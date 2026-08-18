using System.ComponentModel.DataAnnotations;

namespace edumis.Models.Employees.DTO;

#region Input DTOs
public record EmployeeDTO(
   [MaxLength(50)] string EmployeeId,
   [MaxLength(150)][Required] string FirstName,
   [MaxLength(150)] string? MiddleName,
   [MaxLength(150)] string? LastName,
   [MaxLength(150)] string? FatherName,
   [MaxLength(150)] string? MotherName,
   [Required] int Gender,
   [Required] DateOnly DOB,
   [MaxLength(12)][MinLength(12)] string? AadharNo,
   [MaxLength(10)][MinLength(10)] string? PanNo,
   [MaxLength(150)][Required][EmailAddress] string EmailId,
   [MaxLength(10)][MinLength(10)][Required] string MobileNo,
   [MaxLength(250)][Required] string PermanentAddress,
   [MaxLength(150)][Required] string PAddressCity,
   [Required] int PAddressState,
   [MaxLength(10)][MinLength(6)][Required] string PAddressPincode,
   [MaxLength(250)][Required] string CorrespondenceAddress,
   [MaxLength(150)][Required] string CAddressCity,
   [Required] int CAddressState,
   [MaxLength(10)][MinLength(6)][Required] string CAddressPincode,
   [Required] int Category,
   int? SubCategory,
   int? HighestQualification,
   int? MaritalStatus,
   [Required] bool IsAnyDisability,
   int? DisabilityType,
   string? OtherDisabilityType,
   bool? IsGazetted,
   bool? VehicleFacilityAvailed,
   string? ReportingPersonId,
   [Required] bool IsActive,
   string? Remarks
 );

public record AppointmentDTO(
    [MaxLength(50)][Required] string EmployeeId,
    [Required] int Designation,
    int? SeniorityNo,
    [Required] int AppointmentType,
    string? AppointmentOrder,
    [Required] DateOnly AppointmentDate,
    [Required] DateOnly BranchJoiningDate,
    [Required] int RecruitmentType,
    int? SelectionCategory,
    int? CurrentPostHeld,
    [Required] string CurrentBranchID,
    [Required] int Cadre,
    string? CurrentScale,
    string? Grade,
    DateOnly? GradeGrantDate,
    DateOnly? RetirementDate
 );

public record EducationDTO(
    [MaxLength(50)] string EmployeeId,
    int? SerialNo,
    [Required] int Qualification,
    [Required][MaxLength(250)] string Title,
    [Required] DateOnly IssueDate,
    string? Board,
    decimal? Percentage,
    string? Grade,
    [MaxLength(200)] string? Subjects
 );

#endregion

#region Details DTOs
public record EmployeeDetailsDTO(
   string EmployeeId,
   string FirstName,
   string? MiddleName,
   string? LastName,
   string? FatherName,
   string? MotherName,
   int Gender,
   string GenderTitle,
   DateOnly DOB,
   string? AadharNo,
   string? PanNo,
   string EmailId,
   string MobileNo,
   string PermanentAddress,
   string PAddressCity,
   int PAddressState,
   string PAddressStateName,
   string PAddressPincode,
   string CorrespondenceAddress,
   string CAddressCity,
   int CAddressState,
   string CAddressStateName,
   string CAddressPincode,
   int Category,
   string CategoryTitle,
   int? SubCategory,
   string? SubCategoryTitle,
   int? SelectionCategory,
   string? SelectionCategoryTitle,
   int? HighestQualification,
   string? HighestQualificationTitle,
   int? MaritalStatus,
   string? MaritalStatusTitle,
   bool IsAnyDisability,
   int? DisabilityType,
   string? DisabilityTypeDesc,
   string? OtherDisabilityType,
   bool? IsGazetted,
   bool? VehicleFacilityAvailed,
   string? ReportingPersonId,
   bool IsActive,
   string? Remarks,
   int Designation,
    string DesignationTitle,
    int? SeniorityNo,
    int AppointmentType,
    string AppointmentTypeDesc,
    string? AppointmentOrder,
    DateOnly? AppointmentDate,
    DateOnly BranchJoiningDate,
    int RecruitmentType,
    string RecruitmentTypeDesc,
    int? CurrentPostHeld,
    string? CurrentPostTitle,
    string? CurrentBranchID,
    string? CurrentBranchName,
    int? Cadre,
    string? CadreTitle,
    string CurrentScale,
    string? Grade,
    DateOnly? GradeGrantDate,
    DateOnly? RetirementDate,
    byte[]? Photo,
    string? Extension,
    string? ContentType,
   EducationDetailsDTO? EducationDetails
 );

public class SearchResultResponseDTO
{
    public string EmployeeId { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? FatherName { get; set; }
    public string? MotherName { get; set; }
    public int? Gender { get; set; }
    public string GenderTitle { get; set; } = default!;
    public DateOnly? DOB { get; set; }
    public string? AadharNo { get; set; }
    public string? PanNo { get; set; }
    public string? EmailId { get; set; } = default!;
    public string? MobileNo { get; set; } = default!;
    public string? PermanentAddress { get; set; } = default!;
    public string? PAddressCity { get; set; } = default!;
    public int? PAddressState { get; set; } = default!;
    public string? PAddressStateName { get; set; } = default!;
    public string? PAddressPincode { get; set; } = default!;
    public string? CorrespondenceAddress { get; set; } = default!;
    public string? CAddressCity { get; set; } = default!;
    public int? CAddressState { get; set; }
    public string? CAddressStateName { get; set; } = default!;
    public string? CAddressPincode { get; set; } = default!;
    public int? Category { get; set; }
    public string? CategoryTitle { get; set; } = default!;
    public int? SubCategory { get; set; }
    public string? SubCategoryTitle { get; set; }
    public int? SelectionCategory { get; set; }
    public string? SelectionCategoryTitle { get; set; }
    public int? HighestQualification { get; set; }
    public string? HighestQualificationTitle { get; set; }
    public int? MaritalStatus { get; set; }
    public string? MaritalStatusTitle { get; set; }
    public bool IsAnyDisability { get; set; }
    public int? DisabilityType { get; set; }
    public string? DisabilityTypeDesc { get; set; }
    public string? OtherDisabilityType { get; set; }
    public bool? IsGazetted { get; set; }
    public bool? VehicleFacilityAvailed { get; set; }
    public string? ReportingPersonId { get; set; }
    public int? DesignationId { get; set; }
    public string? DesignationTitle { get; set; }
    public int? DesignationGroup { get; set; }
    public string? DesignationGroupTitle { get; set; }
    public bool IsActive { get; set; }
    public string? Remarks { get; set; }
    public byte[]? Photo { get; set; }
    public string? Extension { get; set; }
    public string? ContentType { get; set; }
}

public record AppointmentDetailsDTO(
    string EmployeeId,
    int Designation,
    string DesignationTitle,
    int? SeniorityNo,
    int AppointmentType,
    string AppointmentTypeDesc,
    string? AppointmentOrder,
    DateOnly? AppointmentDate,
    DateOnly BranchJoiningDate,
    int RecruitmentType,
    string RecruitmentTypeDesc,
    int? SelectionCategory,
    string? SelectionCategoryDesc,
    int? CurrentPostHeld,
    string? CurrentPostTitle,
    string? CurrentBranchID,
    string? CurrentBranchName,
    int? Cadre,
    string? CadreTitle,
    string? CurrentScale,
    string? Grade,
    DateOnly? GradeGrantDate,
    DateOnly? RetirementDate
 );

public record EducationDetailsDTO(
   long RecordId,
   string EmployeeId,
   int? SerialNo,
   int Qualification,
   string QualificationTitle,
   string Title,
   DateOnly IssueDate,
   string? Board,
   decimal? Percentage,
   string? Grade,
   string? Subjects
);
#endregion

#region Search DTOs
public class SearchEmployeeRequestDTO
{
    public string? BranchId { get; set; }
    public int? Gender { get; set; } = 0;
    public int? Category { get; set; } = 0;
    public int? SelectionCategory { get; set; } = 0;    
    public int? DisabilityType { get; set; } = 0;
    public int? DesignationGroup { get; set; } = 0;
    public int? DesignationId { get; set; } = 0;
    public bool? GazettedOnly { get; set; }
    public bool? VehiclefacilityAvailed { get; set; }
    public bool? Status { get; set; }
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
#endregion

public record ProfilePhoto
(
    [Required] string EmployeeID,
    [Required] byte[] Photo,
    [Required] string Extension,
    [Required] string ContentType
);


public class EmployeeBasicDto
{
    public string EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string PhoneNumber { get; set; }
    public int? Gender { get; set; }
    public int Designation { get; set; }
    public string DesignationTitle { get; set; }
}