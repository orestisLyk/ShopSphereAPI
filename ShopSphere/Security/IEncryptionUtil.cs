namespace ShopSphere.Security
{
    public interface IEncryptionUtil
    {
        string Encrypt(string plainText);
        bool Verify(string plainText, string hashedText);
    }
}
