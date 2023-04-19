using System;
using System.Collections.ObjectModel;

namespace Luqmit3ish.Models
{
    public class FilterInfo
    {
        public ObservableCollection<string> TypeValues { get; set; }
        public ObservableCollection<string> LocationValues { get; set; }
        public int LowerKeepValid { get; set; }
        public int LowerQuantity { get; set; }
        public int UpperKeepValid { get; set; }
        public int UpperQuantity { get; set; }

        public FilterInfo()
        {
            TypeValues = new ObservableCollection<string>();
            LocationValues = new ObservableCollection<string>();
        }
    }
}

