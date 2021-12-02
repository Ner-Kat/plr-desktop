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
    public partial class RaceDetails : Window, IPlrCardWindow
    {
        private ApiClient _api;
        private int _raceId;
        private Race _race;
        private IWindowsManager _windowsManager;
        private RtbTextHandler _rtbTextHandler;
        private ObservableCollection<Race> _subRaces = new();

        public RaceDetails(IApiClients apiClients, IWindowsManager windowsManager, int raceId)
        {
            InitializeComponent();

            _rtbTextHandler = new(RaceDescription);

            _api = apiClients.Default;
            _raceId = raceId;
            _windowsManager = windowsManager;
        }

        private async Task<Race> GetRace(int id)
        {
            if (id < 0)
                return null;

            Race race = await _api.Methods.Races.Get(id);
            return race;
        }

        private void RaceDetailsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCardData();
        }

        //private void ParentLocationLabel_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        //{
        //    if (_race.ParentRaceId is not null)
        //    {
        //        var editWindow = _windowsManager.CreateRaceDetailsWindow(_race.ParentRaceId.Value);
        //        editWindow.Show();
        //    }
        //}

        //private void SetSubraces()
        //{
        //    _subRaces.Clear();
        //    foreach (var race in _race.Children)
        //    {
        //        _subRaces.Add(race);
        //    }
        //}

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = _windowsManager.CreateRaceEditWindow(_race);
            editWindow.Show();
        }

        //private void SubracesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    var selectedItem = SubracesList.SelectedCells[0].Item;
        //    var selectedRace = (Race)selectedItem;

        //    RaceDetails raceDetails = (RaceDetails)_windowsManager
        //        .CreateRaceDetailsWindow(selectedRace.Id.Value);

        //    raceDetails.Show();
        //}

        public int? GetId()
        {
            return _race.Id ?? null;
        }

        public void UpdateCardData()
        {
            _race = Task.Run(() => GetRace(_raceId)).Result;

            if (_race is not null)
            {
                RaceDetailsWindow.Title = _race.Name + " – карточка";
                RaceNameLabel.Content = _race.Name;

                RaceDescription.Document.Blocks.Clear();
                if (_rtbTextHandler.SetFromString(_race.Desc) is not null)
                    RtbTextHandler.ShowError(_rtbTextHandler.LastException);

                //if (_race.ParentRace is not null)
                //{
                //    ParentRaceLabel.Content = "Является ответвлением расы " + _race.ParentRace.Name;
                //    ParentRaceLabel.MouseLeftButtonUp += ParentRaceLabel_MouseLeftButtonUp;
                //}
                //else
                //{
                //    ParentRaceLabel.Content = "Изначальная раса";
                //}

                //SetSubraces();
                //SubracesList.ItemsSource = _subRaces;
            }
        }
    }
}
