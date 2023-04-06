using System;
namespace Luqmit3ish.Models
{
	public class DishRequest
	{
        public int user_id { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public String type { get; set; }
        public String photo { get; set; }
        public int keep_listed { get; set; }
        public String pick_up_time { get; set; }
        public int number { get; set; }
    }
}

