using Luqmit3ish.Connection;
using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Models;
using Luqmit3ish.Utilities;
using Luqmit3ish.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Luqmit3ish.Services
{
    class FoodServices : IFoodServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = Constants.BaseUrl + "api/Food";
        private readonly IConnection _connection;

        private const string NoInternetConnectionMessage = "There is no internet connection";
        private const string NotAuthorizedMessage = "You are not authorized to do this operation";
        private const string DeleteItemErrorMessage = "Failed to delete the item. Please try again later.";

        public FoodServices()
        {
            _httpClient = new HttpClient();
            _connection = new InternetConnection();
        }

        public async Task<ObservableCollection<Dish>> GetFood()
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException(NoInternetConnectionMessage);
            }
            try
            {
                var response = await _httpClient.GetAsync(_apiUrl);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Dish>>(content);
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

        public async Task<ObservableCollection<Dish>> GetFoodByResId(int userId)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException(NoInternetConnectionMessage);
            }
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/Restaurant/{userId}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<Dish>>(content);
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

        public async Task<ObservableCollection<DishCard>> GetSearchCards(string searchRequest, string type)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException(NoInternetConnectionMessage);
            }
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/Search/{searchRequest}/{type}");
                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ObservableCollection<DishCard>>(content);
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

        public async Task<Dish> GetFoodById(int food_id)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException(NoInternetConnectionMessage);
            }
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/{food_id}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Dish>(content);
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw new Exception($"Failed to retrieve food: {response.StatusCode} - {response.ReasonPhrase}");
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

        public async Task<bool> UpdateDish(Dish dishRequest)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException(NoInternetConnectionMessage);
            }
            try
            {
                string token = Preferences.Get("Token", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                    throw new NotAuthorizedException(NotAuthorizedMessage);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                var json = JsonConvert.SerializeObject(dishRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(_apiUrl + "/" + dishRequest.Id, content);
                await UploadPhoto(dishRequest.Photo, dishRequest.Id);

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

        public async Task<bool> AddNewDish(Dish dishRequest)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException(NoInternetConnectionMessage);
            }
            try
            {
                string token = Preferences.Get("Token", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                    throw new NotAuthorizedException(NotAuthorizedMessage);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                var json = JsonConvert.SerializeObject(dishRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiUrl}/AddDish", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    dynamic responseObject = JsonConvert.DeserializeObject(responseContent);
                    int id = (int)responseObject.id;
                    await UploadPhoto(dishRequest.Photo, dishRequest.Id);
                }
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

        public async Task<bool> UploadPhoto(string photoPath, int foodId)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException(NoInternetConnectionMessage);
            }
            try
            {
                string token = Preferences.Get("Token", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                    throw new NotAuthorizedException(NotAuthorizedMessage);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                ByteArrayContent fileContent;
                if (Uri.TryCreate(photoPath, UriKind.Absolute, out Uri uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                {
                    using (var client = new HttpClient())
                    {
                        var bytes = await client.GetByteArrayAsync(uri);
                        fileContent = new ByteArrayContent(bytes);
                    }
                }
                else
                {
                    fileContent = new ByteArrayContent(File.ReadAllBytes(photoPath));
                }

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(fileContent, "photo", Path.GetFileName(photoPath));
                    var response = await _httpClient.PostAsync($"{_apiUrl}/UploadPhoto/{foodId}", formData);
                    return response.IsSuccessStatusCode;
                }
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

        public async Task<ObservableCollection<DishCard>> GetDishCards()
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException(NoInternetConnectionMessage);
            }
            try
            {
                var response = await _httpClient.GetAsync(_apiUrl + "/DishCard");
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<DishCard>>(content);
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

        public async Task<ObservableCollection<DishCard>> GetDishCardById(int dishId)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException(NoInternetConnectionMessage);
            }
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/DishCard/{dishId}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<DishCard>>(content);
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

        public async Task DeleteFood(int food_id)
        {
            try
            {
                string token = Preferences.Get("Token", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                    throw new NotAuthorizedException(NotAuthorizedMessage);
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.DeleteAsync($"{_apiUrl}/{food_id}");

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(DeleteItemErrorMessage);
                }
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
    }
}