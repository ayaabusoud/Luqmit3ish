using Luqmit3ish.ViewModels;
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
            this.BindingContext = new CharityHomeViewModel(Navigation);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = new CharityHomeViewModel(Navigation);
        }
    }
}