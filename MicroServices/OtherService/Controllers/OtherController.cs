using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [ApiController]
    [Route("api/otherservice/[controller]")]
    public class OtherController : ControllerBase
    {
        [HttpGet("GetTest")]
        public ActionResult Get()
        {
            return Ok("test");
        }
    }
}