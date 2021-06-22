using PlrDesktop.ApiInteraction.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.Datacards.MainCards;
using PlrDesktop.Datacards.InputCards;
using System.Net;
using System.Windows;
using System.IO;

namespace PlrDesktop.ApiInteraction.Methods
{
    public class LocationsMethods
    {
        private ApiServerConnection _server;
        private const string _methodsAddress = "locations/";

        public LocationsMethods(ApiServerConnection server)
        {
            _server = server;
        }

        public Location Get(int id)
        {
            
        }
    }
}
