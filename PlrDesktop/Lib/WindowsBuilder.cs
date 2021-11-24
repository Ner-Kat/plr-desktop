using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PlrDesktop.ApiInteraction;
using PlrDesktop.Windows;

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
            var window = new LocationDetails(_apiClients, id);
            return window;
        }
    }
}
