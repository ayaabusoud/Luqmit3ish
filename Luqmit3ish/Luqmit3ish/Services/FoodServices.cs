using Luqmit3ish.Models;
using Luqmit3ish.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Luqmit3ish.Services
{
    class FoodServices
    {
        private readonly HttpClient _http;
        private static readonly string ApiUrl = "https://luqmit3ish.azurewebsites.net/api/Food";
        private static readonly string AddDishUrl = "https://luqmit3ish.azurewebsites.net/api/Food/AddDish";
        private static readonly string uploadPhotoUrl = "https://luqmit3ish.azurewebsites.net/api/Food/UploadPhoto";
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

        
   public async Task<Dish> GetFoodById(int food_id)
        {
            var response = await _http.GetAsync($"{ApiUrl}/{food_id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Dish>(content);
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

        public async Task<bool> UpdateDish(DishRequest dishRequest, int food_id)
        {
            var json = JsonConvert.SerializeObject(dishRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PutAsync(ApiUrl + "/" + food_id, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<int> AddNewDish(DishRequest dishRequest)
        {
            var json = JsonConvert.SerializeObject(dishRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(AddDishUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                dynamic responseObject = JsonConvert.DeserializeObject(responseContent);
                return (int)responseObject.id;
            }
            return 0;
        }
        
        public async Task DeleteFood(int food_id)
        {
            var response = await _http.DeleteAsync($"{ApiUrl}/{food_id}");

            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine("failed to delete item");
            }
        }

        internal async Task<bool> UploadPhoto(string photoPath, int foodId)
        {
            var fileContent = new ByteArrayContent(File.ReadAllBytes(photoPath));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(fileContent, "photo", Path.GetFileName(photoPath));
                var response = await _http.PostAsync($"{uploadPhotoUrl}/{foodId}", formData);
                return response.IsSuccessStatusCode;
            }
        }
    }
}
