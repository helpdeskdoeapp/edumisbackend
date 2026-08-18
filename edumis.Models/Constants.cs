namespace edumis.Models;

public static class Constants
{
    public static string[] AllowedExtensions { get; } = { ".pdf", ".png", ".jpg", ".jpeg" };
    public static string[] AllowedImageExtensions { get; } = { ".png", ".jpg", ".jpeg" };
    public static string[] AllowedImageMimeTypes { get; } = { "image/png", "image/jpeg", "image/jpg" };
    public static string[] AllowedMimeTypes { get; } = { "application/pdf", "image/png", "image/jpeg", "image/jpg" };
   
    public static string CIRCULARS = "circulars";
    public static string EVENTS = "events";
    public static string TENDERS = "tenders";
    public static string NEWS = "news";
    public static string SMC_MEETINGS = "smc_meetings";
    public static string Library = "library";
    public static string SWACHH_BHARAT = "swachh_bharat";

    public static int OTHER_AGENDA_CODE = 2299;
}
