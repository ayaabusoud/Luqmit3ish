using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Luqmit3ish.Models
{
    public class LocationField : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                    OnPropertyChanged(nameof(FrameBackgroundColor));
                    OnPropertyChanged(nameof(TextBackgroundColor));
                }
            }
        }

        public Color FrameBackgroundColor => IsSelected ? Color.FromArgb(249, 117, 21) : Color.Transparent;
        public Color TextBackgroundColor => IsSelected ? Color.White : Color.FromArgb(249, 117, 21);
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

