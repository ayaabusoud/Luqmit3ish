using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Luqmit3ish.Connection;
using Luqmit3ish.Exceptions;
using Luqmit3ish.Models;
using Luqmit3ish.ViewModels;
using Newtonsoft.Json;

namespace Luqmit3ish.Services
{
    class OrderService
    {
        private readonly HttpClient _httpClient;
        private  readonly string _apiUrl = "https://luqmit3ishv2.azurewebsites.net/api/Orders";
        private  readonly string _orderApiUrl = "https://luqmit3ishv2.azurewebsites.net/api/CharityOrders";
        private  readonly string _restaurantApiUrl = "https://luqmit3ishv2.azurewebsites.net/api/RestaurantOrders";
        private readonly string _bestRestaurantUrl = "https://luqmit3ishv2.azurewebsites.net/BestRestaurant";
        private IConnection _connection;

            public OrderService()
        {
            _httpClient = new HttpClient();
            _connection = new Connection();
        }

        public async Task<ObservableCollection<OrderCard>> GetOrders(int id)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                var response = await _httpClient.GetAsync($"{_orderApiUrl}/{id}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<OrderCard>>(content);

            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        
       public async Task<ObservableCollection<OrderCard>> GetRestaurantOrders(int id,bool receieve)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                var response = await _httpClient.GetAsync($"{_restaurantApiUrl}/{id}/{receieve}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<OrderCard>>(content);
            }catch(HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
        public async Task<DishesOrder> GetBestRestaurant()
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                var response = await _httpClient.GetAsync(_bestRestaurantUrl);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<DishesOrder>(content);
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public async Task<Order> GetOrderById(int id)

        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");

                
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Order>(content);
                
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
            catch (Exception e)
            {

                throw new Exception($"Failed to retrieve user"+e.Message);
            }
            
        }

        public async Task<bool> UpdateOrderDishCount(int id, string operation)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {

                var patchObject = new { id, operation };
                var patchData = JsonConvert.SerializeObject(patchObject);
                var httpContent = new StringContent(patchData, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(new HttpMethod("PATCH"), _orderApiUrl + "/" + id + "/" + operation)
                {
                    Content = httpContent
                };

                var response = await _httpClient.SendAsync(request);

                return response.IsSuccessStatusCode;

            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);


            } 
         
        }

        public async Task<bool> ReserveOrder(Order orderRequest)
        {
            if (_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                var json = JsonConvert.SerializeObject(orderRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_apiUrl, content);
                return response.IsSuccessStatusCode;
            }
            catch(HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public async Task<bool> DeleteOrder(int charityId, int restaurantId)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
            var response = await _httpClient.DeleteAsync($"{_apiUrl}/delete/{charityId}/{restaurantId}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
            catch (Exception e)
            {
                return false;
            }


        }
    }
}


