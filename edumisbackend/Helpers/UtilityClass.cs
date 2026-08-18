namespace edumisbackend.Helpers;

public static class UtilityClass
{
    public static string[] AllowedExtensions { get; } = { ".pdf", ".PDF", ".png", ".PNG", ".jpg", ".JPG", ".jpeg", ".JPEG" };
    public static string[] AllowedPDFExtensions { get; } = { ".pdf", ".PDF" };
    public static string[] AllowedImageExtensions { get; } = { ".png", ".PNG", ".jpg", ".JPG", ".jpeg", ".JPEG" };
    public static string[] AllowedImageMimeTypes { get; } = { "image/png", "image/jpeg", "image/jpg" };
    public static string[] AllowedMimeTypes { get; } = { "application/pdf", "image/png", "image/jpeg", "image/jpg" };
    public static string[] AllowedPDFMimeTypes { get; } = { "application/pdf" };
    public static string[] AllValidExtensions { get; } = { ".pdf", ".PDF", ".png", ".PNG", ".jpg", ".JPG", ".jpeg", ".JPEG", ".doc", ".DOC", ".docx", ".DOCX", ".xls", ".XLS", ".xlsx", ".XLSX", ".ppt", ".PPT", ".pptx", ".PPTX", ".txt", ".TXT" };
    public static string[] AllValidMimeTypes { get; } = { "application/pdf", "image/png", "image/jpeg", "image/jpg", "application/msword", "application/vnd.ms-excel", "application/vnd.ms-powerpoint", "text/plain" };

    public static string NEWS = "news";
    public static string EVENTS = "events";
    public static string GRIEVANCE = "grievance";
    public static string INCOME = "income";
    public static string EXPENSE = "expense";
    public static string CIRCULARS = "circulars";
    public static string TENDERS = "tenders";
    public static string BIOCHEMICALPROFILE_TESTS = "biochemicalprofile_tests";
    public static string ACHIEVEMENT = "achievement";
    public static string EMPLOYEE_DOCS = "employeedocs";
    public static string REGISTRATIONS = "registrations";
    public static string EMAILS = "emails";
    public static string TRAININGSCHEDULE = "trainingschedule";
    public static string LESSONPLAN = "lessonplan";
    public static string GALLERY = "gallery";
    public static string TRAININGASSESSMENT = "trainingassessment";

}
