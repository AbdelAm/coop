namespace coop2._0.Entity
{
    public class BankAccount
    {
        int id;
        string accountNumber;
        int balance;
        System.DateTime? accountDate;
        int ownerId;
        User owner;
        System.ArraySegment<Transaction> transactions;
    }
}
