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
    public partial class OnBoardingPage : CarouselPage
    {
        public OnBoardingPage()
        {
            InitializeComponent();
            this.BindingContext = new OnBoardingViewModel();
        }

        private void SignupClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SignupPage());
        }
    }
}