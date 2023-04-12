using System;
using System.Collections.Generic;
using System.Text;

namespace Luqmit3ish.Models
{
    class OrderCard
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public bool IsExpanded { get; set; }
        public List<OrderDish> data { get; set; }
    }
}
