using System;
using System.Collections.Generic;
using System.Text;

namespace Luqmit3ish.Models
{
    class DishCard
    {
        public int id { get; set; }
        public int restaurantId { get; set; }
        public String dishName { get; set; }
        public String description { get; set; }
        public String type { get; set; }
        public String photo { get; set; }
        public int keepValid { get; set; }
        public String pickUpTime { get; set; }
        public int quantity { get; set; }
        public String restaurantName { get; set; }
        public bool IsExpanded { get; set; }

    }
}
