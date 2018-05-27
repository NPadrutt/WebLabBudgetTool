using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebLabBudgetTool.DataService;
using WebLabBudgetTool.DataTransferObjects;
using WebLabBudgetTool.Entities;

namespace WebLabBudgetTool.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Cors.EnableCors("AllowEverything")]
    public class PaymentsController : Controller
    {
        private readonly IPaymentDataService paymentDataService;
        private readonly IAccountDataService accountDataService;
        private readonly ICategoryDataService categoryDataService;
        private readonly UserManager<AppUser> userManager;

        public PaymentsController(IPaymentDataService paymentDataService, 
                                  IAccountDataService accountDataService, 
                                  ICategoryDataService categoryDataService,
                                  UserManager<AppUser> userManager)
        {
            this.paymentDataService = paymentDataService;
            this.accountDataService = accountDataService;
            this.categoryDataService = categoryDataService;
            this.userManager = userManager;
        }

        private async Task<AppUser> GetCurrentUserAsync()
        {
            var userMail = User.FindFirstValue(ClaimTypes.Email);
            return await userManager.FindByEmailAsync(userMail);
        }

        // GET api/Categories
        [HttpGet]
        public async Task<IEnumerable<PaymentDto>> Get()
        {
            return Mapper.Map<List<PaymentDto>>(await paymentDataService.GetAllPayments(await GetCurrentUserAsync()));
        }

        // GET api/Categories/5
        [HttpGet("{id}")]
        public async Task<PaymentDto> Get(int id)
        {
            return Mapper.Map<PaymentDto>(await paymentDataService.GetById(id, await GetCurrentUserAsync()));
        }

        // POST api/Categories
        [HttpPost]
        public async Task Post([FromBody]PaymentDto paymentDto)
        {
            var user = await GetCurrentUserAsync();
            var payment = Mapper.Map<Payment>(paymentDto);
            payment.Id = 0;
            payment.User = user;

            if (paymentDto.AccountId.HasValue)
            {
                payment.Account = await accountDataService.GetById(paymentDto.AccountId.Value, user);
            }

            if (paymentDto.CategoryId.HasValue)
            {
                payment.Category = await categoryDataService.GetById(paymentDto.CategoryId.Value, user);
            }
            await paymentDataService.SavePayments(payment);
        }

        // PUT api/Categories/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]PaymentDto paymentDto)
        {
            var user = await GetCurrentUserAsync();
            var payment = Mapper.Map<Payment>(paymentDto);
            payment.Id = id;
            payment.User = user;

            if (paymentDto.AccountId.HasValue)
            {
                payment.Account = await accountDataService.GetById(paymentDto.AccountId.Value, user);
            }

            if (paymentDto.CategoryId.HasValue)
            {
                payment.Category = await categoryDataService.GetById(paymentDto.CategoryId.Value, user);
            }
            await paymentDataService.SavePayments(payment);
        }

        // DELETE api/Categories/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await paymentDataService.DeletePayment(id, await GetCurrentUserAsync());
        }
    }
}
