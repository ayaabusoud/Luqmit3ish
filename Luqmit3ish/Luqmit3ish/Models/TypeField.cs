using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Luqmit3ish.Models
{
    public class TypeField : INotifyPropertyChanged
    {
        public string name { get; set; }
        public string iconText { get; set; }

        private bool isSelected;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                    OnPropertyChanged(nameof(StackLayoutBackgroundColor)); // notify the property change of StackLayoutBackgroundColor when IsSelected is changed
                    OnPropertyChanged(nameof(StackLayoutBackgroundColor1)); // notify the property change of StackLayoutBackgroundColor when IsSelected is changed
                }
            }
        }

        // your existing properties and methods...

        public Color StackLayoutBackgroundColor => IsSelected ? Color.White : Color.White; // define a computed property to determine the background color of the StackLayout based on the IsSelected value
        public Color StackLayoutBackgroundColor1 => IsSelected ? Color.FromArgb(0x4D, 0x6B, 0xA3, 0xFF) : Color.White; // define a computed property to determine the background color of the StackLayout based on the IsSelected value
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}