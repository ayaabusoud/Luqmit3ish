using System;
using System.Collections.Generic;
using System.Text;

namespace Luqmit3ish.Models
{
    class Dish
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public String type { get; set; }
        public String photo { get; set; }
        public int keep_listed { get; set; }
        public DateTime pick_up_time { get; set; }
        public int number { get; set; }
    }
}
