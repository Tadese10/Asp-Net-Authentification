using System.Security.Cryptography;
using System.Text;

namespace Services
{
    public class CryptService : ICryptService
    {
        public IConfiguration _configuration;

        public CryptService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Delete existant sessions
        /// </summary>
        /// <param name="SessionCookie">cookie value</param>
        /// <returns>Return session id</returns>
        public int ExtractSessionId(string cookieValue)
        {
            var cookieParts = cookieValue.Split(".JTW");
            var value = DecryptSession(cookieParts[0]);

            return int.Parse(value);
        }

        /// <summary>
        /// Encrypt session Id before putting it into cookie
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns>Encrypt session id</returns>
        public string EncryptSession(int sessionId)
        {
            var secretKey = System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SessionAESKey").Value!);
            var secretIv = System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SessionAESIv").Value!);

            //
            var textToEncrypt = sessionId.ToString();

            if (secretIv is null || secretKey is null)
            {
                throw new NullReferenceException("Appsettings secret is null");
            }

                byte[] encryptedBytes;

            using (Aes aes = Aes.Create())
            {
                aes.Key = secretKey;
                aes.IV = secretIv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] textBytes = Encoding.UTF8.GetBytes(textToEncrypt);

                encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Decrypt sessionId
        /// </summary>
        /// <param name="dataToDecrypt"></param>
        /// <returns>decrypted session id used to do queries</returns>
        public string DecryptSession(string dataToDecrypt)
        {

            var secretKey = System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SessionAESKey").Value!);
            var secretIv = System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SessionAESIv").Value!);

            if (secretIv is null || secretKey is null)
            {
                throw new NullReferenceException("Appsettings secret is null");
            }

            byte[] decryptedBytes;

            using (Aes aes = Aes.Create())
            {
                aes.Key = secretKey;
                aes.IV = secretIv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] textBytes = Convert.FromBase64String(dataToDecrypt);

                decryptedBytes = decryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}