using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebLabBudgetTool.Entities;
using WebLabBudgetTool.Helpers;
using WebLabBudgetTool.QueryExtension;

namespace WebLabBudgetTool.DataService
{
    /// <summary>
    ///     Offers service methods to access and modify payment data.
    /// </summary>
    public interface IPaymentDataService
    {
        /// <summary>
        ///     Returns all payments.
        /// </summary>
        /// <returns>List of Payments</returns>
        Task<IEnumerable<Payment>> GetAllPayments(AppUser user);

        /// <summary>
        ///     Returns all uncleared payments up to the passed enddate who are assigned to the passed Account ID.
        /// </summary>
        /// <param name="enddate">Enddate</param>
        /// <returns>List of Payments.</returns>
        Task<IEnumerable<Payment>> GetUnclearedPayments(DateTime enddate);

        /// <summary>
        ///     Returns a payment searched by ID.
        /// </summary>
        /// <param name="id">Id to select the payment for.</param>
        /// <param name="user">Logged In user.</param>
        /// <returns>The selected payment</returns>
        Task<Payment> GetById(int id, AppUser user);

        /// <summary>
        ///     Saves or updates a one or more payments to the database.
        /// </summary>
        /// <param name="payments">Payments to save or update.</param>
        Task SavePayments(params Payment[] payments);

        /// <summary>
        ///     Deletes the payment wit the passed ID from the dataabase.
        /// </summary>
        /// <param name="id">Id of the payment to delete.</param>
        /// <param name="user">Logged In user.</param>
        Task DeletePayment(int id, AppUser user);
    }

    /// <inheritdoc />
    public class PaymentDataService : IPaymentDataService
    {
        private readonly IAmbientDbContextLocator ambientDbContextLocator;
        private readonly IDbContextScopeFactory dbContextScopeFactory;

        /// <summary>
        ///     Creates a PaymentDataService object.
        /// </summary>
        public PaymentDataService(IAmbientDbContextLocator ambientDbContextLocator, IDbContextScopeFactory dbContextScopeFactory)
        {
            this.dbContextScopeFactory = dbContextScopeFactory;
            this.ambientDbContextLocator = ambientDbContextLocator;
        }

        public async Task<IEnumerable<Payment>> GetAllPayments(AppUser user)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    return await dbContext.Payments
                                          .IsAssignedToUser(user)
                                          .Include(x => x.Category)
                                          .ToListAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Payment>> GetUnclearedPayments(DateTime enddate)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var query = dbContext.Payments
                                         .Include(x => x.Category)
                                         .Include(x => x.Account)
                                         .AreNotCleared()
                                         .HasDateSmallerEqualsThan(enddate);

                    return await query
                        .ToListAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task<Payment> GetById(int id, AppUser user)
        {
            using (dbContextScopeFactory.CreateReadOnly())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var payment = await dbContext.Payments
                                                 .IsAssignedToUser(user)
                                                 .Include(x => x.Account)
                                                 .Include(x => x.Category)
                                                 .FirstAsync(x => x.Id == id);
                    return payment == null ? null : new Payment();
                }
            }
        }

        /// <inheritdoc />
        public async Task SavePayments(params Payment[] payments)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    foreach (var payment in payments)
                    {
                        AddPaymentToChangeSet(dbContext, payment);
                    }
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task DeletePayment(int id, AppUser user)
        {
            using (var dbContextScope = dbContextScopeFactory.Create())
            {
                using (var dbContext = ambientDbContextLocator.Get<ApplicationContext>())
                {
                    var payment = await GetById(id, user);
                    PaymentAmountHelper.RemovePaymentAmount(payment);

                    var paymentEntry = dbContext.Entry(payment);
                    paymentEntry.State = EntityState.Deleted;

                    var chargedAccountEntry = dbContext.Entry(payment.Account);
                    chargedAccountEntry.State = EntityState.Modified;

                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        private void AddPaymentToChangeSet(ApplicationContext dbContext, Payment payment)
        {
            payment.ClearPayment();
            PaymentAmountHelper.AddPaymentAmount(payment);

            SaveOrUpdatePayment(dbContext, payment);
            SaveOrUpdateAccount(dbContext, payment.Account);
        }

        private void SaveOrUpdatePayment(ApplicationContext dbContext, Payment payment)
        {
            if (payment.Id != 0)
            {
                payment.AccountId = payment.Account.Id;
                payment.CategoryId = payment.Category?.Id;
            }

            var paymentEntry = dbContext.Entry(payment);
            paymentEntry.State = payment.Id == 0 ? EntityState.Added : EntityState.Modified;
        }

        private void SaveOrUpdateAccount(ApplicationContext dbContext, Account account)
        {
            var paymentEntry = dbContext.Entry(account);
            paymentEntry.State = account.Id == 0 ? EntityState.Added : EntityState.Modified;
        }
    }
}