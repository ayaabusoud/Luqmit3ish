using System;
namespace Luqmit3ish.Models
{
    public class DishRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public String Type { get; set; }
        public String Photo { get; set; }
        public int keepValid { get; set; }
        public int Quantity { get; set; }
    }
}
