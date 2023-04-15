using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class CheckEmailViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        

        public ICommand ResetCommand { protected set; get; }
        public CheckEmailViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            ResetCommand = new Command(OnResetClicked);

        }

        private void OnResetClicked()
        {
            try
            {
                 Application.Current.MainPage = new ResetPasswordPage();

            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

    }

}
