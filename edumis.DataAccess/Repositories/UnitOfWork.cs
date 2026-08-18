using edumis.DataAccess.IRepositories;
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
using edumis.DataAccess.Repositories.Alumni;
using edumis.DataAccess.Repositories.Circulars;
using edumis.DataAccess.Repositories.Communication;
using edumis.DataAccess.Repositories.Employees;
using edumis.DataAccess.Repositories.Events;
using edumis.DataAccess.Repositories.Global;
using edumis.DataAccess.Repositories.Leave;
using edumis.DataAccess.Repositories.Library.Books;
using edumis.DataAccess.Repositories.Library.Magazine;
using edumis.DataAccess.Repositories.Library.Newspaper;
using edumis.DataAccess.Repositories.Masters;
using edumis.DataAccess.Repositories.MISC;
using edumis.DataAccess.Repositories.News;
using edumis.DataAccess.Repositories.SMC;
using edumis.DataAccess.Repositories.Tenders;
using edumis.DataAccess.Repositories.Users;
using edumis.DataAccess.Repositories.Web;


namespace edumis.DataAccess.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDBContext dBContext;

    #region Global Repos       
    public ICodesRepo Codes { get; private set; }
    public ICodeValuesRepo CodeValues { get; private set; }
    public IExceptionHandlerRepo ExceptionHandler { get; private set; }
    public ISessionInfo AcademicSessions { get; private set; }
    public IDesignationUserTypeMapping DesignationUserTypeMapping { get; private set; }
    public IMenusRepo MenusRepo { get; private set; }
    public IDesignationMenuItems DesignationMenuItems { get; private set; }
    public IVisitorCounterRepo VisitorCounterRepo { get; private set; }
    public IAlumniVisitorCounterRepo AlumniVisitorCounterRepo { get; private set; }
    #endregion

    #region Master Repos
    public IDesignationRepo Designations { get; private set; }
    public IBranchRepo BranchRepo { get; private set; }
    public IInfrastructureRepo Infrastructures { get; private set; }
    public IPostsRepo PostsMaster { get; private set; }
    public IDistrictRepo DistrictRepo { get; private set; }
    public IZoneRepo ZoneRepo { get; private set; }
    public IAcademicClassesRepo AcademicClassesRepo { get; private set; }
    public IAcademicSubjectsRepo AcademicSubjectsRepo { get; private set; }
    #endregion

    #region Employes Repo
    public IEmployeeRepo EmployeesRepo { get; private set; }
    public IEmployeeEducationRepo EmployeeEducationRepo { get; private set; }
    public IEmployeeAchievementRepo EmployeeAchievementRepo { get; private set; }
    public IEmployeeExperienceRepo EmployeeExperienceRepo { get; private set; }
    public IEmployeeAppointmentRepo EmployeeAppointmentRepo { get; private set; }
    #endregion

    #region Circulars & Tenders
    public ITendersRepo Tenders { get; private set; }
    public ICircular Circular { get; private set; }
    #endregion

    #region Users Repo
    public IUserRepo Users { get; private set; }
    public IRefreshTokenRepo RefreshTokenRepo { get; private set; }
    public IUserActivityLogsRepo UserActivityLogs { get; private set; }
    #endregion

    #region Web Page Repos
    //public IContactsRepo Contacts { get; private set; }
    public INewsRepo NewsRepo { get; private set; }
    public IEventsRepo EventsRepo { get; private set; }
    #endregion

    #region SMC App Repos      
    public ISMCMemberRegistrationsRepo SMCMemberRegistrationsRepo { get; private set; }

    public IMeetingRepo MeetingRepo { get; private set; }

    public ISMCUserRepo SMCUserRepo { get; private set; }       

    public IMeetingAgendaRepo MeetingAgendaRepo { get; private set; }

    public IMeetingAttachmentsRepo MeetingAttachmentsRepo { get; private set; }

    public IMeetingHistoryRepo MeetingHistoryRepo { get; private set; }

    public IMeetingResolutionsRepo MeetingResolutionsRepo { get; private set; }
    public IMemberDeviceTokensRepo MemberDeviceTokensRepo { get; private set; }
    public ISMCFundTransactionsRepo SMCFundTransactionsRepo { get; private set; }
    public ISMCTransactionAttachmentsRepo SMCTransactionAttachmentsRepo { get; private set; }
    public ISmcBudgetHistoryRepo SmcBudgetHistoryRepo { get; }
    public ISmcBudgetRepo SmcBudgetRepo { get; }
    public ISmcRefreshTokenRepo SmcRefreshTokenRepo { get; }
    
    
    #endregion

    #region Communication Repos
    public ISMSSettingsRepo SMSSettingsRepo { get; private set; }
    public ISMSTemplatesRepo SMSTemplatesRepo { get; private set; }
    public IOTPSentRepo OTPSentRepo { get; private set; }
    #endregion

    #region Library Repos
    public IBookDetailsRepo BookDetailsRepo { get; private set; }
    public IBookReviewsRepo BookReviewsRepo { get; private set; }
    public IBookProcurementTransactionRepo BookProcurementTransactionRepo { get; private set; }
    public IBookCatalogueRepo BookCatalogueRepo { get; private set; }
    public IMagazineRepo MagazineRepo { get; private set; }
    public IMagazineProcurementTransactionRepo MagazineProcurementTransactionRepo { get; private set; }
    public INewspaperRepo NewspaperRepo { get; private set; }
    #endregion

    #region Leave Repos
    public ILeaveApplicationRepo LeaveApplicationRepo { get; private set; }
    public ILeaveApplicationTrackRepo LeaveApplicationTrackRepo { get; private set; }
    public ILeaveRegisterRepo LeaveRegisterRepo { get; private set; }
    public ILeaveRegisterTrackRepo LeaveRegisterTrackRepo { get; private set; }
    #endregion

    #region Alumni Repos
    public IAlumniDetailsRepo AlumniDetailsRepo { get; private set; }
    public IAlumniInformationShareRepo AlumniInformationShareRepo { get; private set; }
    public IAlumniLoginRepo AlumniLoginRepo { get; private set; }
    public IAlumniRefreshTokensRepo AlumniRefreshTokensRepo { get; private set; }
    #endregion

    #region Misc Repos
    public ISwachhBharatImagesRepo SwachhBharatImagesRepo { get; private set; }
    #endregion

    #region Web Repos
    public IMarqueeDetailsRepo MarqueeDetailsRepo { get; private set; }
    #endregion

    public UnitOfWork(ApplicationDBContext dBContext)
    {
        this.dBContext = dBContext;

        #region Global Repos Initialization      
        Codes = new CodesRepo(dBContext);
        CodeValues = new CodeValuesRepo(dBContext);
        ExceptionHandler = new ExceptionHandlerRepo(dBContext);
        AcademicSessions = new SessionInfoRepo(dBContext);
        DesignationUserTypeMapping = new DesignationUserTypeMappingRepo(dBContext);
        MenusRepo = new MenusRepo(dBContext);
        DesignationMenuItems = new DesignationMenuItemsRepo(dBContext);
        VisitorCounterRepo = new VisitorCounterRepo(dBContext);
        AlumniVisitorCounterRepo = new AlumniVisitorCounterRepo(dBContext);
        #endregion

        #region Master Repos Initialization
        Designations = new DesignationRepo(dBContext);
        BranchRepo = new BranchRepo(dBContext);
        Infrastructures = new InfrastructureRepo(dBContext);
        PostsMaster = new PostsRepo(dBContext);
        DistrictRepo = new DistrictRepo(dBContext);
        ZoneRepo = new ZoneRepo(dBContext);
        AcademicClassesRepo = new AcademicClassesRepo(dBContext);
        AcademicSubjectsRepo = new AcademicSubjectsRepo(dBContext);
        #endregion

        #region Employees Repos Initialization
        EmployeesRepo = new EmployeeRepo(dBContext);
        EmployeeEducationRepo = new EmployeeEducationRepo(dBContext);
        EmployeeAchievementRepo = new EmployeeAchievementRepo(dBContext);
        EmployeeExperienceRepo = new EmployeeExperienceRepo(dBContext);
        EmployeeAppointmentRepo = new EmployeeAppointmentRepo(dBContext);
        #endregion

        #region Users Repos Initialization
        Users = new UserRepo(dBContext);
        RefreshTokenRepo = new RefreshTokenRepo(dBContext);
        UserActivityLogs = new UserActivityLogsRepo(dBContext);
        #endregion

        #region Circulars & Tenders
        Tenders = new TendersRepo(dBContext);
        Circular = new CircularRepo(dBContext);
        #endregion

        #region Web Pages Repos Initialization
        //Contacts = new ContactsRepo(dBContext);
        NewsRepo = new NewsRepo(dBContext);
        EventsRepo = new EventsRepo(dBContext);
        #endregion

        #region SMC App Repos
        SMCUserRepo = new SMCUserRepo(dBContext);
        SMCMemberRegistrationsRepo = new SMCMemberRegistrationsRepo(dBContext);
        MeetingRepo = new MeetingRepo(dBContext);
        MeetingAgendaRepo = new MeetingAgendaRepo(dBContext);
        MeetingAttachmentsRepo = new MeetingAttachmentsRepo(dBContext);
        MeetingHistoryRepo = new MeetingHistoryRepo(dBContext);
        MeetingResolutionsRepo = new MeetingResolutionsRepo(dBContext);
        MemberDeviceTokensRepo = new MemberDeviceTokensRepo(dBContext);
        SMCFundTransactionsRepo = new SMCFundTransactionsRepo(dBContext);
        SMCTransactionAttachmentsRepo = new SMCTransactionAttachmentsRepo(dBContext);
        SmcBudgetHistoryRepo = new SmcBudgetHistoryRepo(dBContext);
        SmcBudgetRepo = new SmcBudgetRepo(dBContext);
        SmcRefreshTokenRepo = new SmcRefreshTokenRepo(dBContext);
        
        
        #endregion

        #region Communication Repos 
        SMSSettingsRepo = new SMSSettingsRepo(dBContext);
        SMSTemplatesRepo = new SMSTemplatesRepo(dBContext);
        OTPSentRepo = new OTPSentRepo(dBContext);
        #endregion

        #region Library Repos Initialization
        BookDetailsRepo = new BookDetailsRepo(dBContext);
        BookReviewsRepo = new BookReviewsRepo(dBContext);
        BookProcurementTransactionRepo = new BookProcurementTransactionRepo(dBContext);
        BookCatalogueRepo = new BookCatalogueRepo(dBContext);
        MagazineRepo = new MagazineRepo(dBContext);
        MagazineProcurementTransactionRepo = new MagazineProcurementTransactionRepo(dBContext);
        NewspaperRepo = new NewspaperRepo(dBContext);
        #endregion

        #region Leave Repos Initialization
        LeaveApplicationRepo = new LeaveApplicationRepo(dBContext);
        LeaveApplicationTrackRepo = new LeaveApplicationTrackRepo(dBContext);
        LeaveRegisterRepo = new LeaveRegisterRepo(dBContext);
        LeaveRegisterTrackRepo = new LeaveRegisterTrackRepo(dBContext);
        #endregion

        #region Alumni Repos Initialization
        AlumniDetailsRepo = new AlumniDetailsRepo(dBContext);
        AlumniInformationShareRepo = new AlumniInformationShareRepo(dBContext);
        AlumniLoginRepo = new AlumniLoginRepo(dBContext);
        AlumniRefreshTokensRepo = new AlumniRefreshTokensRepo(dBContext);
        #endregion

        #region Misc Repos Initialization
        SwachhBharatImagesRepo = new SwachhBharatImagesRepo(dBContext);
        #endregion

        #region Web Repos
        MarqueeDetailsRepo = new MarqueeDetailsRepo(dBContext);
        #endregion
    }

    public void Dispose()
    {
        dBContext.Dispose();
    }

    public async Task Save()
    {
        await dBContext.SaveChangesAsync();
    }
}
