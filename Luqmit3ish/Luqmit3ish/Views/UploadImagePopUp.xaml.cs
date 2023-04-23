using System;
using System.Collections.Generic;
using Luqmit3ish.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Luqmit3ish.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadImagePopUp : Rg.Plugins.Popup.Pages.PopupPage
    {
        public UploadImagePopUp()
        {
            InitializeComponent();
            this.BindingContext = new UploadImagePopUpViewModel(Navigation);
        }
    }
}