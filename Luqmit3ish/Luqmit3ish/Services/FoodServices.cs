using Luqmit3ish.Models;
using Luqmit3ish.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Luqmit3ish.Services
{
    class FoodServices
    {
        private readonly HttpClient _http;
        private static readonly string ApiUrl = "https://luqmit3ishserver.azurewebsites.net/api/Food";
        private static readonly string AddDishUrl = "https://luqmit3ishserver.azurewebsites.net/api/Food/AddDish";
        
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
        public async Task<ObservableCollection<Dish>> GetFoodByResId(int userId)
        {
            var response = await _http.GetAsync($"{ApiUrl}/Restaurant/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Dish>>(content);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new Exception($"Failed to retrieve food: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        public ObservableCollection<Dish> Dishes { get; }

        public ObservableCollection<Dish> GetFoodTest()
        {
            return new ObservableCollection<Dish>
            {
                new Dish { id = 2,keep_listed =3, user_id = 3,photo="https://res.cloudinary.com/hesvvq3zo/image/upload/c_scale,w_1000/v1628074146/01t3V000000zhOuQAI/Images__c/Pizza_Strabuona.jpg.jpg",number = 5, name = "Pizza", description = "mushroom, onion, cheese", pick_up_time = new DateTime(2020, 3, 23) },
                new Dish { id = 2,keep_listed =3, user_id = 3,photo="https://res.cloudinary.com/hesvvq3zo/image/upload/c_scale,w_1000/v1628074146/01t3V000000zhOuQAI/Images__c/Pizza_Strabuona.jpg.jpg",number = 5, name = "Cheese Pizza", description = "mushroom, onion, cheese", pick_up_time = new DateTime(2020, 3, 23) }

        };
        }

        public async Task<bool> AddNewDish(DishRequest dishRequest)
        {
            var json = JsonConvert.SerializeObject(dishRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(AddDishUrl, content);

            return response.IsSuccessStatusCode;
        }

    }
}


