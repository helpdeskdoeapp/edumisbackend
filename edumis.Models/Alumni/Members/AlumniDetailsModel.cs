using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Alumni.Members;

[Table("tbalm_alumnidetails")]
public class AlumniDetailsModel : BaseEntity<long>
{
    [Column("alumni_id", TypeName = "uuid")]
    public Guid AlumniId { get; private set; }

    [Column("doeid", TypeName = "varchar(30)")]
    public string? DOERegistrationId { get; private set; }

    [Column("salutation", TypeName = "integer")]
    public int Salutation { get; private set; }

    [Column("firstname", TypeName = "varchar(150)")]
    public string FirstName { get; private set; } = default!;

    [Column("lastname", TypeName = "varchar(150)")]
    public string? LastName { get; private set; }

    [Column("middlename", TypeName = "varchar(150)")]
    public string? MiddleName { get; private set; }

    [Column("dob", TypeName = "date")]
    public DateOnly DOB { get; private set; }

    [Column("gender", TypeName = "integer")]
    public int Gender { get; private set; }

    [Column("registration_year", TypeName = "integer")]
    public int RegistrationYear { get; private set; }

    [Column("exit_year", TypeName = "integer")]
    public int ExitYear { get; private set; }

    [Column("branchid", TypeName = "varchar(30)")]
    public string? BranchId { get; private set; }

    [Column("branch_not_in_list", TypeName = "boolean")]
    public bool BranchNotInList { get; private set; }

    [Column("other_branch_name", TypeName = "varchar(250)")]
    public string? OtherBranchName { get; private set; }

    [Column("emailid", TypeName = "varchar(150)")]
    public string EmailID { get; private set; } = default!;

    [Column(name: "is_email_verified")]
    public bool? IsEmailVerified { get; set; }

    [Column("altemailid", TypeName = "varchar(150)")]
    public string? AlternateEmailId { get; private set; }

    [Column("current_organization", TypeName = "varchar(250)")]
    public string? CurrentOrganization { get; private set; }

    [Column("current_desig", TypeName = "varchar(250)")]
    public string? CurrentDesignation { get; private set; }

    [Column("current_residence", TypeName = "varchar(250)")]
    public string? CurrentResidence { get; private set; }

    [Column("residence_contactno", TypeName = "varchar(30)")]
    public string? ResidenceContactNo { get; private set; }

    [Column("work_contactno", TypeName = "varchar(30)")]
    public string? WorkContactNo { get; private set; }

    [Column("mobileno", TypeName = "varchar(10)")]
    public string? MobileNo { get; private set; }

    [Column("current_Residence_city", TypeName = "varchar(100)")]
    public string? CurrentResidenceCity { get; private set; }

    [Column("current_profession", TypeName = "integer")]
    public int? CurrentProfession { get; private set; }

    [Column("other_profession", TypeName = "varchar(150)")]
    public string? OtherProfession { get; private set; }

    [Column("is_resident_of_delhi")]
    public bool IsResidentOfDelhi { get; private set; }

    [Column("profile_image", TypeName = "bytea")]
    public byte[]? ProfileImage { get; private set; }

    [Column("profile_image_extn", TypeName = "varchar(50)")]
    public string? ProfileImageExtn { get; private set; }

    [Column("profile_image_contenttype", TypeName = "varchar(100)")]
    public string? ProfileImageContentType { get; private set; }

    [Column("isactive")]
    public bool IsActive { get; private set; }

    [Column("image_url", TypeName = "varchar(120)")]
    public string? ImageUrl { get; private set; }

    [Column("show_on_home_page")]
    public bool ShowOnHomePage { get; private set; }

    public AlumniInformationShareModel AlumniInformationShareDetails { get; private set; } = default!;

    public void SetProfileImage(byte[] ProfileImage, string ProfileImageExtn, string ProfileImageContentType, string userId)
    {
        this.ProfileImage = ProfileImage;
        this.ProfileImageContentType = ProfileImageContentType;
        this.ProfileImageExtn = ProfileImageExtn;
        this.ModifiedBy = userId;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void SetActivationStatus(bool IsActive) { 
        this.IsActive = IsActive;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void SHowOnHomePageStatus(bool status, string updatedBy)
    {
        this.ShowOnHomePage = status;
        this.ModifiedBy = updatedBy;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void UpdateEnrollmentDetails(string? DOERegistrationId, int RegistrationYear, int ExitYear, string? BranchId, bool BranchNotInList, string? OtherBranchName, string userID)
    {
        this.DOERegistrationId = DOERegistrationId;
        this.RegistrationYear = RegistrationYear;
        this.ExitYear = ExitYear;
        this.BranchId = BranchId;
        this.BranchNotInList = BranchNotInList;
        this.OtherBranchName = OtherBranchName;
        this.ModifiedBy = userID;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void UpdatePersonalInfoDetails(int Salutation, string FirstName, string? LastName, string? MiddleName, DateOnly DOB,
        int Gender, string EmailID, string? MobileNo, string userID)
    {        
        this.Salutation = Salutation;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.MiddleName = MiddleName;
        this.DOB = DOB;
        this.Gender = Gender;
        this.EmailID = EmailID;
        this.MobileNo = MobileNo;
        this.ModifiedBy = userID;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void UpdateProfessionalDetails(string? CurrentOrganization, string? CurrentDesignation, int? CurrentProfession, 
        string? OtherProfession, string? WorkContactNo, string userID)
    {
        this.CurrentOrganization = CurrentOrganization;
        this.CurrentDesignation = CurrentDesignation;
        this.CurrentProfession = CurrentProfession;
        this.OtherProfession = OtherProfession;
        this.WorkContactNo = WorkContactNo;
        this.ModifiedBy = userID;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void UpdateContactDetails(string? AlternateEmailId, string? ResidenceContactNo,
       bool IsResidentOfDelhi, string? CurrentResidence, string? CurrentResidenceCity, string userID)
    {       
        this.AlternateEmailId = AlternateEmailId;
        this.ResidenceContactNo = ResidenceContactNo;       
        this.IsResidentOfDelhi = IsResidentOfDelhi;
        this.CurrentResidence = CurrentResidence;
        this.CurrentResidenceCity = CurrentResidenceCity;
        this.ModifiedBy = userID;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public AlumniDetailsModel() { }

    public AlumniDetailsModel(Guid AlumniId, string? DOERegistrationId, int Salutation, string FirstName, string? LastName, string? MiddleName, DateOnly DOB,
        int Gender, int RegistrationYear, int ExitYear, string? BranchId,bool BranchNotInList, string? OtherBranchName, 
        string EmailID, string? AlternateEmailId, string? MobileNo, string? CurrentOrganization, string? CurrentDesignation,
        string? CurrentResidence, string? ResidenceContactNo, string? WorkContactNo,
        string? CurrentResidenceCity, int? CurrentProfession, string? OtherProfession, bool IsResidentOfDelhi, bool IsActive)
    {
        this.AlumniId = AlumniId;
        this.DOERegistrationId = DOERegistrationId;
        this.Salutation = Salutation;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.MiddleName = MiddleName;
        this.DOB = DOB;
        this.Gender = Gender;
        this.RegistrationYear = RegistrationYear;
        this.ExitYear = ExitYear;
        this.BranchId = BranchId;
        this.BranchNotInList = BranchNotInList;
        this.OtherBranchName = OtherBranchName;
        this.EmailID = EmailID;
        this.AlternateEmailId = AlternateEmailId;
        this.CurrentOrganization = CurrentOrganization;
        this.CurrentDesignation = CurrentDesignation;
        this.CurrentResidence = CurrentResidence;
        this.ResidenceContactNo = ResidenceContactNo;
        this.WorkContactNo = WorkContactNo;
        this.MobileNo = MobileNo;       
        this.CurrentResidenceCity = CurrentResidenceCity;
        this.CurrentProfession = CurrentProfession;
        this.OtherProfession = OtherProfession;
        this.IsResidentOfDelhi = IsResidentOfDelhi;
        this.IsActive = IsActive;
        this.IsEmailVerified = false;
        this.ShowOnHomePage = false;       
    }
}
