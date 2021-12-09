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
        private ApiClient _api;
        private IWindowsManager _windowsManager;

        private ObservableCollection<Location> _locationsOc = new();
        private CollectionViewSource _locationsView = new();

        private ObservableCollection<Race> _racesOc = new();
        private CollectionViewSource _racesView = new();

        private ObservableCollection<SocialFormation> _socialFormationsOc = new();
        private CollectionViewSource _socialFormationsView = new();

        private ObservableCollection<Character> _charactersOc = new();
        private CollectionViewSource _charactersView = new();

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

        private async Task<List<Race>> GetRacesList()
        {
            List<Race> races = await _api.Methods.Races.List(null);
            return races;
        }

        private async Task<List<SocialFormation>> GetSocialFormationsList()
        {
            List<SocialFormation> socForm = await _api.Methods.SocForms.List(null);
            return socForm;
        }

        private async Task<List<Character>> GetCharactersList()
        {
            List<Character> characters = await _api.Methods.Chars.List(null);
            return characters;
        }

        public void UpdateLocationsList()
        {
            var locs = Task.Run(() => GetLocationsList()).Result;

            if (locs is not null)
            {
                _locationsOc.Clear();
                foreach (var loc in locs)
                {
                    _locationsOc.Add(loc);
                }
            }
        }

        public void UpdateSocFormsList()
        {
            var socForms = Task.Run(() => GetSocialFormationsList()).Result;

            if (socForms is not null)
            {
                _socialFormationsOc.Clear();
                foreach (var sf in socForms)
                {
                    _socialFormationsOc.Add(sf);
                }
            }
        }

        public void UpdateRacesList()
        {
            var races = Task.Run(() => GetRacesList()).Result;

            if (races is not null)
            {
                _racesOc.Clear();
                foreach (var race in races)
                {
                    _racesOc.Add(race);
                }
            }
        }

        public void UpdateCharactersList()
        {
            var chars = Task.Run(() => GetCharactersList()).Result;

            if (chars is not null)
            {
                _charactersOc.Clear();
                foreach (var chr in chars)
                {
                    _charactersOc.Add(chr);
                }
            }
        }

        private void PrimaryWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PrimaryWindow.Title = "PLR";

            UpdateLocationsList();
            _locationsView.Source = _locationsOc;
            _locationsView.Filter += LocationsView_Filter;
            LocationsDataGrid.ItemsSource = _locationsView.View;
            PlrWpfUtils.ClearDataGridSelection(LocationsDataGrid);

            UpdateRacesList();
            _racesView.Source = _racesOc;
            _racesView.Filter += RacesView_Filter;
            RacesDataGrid.ItemsSource = _racesView.View;
            PlrWpfUtils.ClearDataGridSelection(RacesDataGrid);

            UpdateSocFormsList();
            _socialFormationsView.Source = _socialFormationsOc;
            _socialFormationsView.Filter += SocialFormationsView_Filter;
            SocFormsDataGrid.ItemsSource = _socialFormationsView.View;
            PlrWpfUtils.ClearDataGridSelection(SocFormsDataGrid);

            UpdateCharactersList();
            _charactersView.Source = _charactersOc;
            _charactersView.Filter += CharactersView_Filter;
            CharactersDataGrid.ItemsSource = _charactersView.View;
            PlrWpfUtils.ClearDataGridSelection(CharactersDataGrid);
        }


        // ########################### ОБРАБОТЧИКИ ДЛЯ ЛОКАЦИЙ ########################### \\

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

        private void RemoveLocationButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedLocation = LocationsDataGrid.SelectedItem as Location;

            if (selectedLocation is not null)
            {
                Task.Run(() => _api.Methods.Locs.Remove(selectedLocation.Id.Value)).Wait();
                UpdateLocationsList();
            }
        }

        private void UpdateLocationsButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateLocationsList();
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

        // ########################### ————————————————— ########################### //


        // ########################### ОБРАБОТЧИКИ ДЛЯ РАС ########################### \\

        private void RacesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = RacesDataGrid.SelectedCells[0].Item;
            var selectedRace = (Race)selectedItem;

            RaceDetails raceDetails = (RaceDetails)_windowsManager
                .CreateRaceDetailsWindow(selectedRace.Id.Value);

            raceDetails.Show();
        }

        private void AddRaceButton_Click(object sender, RoutedEventArgs e)
        {
            RaceEdit raceAdd = (RaceEdit)_windowsManager
                .CreateRaceAddWindow();

            raceAdd.Show();
        }

        private void RemoveRaceButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRace = RacesDataGrid.SelectedItem as Race;

            if (selectedRace is not null)
            {
                Task.Run(() => _api.Methods.Races.Remove(selectedRace.Id.Value)).Wait();
                UpdateRacesList();
            }
        }

        private void UpdateRacesButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateRacesList();
        }

        private void RacesView_Filter(object sender, FilterEventArgs e)
        {
            Race race = e.Item as Race;

            if (race is not null)
            {
                if (race.Name.ToLower().Contains(RaceFindTextBox.Text.ToLower()))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        private void RaceFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _racesView.View.Refresh();
        }

        // ########################### ————————————————— ########################### //


        // ########################### ОБРАБОТЧИКИ ДЛЯ СОЦ.ФОРМИРОВАНИЙ ########################### \\

        private void SocFormsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = SocFormsDataGrid.SelectedCells[0].Item;
            var selectedSocialFormation = (SocialFormation)selectedItem;

            SocFormDetails socFormDetails = (SocFormDetails)_windowsManager
                .CreateSocFormDetailsWindow(selectedSocialFormation.Id.Value);

            socFormDetails.Show();
        }

        private void AddSocFormButton_Click(object sender, RoutedEventArgs e)
        {
            SocFormEdit socFormAdd = (SocFormEdit)_windowsManager
                .CreateSocFormAddWindow();

            socFormAdd.Show();
        }

        private void RemoveSocFormButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSocForm = SocFormsDataGrid.SelectedItem as SocialFormation;

            if (selectedSocForm is not null)
            {
                Task.Run(() => _api.Methods.SocForms.Remove(selectedSocForm.Id.Value)).Wait();
                UpdateSocFormsList();
            }
        }

        private void UpdateSocFormsButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSocFormsList();
        }

        private void ClearSocFormCatSelection_Click(object sender, RoutedEventArgs e)
        {
           SocFormCatComboBox.SelectedIndex = -1;
        }

        private void SocialFormationsView_Filter(object sender, FilterEventArgs e)
        {
            SocialFormation socForm = e.Item as SocialFormation;

            if (socForm is not null)
            {
                if (socForm.Name.ToLower().Contains(LocFindTextBox.Text.ToLower()))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        private void SocFormFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _socialFormationsView.View.Refresh();
        }

        // ########################### ————————————————— ########################### //


        // ########################### ОБРАБОТЧИКИ ДЛЯ ПЕРСОНАЖЕЙ ########################### \\

        private void CharactersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedCharacter = CharactersDataGrid.SelectedItem as Character;

            if (selectedCharacter is not null)
            {
                CharacterDetails characterDetails = (CharacterDetails)_windowsManager
                    .CreateCharacterDetailsWindow(selectedCharacter.Id.Value);
                characterDetails.Show();
            }
        }

        private void AddCharacterButton_Click(object sender, RoutedEventArgs e)
        {
            CharacterEdit characterAdd = (CharacterEdit)_windowsManager
                .CreateCharacterAddWindow();

            characterAdd.Show();
        }

        private void RemoveCharacterButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = CharactersDataGrid.SelectedItem;
            var selectedCharacter = (Character)selectedItem;

            Task.Run(() => _api.Methods.Chars.Remove(selectedCharacter.Id.Value)).Wait();
            UpdateCharactersList();
        }

        private void UpdateCharactersButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCharactersList();
        }

        private void CharactersView_Filter(object sender, FilterEventArgs e)
        {
            Character chr = e.Item as Character;

            if (chr is not null)
            {
                if (chr.Name.ToLower().Contains(CharFindTextBox.Text.ToLower()))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        private void CharFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _charactersView.View.Refresh();
        }

        // ########################### ————————————————— ########################### //

    }
}
