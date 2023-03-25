using Luqmit3ish.ViewModels;
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
    public partial class RestaurantHomePage : ContentPage
    {
        public RestaurantHomePage()
        {
            InitializeComponent();
            this.BindingContext = new RestaurantHomeViewModel();
        }
        private void FrameTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FoodDetailPage());
        }

        private void AddClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddFoodPage());
        }

        private void EditClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EditFoodPage());
        }
    }
}