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
    public class CategoriesController : Controller
    {
        private readonly ICategoryDataService categoryDataService;

        public CategoriesController(ICategoryDataService categoryDataService)
        {
            this.categoryDataService = categoryDataService;
        }

        // GET api/Categories
        [HttpGet]
        public async Task<IEnumerable<CategoryDto>> Get()
        {
            return Mapper.Map<List<CategoryDto>>(await categoryDataService.GetAllCategories());
        }

        // GET api/Categories/5
        [HttpGet("{id}")]
        public async Task<CategoryDto> Get(int id)
        {
            return Mapper.Map<CategoryDto>(await categoryDataService.GetById(id));
        }

        // POST api/Categories
        [HttpPost]
        public async Task Post([FromBody]CategoryDto value)
        {
            var category = Mapper.Map<Category>(value);
            category.Id = 0;

            await categoryDataService.SaveCategory(category);
        }

        // PUT api/Categories/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]CategoryDto value)
        {
            var category = Mapper.Map<Category>(value);
            category.Id = id;

            await categoryDataService.SaveCategory(category);
        }

        // DELETE api/Categories/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await categoryDataService.DeleteCategory(id);
        }
    }
}
