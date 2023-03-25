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
    public partial class CharityOrderPage : ContentPage
    {
        public CharityOrderPage()
        {
            InitializeComponent();
            this.BindingContext = new CharityOrderViewModel();
        }

        private async void ProfileTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OtherProfilePage());
        }

        private async void EditClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditOrderPage());
        }
    }
}