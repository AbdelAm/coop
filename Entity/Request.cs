using System;

namespace coop2._0.Entity
{
    public class Request
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public DateTime DateRequest { get; set; }
        public bool IsValid { get; set; }
    }
}
