using Luqmit3ish.ViewModels;
using Xamarin.Forms.Xaml;

namespace Luqmit3ish.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        public FilterPopUp()
        {
            InitializeComponent();
            this.BindingContext = new FilterPopUpViewModel(Navigation);
        }
    }
}