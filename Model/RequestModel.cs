using coop2._0.Entities;
using System;

namespace coop2._0.Model
{
    public class RequestModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime DateRequest { get; set; }
        public Status Status { get; set; }

    }
    
}
