using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Rg.Plugins.Popup.Services;
using System.Threading;
using Luqmit3ish.Views;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Luqmit3ish.Exceptions;
using System.Collections.ObjectModel;

namespace Luqmit3ish.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected string InternetMessage { get; } = "Please Check your internet connection.";
        protected string HttpRequestMessage { get; } = "Something went wrong, please try again.";
        protected string ExceptionMessage { get; } = "Something went wrong, please try again.";
        protected string SessionEndedMessage { get; } = "Your session have been ended.";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;
            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual async Task PopNavigationAsync(string message)
        {
            await PopupNavigation.Instance.PushAsync(new PopUp(message));
            Thread.Sleep(3000);
            await PopupNavigation.Instance.PopAsync();
        }

        protected int GetUserId()
        {
            string id = Preferences.Get("userId", null);
            if (string.IsNullOrEmpty(id))
            {
                throw new EmptyIdException("The user id is empty.");
            }
            return int.Parse(id);
        }

        protected string GetEmail()
        {
            string email = Preferences.Get("userEmail", null);
            if (string.IsNullOrEmpty(email))
            {
                throw new EmailNotFoundException("Email not found in preferences.");
            }
            return email;
        }

        protected void ClearFilterUsers<T>(ObservableCollection<T> collection)
        {
            if (collection != null)
            {
                collection.Clear();
                collection = null;
            }
        }
    }
}