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
    [Microsoft.AspNetCore.Cors.EnableCors("CorsPolicy")]
    public class PaymentsController
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

        // POST api/Categories
        [HttpPost]
        public async Task Post([FromBody]PaymentDto paymentDto)
        {
            var payment = Mapper.Map<Payment>(paymentDto);
            payment.Id = 0;

            if (paymentDto.CategoryId.HasValue)
            {
                payment.Category = await categoryDataService.GetById(paymentDto.CategoryId.Value);
            }
            if (paymentDto.AccountId.HasValue)
            {
                payment.Category = await categoryDataService.GetById(paymentDto.AccountId.Value);
            }

            await paymentDataService.SavePayments(payment);
        }

        // PUT api/Categories/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]PaymentDto paymentDto)
        {
            var payment = Mapper.Map<Payment>(paymentDto);
            payment.Id = id;

            if (paymentDto.CategoryId.HasValue)
            {
                payment.Category = await categoryDataService.GetById(paymentDto.CategoryId.Value);
            }
            if (paymentDto.AccountId.HasValue)
            {
                payment.Category = await categoryDataService.GetById(paymentDto.AccountId.Value);
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
