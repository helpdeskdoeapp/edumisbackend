using System.ComponentModel.DataAnnotations.Schema;

namespace edumis.Models.Alumni.Members;

[Table("tbalm_infoshare_permissions")]
public class AlumniInformationShareModel : BaseEntity<long>
{
    [Column("alumni_id", TypeName = "uuid")]
    public Guid AlumniID { get; set; } = default!;

    [Column("emailid")]
    public bool EmailID { get; set; }

    [Column("mobileno")]
    public bool MobileNo { get; set; }

    [Column("current_Org")]
    public bool CurrentOrganisation { get; set; }

    [Column("current_designation")]
    public bool CurrentDesignation { get; set; }

    [Column("current_residence")]
    public bool CurrentResidence { get; set; }

    [Column("residence_contactno")]
    public bool ResidenceContactNo { get; set; }

    [Column("work_contactno")]
    public bool WorkContactNo { get; set; }

    [Column("current_residence_city")]
    public bool CurrentResidenceCity { get; set; }

    [Column("current_profession")]
    public bool CurrentProfession { get; set; }

    [Column("show_on_home")]
    public bool ShowOnHomepage { get; set; }

    public AlumniDetailsModel AlumniDetailNavigation { get; private set; } = default!;

    public AlumniInformationShareModel() { }

    public AlumniInformationShareModel(Guid AlumniID, bool EmailID, bool MobileNo, bool CurrentOrganisation, bool CurrentDesignation,
       bool CurrentResidence, bool ResidenceContactNo, bool WorkContactNo, bool CurrentResidenceCity, bool CurrentProfession)
    {
        this.AlumniID = AlumniID;
        this.EmailID = EmailID;
        this.MobileNo = MobileNo;
        this.CurrentOrganisation = CurrentOrganisation;
        this.CurrentDesignation = CurrentDesignation;
        this.CurrentResidence = CurrentResidence;
        this.ResidenceContactNo = ResidenceContactNo;
        this.WorkContactNo = WorkContactNo;
        this.CurrentResidenceCity = CurrentResidenceCity;
        this.CurrentProfession = CurrentProfession;
        this.CreatedBy = AlumniID.ToString();
        this.ModifiedBy = AlumniID.ToString();
    }

    public void SetEmailPermissionStatus(bool status, string userId)
    {
        this.EmailID = status;        
        this.ModifiedBy = userId;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void SetMobileNoPermissionStatus(bool status, string userId)
    {
        this.MobileNo = status;
        this.ModifiedBy = userId;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void SetCurrentOrganisationPermissionStatus(bool status, string userId)
    {
        this.CurrentOrganisation = status;
        this.ModifiedBy = userId;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void SetCurrentDesignationPermissionStatus(bool status, string userId)
    {
        this.CurrentDesignation = status;
        this.ModifiedBy = userId;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void SetCurrentResidencePermissionStatus(bool status, string userId)
    {
        this.CurrentResidence = status;
        this.ModifiedBy = userId;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void SetResidenceContactNoPermissionStatus(bool status, string userId)
    {
        this.ResidenceContactNo = status;
        this.ModifiedBy = userId;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void SetWorkContactNoPermissionStatus(bool status, string userId)
    {
        this.WorkContactNo = status;
        this.ModifiedBy = userId;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void SetCurrentResidenceCityPermissionStatus(bool status, string userId)
    {
        this.CurrentResidenceCity = status;
        this.ModifiedBy = userId;
        this.ModifiedDate = DateTime.UtcNow;
    }

    public void SetCurrentProfessionPermissionStatus(bool status, string userId)
    {
        this.CurrentProfession = status;
        this.ModifiedBy = userId;
        this.ModifiedDate = DateTime.UtcNow;
    }
}
