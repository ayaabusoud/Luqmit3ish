using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Models;
using Luqmit3ish.Services;
using Luqmit3ish.Utilities;
using Luqmit3ish.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
namespace Luqmit3ish.ViewModels
{
    class AddFoodViewModel : ViewModelBase
    {
        private INavigation _navigation { get; set; }
        private IFoodServices _foodServices;
        public ICommand SubmitCommand { protected set; get; }
        public ICommand Photo_clicked { protected set; get; }
        public ICommand Blus { get; private set; }
        public ICommand Minus { protected get; set; }
        public ICommand KeepListedBlus { protected get; set; }
        public ICommand KeepListedMinus { protected get; set; }
        public ICommand TakePhotoCommand { protected set; get; }
        public ICommand KeepValidPlusCommand { protected set; get; }
        public ICommand KeepValidMinusCommand { protected set; get; }
        public ICommand QuantityPlusCommand { protected set; get; }
        public ICommand QuantityMinusCommand { protected set; get; }

        public AddFoodViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            SubmitCommand = new Command(async () => await OnSubmitClicked());
            _foodServices = new FoodServices();
            Photo_clicked = new Command(async () => await PhotoClicked());
            TakePhotoCommand = new Command(async () => await PhotoClicked());
            KeepValidPlusCommand = new Command(OnKeepValidPlusClicked);
            KeepValidMinusCommand = new Command(OnKeepValidMinusClicked);
            QuantityPlusCommand = new Command(OnQuantityPlusClicked);
            QuantityMinusCommand = new Command(OnQuantityMinusClicked);
            _typeValues = Constants.TypeValues;
        }

        public ICommand MyCollectionSelectedCommand => new Command(() =>
        {
            _type = SelectedTypeName;
        });

        private async Task PhotoClicked()
        {
            try
            {
                await PopupNavigation.Instance.PushAsync(new UploadImagePopUp());
                MessagingCenter.Subscribe<UploadImagePopUpViewModel, string>(this, "PhotoPath", (sender, photoPath) =>
                {
                    _photoPath = photoPath;
                    OnPropertyChanged(nameof(PhotoPath));
                });
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }


        private async Task OnSubmitClicked()
        {
            try
            {
                string id = Preferences.Get("userId", "0");

                if (id is null)
                {
                    return;
                }
                int userId = int.Parse(id);

                if (_type == null || _title == null || _description == null || KeepValid == 0 || Quantity == 0)
                {
                    await PopupNavigation.Instance.PushAsync(new PopUp("Please fill in all fields"));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                    return;
                }
                if (_photoPath == null)
                {
                    await PopupNavigation.Instance.PushAsync(new PopUp("Please select or take a photo first."));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                    return;
                }

                DishRequest foodRequest = new DishRequest()
                {
                    UserId = userId,
                    Photo = _photoPath,
                    Type = _type,
                    Name = _title,
                    Description = _description,
                    keepValid = KeepValid,
                    Quantity = Quantity
                };

                var response = await _foodServices.AddNewDish(foodRequest);
                if (response)
                {
                    await _navigation.PopAsync();
                    await PopupNavigation.Instance.PushAsync(new PopUp("The dish has been added successfully"));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                    return;
                }
                await PopupNavigation.Instance.PushAsync(new PopUp("The dish was not added."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
            }
        }

        private void OnKeepValidMinusClicked()
        {
            if (KeepValid == 0)
            {
                return;
            }
            else
            {
                KeepValid--;
            }
        }

        private void OnQuantityPlusClicked()
        {
            Quantity++;
        }

        private void OnQuantityMinusClicked()
        {
            if (Quantity == 0)
            {
                return;
            }
            else
            {
                Quantity--;
            }
        }

        private ObservableCollection<TypeField> _typeValues;
        public ObservableCollection<TypeField> TypeValues
        {
            get => _typeValues;
            set => SetProperty(ref _typeValues, value);
        }

        private string _proximateNumberValue = "1";
        public string ProximateNumberValue
        {
            get => _proximateNumberValue;
            set => SetProperty(ref _proximateNumberValue, value);
        }

        private string _keepListedValue = "1";
        public string KeepListedValue
        {
            get => _keepListedValue;
            set => SetProperty(ref _keepListedValue, value);
        }

        private ImageSource _img;
        public ImageSource Img
        {
            get => _img;
            set => SetProperty(ref _img, value);
        }

        private TypeField _selectedType;
        public TypeField SelectedType
        {
            get => _selectedType;
            set
            {
                SetProperty(ref _selectedType, value);
                if (_selectedType != null)
                {
                    foreach (var type in TypeValues)
                    {
                        type.IsSelected = type == _selectedType;
                    }
                }
            }
        }

        public string SelectedTypeName
        {
            get => SelectedType?.Name;
        }

        private void OnKeepValidPlusClicked()
        {
            KeepValid++;
        }

        private int _keepValid = 0;
        public int KeepValid
        {
            get => _keepValid;
            private set => SetProperty(ref _keepValid, value);
        }

        private int _quantity = 0;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        private string _photoPath;
        public string PhotoPath
        {
            get => _photoPath;
            set => SetProperty(ref _photoPath, value);
        }

        private string _type;
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private int _keepListed;
        public int Keep_listed
        {
            get => _keepListed;
            set => SetProperty(ref _keepListed, value);
        }

        private int _proximateNumber;
        public int Number
        {
            get => _proximateNumber;
            set => SetProperty(ref _proximateNumber, value);
        }
    }
}