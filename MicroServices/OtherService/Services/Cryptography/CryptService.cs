namespace Services
{
    public class CryptService : ICryptService
    {

        private readonly IConfiguration _configuration;

        public CryptService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generate a cookie signature
        /// </summary>
        /// <param name="cookie">cookie value</param>
        /// <returns>hash</returns>
        public string GenerateSignature(string value)
        {
            var secretKey = _configuration.GetSection("AppSettings:Secret").Value;
            if(secretKey is null)
            {
                throw new Exception("AppSettings secret is null");
            }

            using(var hmac = new System.Security.Cryptography.HMACSHA512(System.Text.Encoding.UTF8.GetBytes(secretKey)))
            {
                var signature = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));
                return Convert.ToBase64String(signature);
            }
        }

        /// <summary>
        /// verify cookie signature hash
        /// </summary>
        /// <param name="signature">cookie signature</param>
        /// <param name="cookie value">value of the cookie</param>
        /// <returns>boolean</returns>
        public bool VerifyCookie(string value, string signature)
        {
            var expectedSignature = GenerateSignature(value);
            return signature == expectedSignature;
        }
    }
}