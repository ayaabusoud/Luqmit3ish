using System;
using System.Collections.Generic;
using System.Text;

namespace Luqmit3ish.Models
{
    public class OrderCard
    {
        public int Id { get; set; }
        public User Owner { get; set; }
        public List<OrderDish> Orders { get; set; }
    }
}
