using System;
using System.Linq;
using WebLabBudgetTool.Entities;

namespace WebLabBudgetTool.QueryExtension
{
    /// <summary>
    ///     Provides Extensions for account queries.
    /// </summary>
    public static class AccountQueryExtensions
    {
        /// <summary>
        ///     Adds a filter to a query for excluded accounts
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query with a filter for excluded accounts.</returns>
        public static IQueryable<Account> AreNotExcluded(this IQueryable<Account> query)
        {
            return query.Where(x => !x.IsExcluded);
        }

        /// <summary>
        ///     Adds a filter to a query for not excluded accounts
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Query with a filter for not excluded accounts.</returns>
        public static IQueryable<Account> AreExcluded(this IQueryable<Account> query)
        {
            return query.Where(x => x.IsExcluded);
        }

        /// <summary>
        ///     Adds a filter to a query to find all accounts with the passed name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="name">Name to filter for</param>
        /// <returns>Query with the added filter.</returns>
        public static IQueryable<Account> NameEquals(this IQueryable<Account> query, string name)
        {
            return query.Where(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        ///     Ordery an account query by the name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Ordered query.</returns>
        public static IQueryable<Account> OrderByName(this IQueryable<Account> query)
        {
            return query.OrderBy(x => x.Name);
        }
    }
}
