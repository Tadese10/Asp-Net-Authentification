namespace Services
{
    public interface ICryptService
    {
        string GenerateSignature(string value);
        bool VerifyCookie(string value, string signature);
    }
}