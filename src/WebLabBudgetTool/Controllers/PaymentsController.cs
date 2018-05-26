using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

        public PaymentsController(IPaymentDataService paymentDataService, 
                                  IAccountDataService accountDataService, 
                                  ICategoryDataService categoryDataService)
        {
            this.paymentDataService = paymentDataService;
            this.accountDataService = accountDataService;
            this.categoryDataService = categoryDataService;
        }

        // GET api/Categories
        [HttpGet]
        public async Task<IEnumerable<PaymentDto>> Get()
        {
            return Mapper.Map<List<PaymentDto>>(await paymentDataService.GetAllPayments());
        }

        // GET api/Categories/5
        [HttpGet("{id}")]
        public async Task<PaymentDto> Get(int id)
        {
            return Mapper.Map<PaymentDto>(await paymentDataService.GetById(id));
        }

        [HttpPost]
        public async Task Post([FromBody]PaymentDto paymentDto)
        {
            var payment = Mapper.Map<Payment>(paymentDto);
            payment.Id = 0;

            if (paymentDto.AccountId.HasValue)
            {
                payment.Account = await accountDataService.GetById(paymentDto.AccountId.Value);
            }

            if (paymentDto.CategoryId.HasValue)
            {
                payment.Category = await categoryDataService.GetById(paymentDto.CategoryId.Value);
            }
            await paymentDataService.SavePayments(payment);
        }

        // POST api/Categories

        // PUT api/Categories/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]PaymentDto paymentDto)
        {
            var payment = Mapper.Map<Payment>(paymentDto);
            payment.Id = id;

            if (paymentDto.AccountId.HasValue)
            {
                payment.Account = await accountDataService.GetById(paymentDto.AccountId.Value);
            }

            if (paymentDto.CategoryId.HasValue)
            {
                payment.Category = await categoryDataService.GetById(paymentDto.CategoryId.Value);
            }
            await paymentDataService.SavePayments(payment);
        }

        // DELETE api/Categories/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await paymentDataService.DeletePayment(id);
        }
    }
}
