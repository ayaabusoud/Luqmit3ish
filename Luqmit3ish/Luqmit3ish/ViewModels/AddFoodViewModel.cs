using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Luqmit3ish.Exceptions;
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
        private int _userId;
        private FoodServices _foodServices;

        public ICommand SubmitCommand { protected set; get; }
        public ICommand TakePhotoCommand { protected set; get; }
        public ICommand KeepValidPlusCommand { protected set; get; }
        public ICommand KeepValidMinusCommand { protected set; get; }
        public ICommand QuantityPlusCommand { protected set; get; }
        public ICommand QuantityMinusCommand { protected set; get; }

        public AddFoodViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            _foodServices = new FoodServices();
            _typeValues = Constants.TypeValues;
            _userId = GetUserId();

            SubmitCommand = new Command(async () => await OnSubmitClicked());
            TakePhotoCommand = new Command(async () => await PhotoClicked());
            KeepValidPlusCommand = new Command(OnKeepValidPlusClicked);
            KeepValidMinusCommand = new Command(OnKeepValidMinusClicked);
            QuantityPlusCommand = new Command(OnQuantityPlusClicked);
            QuantityMinusCommand = new Command(OnQuantityMinusClicked);
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
                await PopNavigationAsync(InternetMessage);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(HttpRequestMessage);
            }
            catch (NotAuthorizedException e)
            {
                Debug.WriteLine(e.Message);
                NotAuthorized();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        private async Task OnSubmitClicked()
        {
            try
            {
                if (!IsDishDataValid())
                {
                    await PopNavigationAsync("Please fill in all fields");
                    return;
                }
                if (_photoPath == null)
                {
                    await PopNavigationAsync("Please select or take a photo first.");
                    return;
                }

                DishRequest foodRequest = CreateDishRequest();

                var response = await _foodServices.AddNewDish(foodRequest);
                if (response)
                {
                    await _navigation.PopAsync();
                    await PopNavigationAsync("The dish has been added successfully");
                    return;
                }
                await PopNavigationAsync("The dish was not added.");
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(InternetMessage);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(HttpRequestMessage);
            }
            catch (EmptyIdException e)
            {
                Debug.WriteLine(e.Message);
                EndSession();
            }
            catch (EmailNotFoundException e)
            {
                Debug.WriteLine(e.Message);
                EndSession();
            }
            catch (NotAuthorizedException e)
            {
                Debug.WriteLine(e.Message);
                NotAuthorized();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopNavigationAsync(ExceptionMessage);
            }
        }

        private bool IsDishDataValid()
        {
            return (_type != null && _title != null && _description != null && KeepValid != 0 && Quantity != 0);
        }

        private DishRequest CreateDishRequest()
        {
            var foodRequest = new DishRequest()
            {
                UserId = _userId,
                Photo = _photoPath,
                Type = _type,
                Name = _title,
                Description = _description,
                keepValid = KeepValid,
                Quantity = Quantity
            };
            return foodRequest;
        }


        private void OnKeepValidMinusClicked()
        {
            if (KeepValid > 0)
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
            if (Quantity > 0)
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
    }
}