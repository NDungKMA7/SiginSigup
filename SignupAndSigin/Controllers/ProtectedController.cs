using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SignupAndSigin.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    [Authorize]
    public class ProtectedController : ControllerBase
    {
        public IActionResult getData()
        {
            return Ok("Data from Protected");
        }
    }
}
