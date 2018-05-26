using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebLabBudgetTool.DataTransferObjects;
using WebLabBudgetTool.Entities;

namespace WebLabBudgetTool.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly JWTSettings options;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;

        public UserController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IOptions<JWTSettings> optionsAccessor)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            options = optionsAccessor.Value;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Credentials credentials)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser {UserName = credentials.Email, Email = credentials.Email};
                var result = await userManager.CreateAsync(user, credentials.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return new JsonResult(new Dictionary<string, object>
                    {
                        { "access_token",  GetToken(new Claim(ClaimTypes.Email, user.Email)) }
                    });
                }

                return Errors(result);
            }

            return Error("Unexpected error");
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] Credentials credentials)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(credentials.Email, credentials.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(credentials.Email);
                    return new JsonResult(new Dictionary<string, object>
                    {
                        { "access_token",  GetToken(new Claim(ClaimTypes.Email, user.Email)) }
                    });
                }
                return new JsonResult("Unable to sign in") { StatusCode = 401 };
            }
            return Error("Unexpected error");
        }

        private string GetToken(Claim claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: new[]{ claims },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JsonResult Errors(IdentityResult result)
        {
            var items = result.Errors
                              .Select(x => x.Description)
                              .ToArray();
            return new JsonResult(items) {StatusCode = 400};
        }

        private JsonResult Error(string message)
        {
            return new JsonResult(message) {StatusCode = 400};
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}