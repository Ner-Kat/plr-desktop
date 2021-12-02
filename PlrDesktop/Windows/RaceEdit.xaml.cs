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
using System.IO;

namespace PlrDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для LocationEdit.xaml
    /// </summary>
    public partial class RaceEdit : Window, IPlrCardWindow
    {
        private ApiClient _api;
        private Race _race;
        private RtbTextHandler _rtbTextHandler;
        private IWindowsManager _windowsManager;

        private ObservableCollection<Race> _avalibleParentRaces = new();
        private CollectionViewSource _avParentRacesView = new();

        private bool _addMode = true;

        public RaceEdit(IApiClients apiClients, IWindowsManager windowsManager, Race race)
        {
            InitializeComponent();

            _rtbTextHandler = new RtbTextHandler(RaceDescField);

            _api = apiClients.Default;
            _windowsManager = windowsManager;
            _race = race;
        }

        //private async Task<List<Race>> GetAllRaces()
        //{
        //    List<Race> races = await _api.Methods.Races.List(null);

        //    if (_race is not null)
        //    {
        //        var selfLoc = races.FirstOrDefault(loc => loc.Id == _race.Id);
        //        races.Remove(selfLoc);
        //    }

        //    return races;
        //}

        //private void SetParentLocationsList()
        //{
        //    _avalibleParentRaces.Clear();
        //    foreach (var race in Task.Run(() => GetAllRaces()).Result)
        //    {
        //        _avalibleParentRaces.Add(race);
        //    }

        //    if (_race is not null && _race.ParentRace is not null)
        //    {
        //        var parentLoc = _avalibleParentRaces.FirstOrDefault(loc => loc.Id == _race.ParentRace.Id);
        //        ParentRaceComboBox.SelectedItem = parentLoc;
        //    }
        //}

        private void RaceEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCardData();

            //_avParentRacesView.Source = _avalibleParentRaces;
            //_avParentRacesView.Filter += AvaliableParentRaces_Filter;
            //ParentRaceComboBox.ItemsSource = _avParentRacesView.View;
        }

        //private void AvaliableParentRaces_Filter(object sender, FilterEventArgs e)
        //{
        //    Race race = e.Item as Race;

        //    if (race is not null)
        //    {
        //        if (race.Name.ToLower().Contains(ParentRaceFindTextBox.Text.ToLower()))
        //            e.Accepted = true;
        //        else
        //            e.Accepted = false;
        //    }
        //}

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var editedRace = new Race()
            {
                Name = RaceNameTextBox.Text,
                Desc = _rtbTextHandler.GetAsString()
            };

            //var selectedParentRace = ParentRaceComboBox.SelectedItem;
            //editedRace.ParentLocId = selectedParentRace is not null ? ((Race)selectedParentRace).Id : null;

            if (_addMode)
            {
                Task.Run(() => _api.Methods.Races.Add(editedRace));
            }
            else
            {
                editedRace.Id = _race.Id;
                Task.Run(() => _api.Methods.Races.Change(editedRace));
            }
            
            this.Close();
        }

        private void TextEditingToolbar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }

        public int? GetId()
        {
            return _race.Id ?? null;
        }

        //private void ClearParentRaceSelection_Click(object sender, RoutedEventArgs e)
        //{
        //    ParentRaceComboBox.SelectedIndex = -1;
        //}

        public void UpdateCardData()
        {
            if (_race is not null)
            {
                RaceEditWindow.Title = _race.Name + " – изменение";
                _addMode = false;

                RaceNameTextBox.Text = _race.Name;
                RaceDescField.Document.Blocks.Clear();
                if (_rtbTextHandler.SetFromString(_race.Desc) is not null)
                    RtbTextHandler.ShowError(_rtbTextHandler.LastException);
            }

            //SetParentRacesList();
        }

        //private void ParentRaceFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    _avParentRacesView.View.Refresh();
        //    ParentRaceComboBox.UpdateLayout();
        //}
    }
}
