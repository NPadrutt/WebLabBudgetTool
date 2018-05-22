using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebLabBudgetTool.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Note { get; set; }

        public virtual List<Payment> Payments { get; set; }
    }
}
