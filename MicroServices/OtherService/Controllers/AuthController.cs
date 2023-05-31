using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/public/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        /// <summary>
        /// Register a new user and log him
        /// </summary>
        /// <param name="request"></param>
        /// <returns>SessionDto</returns>
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<SessionDto>>> Register(RegisterDto request)
        {
            var response = await _authService.Register(
                new User {
                    Username = request.Username,
                    Email = request.Email,
                }, request.Password
            );

            if(!response.Success)
            {
                if(response.Message == "User already exist...")
                {
                    return Conflict(response);
                }

                return NotFound(response);
            }

            
            if(response.Data is null)
            {
                return BadRequest( response.Message);
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7),
            };

            //send token cookie
            Response.Cookies.Append("access_cookie", response.Data.Token, cookieOptions);
            Response.Cookies.Append("session_cookie", response.Data.SessionId, cookieOptions);

            return Ok("welcome to UnlyMeet");
        }


        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Session Dtos (encrypt sessionId & token)</returns>
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<SessionDto>>> Login(LoginDto request)
        {
            var response = await _authService.Login(request.Email, request.Password);

            if(!response.Success)
            {
                if(response.Message == "ressource error")
                {
                    return NotFound(response);
                }
                else if(response.Message == "incorrect fields")
                {
                    return Unauthorized(response);
                }
                return BadRequest(response);
            }

            if(response.Data is null)
            {
                return BadRequest( response.Message);
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(7),
            };

            //send token cookie
            Response.Cookies.Append("access_cookie", response.Data.Token, cookieOptions);
            Response.Cookies.Append("session_cookie", response.Data.SessionId, cookieOptions);

            return Ok("connected");
        }
    }
}