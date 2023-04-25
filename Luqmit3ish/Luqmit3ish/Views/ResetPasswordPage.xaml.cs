using Luqmit3ish.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luqmit3ish.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPasswordPage : ContentPage
    {
        public ResetPasswordPage(string email)
        {
            InitializeComponent();
            this.BindingContext = new ResetPasswordViewModel(Navigation, email);
        }
    }
}
