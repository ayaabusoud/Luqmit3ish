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

        public System.Drawing.Color FrameBackgroundColor => IsSelected ? System.Drawing.Color.FromArgb(249, 117, 21) : System.Drawing.Color.FromArgb(0x4D, 249, 117, 21);
        public System.Drawing.Color TextBackgroundColor => IsSelected ? System.Drawing.Color.White : System.Drawing.Color.FromArgb(249, 117, 21);
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

