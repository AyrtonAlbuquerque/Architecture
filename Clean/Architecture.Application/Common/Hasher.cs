using System.Security.Cryptography;

namespace Architecture.Application.Common
{
    public static class Hasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 500000;

        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

        public static string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

            return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
        }

        public static bool Verify(string password, string passwordHash)
        {
            var parts = passwordHash.Split('-');
            var hash = Convert.FromHexString(parts[0]);
            var salt = Convert.FromHexString(parts[1]);

            return CryptographicOperations.FixedTimeEquals(hash, Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize));
        }
    }
}