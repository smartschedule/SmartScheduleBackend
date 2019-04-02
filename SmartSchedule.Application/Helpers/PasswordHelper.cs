namespace SmartSchedule.Application.Helpers
{
    using System.Security.Cryptography;

    public static class PasswordHelper
    {
        private const int SALT_BYTE_SIZE = 16;
        private const int HASH_BYTE_SIZE = 16;
        private const int PBKDF2_ITERATIONS = 40000;

        public static string CreateHash(string password)
        {
            byte[] salt;
            using (RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider())
            {
                salt = new byte[SALT_BYTE_SIZE];
                csprng.GetBytes(salt);
            }

            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
            return new HashedPassword(salt, hash).ToSaltedPassword();
        }

        public static bool ValidatePassword(string password, string saltedPassword)
        {
            var correctPassword = new HashedPassword(saltedPassword);
            var testHash = PBKDF2(password, correctPassword.SaltToArray(), PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
            return ConstantTimeEquals(correctPassword.HashToArray(), testHash);
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return pbkdf2.GetBytes(outputBytes);
            }
        }

        private static bool ConstantTimeEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }
    }
}
