using System;
using System.Collections.Generic;
using System.Text;

namespace Luqmit3ish.Models
{
    public class DishCard
    {
        public int Id { get; set; }
        public string DishName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Photo { get; set; }
        public int KeepValid { get; set; }
        public string PickUpTime { get; set; }
        public int Quantity { get; set; }
        public User Restaurant { get; set; }

    }
}
