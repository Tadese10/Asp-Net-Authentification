using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class SessionService : ISessionService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly ICryptService _cryptService;

        public SessionService(IMapper mapper, DataContext context, IConfiguration configuration, ITokenService tokenService, ICryptService cryptService)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
            _tokenService = tokenService;
            _cryptService = cryptService;
        }

        /// <summary>
        /// Function used to get user session informations based on cookies and sessionId hide inside
        /// </summary>
        /// <param name="SessionCookie">SessionCookieValue</param>
        /// <returns>Get user session informations</returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<ServiceResponse<UserDto>> GetUserInformation(string sessionCookie)
        {
            var response = new ServiceResponse<UserDto>();

            if(sessionCookie is null)
            {
                response.Success = false;
                response.Message = "no cookies founded";
            }

            else 
            {
                try{
                    //non null gerer la session; 
                    var cookieSessionId = _cryptService.ExtractSessionId(sessionCookie);

                    var dbSession = await _context.Sessions.FirstOrDefaultAsync(s => s.SessionId == cookieSessionId);

                    if(dbSession is null)
                    {
                        throw new NullReferenceException($"session id {cookieSessionId} was not found");
                    }

                    response.Data = new UserDto(dbSession.UserId);
                }
                catch(Exception e)
                {
                    response.Success = false;
                    response.Message = e.Message;
                }
            }

            return response;
        }


        /// <summary>
        /// Verifiy is user session still exist (if he's logged)
        /// </summary>
        /// <param name="SessionCookie">SessionCookie</param>
        /// <returns>True or False if user is connected</returns>
        public async Task<ServiceResponse<bool>> IsConnected(string sessionCookie)
        {
            var response = new ServiceResponse<bool>();

            if(sessionCookie is null)
            {
                response.Success = false;
                response.Message = "no cookies founded";
            }

            else 
            {
                try{
                    //non null gerer la session; 
                    var cookieSessionId = _cryptService.ExtractSessionId(sessionCookie);

                    if(!await SessionExist(cookieSessionId))
                    {
                        response.Success = false;
                        response.Data = false;
                    }

                    else{
                        response.Data = true;
                    }
                }
                catch(Exception e)
                {
                    response.Data = false;
                    response.Success = false;
                    response.Message = e.Message;
                }
            }

            return response;
        }

        /// <summary>
        /// Complete function able to check cookie and verify to create a session for user
        /// </summary>
        /// <param name="UserSessionDtos" name="SessionCookie">User session dto and cookie informations</param>
        /// <returns>Session Id or error message</returns>
        public async Task<ServiceResponse<SessionDto>> CreateSession(int userId, string sessionCookie)
        {
            var response = new ServiceResponse<SessionDto>();

            var token = _tokenService.CreateToken(userId);

            var informations = new UserDto(userId);

            if(sessionCookie is null)
            {
                try 
                {
                    var newSessionInformation = _mapper.Map<Session>(informations);
                    
                    _context.Sessions.Add(newSessionInformation);
                    await _context.SaveChangesAsync();

                    //encrypt session id
                    var encryptedId = _cryptService.EncryptSession(newSessionInformation.SessionId);

                    response.Data = new SessionDto(encryptedId, token);
                }

                catch(Exception e)
                {
                    response.Success = false;
                    response.Message = e.Message;
                }
            }

            else 
            {
                //non null gerer la session; 
                var cookieSessionId = _cryptService.ExtractSessionId(sessionCookie);

                if(await SessionExist(cookieSessionId))
                {
                    response.Data = new SessionDto(_cryptService.EncryptSession(cookieSessionId), token);
                }

                else{
                    try 
                    {
                        var newSessionInformation = _mapper.Map<Session>(informations);
                        
                        _context.Sessions.Add(newSessionInformation);
                        await _context.SaveChangesAsync();

                        //encrypt session id
                        var encryptedId = _cryptService.EncryptSession(newSessionInformation.SessionId);

                        response.Data = new SessionDto(encryptedId, token);
                    }

                    catch(Exception e)
                    {
                        response.Success = false;
                        response.Message = e.Message;
                    }
                }
            }
        
            return response;
        }

        /// <summary>
        /// Delete existant sessions in database based on userId
        /// </summary>
        /// <param name="UserId">user id</param>
        /// <returns>True or false</returns>
        public async Task<ServiceResponse<bool>> DeleteSession(int userId)
        {
            var serviceResponse = new ServiceResponse<bool>();

            try
            {
                var dbSession = await _context.Sessions.FirstOrDefaultAsync(u => u.UserId == userId);

                if(dbSession is null)
                {
                    throw new NullReferenceException($"session for user with userId : {userId}  not found");
                }

                _context.Sessions.Remove(dbSession);
                await _context.SaveChangesAsync();

                serviceResponse.Data = true;
            }

            catch(Exception e)
            {
                serviceResponse.Data = false;
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }

        /// <summary>
        /// Check if session exist in Database
        /// </summary>
        /// <param name="session id">cookie value</param>
        /// <returns>True or false</returns>
        private async Task<bool> SessionExist(int sessionId)
        {
            var session = await _context.Sessions.FirstOrDefaultAsync(s => s.SessionId == sessionId);

            if(session is null)
            {
                return false;
            }

            else
            {
                if(session.DateExpiration < DateTime.Now)
                {
                    return false;
                }

                else
                {
                    return true;
                }
            }
        }
    }
}