using Microsoft.AspNetCore.Identity;

namespace University_system.Models
{
    public class User : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
