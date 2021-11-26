using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PlrDesktop.ApiInteraction;
using PlrDesktop.Windows;
using PlrDesktop.Datacards;

namespace PlrDesktop.Lib
{
    public class WindowsBuilder : IWindowsBuilder
    {
        private IApiClients _apiClients;

        public WindowsBuilder(IApiClients apiClients)
        {
            _apiClients = apiClients;
        }

        public Window CreateLocationDetailsWindow(int id)
        {
            var window = new LocationDetails(_apiClients, this, id);
            return window;
        }

        public Window CreateLocationEditWindow(Location location)
        {
            var window = new LocationEdit(_apiClients, location);
            return window;
        }
    }
}
