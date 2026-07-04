namespace ShopSphere.Security
{
    public class EncryptionUtil : IEncryptionUtil
    {
        public string Encrypt(string plainText)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainText);
        }

        public bool Verify(string plainText, string hashedText)
        {
            return BCrypt.Net.BCrypt.Verify(plainText, hashedText);
        }
    }
}
