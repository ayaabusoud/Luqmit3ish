using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Luqmit3ish.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string PrpertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PrpertyName));
        }

        public void SetValue<T>(ref T backingField, T value, [CallerMemberName] string prototypeName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value)) return;

            backingField = value;
            OnPropertyChanged(prototypeName);
        }
    }
}

