using System;
using Luqmit3ish.Models;
using System.Collections.ObjectModel;

namespace Luqmit3ish.Utilities
{
    public class Constants
    {
        public readonly static string BaseUrl = "https://luqmit3ishv4.azurewebsites.net/";

        public static ObservableCollection<TypeField> TypeValues { get; } = new ObservableCollection<TypeField>
        {
            new TypeField { Name = "Food", IconText = "\ue4c6;" },
            new TypeField { Name = "Drink", IconText = "\uf4e3;" },
            new TypeField { Name = "Dessert", IconText = "\uf7ef;" },
            new TypeField { Name = "Appetizer", IconText = "\uf2e7;" },
            new TypeField { Name = "Fast Food", IconText = "\uf805;" },
            new TypeField { Name = "Fish", IconText = "\uf578;" }
        };

        public static ObservableCollection<LocationField> LocationValues { get; } = new ObservableCollection<LocationField>
        {
            new LocationField{ Name="Nablus" },
            new LocationField{ Name="Ramallah" },
            new LocationField{ Name="Jenin" },
            new LocationField{ Name="Jericho" },
            new LocationField{ Name="Jerusalem" },
            new LocationField{ Name="Tulkarm" },
            new LocationField{ Name="Bethlehem" },
            new LocationField{ Name="Hebron" },
        };
    }
}