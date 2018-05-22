using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebLabBudgetTool.Entities;
using WebLabBudgetTool.QueryExtension;

namespace WebLabBudgetTool.DataService
{
    /// <summary>
    ///     Offers service methods to access and modify category data.
    /// </summary>
    public interface ICategoryDataService
    {
        /// <summary>
        ///     Returns all categories
        /// </summary>
        /// <returns>List with all categories.</returns>
        Task<IEnumerable<Category>> GetAllCategories();

        /// <summary>
        ///     Looks up the category with the passed id and returns it.
        /// </summary>
        /// <param name="id">Id to look for.</param>
        /// <returns>Found Id.</returns>
        Task<Category> GetById(int id);

        /// <summary>
        ///     Checks if the name is already taken by another category.
        /// </summary>
        /// <param name="name">Name to look for.</param>
        /// <returns>If category name is already taken.</returns>
        Task<bool> CheckIfNameAlreadyTaken(string name);

        /// <summary>
        ///     Save the passed category.
        /// </summary>
        /// <param name="category">Category to save.</param>
        Task SaveCategory(Category category);

        /// <summary>
        ///     Deletes the category with the passed Id and sets references to it to null.
        /// </summary>
        /// <param name="id">Id of the catgory to delete.</param>
        Task DeleteCategory(int id);
    }

    /// <summary>
    ///     Offers service methods to access and modify category data.
    /// </summary>
    public class CategoryDataService : ICategoryDataService
    {
        private readonly IAmbientDbContextLocator ambientDbContextLocator;
        private readonly IDbContextScopeFactory dbContextScopeFactory;

        /// <summary>
        ///     Constructor
        /// </summary>
        public CategoryDataService(IAmbientDbContextLocator ambientDbContextLocator,
                               IDbContextScopeFactory dbContextScopeFactory)
        {
            this.ambientDbContextLocator = ambientDbContextLocator;
            this.dbContextScopeFactory = dbContextScopeFactory;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var list = await dbContext.Categories
                                              .OrderByName()
                                              .ToListAsync();

                    return list.ToList();
                }
            }
        }

        /// <inheritdoc />
        public async Task<Category> GetById(int id)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var category = await dbContext.Categories.FindAsync(id);
                    return category == null ? null : new Category();
                }
            }
        }

        /// <inheritdoc />
        public async Task<bool> CheckIfNameAlreadyTaken(string name)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    return await dbContext.Categories
                                          .NameEquals(name)
                                          .AnyAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task SaveCategory(Category category)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    dbContext.Entry(category).State = category.Id == 0 
                        ? EntityState.Added 
                        : EntityState.Modified;

                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task DeleteCategory(int id)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    dbContext.Entry(await GetById(id)).State = EntityState.Deleted;
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }
    }
}
