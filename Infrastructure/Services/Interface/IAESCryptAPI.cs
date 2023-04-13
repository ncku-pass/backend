namespace Infrastructure.Services.Interface
{
    public interface IAESCryptAPI
    {
        string Encrypt(string input);
        string Decrypt(string input);
        string GenerateRandomString(int length);
    }
}
