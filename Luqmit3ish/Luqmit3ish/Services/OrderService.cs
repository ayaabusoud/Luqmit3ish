using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            var response = await _http.GetAsync($"{ OrderApiUrl}/{id}");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ObservableCollection<OrderCard>>(content);
        }
    }
}
