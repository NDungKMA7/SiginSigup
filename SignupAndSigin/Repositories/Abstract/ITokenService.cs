using SignupAndSigin.Models.DTO;
using System.Security.Claims;

namespace SignupAndSigin.Repositories.Abstract
{
    public interface ITokenService
    {
        TokenResponse getToken(IEnumerable<Claim> claims);
        string GetRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
