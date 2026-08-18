using edumis.Models.Alumni.Members;
using edumis.Models.Alumni.UserAccounts;
using edumis.Models.Circulars;
using edumis.Models.Communication;
using edumis.Models.Employees;
using edumis.Models.Events;
using edumis.Models.Global;
using edumis.Models.Leave;
using edumis.Models.Library.Books;
using edumis.Models.Library.Magazine;
using edumis.Models.Library.Newspaper;
using edumis.Models.Masters;
using edumis.Models.MISC;
using edumis.Models.News;
using edumis.Models.SMC;
using edumis.Models.Tenders;
using edumis.Models.Users;
using edumis.Models.Web;
using Microsoft.EntityFrameworkCore;

namespace edumis.DataAccess
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            /* DO NOT Specify any configurtion below these two lines. */
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDBContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        #region Global Classes       
        public DbSet<CodesModel> Codes { get; set; }
        public DbSet<CodeValuesModel> CodeValues { get; set; }
        public DbSet<ExceptionLogs> ExceptionLogs { get; set; }
        public DbSet<SessionInfoModel> AcademicSessions { get; set; }
        public DbSet<DesignationUserTypeMapping> DesignationUserTypeMappings { get; set; }
        public DbSet<MenusModel> Menus { get; set; }
        public DbSet<DesignationMenuItems> DesignationMenuItems { get; set; }
        public DbSet<VisitorCounterModel> VisitorCounter { get; set; }
        public DbSet<AlumniVisitorCounterModel> AlumniVisitorCounter { get; set; }
        #endregion

        #region Master Classes
        public DbSet<DesignationModel> Designations { get; set; }
        public DbSet<BranchesModel> Branches { get; set; }
        public DbSet<SchoolDetailsModel> SchoolDetails { get; set; }
        public DbSet<InfrastructureModel> Infrastructures { get; set; }
        public DbSet<PostsModel> PostsMaster { get; set; }
        public DbSet<DistrictsModel> Districts { get; set; }
        public DbSet<ZonesModel> Zones { get; set; }
        public DbSet<AcademicClassesModel> AcademicClasses { get; set; }
        public DbSet<AcademicSubjectsModel> AcademicSubjects { get; set; }
        #endregion

        #region Branch Employees Table
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<AppointmentModel> EmployeeAppointmentDetails { get; set; }
        public DbSet<EducationModel> EmployeeEducationDetails { get; set; }
        public DbSet<EmployeeAchievementModel> EmployeeAchievements { get; set; }
        public DbSet<EmployeeExperienceModel> EmployeeExperiences { get; set; }
        #endregion

        #region User Accounts Tables
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserActivityLogsModel> UserActivityLogs { get; set; }
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
        #endregion

        #region Circulars & Tenders Table
        public DbSet<CircularModel> Circular { get; set; }
        public DbSet<TendersModel> Tenders { get; set; }
        #endregion

        #region News & events Table
        public DbSet<NewsModel> News { get; set; }
        public DbSet<EventsModel> Events { get; set; }
        #endregion

        #region SMC App Tables 
        public DbSet<SMCAccountsModel> SMCAccounts {  get; set; }
        public DbSet<MemberRegistrationsModel> MemberRegistrations { get; set; }
        public DbSet<MeetingModel> SMCMeeting { get; set; }
        public DbSet<MeetingHistoryModel> SMCMeetingHistory { get; set; }
        public DbSet<MeetingAgendaModel> SMCMeetingAgenda { get; set; }
        public DbSet<MeetingAttachmentsModel> SMCMeetingAttachments { get; set; }
        public DbSet<MeetingResolutionsModel> SMCMeetingResolutions { get; set; }
        public DbSet<SMCFundTransactionsModel> SMCFundTransactions { get; set; }
        public DbSet<SMCTransactionAttachmentsModel> SMCTransactionAttachments { get; set; }
        public DbSet<SmcBudgetAllocationModel> SmcBudgetAllocations { get; set; }
        public DbSet<SmcBudgetAllocationHistoryModel> SmcBudgetAllocationHistory { get; set; }
        public DbSet<SmcRefreshTokenModel> SmcRefreshTokens { get; set; }
        #endregion

        #region Communication Model
        public DbSet<SMSSettingsModel> SMSSettings { get; set; }
        public DbSet<SMSTemplatesModel> SMSTemplates { get; set; }
        public DbSet<OTPSentModel> OTPSent { get; set; }
        #endregion

        #region Library Models
        public DbSet<BookDetailsModel> BookDetails { get; set; }
        public DbSet<BookReviewsModel> BookReviews { get; set; }
        public DbSet<ProcurementTransactionModel> BookProcurementTransactions { get; set; }
        public DbSet<BookCatalogueModel> BookCatalogue { get; set; }
        public DbSet<MagazineModel> Magazines { get; set; }
        public DbSet<MagazineProcurementTransactionModel> MagazineProcurementTransactions { get; set; }
        public DbSet<NewspaperModel> Newspapers { get; set; }

        #endregion

        #region Leave Models
        public DbSet<LeaveApplicationModel> LeaveApplications { get; set; }
        public DbSet<LeaveApplicationTrackModel> LeaveApplicationTrack { get; set; }
        public DbSet<LeaveRegisterModel> LeaveRegister { get; set; }
        public DbSet<LeaveRegisterTrackModel> LeaveRegisterTrack { get; set; }

        #endregion

        #region Alumni Models
        public DbSet<AlumniDetailsModel> AlumniDetails { get; set; }
        public DbSet<AlumniInformationShareModel> AlumniInformationShare { get; set; }
        public DbSet<AlumniLoginModel> AlumniLogins { get; set; }
        public DbSet<AlumniRefreshTokenModel> AlumniRefreshTokens { get; set; }
        #endregion

        #region MISC Tables
        public DbSet<SwachhBharatImagesModel> SwachhBharatImages { get; set; }
        #endregion

        #region Web Table
        public DbSet<MarqueeDetailsModels> MarqueeDetails { get; set; }
        #endregion
    }
}
