using Luqmit3ish.ViewModels;

namespace Luqmit3ish.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PopUp(string message)
        {
            InitializeComponent();
            this.BindingContext = new PopUpViewModel(message);

        }
    }
}