﻿using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class RestaurantOrderViewModel : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ProfileCommand { protected set; get; }
        public RestaurantOrderViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            ProfileCommand = new Command(async () => await OnProfileClicked());
        }
        private async Task OnProfileClicked()
        {
            await Navigation.PushAsync(new OtherProfilePage());
        }
    }
}
