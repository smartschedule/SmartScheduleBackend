namespace SmartSchedule.Application.Helpers
{
    using System;

    public class HashedPassword
    {
        private const int saltIndex = 24;

        public string Salt { get; private set; }
        public string Hash { get; private set; }

        private HashedPassword()
        {
        }

        public HashedPassword(byte[] salt, byte[] hash)
        {
            Salt = Convert.ToBase64String(salt);
            Hash = Convert.ToBase64String(hash);
        }

        public HashedPassword(string salt, string hash)
        {
            Salt = salt;
            Hash = hash;
        }

        public HashedPassword(string saltedPassword)
        {
            Salt = saltedPassword.Substring(0, saltIndex);
            Hash = saltedPassword.Substring(saltIndex);
        }

        public byte[] SaltToArray()
        {
            return Convert.FromBase64String(Salt);
        }

        public byte[] HashToArray()
        {
            return Convert.FromBase64String(Hash);
        }

        public string ToSaltedPassword()
        {
            return Salt + Hash;
        }
    }
}
