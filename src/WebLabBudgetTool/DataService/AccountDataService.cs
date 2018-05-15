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
        Task<IEnumerable<Account>> GetAllAccounts();

        /// <summary>
        ///     Returns a Account searched by ID.
        /// </summary>
        /// <param name="id">Id to select the account for.</param>
        /// <returns>The selected Account</returns>
        Task<Account> GetById(int id);
       
        /// <summary>
        ///     Checks if the name is already taken by another account.
        /// </summary>
        /// <param name="name">Name to look for.</param>
        /// <returns>if account name is already taken.</returns>
        Task<bool> CheckIfNameAlreadyTaken(string name);

        /// <summary>
        ///     Returns a list with all not excluded Accounts.
        /// </summary>
        /// <returns>List with all not excluded Accounts.</returns>
        Task<IEnumerable<Account>> GetNotExcludedAccounts();

        /// <summary>
        ///     Returns a list with all excluded Accounts.
        /// </summary>
        /// <returns>List with all  excluded Accounts.</returns>
        Task<IEnumerable<Account>> GetExcludedAccounts();

        /// <summary>
        ///     Save the passed account.
        /// </summary>
        Task SaveAccount(Account account);

        /// <summary>
        ///     Deletes the passed account with the passed id and all associated payments.
        /// </summary>
        Task DeleteAccount(int accountId);
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
        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var list = await dbContext.Accounts
                                              .OrderByName()
                                              .ToListAsync();

                    return list.ToList();
                }
            }
        }/// <inheritdoc />
        public async Task<Account> GetById(int id)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var account = await dbContext.Accounts
                                                 .Include(x => x.Payments).ThenInclude(p => p.Category)
                                                 .FirstOrDefaultAsync(x => x.Id == id);

                    return account;
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
                    return await dbContext.Accounts
                                                  .NameEquals(name)
                                                  .AnyAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Account>> GetNotExcludedAccounts()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var list = await dbContext.Accounts
                        .AreNotExcluded()
                        .OrderByName()
                        .ToListAsync();

                    return list.ToList();
                }
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Account>> GetExcludedAccounts()
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var list = await dbContext.Accounts
                        .AreExcluded()
                        .OrderByName()
                        .ToListAsync();

                    return list.ToList();
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
        public async Task DeleteAccount(int accountId)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var account = GetById(accountId);

                    dbContext.Entry(account).State = EntityState.Deleted;
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }
    }
}