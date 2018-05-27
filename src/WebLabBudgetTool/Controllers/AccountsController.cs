using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebLabBudgetTool.DataService;
using WebLabBudgetTool.DataTransferObjects;
using WebLabBudgetTool.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebLabBudgetTool.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Cors.EnableCors("AllowEverything")]
    public class AccountsController : Controller
    {
        private readonly IAccountDataService accountDataService;
        private readonly UserManager<AppUser> userManager;

        public AccountsController(IAccountDataService accountDataService, UserManager<AppUser> userManager)
        {
            this.accountDataService = accountDataService;
            this.userManager = userManager;
        }

        private async Task<AppUser> GetCurrentUserAsync()
        {
            var userMail = User.FindFirstValue(ClaimTypes.Email);
            return await userManager.FindByEmailAsync(userMail);
        }

        // GET api/Accounts
        [HttpGet]
        public async Task<IEnumerable<AccountDto>> Get()
        {
            return Mapper.Map<List<AccountDto>>(await accountDataService.GetAllAccounts(await GetCurrentUserAsync()));
        }

        // GET api/Accounts/5
        [HttpGet("{id}")]
        public async Task<AccountDto> Get(int id)
        {
            return Mapper.Map<AccountDto>(await accountDataService.GetById(id, await GetCurrentUserAsync()));
        }

        // POST api/Accounts
        [HttpPost]
        public async Task Post([FromBody]AccountDto value)
        {
            var account = Mapper.Map<Account>(value);
            account.Id = 0;
            account.User = await GetCurrentUserAsync();

            await accountDataService.SaveAccount(account);
        }

        // PUT api/Accounts/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]AccountDto value)
        {
            var account = Mapper.Map<Account>(value);
            account.Id = id;
            account.User = await GetCurrentUserAsync();

            await accountDataService.SaveAccount(account);
        }

        // DELETE api/Accounts/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await accountDataService.DeleteAccount(id, await GetCurrentUserAsync());
        }
    }
}
