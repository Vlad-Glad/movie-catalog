using System.Text;
using System.Security.Cryptography;

namespace CatalogInfrastructure.Services
{
    public class PasswordService
    {
        //hardocoded salt - shit idea but still it works)
        private static readonly string HardcodedSalt = "YourVerySecureAndStaticSalt123!";

        public byte[] HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = password + HardcodedSalt;
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            }
        }

        public bool VerifyPassword(string password, byte[] storedHash)
        {
            var hashToCompare = HashPassword(password);
            return hashToCompare.SequenceEqual(storedHash);
        }
    }
}
