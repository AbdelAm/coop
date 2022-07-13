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
            base.OnModelCreating(builder);
            this.SeedRoles(builder);

        }
        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "ADMIN" },
                new IdentityRole() { Id = Guid.NewGuid().ToString(), Name = "USER", ConcurrencyStamp = Guid.NewGuid().ToString(), NormalizedName = "USER" }
            );
        }

    }
}
