using PlrDesktop.ApiInteraction;
using PlrDesktop.ApiInteraction.Connection;
using PlrDesktop.Datacards;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using PlrDesktop.Lib;

namespace PlrDesktop.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Location> _locationsOc = new();
        private CollectionViewSource _locationsView = new();

        private ObservableCollection<Race> _raceOc;
        private ObservableCollection<SocialFormation> _socialFormationOc;
        private ObservableCollection<Character> _characterOc;
        private ApiClient _api;
        private IWindowsManager _windowsManager;

        public MainWindow(IApiClients apiClients, IWindowsManager windowsManager)
        {
            InitializeComponent();

            _api = apiClients.Default;
            _windowsManager = windowsManager;
            _windowsManager.MainWindow = this;
        }

        private async Task<List<Location>> GetLocationsList()
        {
            List<Location> locations = await _api.Methods.Locs.List(null);
            return locations;
        }

        private async Task<List<Race>> GetRaceList()
        {
            List<Race> races = await _api.Methods.Races.List(null);
            return races;
        }

        private async Task<List<SocialFormation>> GetSocialFormationList()
        {
            List<SocialFormation> socForm = await _api.Methods.SocForms.List(null);
            return socForm;
        }

        private async Task<List<Character>> GetCharacterList()
        {
            List<Character> characters = await _api.Methods.Chars.List(null);
            return characters;
        }

        private void PrimaryWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PrimaryWindow.Title = "PLR";

            UpdateLocationsList();
            _locationsView.Source = _locationsOc;
            _locationsView.Filter += LocationsView_Filter;
            LocationsDataGrid.ItemsSource = _locationsView.View;

            _raceOc = new ObservableCollection<Race>(Task.Run(() => GetRaceList()).Result);
            RacesDataGrid.ItemsSource = _raceOc;

            _socialFormationOc = new ObservableCollection<SocialFormation>(Task.Run(() => GetSocialFormationList()).Result);
            SocFormsDataGrid.ItemsSource = _socialFormationOc;

            _characterOc = new ObservableCollection<Character>(Task.Run(() => GetCharacterList()).Result);
            CharactersDataGrid.ItemsSource = _characterOc;
        }

        private void LocationsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = LocationsDataGrid.SelectedCells[0].Item;
            var selectedLocation = (Location)selectedItem;

            LocationDetails locationDetails = (LocationDetails)_windowsManager
                .CreateLocationDetailsWindow(selectedLocation.Id.Value);

            locationDetails.Show();
        }

        private void AddLocationButton_Click(object sender, RoutedEventArgs e)
        {
            LocationEdit locationAdd = (LocationEdit)_windowsManager
                .CreateLocationAddWindow();

            locationAdd.Show();
        }

        public void UpdateLocationsButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateLocationsList();
        }

        private void UpdateLocationsList()
        {
            _locationsOc.Clear();
            foreach (var loc in Task.Run(() => GetLocationsList()).Result)
            {
                _locationsOc.Add(loc);
            }
        }

        private void ClearLocLevelSelection_Click(object sender, RoutedEventArgs e)
        {
            LocLevelComboBox.SelectedIndex = -1;
        }

        private void LocationsView_Filter(object sender, FilterEventArgs e)
        {
            Location loc = e.Item as Location;

            if (loc is not null)
            {
                if (loc.Name.ToLower().Contains(LocFindTextBox.Text.ToLower()))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        private void LocFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _locationsView.View.Refresh();
        }
    }
}
