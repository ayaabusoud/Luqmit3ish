using System;
using System.Collections.Generic;
using System.Text;

namespace Luqmit3ish.Models
{
    class Order
    {
        public int res_id { get; set; }
        public int char_id { get; set; }
        public int dish_id { get; set; }
        public DateTime date { get; set; }
        public int number_of_dish { get; set; }
        public Boolean receive { get; set; }
    }
    
}
