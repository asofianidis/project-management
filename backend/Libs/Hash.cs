using System.Security.Cryptography;
using System.Text;

namespace backend.Libs;

public class Hash
{
    public static string ComputeSHA256Hash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Convert the input string to a byte array and compute the hash
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            // Convert the byte array to a hexadecimal string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2")); // "x2" formats the byte as a hexadecimal string
            }

            return builder.ToString();
        }
    }
}