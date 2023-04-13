using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Luqmit3ish.Models;
using Luqmit3ish.ViewModels;
using Newtonsoft.Json;

namespace Luqmit3ish.Services
{
    class OrderService
    {
        private readonly HttpClient _http;
        private static readonly string ApiUrl = "https://luqmit3ish.azurewebsites.net/api/Orders";
        private static readonly string OrderApiUrl = "https://luqmit3ish.azurewebsites.net/api/CharityOrders";

        public OrderService()
        {
            _http = new HttpClient();
        }
        public async Task<ObservableCollection<OrderCard>> GetOrders(int id)
        {
            var response = await _http.GetAsync($"{OrderApiUrl}/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ObservableCollection<OrderCard>>(content);
        }
        public async Task<ObservableCollection<OrderCard>> GetRestaurantOrders(int id,bool receieve)
        {
            var response = await _http.GetAsync("https://luqmit3ish.azurewebsites.net/api/RestaurantOrders/"+id+"/"+ receieve);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ObservableCollection<OrderCard>>(content);
        }
        public async Task<Order> GetOrderById(int id)
        {
            var response = await _http.GetAsync($"{ApiUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(content);
                return order;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new Exception($"Failed to retrieve user_id: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        public async Task<bool> UpdateOrderDishCount(int id, string operation)
        {
            var patchObject = new { id = id, operation = operation };
            var patchData = JsonConvert.SerializeObject(patchObject);
            var httpContent = new StringContent(patchData, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), "https://luqmit3ish.azurewebsites.net/api/CharityOrders/" + id + "/" + operation)
            {
                Content = httpContent
            };

            try
            {
                var response = await _http.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(response.StatusCode);
                    return true;
                }
                else
                {
                    Debug.WriteLine(response.StatusCode);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught: " + ex.Message);
                return false;
            }
        }

         public async Task<bool> ReserveOrder(Order orderRequest)
        {
            var json = JsonConvert.SerializeObject(orderRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(ApiUrl, content);
            return response.IsSuccessStatusCode;
        }
        public async Task DeleteOrder(int charityId, int restaurantId)
        {
            var response = await _http.DeleteAsync($"{ApiUrl}/delete/{charityId}/{restaurantId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("delete failed");
            }
        }
    }
}
