using System;
using System.Collections.Generic;
using System.Text;

namespace Luqmit3ish.ViewModels
{
    class PopUpViewModel : ViewModelBase
    {
        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
        public PopUpViewModel(string message)
        {
            _message = message;
        }
    }
}
