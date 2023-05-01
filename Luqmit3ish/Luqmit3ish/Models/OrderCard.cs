using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Luqmit3ish.Models
{
    public class OrderCard
    {
        public int Id { get; set; }
        public User Owner { get; set; }
        public ObservableCollection<OrderDish> Orders { get; set; }
        public string Items { get; set; }
    }
}
