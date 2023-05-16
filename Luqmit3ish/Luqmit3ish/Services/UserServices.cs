using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Luqmit3ish.Connection;
using Luqmit3ish.Exceptions;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Models;
using Luqmit3ish.Utilities;
using Luqmit3ish.ViewModels;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Luqmit3ish.Services
{
    public class UserServices: IUserServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = Constants.BaseUrl + "api/Users";
        private readonly IConnection _connection;

        public UserServices()
        {
            _httpClient = new HttpClient();
            _connection = new InternetConnection();
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

                var token = await response.Content.ReadAsStringAsync();
                Preferences.Set("Token", token);
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

        public async Task<bool> DeleteAccount(int userId)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                string token = Preferences.Get("Token", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                    throw new NotAuthorizedException("You are not Authorized to do this operation");
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                var response = await _httpClient.DeleteAsync($"{_apiUrl}/{userId}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
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
                var token = await response.Content.ReadAsStringAsync();
                Preferences.Set("Token", token);
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
                string token = Preferences.Get("Token", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                    throw new NotAuthorizedException("You are not Authorized to do this operation");
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                var content = JsonConvert.SerializeObject(user);
                var response = await _httpClient.PutAsync($"{_apiUrl}/{user.Id}", new StringContent(content, UnicodeEncoding.UTF8, "application/json"));
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
        
        public async Task<bool> ResetPassword(int id, string password)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                string token = Preferences.Get("Token", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                    throw new NotAuthorizedException("You are not Authorized to do this operation");
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                var patchObject = new { id, password};
                var patchData = JsonConvert.SerializeObject(patchObject);
                var httpContent = new StringContent(patchData, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_apiUrl}/resetPassword" + "/" + id + "/" + password)
                {
                    Content = httpContent
                };

                var response = await _httpClient.SendAsync(request);
                Debug.WriteLine(response.StatusCode);

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
        public async Task<bool> ForgotPassword(int id, string password)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
           
                var patchObject = new { id, password };
                var patchData = JsonConvert.SerializeObject(patchObject);
                var httpContent = new StringContent(patchData, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{_apiUrl}/ForgetPassword" + "/" + id + "/" + password)
                {
                    Content = httpContent
                };

                var response = await _httpClient.SendAsync(request);
                Debug.WriteLine(response.StatusCode);

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

      

        internal async Task<bool> UploadPhoto(string photoPath, int userId)
        {
            if (!_connection.CheckInternetConnection())
            {
                throw new ConnectionException("There is no internet connection");
            }
            try
            {
                string token = Preferences.Get("Token", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                    throw new NotAuthorizedException("You are not Authorized to do this operation");
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
                    var response = await _httpClient.PostAsync($"{_apiUrl}/UploadPhoto/{userId}", formData);
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

        async Task<bool> IUserServices.UploadPhoto(string photoPath, int userId)
        {
            return await UploadPhoto(photoPath, userId);
        }
    }
}
