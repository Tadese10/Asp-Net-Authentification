namespace Services
{
    public interface ICryptService
    {
        string DecryptSession(string dataToDecrypt);
        string EncryptSession(int sessionId);

        int ExtractSessionId(string cookieValue);
    }
}