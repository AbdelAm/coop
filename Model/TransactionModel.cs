using System;

namespace coop2._0.Model
{
    public class TransactionModel
    {
        public double Amount { get; set; }
        public string Motif { get; set; }

        public string SenderBankAccountNumber { get; set; }
        public string ReceiverBankAccountNumber { get; set; }

    }
}