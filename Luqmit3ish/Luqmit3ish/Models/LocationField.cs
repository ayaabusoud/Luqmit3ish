using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Luqmit3ish.Models
{
	public class LocationField : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public LocationValue Value { get; set; }

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
                    OnPropertyChanged(nameof(StackLayoutBackgroundColor));
                    OnPropertyChanged(nameof(FrameBackgroundColor));
                    OnPropertyChanged(nameof(TextBackgroundColor));
                }
            }
        }

        public Color StackLayoutBackgroundColor => IsSelected ? Color.FromArgb(237, 242, 245) : Color.FromArgb(237, 242, 245);
        public Color FrameBackgroundColor => IsSelected ? Color.FromArgb(0x4D, 0x6B, 0xA3) : Color.FromArgb(0x4D, 0x6B, 0xA3, 0xFF);
        public Color TextBackgroundColor => IsSelected ? Color.White : Color.FromArgb(0x4D, 0x6B, 0xA3);
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

