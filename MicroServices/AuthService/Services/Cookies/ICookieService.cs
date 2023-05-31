namespace Services
{
    public interface ICookieService
    {
        string GenerateSignature(string value);
        bool VerifyCookie(string value, string signature);
    }
}