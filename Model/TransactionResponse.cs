using System;
using coop2._0.Entities;

namespace coop2._0.Model
{
    public class TransactionResponse
    {
        public double Amount { get; set; }
        public string Motif { get; set; }
        public  string SenderName { get; set; }
        public  string SenderBankAccountNumber { get; set; }
        public  string ReceiverName { get; set; }
        public  string ReceiverBankAccountNumber { get; set; }
        public Status Status { get; set; }
        public string DateTransaction { get; set; }
    }
}