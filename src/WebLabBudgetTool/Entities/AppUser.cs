using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebLabBudgetTool.Entities
{
    public class AppUser : IdentityUser<int>
    {
    }

    public class AppRole : IdentityRole<int> { }
    public class AppUserClaim : IdentityUserClaim<int> { }
    public class AppUserRole : IdentityUserRole<int> { }

    public class AppUserLogin : IdentityUserLogin<int>
    {
        public int Id { get; set; }
    }
    public class AppRoleClaim : IdentityRoleClaim<int> { }
    public class AppUserToken : IdentityUserToken<int>
    {
        public int Id { get; set; }
    }
}
