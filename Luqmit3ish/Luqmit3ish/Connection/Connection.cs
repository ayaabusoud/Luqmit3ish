using Luqmit3ish.Connection;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Luqmit3ish.Services
{
    class Connection : IConnection
    {
        public bool CheckInternetConnection()
        {
            var connection = Connectivity.NetworkAccess;
            if (connection == NetworkAccess.None)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
       
    }
}
