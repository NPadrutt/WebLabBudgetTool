using System;
using WebLabBudgetTool.Entities;

namespace WebLabBudgetTool.Helpers
{
    /// <summary>
    ///     Provides helper functions for payment amount.
    /// </summary>
    public static class PaymentAmountHelper
    {
        /// <summary>
        ///     If the payment is an expense it subtracts the amount from the charged account.
        ///     if it is an income it adds it to the target account.
        ///     IMPORTANT: Payment has to be cleared. Otherwise no action will take place.
        /// </summary>
        /// <param name="payment">Payment.</param>
        public static void AddPaymentAmount(Payment payment)
        {
            if (!payment.IsCleared) return;

            payment.Account.CurrentBalance += payment.Type == PaymentType.Income
                ? payment.Amount
                : -payment.Amount;

            payment.Account.IsOverdrawn = payment.Account.CurrentBalance < 0;
        }

        /// <summary>
        ///     If the payment is an expense it adds the amount from the charged account.
        ///     if it is an income it subtracts it to the target account.
        ///     IMPORTANT: Payment has to be cleared. Otherwise no action will take place.
        /// </summary>
        /// <param name="payment">Payment.</param>
        public static void RemovePaymentAmount(Payment payment)
        {
            if (!payment.IsCleared) return;

            payment.Account.CurrentBalance += payment.Type == PaymentType.Income
                ? -payment.Amount
                : payment.Amount;

            payment.Account.IsOverdrawn = payment.Account.CurrentBalance < 0;
        }
    }
}
