using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace edumis.Common;

public static class Utilities
{
    public static string GeneratePasswordSalt()
    {
        try
        {
            return BCrypt.Net.BCrypt.GenerateSalt(13);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static string HashPassword(string Password)
    {
        try
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(Password, 13);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static bool VerifyPassword(string UserPassword, string DBPassword)
    {
        try
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(UserPassword, DBPassword);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static void LogExcetionDetailsToTextFile(Exception pEx, string pLogFilePath)
    {

        string ErrorMsg = string.Empty;
        string ExType = string.Empty;
        string ErrorLocation = string.Empty;
        string StackTrace = string.Empty;
        string TargetSite = string.Empty;
        string InnerException = string.Empty;
        string InnerMessage = string.Empty;
        var NewLine = Environment.NewLine + Environment.NewLine;

        try
        {
            ErrorMsg = pEx.GetType().Name.ToString();
            ExType = pEx.GetType().ToString();
            ErrorLocation = pEx.Message.ToString();
            StackTrace = pEx.StackTrace.ToString();
            TargetSite = pEx.TargetSite.ToString();
            if (pEx.InnerException != null)
            {
                InnerException = pEx.InnerException.ToString();
                InnerMessage = pEx.InnerException.Message;
            }

            if (!Directory.Exists(pLogFilePath))
            {
                Directory.CreateDirectory(pLogFilePath);
            }
            pLogFilePath = pLogFilePath + "/" + DateTime.Today.ToString("dd-MM-yyyy") + ".txt";
            if (!File.Exists(pLogFilePath))
            {
                File.Create(pLogFilePath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(pLogFilePath))
            {
                string error = "Log Written Date:" + " " + DateTime.Now.ToString() + NewLine
                    + "Error Message:" + " " + ErrorMsg + NewLine
                    + "Exception Type:" + " " + ExType + NewLine
                    + "Error Location :" + " " + ErrorLocation + NewLine
                    + "Stack Trace: " + Environment.NewLine + StackTrace + NewLine
                    + "Target Site: " + Environment.NewLine + TargetSite + NewLine
                    + "Inner Exception: " + InnerException + Environment.NewLine
                    + "Inner Exception Message: " + InnerMessage + Environment.NewLine;

                sw.WriteLine("----------*Exception Details on " + " " + DateTime.Now.ToString() + " Starts*-----------------");
                sw.WriteLine("----------------------------------------------------------------------------------------------");
                sw.WriteLine(Environment.NewLine);
                sw.WriteLine(error);
                sw.WriteLine("--------------------------------*Exception Log Ends*------------------------------------------");
                sw.WriteLine(Environment.NewLine);
                sw.Flush();
                sw.Close();

            }
        }
        catch (Exception e)
        {
            e.ToString();
        }
    }

    public static string EncryptString(string pParameterValue)
    {
        try
        {
            string ReturnVal = string.Empty;

            string EncryptionKey = "Jxce8VAzLrFB7vclojOq8p8jPrlkONKQ";

            byte[] clearBytes = Encoding.Unicode.GetBytes(pParameterValue);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    ReturnVal = Convert.ToBase64String(ms.ToArray());
                }
            }

            return ReturnVal;
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    public static string DecryptString(string pParameterValue)
    {
        try
        {
            string ReturnVal = string.Empty;

            string EncryptionKey = "Jxce8VAzLrFB7vclojOq8p8jPrlkONKQ";
            pParameterValue = pParameterValue.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(pParameterValue);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey,
                    new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    ReturnVal = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return ReturnVal;
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }

    public static byte[] GenerateRandomKey(int KeyBitsSize)
    {
        int keySizeInBytes = KeyBitsSize / 8;
        byte[] key = new byte[keySizeInBytes];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(key);
        }
        return key;
    }

    public static bool IsValidPassword(string password)
    {
        // ≥8 chars, at least 1 lowercase, 1 uppercase, 1 digit, 1 special
        const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$";
        return Regex.IsMatch(password, pattern);
    }

    public static string ComputeSha256Hash(string inputStr)
    {
        using (var sha = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(inputStr);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
    
    public static bool ExistsIn<T>(this T? item, params T[] list) => item!=null && list.Contains(item);
    public static bool NotIn<T>(this T? item, params T[] list) => item==null || !list.Contains(item);
    public static bool IsNullOrBlank([NotNullWhen(false)] this string? str) => string.IsNullOrWhiteSpace(str);
    
    public static T? AsEnumOrNull<T>(this int value) where T : struct, Enum => Enum.IsDefined(typeof(T), value) ? (T)(object)value : null;
    
}

