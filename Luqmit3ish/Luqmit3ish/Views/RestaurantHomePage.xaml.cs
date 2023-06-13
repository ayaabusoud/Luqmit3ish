using Luqmit3ish.ViewModels;
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
            this.BindingContext = new RestaurantHomeViewModel(Navigation);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new RestaurantHomeViewModel(Navigation);
        }
    }
}