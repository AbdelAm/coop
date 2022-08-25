using System.Collections.Generic;

namespace coop2._0.Model
{
    public class ItemsModel<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int ItemsNumber { get; set; }
        public int ProgressNumber { get; set; }
    }
}