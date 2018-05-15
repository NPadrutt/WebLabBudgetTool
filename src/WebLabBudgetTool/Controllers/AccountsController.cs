using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebLabBudgetTool.DataService;
using WebLabBudgetTool.DataTransferObjects;
using WebLabBudgetTool.Entities;

namespace WebLabBudgetTool.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController
    {
        private readonly IAccountDataService accountDataService;

        public AccountsController(IAccountDataService accountDataService)
        {
            this.accountDataService = accountDataService;
        }

        // GET api/Accounts
        [HttpGet]
        public async Task<IEnumerable<AccountDto>> Get()
        {
            return Mapper.Map<List<AccountDto>>(await accountDataService.GetAllAccounts());
        }

        // GET api/Accounts/5
        [HttpGet("{id}")]
        public async Task<AccountDto> Get(int id)
        {
            return Mapper.Map<AccountDto>(await accountDataService.GetById(id));
        }

        // POST api/Accounts
        [HttpPost]
        public async Task Post([FromBody]AccountDto value)
        {
            var account = Mapper.Map<Account>(value);
            account.Id = 0;

            await accountDataService.SaveAccount(account);
        }

        // PUT api/Accounts/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]AccountDto value)
        {
            var account = Mapper.Map<Account>(value);
            account.Id = id;

            await accountDataService.SaveAccount(account);
        }

        // DELETE api/Accounts/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await accountDataService.DeleteAccount(id);
        }
    }
}
