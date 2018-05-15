﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebLabBudgetTool.DataAccess.Entities
{
    public class AccountEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Iban { get; set; }
        public double CurrentBalance { get; set; }
        public string Note { get; set; }
        public bool IsOverdrawn { get; set; }
        public bool IsExcluded { get; set; }

        public virtual List<PaymentEntity> ChargedPayments { get; set; }
        public virtual List<PaymentEntity> TargetedPayments { get; set; }
    }
}
