using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SignupAndSigin.Models.Domain;
using SignupAndSigin.Models.DTO;
using SignupAndSigin.Repositories.Abstract;
using System.Security.Claims;

namespace SignupAndSigin.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly ITokenService _service;

        public TokenController(MyDbContext context, ITokenService service)
        {
            _context = context;
            _service = service; 
            
        }
        [HttpPost]
        public IActionResult Refresh(RefreshTokenRequest tokenApiModel)
        {
            if (tokenApiModel == null)
            {
                return BadRequest("Invalid client request");

            }
            string accesstoken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;
            var principal = _service.GetPrincipalFromExpiredToken(accesstoken);
            var userName = principal.Identity.Name;
            var user = _context.TokenInfos.SingleOrDefault(x => x.UserName == userName);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.Now ) {
                return BadRequest("Invalid client request");
            }
            var newAccessToken = _service.getToken(principal.Claims);
            var newRefreshToken = _service.GetRefreshToken();
            _context.SaveChanges();
            return Ok(new RefreshTokenRequest()
            {
                AccessToken = newAccessToken.TokenString,
                RefreshToken = newRefreshToken
            }) ;

        }

        [HttpPost, Authorize]
        public IActionResult Revoke()
        {
            try
            {
                var userName = User.Identity.Name;
                var user = _context.TokenInfos.SingleOrDefault(u => u.UserName == userName);
                if (user == null)
                {
                    return BadRequest();
                }

                user.RefreshToken = null;
                _context.SaveChanges();

                return Ok(true);
            }
            catch(Exception ex) { 
                return BadRequest(ex.Message + "hello word");  
            }
        }
    }
}
