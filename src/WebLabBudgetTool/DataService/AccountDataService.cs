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
    ///     Offers service methods to access and modify account data.
    /// </summary>
    public interface IAccountDataService
    {
        /// <summary>
        ///     Returns a list with all accounts.
        /// </summary>
        /// <returns>Returns a IEnumerable with all accounts.</returns>
        Task<IEnumerable<Account>> GetAllAccounts(AppUser user);

        /// <summary>
        ///     Returns a Account searched by ID.
        /// </summary>
        /// <param name="id">Id to select the account for.</param>
        /// <param name="user">Current logged in user.</param>
        /// <returns>The selected Account</returns>
        Task<Account> GetById(int id, AppUser user);
       
        /// <summary>
        ///     Save the passed account.
        /// </summary>
        Task SaveAccount(Account account);

        /// <summary>
        ///     Deletes the passed account with the passed id and all associated payments.
        /// </summary>
        /// <param name="accountId">Id of the account to delete.</param>
        /// <param name="user">Current logged in user.</param>
        Task DeleteAccount(int accountId, AppUser user);
    }

    public class AccountDataService : IAccountDataService
    {
        private readonly IAmbientDbContextLocator ambientDbContextLocator;
        private readonly IDbContextScopeFactory dbContextScopeFactory;

        public AccountDataService(IAmbientDbContextLocator ambientDbContextLocator, IDbContextScopeFactory dbContextScopeFactory)
        {
            this.ambientDbContextLocator = ambientDbContextLocator;
            this.dbContextScopeFactory = dbContextScopeFactory;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Account>> GetAllAccounts(AppUser user)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var list = await dbContext.Accounts
                                              .IsAssignedToUser(user)
                                              .OrderByName()
                                              .ToListAsync();

                    return list.ToList();
                }
            }
        }/// <inheritdoc />
        public async Task<Account> GetById(int id, AppUser user)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var account = await dbContext.Accounts
                                                 .IsAssignedToUser(user)
                                                 .Include(x => x.Payments).ThenInclude(p => p.Category)
                                                 .FirstOrDefaultAsync(x => x.Id == id);

                    return account;
                }
            }
        }

        /// <inheritdoc />
        public async Task SaveAccount(Account account)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    dbContext.Entry(account).State = account.Id == 0
                        ? EntityState.Added
                        : EntityState.Modified;
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task DeleteAccount(int accountId, AppUser user)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var account = GetById(accountId, user);

                    dbContext.Entry(account).State = EntityState.Deleted;
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }
    }
}