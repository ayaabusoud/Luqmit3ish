using Luqmit3ish.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Diagnostics;
using Luqmit3ish.Services;
using System.Collections.ObjectModel;
using Luqmit3ish.Models;

namespace Luqmit3ish.ViewModels
{
    class CharityHomeViewModel : ViewModelBase
    {
        public INavigation Navigation { get; set; }
        public ICommand FilterCommand { protected set; get; }
        public ICommand SearchCommand { protected set; get; }
        public ICommand ProfileCommand { protected set; get; }
        public ICommand FoodDetailCommand { protected set; get; }

        public FoodServices foodServices;
        public UserServices userServices;


        private ObservableCollection<Dish> _dishes;

        public ObservableCollection<Dish> Dishes
        {
            get => _dishes;
            set => SetProperty(ref _dishes, value);
        }
        private ObservableCollection<DishCard> _dishCard;

        public ObservableCollection<DishCard> DishCard
        {
            get => _dishCard;
            set => SetProperty(ref _dishCard, value);
        }

        public  CharityHomeViewModel(INavigation navigation)
        {
            this.Navigation = navigation;
            FilterCommand = new Command(async () => await OnFilterClicked());
            SearchCommand = new Command(async () => await OnSearchClicked());
            ProfileCommand = new Command(async () => await OnProfileClicked());
            FoodDetailCommand = new Command(async () => await OnFoodDetailClicked());
            foodServices = new FoodServices();
            userServices = new UserServices();
            OnInit();
        }

        private async Task OnInit()
        {
            DishCard = await foodServices.GetDishCards();
            Debug.WriteLine(DishCard[0].dishName);
        }

        private async Task OnFilterClicked()
        {
            try
            {
                await Navigation.PushAsync(new FilterFoodPage());
            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }
        private async Task OnSearchClicked()
        {
            try
            {
                await Navigation.PushAsync(new SearchPage());

            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        private async Task OnProfileClicked()
        {
            try
            {
                await Navigation.PushAsync(new OtherProfilePage());

            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        private async Task OnFoodDetailClicked()
        {
            try
            {
                await Navigation.PushAsync(new FoodDetailPage());

            }
            catch (ArgumentException e)
            {
                Debug.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}