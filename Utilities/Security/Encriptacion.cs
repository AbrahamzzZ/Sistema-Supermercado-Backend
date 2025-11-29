using System.Security.Cryptography;
using System.Text;

namespace Utilities.Security
{
    public class Encriptacion
    {
        public static string EncriptarSHA256(string input)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            StringBuilder resultado = new();
            foreach (byte b in bytes)
            {
                resultado.Append(b.ToString("x2"));
            }
            return resultado.ToString();
        }
    }
}
