using coop2._0.Entities;
using System.Linq;

namespace coop2._0.Model
{
    public class UserBankItemModel : UserItemModel
    {
        public BankAccount BankAccount { get; set; }


        public UserBankItemModel() : base()
        {
        }

        public UserBankItemModel(User u) : base(u)
        {
            BankAccount = u.BankAccounts.FirstOrDefault();
        }
    }
}