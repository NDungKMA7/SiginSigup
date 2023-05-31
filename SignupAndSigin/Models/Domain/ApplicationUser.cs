using Microsoft.AspNetCore.Identity;

namespace SignupAndSigin.Models.Domain
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }    
        
    }
}
