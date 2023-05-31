using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignupAndSigin.Models.DTO;

namespace SignupAndSigin.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    [Authorize(Roles ="admin")]
    public class AdminController : ControllerBase
    {
        public IActionResult getData()
        {
            var status = new Status();
            status.StatusCode = 1;
            status.Message = "Data from admin controller";
            return Ok(status);
        }
    }
}
