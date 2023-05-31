using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("api/authservice/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ISessionService _sessionService;
        private readonly ICookieService _cookieService;

        public AuthController(ISessionService sessionService, ICookieService cookieService)
        {
            _sessionService = sessionService;
            _cookieService = cookieService;
        }

        /// <summary>
        /// Create session controller (hanlde session and credentials)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>SessionDtos</returns>
        [HttpGet("CreateSession/{userId}")]
        public async Task<ActionResult<ServiceResponse<SessionDto>>> CreateSession(int userId)
        {
            var response = await _sessionService.CreateSession(userId, HttpContext.Request.Cookies["SessionCookie"]!);

            if(!response.Success)
            {
                return NotFound(response.Message);
            }

            if(response.Data is null)
            {
                return NotFound(response.Message);
            }

            //generating cookie signature
            var sessionCookieSignature = _cookieService.GenerateSignature(response.Data.SessionId);
            var tokenCookieSignature = _cookieService.GenerateSignature(response.Data.Token);

            //preparing cookie values
            response.Data.Token = response.Data.Token + ".JTW" + tokenCookieSignature;
            response.Data.SessionId = response.Data.SessionId + ".JTW" + sessionCookieSignature;

            return Ok(response);
        }

        /// <summary>
        /// Controller used to know if user authentification are still valid or if he's simply not connected
        /// </summary>
        /// <param>no params</param>
        /// <returns>boolean succes value</returns>
        [HttpGet("isConnected")]
        public async Task<ActionResult<ServiceResponse<bool>>> IsUserConnected()
        {
            var response = await _sessionService.IsConnected(HttpContext.Request.Cookies["session_cookie"]!);

            if(!response.Success)
            {
                Response.Cookies.Delete("session_cookie");
                Response.Cookies.Delete("access_cookie");
                return Ok(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Controller used to deconnect user and delete sessions credentials
        /// </summary>
        /// <param>no params</param>
        /// <returns>Logout user and delete his session credentials</returns>
        [HttpPost("Logout")]
        public async Task<ActionResult<ServiceResponse<bool>>> Logout()
        {
            var session = await _sessionService.GetUserInformation(HttpContext.Request.Cookies["session_cookie"]!);

            if(!session.Success)
            {
                return NotFound();
            }

            else if(session.Data is null)
            {
                return NotFound();
            }

            Response.Cookies.Delete("session_cookie");
            Response.Cookies.Delete("access_cookie");

            return Ok(await _sessionService.DeleteSession(session.Data.UserId));
        }
    }
}