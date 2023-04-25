using System;
using System.Collections.Generic;
using System.Text;

namespace Luqmit3ish.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Type { get; set; }
        public String Photo { get; set; }
        public int KeepValid { get; set; }
        public String PickUpTime { get; set; }
        public int Quantity { get; set; }
        public string Items { get; set; }
    }
}
