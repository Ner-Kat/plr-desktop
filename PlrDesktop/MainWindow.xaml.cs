using PlrDesktop.ApiInteraction;
using PlrDesktop.ApiInteraction.Connection;
using PlrDesktop.Datacards.MainCards;
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

namespace PlrDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Location> _locationsOc;
        private ObservableCollection<Race> _raceOc;
        private ObservableCollection<SocialFormation> _socialFormationOc;
        private ObservableCollection<Character> _characterOc;
        private ApiClient _api;

        public MainWindow(IApiClients apiClients)
        {
            InitializeComponent();

            _api = apiClients.Default;
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

        private async void PrimaryWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _locationsOc = new ObservableCollection<Location>(await GetLocationsList());
            LocationsDataGrid.ItemsSource = _locationsOc;

            _raceOc = new ObservableCollection<Race>(await GetRaceList());
            RacesDataGrid.ItemsSource = _raceOc;

            _socialFormationOc = new ObservableCollection<SocialFormation>(await GetSocialFormationList());
            SocFormsDataGrid.ItemsSource = _socialFormationOc;

            _characterOc = new ObservableCollection<Character>(await GetCharacterList());
            CharactersDataGrid.ItemsSource = _characterOc;
        }
    }
}
