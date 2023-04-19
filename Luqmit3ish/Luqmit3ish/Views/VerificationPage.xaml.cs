using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Luqmit3ish.ViewModels;
using Luqmit3ish.Models;

namespace Luqmit3ish.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerificationPage : ContentPage
    {
        public VerificationPage(SignUpRequest signUpRequest)
        {
            InitializeComponent();
            this.BindingContext = new VerificationViewModel(signUpRequest);
        }
    }
}