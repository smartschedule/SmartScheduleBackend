namespace SmartSchedule.Application.Helpers
{
    using System;

    public class HashedPassword
    {
        public int SaltByteSize { get; }
        public int SaltIndex { get; }

        public string Salt { get; private set; }
        public string Hash { get; private set; }

        private HashedPassword(int saltByteSize)
        {
            SaltByteSize = saltByteSize;
            SaltIndex = GetBase64EncodedLength(saltByteSize);
        }

        public HashedPassword(byte[] salt, byte[] hash, int saltByteSize) : this(saltByteSize)
        {
            Salt = Convert.ToBase64String(salt);
            Hash = Convert.ToBase64String(hash);
        }

        public HashedPassword(string salt, string hash, int saltByteSize) : this(saltByteSize)
        {
            Salt = salt;
            Hash = hash;
        }

        public HashedPassword(string saltedPassword, int saltByteSize) : this(saltByteSize)
        {
            Salt = saltedPassword.Substring(0, SaltIndex);
            Hash = saltedPassword.Substring(SaltIndex);
        }

        private int GetBase64EncodedLength(int byteSize)
        {
            return 4 * (int)Math.Ceiling((double)byteSize / 3);
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
