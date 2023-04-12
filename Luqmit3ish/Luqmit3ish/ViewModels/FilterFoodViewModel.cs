using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Luqmit3ish.Models;
using Xamarin.Forms;

namespace Luqmit3ish.ViewModels
{
    class FilterFoodViewModel : BindableObject, INotifyPropertyChanged
    {
        public ICommand Apply { get; set; }
        public ICommand ClearAll { get; set; }
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
            Apply = new Command(OnApply);
            ClearAll = new Command(OnClearAll);
            _upperQuantity = _upperKeepValid = 10;
            _lowerQuantity = _lowerKeepValid = 0;
            
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.PreviousSelection)
            {
                var typeField = item as TypeField;
                typeField.IsSelected = false;
            }

            foreach (var item in e.CurrentSelection)
            {
                var typeField = item as TypeField;
                typeField.IsSelected = true;
            }
        }


        private ObservableCollection<TypeField> _selectedItems = new ObservableCollection<TypeField>();
        public ObservableCollection<TypeField> SelectedItems
        {
            get
            {
                var selectedTypes = _typeValues.Where(t => t.IsSelected).ToList();
                _selectedItems.Clear();
                foreach (var type in selectedTypes)
                {
                    _selectedItems.Add(type);
                }
                return _selectedItems;
            }
            set
            {
                if (_selectedItems == value)
                {
                    return;
                }
                _selectedItems = value;
                OnPropertyChanged(nameof(SelectedItems));
            }
        }

        private void OnClearAll()
        {
            foreach (var type in _typeValues)
            {
                Console.WriteLine("Selected Type: "+ type.Name +" " + type.IsSelected);
            }
            Console.WriteLine("___________________");

            foreach (var type in SelectedItems)
            {
                Console.WriteLine("Selected Type: " + type.Name);
            }
            SelectedItems.Clear();
        }

        private void OnApply()
        {
            throw new NotImplementedException();
        }

        private int _upperKeepValid;
        public int UpperKeepValid
        {
            get => _upperKeepValid;
            set
            {
                if(_upperKeepValid == value)
                {
                    return;
                }
                _upperKeepValid = value;
                OnPropertyChanged(nameof(UpperKeepValid));
            }
        }

        private int _lowerKeepValid;
        public int LowerKeepValid
        {
            get => _lowerKeepValid;
            set
            {
                if (_lowerKeepValid == value)
                {
                    return;
                }
                _lowerKeepValid = value;
                OnPropertyChanged(nameof(LowerKeepValid));
            }
        }

        private int _lowerQuantity;
        public int LowerQuantity
        {
            get => _lowerQuantity;
            set
            {
                if (_lowerQuantity == value)
                {
                    return;
                }
                _lowerQuantity = value;
                OnPropertyChanged(nameof(LowerQuantity));
            }
        }

        private int _upperQuantity;
        public int UpperQuantity
        {
            get => _upperQuantity;
            set
            {
                if (_upperQuantity == value)
                {
                    return;
                }
                _upperQuantity = value;
                OnPropertyChanged(nameof(_upperQuantity));
            }
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

        private TypeField _selectedType;
        public TypeField SelectedType
        {
            get { return _selectedType; }
            set
            {
                if (_selectedType != value)
                {
                    if (_selectedType != null)
                    {
                        _selectedType.IsSelected = false;
                    }
                    _selectedType = value;
                    OnPropertyChanged(nameof(_selectedType));

                    if (_selectedType != null)
                    {
                        foreach (var type in TypeValues)
                        {
                            type.IsSelected = type == _selectedType;
                        }
                    }
                }
            }
        }
    }
}
