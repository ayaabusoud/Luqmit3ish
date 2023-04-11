using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Luqmit3ish.Models
{
    public class TypeField : INotifyPropertyChanged
    {
        public TypeFieldValue Value { get; set; }
        public string Name { get; set; }
        public string IconText { get; set; }
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
                    OnPropertyChanged(nameof(BackgroundColor));
                    OnPropertyChanged(nameof(FrameBackgroundColor));
                    OnPropertyChanged(nameof(TextBackgroundColor));
                }
            }
        }

        public Color BackgroundColor => IsSelected ? Color.FromArgb(237, 242, 245) : Color.Transparent;
        public Color FrameBackgroundColor => IsSelected ? Color.FromArgb(0x4D, 0x6B, 0xA3) : Color.FromArgb(0x4D, 0x6B, 0xA3, 0xFF);
        public Color TextBackgroundColor => IsSelected ? Color.White : Color.FromArgb(0x4D, 0x6B, 0xA3);
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
