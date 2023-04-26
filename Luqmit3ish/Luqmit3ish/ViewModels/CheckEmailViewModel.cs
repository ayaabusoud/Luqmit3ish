using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class CheckEmailViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        private string _email;

        public ICommand ResetCommand { protected set; get; }
        public CheckEmailViewModel(INavigation navigation, string email)
        {
            _email = email;
            this._navigation = navigation;
            ResetCommand = new Command(OnResetClicked);

        }

        private void OnResetClicked()
        {
            try
            {
                 Application.Current.MainPage = new ResetPasswordForgetPage(_email);

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