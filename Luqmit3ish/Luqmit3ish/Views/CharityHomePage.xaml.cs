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
    public partial class CharityHomePage : ContentPage
    {
        public CharityHomePage()
        {
            InitializeComponent();
            this.BindingContext = new CharityHomeViewModel();
        }

        private void FilterClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FilterFoodPage());
        }

        private void FrameTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FoodDetailPage());
        }

        private void SearchClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SearchPage());
        }

        private async void ProfileTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OtherProfilePage());
        }
    }
}