using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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