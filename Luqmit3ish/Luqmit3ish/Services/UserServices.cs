﻿using System;
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
        private readonly HttpClient _http;
        private static readonly string ApiUrl = "https://luqmit3ish.azurewebsites.net/api/Users";
        private static readonly string ApiSignUp = "https://luqmit3ish.azurewebsites.net/api/Users";
        private static readonly string ApiLoginUrl = "https://luqmit3ish.azurewebsites.net/api/Users/login";
        private IConnection _connection;


        public UserServices()
        {
            _http = new HttpClient();
            _connection = new Connection();
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

        public async Task<User> GetUserById(int id)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                var response = await _http.GetAsync($"{ApiUrl}/id/{id}");
                var content = await response.Content.ReadAsStringAsync();
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
            var content = JsonConvert.SerializeObject(user);
            var response = await _http.PutAsync($"{ApiUrl}/{user.id}", new StringContent(content, UnicodeEncoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.StatusCode + ": failed to update data " + response.ReasonPhrase);
            }

        }

    }
}
