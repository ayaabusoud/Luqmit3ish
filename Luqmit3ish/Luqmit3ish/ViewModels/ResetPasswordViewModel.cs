﻿using Luqmit3ish.Views;
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
    class ResetPasswordViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        public ICommand ButtonCommand { protected set; get; }

        public ResetPasswordViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            ButtonCommand = new Command(async () => await OnButtonClicked());
        }

        private async Task OnButtonClicked()
        {
            try
            {
            await _navigation.PushModalAsync(new LoginPage());

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

