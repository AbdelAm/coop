using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace coop2._0.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public int SenderBankAccountId { get; set; }
        public int ReceiverBankAccountId { get; set; }
        public virtual BankAccount SenderBankAccount { get; set; }
        public virtual BankAccount ReceiverBankAccount { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [DefaultValue(Status.Progress)]
        public Status Status { get; set; }
        public DateTime DateTransaction { get; set; }
    }
}
