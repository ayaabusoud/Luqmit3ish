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
        public RestaurantOfTheMonth()
        {
            InitializeComponent();
            this.BindingContext = new RestaurantOfTheMonthViewModel(Navigation);
        }
    }
}