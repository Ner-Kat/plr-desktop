using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PlrDesktop.ApiInteraction;
using PlrDesktop.Windows;
using PlrDesktop.Datacards;
using System.ComponentModel;

namespace PlrDesktop.Lib
{
    public class WindowsManager : IWindowsManager
    {
        private IApiClients _apiClients;

        public MainWindow MainWindow { get; set; }

        private List<IPlrCardWindow> _locationDetailsWindows = new();
        private List<IPlrCardWindow> _locationEditWindows = new();


        public WindowsManager(IApiClients apiClients)
        {
            _apiClients = apiClients;
        }

        // Получение списка окон просмотра по объекту окна редактирования
        private List<IPlrCardWindow> GetDetailByEditWindowsList(object window)
        {
            if (window is LocationEdit)
                return _locationDetailsWindows;

            return null;
        }

        // Получение списка открытых окон по их типу
        private List<IPlrCardWindow> GetWindowsList<WType>() where WType : Window, IPlrCardWindow
        {
            if (typeof(WType) == typeof(LocationDetails))
                return _locationDetailsWindows;
            if (typeof(WType) == typeof(LocationEdit))
                return _locationEditWindows;

            return null;
        }

        // Проверка, не является ли создаваемое окно уже осзданным и открытым
        private Window CheckOpenedWindows<WType>(int id) where WType : Window, IPlrCardWindow
        {
            var openedWins = Application.Current.Windows.OfType<WType>();
            var windowsList = GetWindowsList<WType>();

            foreach (var wnd in windowsList)
            {
                var wndWin = (Window)wnd ?? null;

                if (wndWin is null || !wndWin.IsLoaded || !openedWins.Contains(wndWin))
                {
                    windowsList.Remove(wnd);
                    break;
                }
                else if (wnd.GetId() == id)
                {
                    wndWin.Activate();
                    return wndWin;
                }
            }

            return null;
        }

        // Создание окна просмотра карточки данных
        private Window CreateDetailsWindow<WType>(int id) where WType : Window, IPlrCardWindow
        {
            Window openedWin = CheckOpenedWindows<WType>(id);

            if (openedWin is not null)
                return openedWin;
            
            Window window = null;
            if (typeof(WType) == typeof(LocationDetails))
                 window = new LocationDetails(_apiClients, this, id);

            GetWindowsList<WType>().Add(window as IPlrCardWindow);
            return window;
        }

        // Операция обновления данных карточки окна просмотра
        private void TryUpdateDetailsWindow(object sender, EventArgs e)
        {
            var window = sender as IPlrCardWindow;
            int? id = window is not null ? window.GetId() : null;
            
            if (id is not null)
            {
                foreach (var detailsWindow in GetDetailByEditWindowsList(sender))
                {
                    if (detailsWindow.GetId() == id)
                        detailsWindow.UpdateCardData();
                }
            }
        }

        // Создание окна редактирования карточки
        public Window CreateEditWindow<WType>(IApiDataCard dataCard) where WType : Window, IPlrCardWindow
        {
            if (dataCard is not null)
            {
                if (dataCard.Id is null)
                    return null;

                Window openedWin = CheckOpenedWindows<WType>(dataCard.Id.Value);

                if (openedWin is not null)
                    return openedWin;
            }

            Window window = null;
            if (typeof(WType) == typeof(LocationEdit))
                window = new LocationEdit(_apiClients, this, dataCard as Location);

            GetWindowsList<WType>().Add(window as IPlrCardWindow);
            window.Closed += (object sender, EventArgs e) => MainWindow.UpdateLocationsList();

            return window;
        }



        public Window CreateLocationEditWindow(Location location)
        {
            var window = CreateEditWindow<LocationEdit>(location);
            window.Closed += TryUpdateDetailsWindow;
            return window;
        }

        public Window CreateLocationDetailsWindow(int id)
        {
            return CreateDetailsWindow<LocationDetails>(id);
        }

        public Window CreateLocationAddWindow()
        {
            var window = CreateEditWindow<LocationEdit>(null);
            return window;
        }
    }
}
