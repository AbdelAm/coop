using System;
using System.Collections.Generic;

namespace coop2._0.Entity
{
    public class BankAccount
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public double Balance { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public bool IsValid { get; set; }
    }
}
