using System;
using Luqmit3ish.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Luqmit3ish.Interfaces
{
	public interface IOrderService
	{
        Task<ObservableCollection<OrderCard>> GetOrders(int id);
        Task<ObservableCollection<OrderCard>> GetRestaurantOrders(int id, bool receieve);
        Task<DishesOrder> GetBestRestaurant();
        Task<Order> GetOrderById(int id);
        Task<bool> UpdateOrderDishCount(int id, string operation);
        Task<bool> ReserveOrder(Order orderRequest);
        Task<bool> DeleteOrder(int charityId, int restaurantId);
        Task<bool> UpdateOrderReceiveStatus(int id);
    }
}