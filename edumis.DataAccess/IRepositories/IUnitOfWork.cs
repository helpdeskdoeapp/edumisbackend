using edumis.DataAccess.IRepositories.IAlumni;
using edumis.DataAccess.IRepositories.ICirculars;
using edumis.DataAccess.IRepositories.ICommunication;
using edumis.DataAccess.IRepositories.IEmployees;
using edumis.DataAccess.IRepositories.IEvents;
using edumis.DataAccess.IRepositories.IGlobal;
using edumis.DataAccess.IRepositories.ILeave;
using edumis.DataAccess.IRepositories.ILibrary.IBooks;
using edumis.DataAccess.IRepositories.ILibrary.IMagazine;
using edumis.DataAccess.IRepositories.ILibrary.INewsPaper;
using edumis.DataAccess.IRepositories.IMasters;
using edumis.DataAccess.IRepositories.IMISC;
using edumis.DataAccess.IRepositories.INews;
using edumis.DataAccess.IRepositories.ISMC;
using edumis.DataAccess.IRepositories.ITenders;
using edumis.DataAccess.IRepositories.IUsers;
using edumis.DataAccess.IRepositories.IWeb;

namespace edumis.DataAccess.IRepositories;

public interface IUnitOfWork : IDisposable
{
    #region Global Repos    
    ICodesRepo Codes { get; }
    ICodeValuesRepo CodeValues { get; }
    IExceptionHandlerRepo ExceptionHandler { get; }
    ISessionInfo AcademicSessions { get; }
    IDesignationUserTypeMapping DesignationUserTypeMapping { get; }
    IMenusRepo MenusRepo { get; }
    IDesignationMenuItems DesignationMenuItems { get; }
    IVisitorCounterRepo VisitorCounterRepo { get; }
    IAlumniVisitorCounterRepo AlumniVisitorCounterRepo { get; }
    #endregion

    #region Masters Repos
    IDesignationRepo Designations { get; }
    IBranchRepo BranchRepo { get; }
    IInfrastructureRepo Infrastructures { get; }
    IPostsRepo PostsMaster { get; }
    IDistrictRepo DistrictRepo { get; }
    IZoneRepo ZoneRepo { get; }
    IAcademicClassesRepo AcademicClassesRepo { get; }
    IAcademicSubjectsRepo AcademicSubjectsRepo { get; }
    #endregion

    #region Employees Repo
    IEmployeeRepo EmployeesRepo { get; }
    IEmployeeEducationRepo EmployeeEducationRepo { get; }
    IEmployeeAchievementRepo EmployeeAchievementRepo { get; }
    IEmployeeExperienceRepo EmployeeExperienceRepo { get; }
    IEmployeeAppointmentRepo EmployeeAppointmentRepo { get; }
    #endregion

    #region Users Repo
    IUserRepo Users { get; }
    IUserActivityLogsRepo UserActivityLogs { get; }
    IRefreshTokenRepo RefreshTokenRepo { get; }
    #endregion

    #region Circulars & Tenders
    ITendersRepo Tenders { get; }
    ICircular Circular { get; }
    #endregion

    #region Web page Repos
   // IContactsRepo Contacts { get; }
    INewsRepo NewsRepo { get; }
    IEventsRepo EventsRepo { get; }    
    #endregion

    #region SMC App Repos
    ISMCUserRepo SMCUserRepo { get; }
    ISMCMemberRegistrationsRepo SMCMemberRegistrationsRepo { get; }
    IMeetingRepo MeetingRepo {  get; }
    IMeetingAgendaRepo MeetingAgendaRepo { get; }
    IMeetingAttachmentsRepo MeetingAttachmentsRepo { get; }
    IMeetingHistoryRepo MeetingHistoryRepo { get; }
    IMeetingResolutionsRepo MeetingResolutionsRepo { get; }
    IMemberDeviceTokensRepo MemberDeviceTokensRepo { get; }
    ISMCFundTransactionsRepo SMCFundTransactionsRepo { get; }
    ISMCTransactionAttachmentsRepo SMCTransactionAttachmentsRepo { get; }
    ISmcBudgetRepo SmcBudgetRepo { get; }
    ISmcBudgetHistoryRepo SmcBudgetHistoryRepo { get; }
    ISmcRefreshTokenRepo SmcRefreshTokenRepo { get; }
    
    #endregion

    #region Communication Repos
    ISMSSettingsRepo SMSSettingsRepo { get; }
    ISMSTemplatesRepo SMSTemplatesRepo { get; }
    IOTPSentRepo OTPSentRepo { get; }
    #endregion

    #region Library Repos
    IBookDetailsRepo BookDetailsRepo { get; }
    IBookReviewsRepo BookReviewsRepo { get; }
    IBookProcurementTransactionRepo BookProcurementTransactionRepo { get; }
    IBookCatalogueRepo BookCatalogueRepo { get; }
    IMagazineRepo MagazineRepo { get; }
    IMagazineProcurementTransactionRepo MagazineProcurementTransactionRepo { get; }
    INewspaperRepo NewspaperRepo { get; }
    #endregion

    #region Leave Repos
    ILeaveApplicationRepo LeaveApplicationRepo { get; }
    ILeaveApplicationTrackRepo LeaveApplicationTrackRepo { get; }
    ILeaveRegisterRepo LeaveRegisterRepo { get; }
    ILeaveRegisterTrackRepo LeaveRegisterTrackRepo { get; }
    #endregion

    #region Alumni Repos
    IAlumniDetailsRepo AlumniDetailsRepo { get; }
    IAlumniInformationShareRepo AlumniInformationShareRepo { get; }
    IAlumniLoginRepo AlumniLoginRepo { get; }
    IAlumniRefreshTokensRepo AlumniRefreshTokensRepo { get; }
    #endregion

    #region MISC Repos
    ISwachhBharatImagesRepo SwachhBharatImagesRepo { get; }
    #endregion

    #region Web Repos
    IMarqueeDetailsRepo MarqueeDetailsRepo { get; }
    #endregion

    Task Save();       
}
