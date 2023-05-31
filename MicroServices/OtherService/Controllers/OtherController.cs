using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/userservice/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly ICryptService _cryptService;

        public UserController(IUserService userService, ICryptService cryptService)
        {
            _userService = userService;
            _cryptService = cryptService;
        }


        /// <summary>
        /// Get a user account informations based on his username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("GetUserByUsername/{username}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetUserByUsername(string username)
        {
            var response = await _userService.GetUserByUsername(username);

            if(!response.Success)
            {
                if(response.Message == "missing type")
                {
                    return BadRequest(response.Message);
                }

                return NotFound(response.Message);
            }

            return Ok(response);
        }
        
        /// <summary>
        /// Get my profile informations
        /// </summary>
        /// <returns>User personnal profile informations (GetUserDto)</returns>
        [HttpGet("GetMyProfile")]
        public async Task<ActionResult<ServiceResponse<int>>> GetMyProfile()
        {
            //get userId with JWT
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

            var response = await _userService.GetMyProfile(userId);

            if(!response.Success)
            {
                if(response.Message == "missing type")
                {
                    return BadRequest(response.Message);
                }

                return NotFound(response.Message);
            }

            return Ok(response);
        }
    }
}