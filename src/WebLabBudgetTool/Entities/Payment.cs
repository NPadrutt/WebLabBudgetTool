﻿using System;
using System.ComponentModel.DataAnnotations;

namespace WebLabBudgetTool.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public bool IsCleared { get; set; }
        public PaymentType Type { get; set; }
        public string Note { get; set; }

        public virtual Category Category { get; set; }

        public virtual Account Account { get; set; }

        public virtual AppUser User { get; set; }

        /// <summary>
        ///     This method checks if the payment is up for clearing and sets the
        ///     flags accordingly.
        /// </summary>
        public void ClearPayment()
        {
            if (IsCleared) return;
            IsCleared = Date.Date <= DateTime.Now.Date;
        }
    }
}
