using System;
using System.Collections.Generic;

namespace coop2._0.Model
{
    public class TokenModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
