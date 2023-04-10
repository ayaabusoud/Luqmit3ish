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

        public OrderService()
        {
            _http = new HttpClient();
        }
         public async Task<bool> ReserveOrder(Order orderRequest)
        {
            var json = JsonConvert.SerializeObject(orderRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(ApiUrl, content);
            return response.IsSuccessStatusCode;
        }
    }
}
