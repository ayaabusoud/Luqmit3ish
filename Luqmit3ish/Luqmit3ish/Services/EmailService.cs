using Luqmit3ish.Connection;
using Luqmit3ish.Interfaces;
using Luqmit3ish.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Luqmit3ish.Services
{
    class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = Constants.BaseUrl + "api/Email/send";
        private readonly IConnection _connection;

        public EmailService()
        {
            _httpClient = new HttpClient();
            _connection = new Connection();
        }
        public async Task<string> SendVerificationCode(string recipientName, string recipientEmail)
        {
            
            var json = JsonConvert.SerializeObject(new { recipientName = recipientName, recipientEmail = recipientEmail });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObj = JsonConvert.DeserializeObject<dynamic>(responseContent);
                var verificationCode = responseObj.verificationCode;
                if(verificationCode == null)
                {
                    return null;
                }
                return verificationCode;
            }

            else
            {
                throw new Exception("Failed to send email.");
            }
        }

    }
}
