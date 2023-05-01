using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    public class ResturantOrderDetailsViewModle : ViewModelBase
    {
        private INavigation _navigation { get; set; }

        private OrderCard _order;
        public OrderCard Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        public ICommand ProfileCommand { protected set; get; }

        public ResturantOrderDetailsViewModle(OrderCard order, INavigation navigation)
        {
            this._navigation = navigation;
            this._order = order;
            ProfileCommand = new Command<User>(async (User restaurant) => await OnProfileClicked(restaurant));
        }

        private async Task OnProfileClicked(User restaurant)
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new OtherProfilePage(restaurant));

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
