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
    public partial class LocationDetails : Window, IPlrCardWindow
    {
        private ApiClient _api;
        private int _locId;
        private Location _location;
        private IWindowsManager _windowsManager;
        private RtbTextHandler _rtbTextHandler;
        private ObservableCollection<Location> _subLocations = new();

        public LocationDetails(IApiClients apiClients, IWindowsManager windowsManager, int locId)
        {
            InitializeComponent();

            _rtbTextHandler = new(LocationDescription);

            _api = apiClients.Default;
            _locId = locId;
            _windowsManager = windowsManager;
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
            UpdateCardData();

            PlrWpfUtils.ClearDataGridSelection(SublocationsList);
        }

        private void ParentLocationLabel_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            if (_location.ParentLocId is not null)
            {
                var editWindow = _windowsManager.CreateLocationDetailsWindow(_location.ParentLocId.Value);
                editWindow.Show();
            }
        }

        private void SetSublocations()
        {
            _subLocations.Clear();
            foreach (var loc in _location.Children)
            {
                _subLocations.Add(loc);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = _windowsManager.CreateLocationEditWindow(_location);
            editWindow.Show();
        }

        private void SublocationsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = SublocationsList.SelectedCells[0].Item;
            var selectedLocation = (Location)selectedItem;

            LocationDetails locationDetails = (LocationDetails)_windowsManager
                .CreateLocationDetailsWindow(selectedLocation.Id.Value);

            locationDetails.Show();
        }

        public int? GetId()
        {
            return _location.Id ?? null;
        }

        public void UpdateCardData()
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
                {
                    ParentLocationLabel.Content = "Является частью локации " + _location.ParentLoc.Name;
                    ParentLocationLabel.MouseLeftButtonUp += ParentLocationLabel_MouseLeftButtonUp;
                }
                else
                {
                    ParentLocationLabel.Content = "Корневая локация";
                }

                SetSublocations();
                SublocationsList.ItemsSource = _subLocations;
            }
        }
    }
}
