using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Luqmit3ish.Models;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class FilterFoodViewModel : BindableObject, INotifyPropertyChanged
    {
       public FilterFoodViewModel()
        {
            _typeValues = new ObservableCollection<TypeField>
            {
                 new TypeField { Value = TypeFieldValue.Food, Name = "Food", IconText = "\ue4c6;" },
                new TypeField { Value = TypeFieldValue.Drink, Name = "Drink", IconText = "\uf4e3;" },
                new TypeField { Value = TypeFieldValue.Cake, Name = "Cake", IconText = "\uf1fd;" },
                new TypeField { Value = TypeFieldValue.Snack, Name = "Snack", IconText = "\uf564;" },
                new TypeField { Value = TypeFieldValue.Candies, Name = "Candies", IconText = "\uf786;" },
                new TypeField { Value = TypeFieldValue.Fish, Name = "Fish", IconText = "\uf578;" },
            };

             _locationValues = new ObservableCollection<LocationField>
            {
                 new LocationField{ Value = LocationValue.Nablus, Name="Nablus" },
                 new LocationField{ Value = LocationValue.Ramallah, Name="Ramallah" },
                 new LocationField{ Value = LocationValue.Jenin, Name="Jenin" },
                 new LocationField{ Value = LocationValue.Jericho, Name="Jericho" },
                 new LocationField{ Value = LocationValue.Jerusalem, Name="Jerusalem" },
                 new LocationField{ Value = LocationValue.Tulkarm, Name="Tulkarm" },
            };
        }

        private ObservableCollection<TypeField> _typeValues;
        public ObservableCollection<TypeField> TypeValues
        {
            get => _typeValues;
            set
            {
                if (_typeValues == value) return;
                _typeValues = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<LocationField> _locationValues;
        public ObservableCollection<LocationField> LocationValues
        {
            get => _locationValues;
            set
            {
                if (_locationValues == value) return;
                _locationValues = value;
                OnPropertyChanged();
            }
        }

        private LocationField _selectedLocation;
        public LocationField SelectedLocation
        {
            get { return _selectedLocation; }
            set
            {
                if (_selectedLocation != value)
                {
                    _selectedLocation = value;
                    OnPropertyChanged(nameof(LocationField));

                    if (_selectedLocation != null)
                    {
                        foreach (var location in LocationValues)
                        {
                            location.IsSelected = location == _selectedLocation;
                        }
                    }
                }
            }
        }

        private TypeField selectedType;
        public TypeField SelectedType
        {
            get { return selectedType; }
            set
            {
                if (selectedType != value)
                {
                    selectedType = value;
                    OnPropertyChanged(nameof(SelectedType));

                    if (selectedType != null)
                    {
                        foreach (var type in TypeValues)
                        {
                            type.IsSelected = type == selectedType;
                        }
                    }
                }
            }
        }

        
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
                }
            }
        }

    }
}
