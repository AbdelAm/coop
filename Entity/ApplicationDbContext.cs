using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace coop2._0.Entity
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Transaction>()
             .HasKey(ds => new { ds.Id, ds.SenderBankAccountId, ds.ReceiverBankAccountId });
            builder.Entity<Transaction>()
                .HasOne(d => d.SenderBankAccount)
                .WithMany(ds => ds.TransactionsSended)
                .HasForeignKey(ds => ds.SenderBankAccountId);
            builder.Entity<Transaction>()
                .HasOne(s => s.ReceiverBankAccount)
                .WithMany(ds => ds.TransactionsReceived)
                .HasForeignKey(ds => ds.ReceiverBankAccountId);
            //this.SeedRoles(builder);
            base.OnModelCreating(builder);

        }
        /*private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "ADMIN" },
                new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = "USER", ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "USER" }
            );
        }*/

    }
}
