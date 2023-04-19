using System;
using System.Collections.Generic;
using System.Text;

namespace Luqmit3ish.Models
{
    public class OrderCard
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public List<OrderDish> data { get; set; }
    }
}
