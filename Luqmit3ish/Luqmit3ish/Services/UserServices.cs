using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
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
        private static readonly string ApiUrl = "https://luqmit3ish.azurewebsites.net/api/Users";
        private static readonly string ApiSignUp = "https://luqmit3ish.azurewebsites.net/api/Users";
        private static readonly string ApiLoginUrl = "https://luqmit3ish.azurewebsites.net/api/Users/login";

        public UserServices()
        {
            _http = new HttpClient();
        }
     
        public async Task<bool> Login(LoginRequest loginRequest)
        {
            var json = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(ApiLoginUrl, content);
            Console.WriteLine(response.StatusCode);

            return response.IsSuccessStatusCode;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            var response = await _http.GetAsync($"{ApiUrl}/{email}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(content);
                return user;
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

        public async Task<ObservableCollection<User>> GetUsers()
        {
            var response = await _http.GetAsync(ApiUrl);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ObservableCollection<User>>(content);
        }

        public async Task<bool> InsertUser(SignUpRequest user)
        {
            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(ApiSignUp, content);

            return response.IsSuccessStatusCode;
        }

    }
}
