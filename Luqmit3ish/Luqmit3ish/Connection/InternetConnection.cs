﻿using Luqmit3ish.Connection;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Luqmit3ish.Connection
{
    class InternetConnection : IConnection
    {
        public bool CheckInternetConnection()
        {
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
