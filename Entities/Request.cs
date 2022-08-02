using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace coop2._0.Entities
{
    public class Request
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime DateRequest { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [DefaultValue(Status.Progress)]
        public Status Status { get; set; }
    }
}
