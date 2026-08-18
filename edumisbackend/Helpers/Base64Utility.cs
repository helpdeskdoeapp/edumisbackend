using System.Text;

namespace edumisbackend.Helpers;

public static class Base64Utility
{
    public static string Encode(string plainText)
    {
        byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    public static string Decode(string base64EncodedData)
    {
        try
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        catch (FormatException ex)
        {
            // Log the error or handle it appropriately
            // For debugging purposes, you can also print the problematic input:
            // Console.WriteLine($"Invalid Base64 string: {base64EncodedData}");
            throw; // Rethrow the exception to propagate it further if needed
        }
    }
}
