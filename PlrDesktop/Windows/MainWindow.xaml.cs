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
            _locationsOc.Clear();
            foreach (var loc in Task.Run(() => GetLocationsList()).Result)
            {
                _locationsOc.Add(loc);
            }
        }

        public void UpdateSocFormsList()
        {
            _socialFormationsOc.Clear();
            foreach (var sf in Task.Run(() => GetSocialFormationsList()).Result)
            {
                _socialFormationsOc.Add(sf);
            }
        }

        public void UpdateRacesList()
        {
            _racesOc.Clear();
            foreach (var race in Task.Run(() => GetRacesList()).Result)
            {
                _racesOc.Add(race);
            }
        }

        public void UpdateCharactersList()
        {
            _charactersOc.Clear();
            foreach (var chr in Task.Run(() => GetCharactersList()).Result)
            {
                _charactersOc.Add(chr);
            }
        }

        private void PrimaryWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PrimaryWindow.Title = "PLR";

            UpdateLocationsList();
            _locationsView.Source = _locationsOc;
            _locationsView.Filter += LocationsView_Filter;
            LocationsDataGrid.ItemsSource = _locationsView.View;

            UpdateRacesList();
            _racesView.Source = _racesOc;
            _racesView.Filter += RacesView_Filter;
            RacesDataGrid.ItemsSource = _racesView.View;

            UpdateSocFormsList();
            _socialFormationsView.Source = _socialFormationsOc;
            _socialFormationsView.Filter += SocialFormationsView_Filter;
            SocFormsDataGrid.ItemsSource = _socialFormationsView.View;

            UpdateCharactersList();
            _charactersView.Source = _charactersOc;
            _charactersView.Filter += CharactersView_Filter;
            CharactersDataGrid.ItemsSource = _charactersView.View;
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
            var selectedItem = CharactersDataGrid.SelectedCells[0].Item;
            var selectedCharacter = (Character)selectedItem;

            //CharacterDetails characterDetails = (CharacterDetails)_windowsManager
            //    .CreateCharacterDetailsWindow(selectedCharacter.Id.Value);

            //characterDetails.Show();
        }

        private void AddCharacterButton_Click(object sender, RoutedEventArgs e)
        {
            //CharacterEdit characterAdd = (CharacterEdit)_windowsManager
            //    .CreateCharacterAddWindow();

            //characterAdd.Show();
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
