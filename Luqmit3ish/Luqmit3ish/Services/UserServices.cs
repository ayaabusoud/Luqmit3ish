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
    public class UserServices
    {
        private readonly HttpClient _http;
        private static readonly string ApiUrl = "https://luqmit3ishserver.azurewebsites.net/api/Users";
        private static readonly string ApiLoginUrl = "https://luqmit3ishserver.azurewebsites.net/api/Users/login";

        public UserServices()
        {
            _http = new HttpClient();
        }

        public async Task<ObservableCollection<User>> GetUsers()
        {
            var response = await _http.GetAsync(ApiUrl);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ObservableCollection<User>>(content);
        }

        public async Task<bool> InsertUser(User user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(ApiUrl, content);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> Login(LoginRequest loginRequest)
        {
            var json = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(ApiLoginUrl, content);
            Console.WriteLine(response.StatusCode);

            return response.IsSuccessStatusCode;
        }

    }
}