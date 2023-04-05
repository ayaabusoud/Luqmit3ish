using Luqmit3ish.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Luqmit3ish.Services
{
    class FoodServices
    {
        private readonly HttpClient _http;
        private static readonly string ApiUrl = "https://luqmit3ishserver.azurewebsites.net/api/Food";
        
        public FoodServices()
        {
            _http = new HttpClient();
        }
        public async Task<ObservableCollection<Dish>> GetFood()
        {
            var response = await _http.GetAsync(ApiUrl);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ObservableCollection<Dish>>(content);
        }

        public async Task<ObservableCollection<DishCard>> GetDishCards()
        {
            var response = await _http.GetAsync(ApiUrl+ "/DishCard");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ObservableCollection<DishCard>>(content);
        }


    }
}


