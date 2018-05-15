using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebLabBudgetTool.DataAccess.Entities;

namespace WebLabBudgetTool.DataAccess
{
    public class ApplicationContext : DbContext, IDbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions options) : base(options) { }

        internal DbSet<AccountEntity> Accounts { get; set; }
        internal DbSet<PaymentEntity> Payments { get; set; }
        internal DbSet<CategoryEntity> Categories { get; set; }

    
        /// <summary>
        ///     Called when the models are created.
        ///     Enables to configure advanced settings for the models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set FK from payment to account for charged account with cascade
            modelBuilder.Entity<AccountEntity>()
                        .HasMany(m => m.ChargedPayments)
                        .WithOne(t => t.ChargedAccount)
                        .HasForeignKey(m => m.ChargedAccountId)
                        .OnDelete(DeleteBehavior.Cascade);

            // Set FK from payment to account for target account with cascade
            modelBuilder.Entity<AccountEntity>()
                        .HasMany(m => m.TargetedPayments)
                        .WithOne(t => t.TargetAccount)
                        .HasForeignKey(m => m.TargetAccountId)
                        .OnDelete(DeleteBehavior.Cascade);
           
            // Set FK from category to payment with cascade with cascade
            modelBuilder.Entity<CategoryEntity>()
                        .HasMany(m => m.Payments)
                        .WithOne(t => t.Category)
                        .HasForeignKey(m => m.CategoryId)
                        .OnDelete(DeleteBehavior.SetNull);

            // Set Indizies
            modelBuilder.Entity<AccountEntity>()
                        .HasIndex(b => b.Name);
            modelBuilder.Entity<CategoryEntity>()
                        .HasIndex(b => b.Name);
        }
    }
}
