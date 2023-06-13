using Luqmit3ish.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luqmit3ish.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestaurantOfTheMonth : ContentPage
    {
        public RestaurantOfTheMonth(DishesOrder bestRestaurant)
        {
            InitializeComponent();
            this.BindingContext = new RestaurantOfTheMonthViewModel(bestRestaurant);
        }
    }
}