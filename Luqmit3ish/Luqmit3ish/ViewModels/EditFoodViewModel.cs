using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
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
    class EditFoodViewModel : ViewModelBase
    {
        private int _foodId;
        private Dish _dish;
        private INavigation _navigation;
        private FoodServices _foodServices;

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

        public EditFoodViewModel(INavigation navigation, Dish dish)
        {
            _foodId = dish.Id;
            _navigation = navigation;
            _dish = dish;
            _foodServices = new FoodServices();

            SubmitCommand = new Command(async () => await OnSubmitClicked());
            Photo_clicked = new Command(async () => await PhotoClicked());
            TakePhotoCommand = new Command(async () => await PhotoClicked());

            KeepValidPlusCommand = new Command(OnKeepValidPlusClicked);
            KeepValidMinusCommand = new Command(OnKeepValidMinusClicked);
            QuantityPlusCommand = new Command(OnQuantityPlusClicked);
            QuantityMinusCommand = new Command(OnQuantityMinusClicked);

            _typeValues = Constants.TypeValues;
            SelectedType = TypeValues.FirstOrDefault();
            InitializeAsync();
        }

        private void InitializeAsync()
        {
            try
            {
                if (_dish != null)
                {
                    Type = _dish.Type;
                    Title = _dish.Name;
                    Description = _dish.Description;
                    KeepValid = _dish.KeepValid;
                    Quantity = _dish.Quantity;
                    PhotoPath = _dish.Photo;

                    var selectedTypeName = _dish.Type;
                    var selectedType = TypeValues.FirstOrDefault(tf => tf.Name == selectedTypeName);
                    SelectedType = selectedType;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private int _keepValid = 0;
        public int KeepValid
        {
            get => _keepValid;
            set => SetProperty(ref _keepValid, value);
        }

        private int _quantity = 0;
        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
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
            if (Quantity > 1)
            {
                Quantity--;
            }
        }

        private void OnKeepValidPlusClicked()
        {
            KeepValid++;
        }

        public string SelectedTypeName
        {
            get { return SelectedType?.Name; }
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

                if (_type == null || _title == null || _description == null || KeepValid == 0 || _packTime == null || Quantity == 0)
                {
                    await PopupNavigation.Instance.PushAsync(new PopUp("Please fill in all fields."));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                    return;
                }

                DishRequest foodRequest = new DishRequest()
                {
                    Id = _foodId,
                    UserId = userId,
                    Photo = "",
                    Type = _type,
                    Name = _title,
                    Description = _description,
                    keepValid = KeepValid,
                    Quantity = Quantity
                };

                UpdateDish(foodRequest);
                
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

        public async Task<ObservableCollection<Dish>> GetFood(int id)
        {
            try
            {
                ObservableCollection<Dish> request = await _foodServices.GetFoodByResId(id);
                if (request != null)
                {
                    return request;
                }
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
                return null;
            }
            catch (ConnectionException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Please Check your internet connection."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
                return null;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
                return null;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
                await PopupNavigation.Instance.PopAsync();
                return null;
            }
        }

        public async void UpdateDish(DishRequest foodRequest)
        {
            try
            {
                var request = await _foodServices.UpdateDish(foodRequest, _foodId);
                if (request)
                {
                    await UpdatePhoto(_photoPath, _foodId);
                    return;
                }
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
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

        private async Task UpdatePhoto(string photoPath, int foodId)
        {
            try
            {
                var response = await _foodServices.UploadPhoto(photoPath, foodId);
                if (response)
                {
                    await _navigation.PopAsync();
                    await PopupNavigation.Instance.PushAsync(new PopUp("The Dish have been updated successfully."));
                    Thread.Sleep(3000);
                    await PopupNavigation.Instance.PopAsync();
                    return;
                }
                await PopupNavigation.Instance.PushAsync(new PopUp("Something went wrong, please try again."));
                Thread.Sleep(3000);
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

        private TypeField _selectedType = new TypeField();
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

        private string _packTime;
        public string Pack_time
        {
            get => _packTime;
            set => SetProperty(ref _packTime, value);
        }

        private int _proximateNumber;
        public int Number
        {
            get => _proximateNumber;
            set => SetProperty(ref _proximateNumber, value);
        }
    }
}