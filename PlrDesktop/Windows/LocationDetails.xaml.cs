using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PlrDesktop.Lib;
using PlrDesktop.ApiInteraction;
using PlrDesktop.Datacards;
using System.Collections.ObjectModel;

namespace PlrDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для LocationDetails.xaml
    /// </summary>
    public partial class LocationDetails : Window
    {
        private ApiClient _api;
        private int _locId;
        private Location _location;
        private IWindowsBuilder _windowsBuilder;
        private RtbTextHandler _rtbTextHandler;
        private ObservableCollection<Location> _subLocations;

        public LocationDetails(IApiClients apiClients, IWindowsBuilder windowsBuilder, int locId)
        {
            InitializeComponent();

            _rtbTextHandler = new(LocationDescription);

            _api = apiClients.Default;
            _locId = locId;
            _windowsBuilder = windowsBuilder;
        }

        private async Task<Location> GetLocation(int id)
        {
            if (id < 0)
                return null;

            Location location = await _api.Methods.Locs.Get(id);
            return location;
        }

        private void LocationDetailsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _location = Task.Run(() => GetLocation(_locId)).Result;

            if (_location is not null)
            {
                LocationDetailsWindow.Title = _location.Name + " – карточка";
                LocationNameLabel.Content = _location.Name;

                LocationDescription.Document.Blocks.Clear();
                if (_rtbTextHandler.SetFromString(_location.Desc) is not null)
                    RtbTextHandler.ShowError(_rtbTextHandler.LastException);

                if (_location.ParentLoc is not null)
                    ParentLocationLabel.Content = "Является частью локации: " + _location.ParentLoc.Name;
                else
                    ParentLocationLabel.Content = "Корневая локация";

                SetSublocations();
                SublocationsList.ItemsSource = _subLocations;
            }
        }

        private void SetSublocations()
        {
            _subLocations = new();
            foreach (var loc in _location.Children)
            {
                _subLocations.Add(loc);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = _windowsBuilder.CreateLocationEditWindow(_location);
            editWindow.Show();
        }
    }
}
