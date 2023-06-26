using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly HttpClient _httpClient;

        public AuthService(IMapper mapper, IConfiguration configuration, DataContext context, HttpClient httpClient)
        {
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
            _httpClient = httpClient;
        }


        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>SessionDto</returns>
        public async Task<ServiceResponse<SessionDto>> Login(string email, string password)
        {
            var response = new ServiceResponse<SessionDto>();

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));

                if(user is null)
                {
                    throw new NullReferenceException("user not found");
                }

                else if(!VerifiyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                {
                    throw new Exception("password or email incorrect");
                }

                else
                {
                    var microServiceResponse = await CallSessionManagerService($"http://localhost:5002/api/authservice/Auth/CreateSession/{user.UserId}");
                    
                    if(!microServiceResponse.Success)
                    {
                        throw new Exception("request failed");
                    }

                    if(microServiceResponse.Data is null)
                    {
                        throw new Exception("request failed");
                    }

                    response.Data = new SessionDto(microServiceResponse.Data.SessionId, microServiceResponse.Data.Token);
                }
            }
            catch(Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }


        /// <summary>
        /// Register a user 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns>SessionDto</returns>
        public async Task<ServiceResponse<SessionDto>> Register(User user, string password)
        {
            var response = new ServiceResponse<SessionDto>();

            if(await UserExists(user.Username))
            {
                response.Success = false;
                response.Message = "User already exist...";
                return response;
            }

            try 
            {
                //mdp
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var microServiceResponse = await CallSessionManagerService($"http://localhost:5002/api/authservice/Auth/CreateSession/{user.UserId}");
                
                if(!microServiceResponse.Success)
                {
                    throw new Exception("request failed");
                }

                if(microServiceResponse.Data is null)
                {
                    throw new Exception("request failed");
                }

                response.Data = new SessionDto(microServiceResponse.Data.SessionId, microServiceResponse.Data.Token);

            }
            catch(Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }

        /// <summary>
        /// Call usermicroservice controller to get informations
        /// </summary>
        /// <param name="url">url of the microService</param>
        /// <returns>SessionDto</returns>
        private async Task<ServiceResponse<SessionDto>> CallSessionManagerService(string url)
        {
            var response = new ServiceResponse<SessionDto>();

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30);

                    var apiRequest = await httpClient.GetAsync(url);

                    apiRequest.EnsureSuccessStatusCode();
                    
                    var responseBody = await apiRequest.Content.ReadAsStringAsync();

                    var responseObject = JsonConvert.DeserializeObject<ServiceResponse<SessionDto>>(responseBody);

                    if(responseObject is null)
                    {
                        throw new NullReferenceException("api not reachable");
                    }
                    else if(responseObject.Data is null)
                    {
                        throw new NullReferenceException("resource not found");
                    }

                    response.Data = responseObject.Data;
                }
            }
            catch(Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }

            return response;
        }


        /// <summary>
        /// Verufy if user exist
        /// </summary>
        /// <param name="username"></param>
        /// <returns>bool</returns>
        private async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
            {
                return true;
            }

            return false;
        }


        /*
            THESE 2 FUNCTION ABOVE ARE USED TO HASH PASSWORD ON USER REGISTER
        */
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifiyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}