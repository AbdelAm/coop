using System;
using coop2._0.Entities;

namespace coop2._0.Model
{
    public class TransactionResponse
    {
        public double Amount { get; set; }
        public string Motif { get; set; }
        public virtual string SenderName { get; set; }
        public virtual string SenderBankAccountNumber { get; set; }
        public virtual string ReceiverName { get; set; }
        public virtual string ReceiverBankAccountNumber { get; set; }
        public string DateTransaction { get; set; }

        public Status Status { get; set; }
    }
}