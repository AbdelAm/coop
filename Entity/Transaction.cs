namespace coop2._0.Entity
{
    public class Transaction
    {
        int id;
        int amount;
        int senderAccountId;
        int receiverAccountId;
        BankAccount senderAccount;
        BankAccount receiverAccount;
        string status;
        System.DateTime dateTransaction; 
    }
}
