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
    /// Логика взаимодействия для RaceEdit.xaml
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

        private void RaceEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCardData();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var editedRace = new Race()
            {
                Name = RaceNameTextBox.Text,
                Desc = _rtbTextHandler.GetAsString()
            };

            var result = false;
            if (_addMode)
            {
                result = Task.Run(() => _api.Methods.Races.Add(editedRace)).Result;
            }
            else
            {
                editedRace.Id = _race.Id;
                result = Task.Run(() => _api.Methods.Races.Change(editedRace)).Result;
            }

            if (!result)
                MessageBox.Show("Произошла ошибка, данные не добавлены");
            else
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
            return _race is not null ? _race.Id : null;
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
