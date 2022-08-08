using System;
using System.Collections.Generic;

namespace coop2._0.Model
{
    public class TokenModel
    {
        public string Cif { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
        public int BankAccount { get; set; }
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
