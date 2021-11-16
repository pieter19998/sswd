using Microsoft.AspNetCore.Identity;

namespace Core
{
    public class User : IdentityUser
    {
        [PersonalData] public int UserId { get; set; }
        [PersonalData] public bool Active { get; set; } = true;
    }
}