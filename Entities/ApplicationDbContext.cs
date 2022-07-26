using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace coop2._0.Entities
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

        /*public ApplicationDbContext() //ajoute par Abdelaziz pour request controller
        {
        }*/

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Transaction>()
                .HasOne(d => d.SenderBankAccount)
                .WithMany(ds => ds.TransactionsSended)
                .HasForeignKey(ds => ds.SenderBankAccountId);
            builder.Entity<Transaction>()
                .HasOne(s => s.ReceiverBankAccount)
                .WithMany(ds => ds.TransactionsReceived)
                .HasForeignKey(ds => ds.ReceiverBankAccountId);
            this.SeedRoles(builder);
            base.OnModelCreating(builder);
        }
        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "6ee2a5ea-ab9f-41a4-875e-fd4c1b142631", Name = "ADMIN", ConcurrencyStamp = "991e6117-550a-4823-a9b5-86db99beaf72", NormalizedName = "ADMIN" },
                new IdentityRole() { Id = "df03e6fe-bb22-42bf-a972-6115b8270ad1", Name = "USER", ConcurrencyStamp = "0f1dc26a-a485-4bbb-bd0b-14ffa93a94e7", NormalizedName = "USER" }
            );
        }
    }
}