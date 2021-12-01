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

        public Window MainWindow { get; set; }
        private List<IHasId> _locationDetailsWindows = new();
        private List<IHasId> _locationEditWindows = new();


        public WindowsBuilder(IApiClients apiClients)
        {
            _apiClients = apiClients;
        }

        public Window CreateLocationDetailsWindow(int id)
        {
            var openedWins = Application.Current.Windows.OfType<LocationDetails>();

            foreach (var wnd in _locationDetailsWindows)
            {
                var wndWin = (Window)wnd ?? null;

                if (wndWin is null || !wndWin.IsLoaded || !openedWins.Contains(wndWin))
                {
                    _locationDetailsWindows.Remove(wnd);
                    break;
                }
                else if (wnd.GetId() == id)
                {
                    wndWin.Activate();
                    return wndWin;
                }
            }

            var window = new LocationDetails(_apiClients, this, id);
            _locationDetailsWindows.Add(window);
            return window;
        }

        public Window CreateLocationEditWindow(Location location)
        {
            var openedWins = Application.Current.Windows.OfType<LocationEdit>();

            foreach (var wnd in _locationEditWindows)
            {
                var wndWin = (Window)wnd ?? null;

                if (wndWin is null || !wndWin.IsLoaded || !openedWins.Contains(wndWin))
                {
                    _locationEditWindows.Remove(wnd);
                    break;
                }
                else if (wnd.GetId() == location.Id)
                {
                    wndWin.Activate();
                    return wndWin;
                }
            }

            var window = new LocationEdit(_apiClients, location);
            _locationEditWindows.Add(window);
            return window;
        }
        public Window CreateLocationAddWindow()
        {
            var window = new LocationEdit(_apiClients, null);
            return window;
        }
    }
}
