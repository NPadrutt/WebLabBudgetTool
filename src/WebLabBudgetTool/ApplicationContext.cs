using EntityFramework.DbContextScope.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebLabBudgetTool.Entities;

namespace WebLabBudgetTool
{
    public sealed class ApplicationContext : IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>, IDbContext
    {
        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public static string ConnectionString;

        internal DbSet<Account> Accounts { get; set; }
        internal DbSet<Payment> Payments { get; set; }
        internal DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        ///     Called when the models are created.
        ///     Enables to configure advanced settings for the models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Set FK from payment to account for charged account with cascade
            modelBuilder.Entity<Account>()
                        .HasMany(m => m.Payments)
                        .WithOne(t => t.Account)
                        .HasForeignKey(m => m.AccountId)
                        .OnDelete(DeleteBehavior.Cascade);
           
            // Set FK from category to payment with cascade with cascade
            modelBuilder.Entity<Category>()
                        .HasMany(m => m.Payments)
                        .WithOne(t => t.Category)
                        .HasForeignKey(m => m.CategoryId)
                        .OnDelete(DeleteBehavior.SetNull);

            // Set Indizies
            modelBuilder.Entity<Account>()
                        .HasIndex(b => b.Name);
            modelBuilder.Entity<Category>()
                        .HasIndex(b => b.Name);

            base.OnModelCreating(modelBuilder);
        }
    }
}
