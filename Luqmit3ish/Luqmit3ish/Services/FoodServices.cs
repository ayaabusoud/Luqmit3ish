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

        public ObservableCollection<Dish> Dishes { get; }

        public ObservableCollection<Dish> GetFoodTest()
        {
            return new ObservableCollection<Dish>
            {
                new Dish { id = 2,keep_listed =3, user_id = 3,photo="https://res.cloudinary.com/hesvvq3zo/image/upload/c_scale,w_1000/v1628074146/01t3V000000zhOuQAI/Images__c/Pizza_Strabuona.jpg.jpg",number = 5, name = "Pizza", description = "mushroom, onion, cheese", pick_up_time = new DateTime(2020, 3, 23) },
                new Dish { id = 2,keep_listed =3, user_id = 3,photo="https://res.cloudinary.com/hesvvq3zo/image/upload/c_scale,w_1000/v1628074146/01t3V000000zhOuQAI/Images__c/Pizza_Strabuona.jpg.jpg",number = 5, name = "Cheese Pizza", description = "mushroom, onion, cheese", pick_up_time = new DateTime(2020, 3, 23) }

        };
        }

    }
}


