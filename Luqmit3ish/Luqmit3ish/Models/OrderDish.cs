using System;
using System.Collections.Generic;
using System.Text;

namespace Luqmit3ish.Models
{
    public class OrderDish
    {
        public int Id { get; set; }
        public int ResId { get; set; }
        public int CharId { get; set; }
        public Dish Dish { get; set; }
        public int Quantity { get; set; }
        public Boolean Receive { get; set; }
        public string Items { get; set; }

    }
}
