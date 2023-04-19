using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class UserServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "https://luqmit3ishv2.azurewebsites.net/api/Users";
        private readonly IConnection _connection;

        public UserServices()
        {
            _httpClient = new HttpClient();
            _connection = new Connection();
        }

       public async Task<bool> Login(LoginRequest loginRequest)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                var json = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiUrl}/login", content);
                return response.IsSuccessStatusCode;
            }
            catch(HttpRequestException e)
            {
                throw new HttpRequestException(e.Message);
            }catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/{email}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<User>(content);
                    return user;
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw new Exception($"Failed to retrieve user_id: {response.StatusCode} - {response.ReasonPhrase}");
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

        public async Task<ObservableCollection<User>> GetUsers()
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
            var response = await _httpClient.GetAsync(_apiUrl);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ObservableCollection<User>>(content);
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

        public async Task<bool> InsertUser(SignUpRequest user)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_apiUrl, content);

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

        public async Task<User> GetUserById(int id)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/id/{id}");
                var content = await response.Content.ReadAsStringAsync();
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                throw new Exception($"Failed to retrieve user_id: {response.StatusCode} - {response.ReasonPhrase}");
                return JsonConvert.DeserializeObject<User>(content);

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

         public async Task EditProfile(User user)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                var content = JsonConvert.SerializeObject(user);
                var response = await _httpClient.PutAsync($"{_apiUrl}/{user.id}", new StringContent(content, UnicodeEncoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(response.StatusCode + ": failed to update data " + response.ReasonPhrase);
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
