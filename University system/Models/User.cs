using Microsoft.AspNetCore.Identity;

namespace University_system.Models
{
    public class User : IdentityUser
    {

        public string Name { get; set; }

    }
}
