using Luqmit3ish.Connection;
using Plugin.Connectivity;
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
            if (!CrossConnectivity.Current.IsConnected)
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
