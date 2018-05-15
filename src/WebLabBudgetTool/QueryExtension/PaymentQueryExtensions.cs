﻿using System;
using System.Linq;
using WebLabBudgetTool.Entities;

namespace WebLabBudgetTool.QueryExtension
{
    /// <summary>
    ///     Provides Extensions for payments queries.
    /// </summary>
    public static class PaymentQueryExtensions
    {
        /// <summary>
        ///     Adds a filter to a query for cleared payments
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query filtered for cleared payments.</returns>
        public static IQueryable<Payment> AreCleared(this IQueryable<Payment> query)
        {
            return query.Where(payment => payment.IsCleared);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who are not cleared
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query filtered for not cleared payments.</returns>
        public static IQueryable<Payment> AreNotCleared(this IQueryable<Payment> query)
        {
            return query.Where(payment => !payment.IsCleared);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who has a date larger or equals to the passed date.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="date">Date to filter for.</param>
        /// <returns>Query filtered for not cleared payments.</returns>
        public static IQueryable<Payment> HasDateLargerEqualsThan(this IQueryable<Payment> query, DateTime date)
        {
            return query.Where(payment => payment.Date.Date >= date.Date);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who has a date smaller or equals to the passed date.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="date">Date to filter for.</param>
        /// <returns>Query filtered for the date.</returns>
        public static IQueryable<Payment> HasDateSmallerEqualsThan(this IQueryable<Payment> query, DateTime date)
        {
            return query.Where(payment => payment.Date.Date <= date.Date);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who has a certain id as charged or target account.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="accountId">AccountId to filter for </param>
        /// <returns>Query filtered for the account id.</returns>
        public static IQueryable<Payment> HasAccountId(this IQueryable<Payment> query, int accountId)
        {
            return query.Where(payment => payment.AccountId == accountId);
        }

        /// <summary>
        ///     Adds a filter to a query for payments who are not Transfers
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query filtered for payments who are not transfers.</returns>
        public static IQueryable<Payment> WithoutTransfers(this IQueryable<Payment> query)
        {
            return query.Where(payment => payment.Type != PaymentType.Transfer);
        }
    }
}
