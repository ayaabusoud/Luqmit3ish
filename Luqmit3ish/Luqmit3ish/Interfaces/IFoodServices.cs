using System;
using Luqmit3ish.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Luqmit3ish.Interfaces
{
	public interface IFoodServices
	{
        Task<ObservableCollection<Dish>> GetFood();
        Task<ObservableCollection<Dish>> GetFoodByResId(int userId);
        Task<ObservableCollection<DishCard>> GetSearchCards(string searchRequest, string type);
        Task<Dish> GetFoodById(int food_id);
        Task<bool> UpdateDish(Dish dishRequest);
        Task<bool> AddNewDish(DishRequest dishRequest);
        Task<bool> UploadPhoto(string photoPath, int foodId);
        Task<ObservableCollection<DishCard>> GetDishCards();
        Task<ObservableCollection<DishCard>> GetDishCardById(int dishId);
        Task DeleteFood(int food_id);
    }
}