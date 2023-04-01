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
    class OrderService
    {
        private readonly HttpClient _http;
        private static readonly string ApiUrl = "https://luqmit3ishserver.azurewebsites.net/api/Orders";

        public OrderService()
        {
            _http = new HttpClient();
        }
    }
}
