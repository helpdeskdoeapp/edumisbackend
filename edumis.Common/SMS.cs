using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace edumis.Common;

public static class SMS
{
    /// <summary>
    /// Method for sending single SMS.
    /// </summary>
    /// <param name="username"> Registered user name
    /// <param name="password"> Valid login password
    /// <param name="senderid">Sender ID 
    /// <param name="mobileNo"> valid Single Mobile Number 
    /// <param name="message">Message Content 
    /// <param name="secureKey">Department generate key by login to services portal
    /// <param name="templateid">templateid unique for each template message content
   
    // Method for sending single SMS.
    public static string sendSingleSMS(string username, string password, string senderid, string mobileNo, string message, string secureKey, string templateid, string sendUrl)
    {
        string encryptedPassword = encryptedPasswod(password);
        string NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim());
        string smsservicetype = "singlemsg"; //For single message.

        string Url = "username=" + HttpUtility.UrlEncode(username.Trim()) +
            "&password=" + HttpUtility.UrlEncode(encryptedPassword) +

            "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +

            "&content=" + HttpUtility.UrlEncode(message.Trim()) +

            "&mobileno=" + HttpUtility.UrlEncode(mobileNo) +

            "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
          "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) +
          "&templateid=" + HttpUtility.UrlEncode(templateid.Trim());


        Stream dataStream;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sendUrl);
        request.ProtocolVersion = HttpVersion.Version10;
        request.KeepAlive = false;
        request.ServicePoint.ConnectionLimit = 1;
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //forcing .Net framework to use TLSv1.2
               
        ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
        request.Method = "POST";

        byte[] byteArray = Encoding.ASCII.GetBytes(Url);
        request.ContentType = "application/x-www-form-urlencoded";//x-www-form-urlencoded
        request.Accept = "*/*";
        request.ContentLength = byteArray.Length;
        dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        WebResponse response = request.GetResponse();
        string Status = ((HttpWebResponse)response).StatusDescription;
        dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();

        reader.Close();
        dataStream.Close();
        response.Close();
        return responseFromServer;  
    }


    /// <summary>
    /// Method for sending bulk SMS.
    /// </summary>
    /// <param name="username"> Registered user name
    /// <param name="password"> Valid login password
    /// <param name="senderid">Sender ID 
    /// <param name="mobileNo"> valid Mobile Numbers 
    /// <param name="message">Message Content 
    /// <param name="secureKey">Department generate key by login to services portal
    /// <param name="templateid">templateid unique for each template message content

    // method for sending bulk SMS
    public static string sendBulkSMS(string username, string password, string senderid, string mobileNos, string message, string secureKey, string templateid, string sendUrl)
    {
        string encryptedPassword = encryptedPasswod(password);
        string NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim());
        string smsservicetype = "bulkmsg"; // for bulk msg
        
        string Url = "username=" + HttpUtility.UrlEncode(username.Trim()) +

         "&password=" + HttpUtility.UrlEncode(encryptedPassword) +

         "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +

         "&content=" + HttpUtility.UrlEncode(message.Trim()) +

         "&bulkmobno=" + HttpUtility.UrlEncode(mobileNos) +

         "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +

        "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) +
    "&templateid=" + HttpUtility.UrlEncode(templateid.Trim());


        Stream dataStream;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sendUrl);
        request.ProtocolVersion = HttpVersion.Version10;
        request.KeepAlive = false;
        request.ServicePoint.ConnectionLimit = 1;
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //forcing .Net framework to use TLSv1.2

        ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
        request.Method = "POST";

        byte[] byteArray = Encoding.ASCII.GetBytes(Url);
        request.ContentType = "application/x-www-form-urlencoded";//x-www-form-urlencoded
        request.Accept = "*/*";
        request.ContentLength = byteArray.Length;
        dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        WebResponse response = request.GetResponse();
        string Status = ((HttpWebResponse)response).StatusDescription;
        dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();

        reader.Close();
        dataStream.Close();
        response.Close();
        return responseFromServer;
    }


    /// <summary>
    /// method for Sending unicode..
    /// </summary>
    /// <param name="username"> Registered user name
    /// <param name="password"> Valid login password
    /// <param name="senderid">Sender ID 
    /// <param name="mobileNo"> valid Mobile Numbers 
    /// <param name="Unicodemessage">Unicodemessage Message Content 
    /// <param name="secureKey">Department generate key by login to services portal
    /// <param name="templateid">templateid unique for each template message content

    //method for Sending unicode message..
    public static string sendUnicodeSMS(string username, string password, string senderid, string mobileNos, string Unicodemessage, string secureKey, string templateid, string sendUrl)
    {
        string U_Convertedmessage = "";
        foreach (char c in Unicodemessage)
        {
            int j = (int)c;
            string sss = "&#" + j + ";";
            U_Convertedmessage = U_Convertedmessage + sss;
        }
        string encryptedPassword = encryptedPasswod(password);
        string NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), U_Convertedmessage.Trim(), secureKey.Trim());

        string smsservicetype = "unicodemsg"; // for unicode msg
        string Url = "username=" + HttpUtility.UrlEncode(username.Trim()) +
            "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
            "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
            "&content=" + HttpUtility.UrlEncode(U_Convertedmessage.Trim()) +
            "&bulkmobno=" + HttpUtility.UrlEncode(mobileNos) +
            "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
            "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) +
            "&templateid=" + HttpUtility.UrlEncode(templateid.Trim());

        Stream dataStream;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sendUrl);
        request.ProtocolVersion = HttpVersion.Version10;
        request.KeepAlive = false;
        request.ServicePoint.ConnectionLimit = 1;
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //forcing .Net framework to use TLSv1.2

        ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
        request.Method = "POST";

        byte[] byteArray = Encoding.ASCII.GetBytes(Url);
        request.ContentType = "application/x-www-form-urlencoded";//x-www-form-urlencoded
        request.Accept = "*/*";
        request.ContentLength = byteArray.Length;
        dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        WebResponse response = request.GetResponse();
        string Status = ((HttpWebResponse)response).StatusDescription;
        dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();

        reader.Close();
        dataStream.Close();
        response.Close();
        return responseFromServer;
    }

    /// <summary>
    /// Method for sending OTP MSG.
    /// </summary>
    /// <param name="username"> Registered user name
    /// <param name="password"> Valid login password
    /// <param name="senderid">Sender ID 
    /// <param name="mobileNo"> valid single  Mobile Number 
    /// <param name="message">Message Content 
    /// <param name="secureKey">Department generate key by login to services portal
    /// <param name="templateid">templateid unique for each template message content

    // Method for sending OTP MSG.
    public static string sendOTPMSG(string username, string password, string senderid, string mobileNo, string message, string secureKey, string templateid, string sendUrl)
    {
        string encryptedPassword = encryptedPasswod(password);
        string key = hashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim());
        string smsservicetype = "otpmsg"; //For OTP message.

        string Url = "username=" + HttpUtility.UrlEncode(username.Trim()) +
            "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
            "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
            "&content=" + HttpUtility.UrlEncode(message.Trim()) +
            "&mobileno=" + HttpUtility.UrlEncode(mobileNo) +
            "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
            "&key=" + HttpUtility.UrlEncode(key.Trim()) +
            "&templateid=" + HttpUtility.UrlEncode(templateid.Trim());

        Stream dataStream;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sendUrl);
        request.ProtocolVersion = HttpVersion.Version10;
        request.KeepAlive = false;
        request.ServicePoint.ConnectionLimit = 1;
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //forcing .Net framework to use TLSv1.2

        ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
        request.Method = "POST";

        byte[] byteArray = Encoding.ASCII.GetBytes(Url);
        request.ContentType = "application/x-www-form-urlencoded";//x-www-form-urlencoded
        request.Accept = "*/*";
        request.ContentLength = byteArray.Length;
        dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        WebResponse response = request.GetResponse();
        string Status = ((HttpWebResponse)response).StatusDescription;
        dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();

        reader.Close();
        dataStream.Close();
        response.Close();
        return responseFromServer;  
    }


    // Method for sending UnicodeOTP MSG.

    /// <summary>
    /// method for Sending unicode..
    /// </summary>
    /// <param name="username"> Registered user name
    /// <param name="password"> Valid login password
    /// <param name="senderid">Sender ID 
    /// <param name="mobileNo"> valid Mobile Numbers 
    /// <param name="Unicodemessage">Unicodemessage Message Content 
    /// <param name="secureKey">Department generate key by login to services portal
    /// <param name="templateid">templateid unique for each template message content

    //method for Sending unicode message..
    public static string sendUnicodeOTPSMS(string username, string password, string senderid, string mobileNos, string UnicodeOTPmsg, string secureKey, string templateid, string sendUrl)
    {
        string U_Convertedmessage = "";
        foreach (char c in UnicodeOTPmsg)
        {
            int j = (int)c;
            string sss = "&#" + j + ";";
            U_Convertedmessage = U_Convertedmessage + sss;
        }
        string encryptedPassword = encryptedPasswod(password);
        string NewsecureKey = hashGenerator(username.Trim(), senderid.Trim(), U_Convertedmessage.Trim(), secureKey.Trim());

        string smsservicetype = "unicodeotpmsg"; // for unicode msg
        string Url = "username=" + HttpUtility.UrlEncode(username.Trim()) +
            "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
            "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
            "&content=" + HttpUtility.UrlEncode(U_Convertedmessage.Trim()) +
            "&bulkmobno=" + HttpUtility.UrlEncode(mobileNos) +
            "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
            "&key=" + HttpUtility.UrlEncode(NewsecureKey.Trim()) +
            "&templateid=" + HttpUtility.UrlEncode(templateid.Trim());

        Stream dataStream;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sendUrl);
        request.ProtocolVersion = HttpVersion.Version10;
        request.KeepAlive = false;
        request.ServicePoint.ConnectionLimit = 1;
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //forcing .Net framework to use TLSv1.2

        ((HttpWebRequest)request).UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
        request.Method = "POST";

        byte[] byteArray = Encoding.ASCII.GetBytes(Url);
        request.ContentType = "application/x-www-form-urlencoded";//x-www-form-urlencoded
        request.Accept = "*/*";
        request.ContentLength = byteArray.Length;
        dataStream = request.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();

        WebResponse response = request.GetResponse();
        string Status = ((HttpWebResponse)response).StatusDescription;
        dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        string responseFromServer = reader.ReadToEnd();

        reader.Close();
        dataStream.Close();
        response.Close();
        return responseFromServer;
    }


    /// <summary>
    /// Method to get Encrypted the password 
    /// </summary>
    /// <param name="password"> password as String"
    private static string encryptedPasswod(String password)
    {
        byte[] encPwd = Encoding.UTF8.GetBytes(password);
        //static byte[] pwd = new byte[encPwd.Length];
        HashAlgorithm sha1 = HashAlgorithm.Create("SHA1");
        byte[] pp = sha1.ComputeHash(encPwd);
        // static string result = System.Text.Encoding.UTF8.GetString(pp);
        StringBuilder sb = new StringBuilder();
        foreach (byte b in pp)
        {

            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }

    /// <summary>
    /// Method to Generate hash code  
    /// </summary>
    /// <param name="secure_key">your last generated Secure_key 
    private static String hashGenerator(String Username, String sender_id, String message, String secure_key)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
        byte[] genkey = Encoding.UTF8.GetBytes(sb.ToString());
        //static byte[] pwd = new byte[encPwd.Length];
        HashAlgorithm sha1 = HashAlgorithm.Create("SHA512");
        byte[] sec_key = sha1.ComputeHash(genkey);

        StringBuilder sb1 = new StringBuilder();
        for (int i = 0; i < sec_key.Length; i++)
        {
            sb1.Append(sec_key[i].ToString("x2"));
        }
        return sb1.ToString();
    }
}