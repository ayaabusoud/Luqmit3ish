using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Luqmit3ish.Interfaces;
using Xamarin.Forms;

namespace Luqmit3ish.Models
{
    public class TypeField : INotifyPropertyChanged, ISelectable
    {
        public ICommand SelectedCommand { get; }

        public string Name { get; set; }
        public string IconText { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TypeField()
        {
            SelectedCommand = new Command<TypeField>(OnSelected);
        }
        
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                {
                    return;
                }
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(FrameBackgroundColor));
                OnPropertyChanged(nameof(TextBackgroundColor));
            }
        }

        private void OnSelected(TypeField field)
        {
            field.IsSelected = !field.IsSelected;
        }

        public System.Drawing.Color FrameBackgroundColor => IsSelected ? System.Drawing.Color.FromArgb(249, 117, 21) : System.Drawing.Color.White;
        public System.Drawing.Color TextBackgroundColor => IsSelected ? System.Drawing.Color.White : System.Drawing.Color.FromArgb(249, 117, 21);

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}