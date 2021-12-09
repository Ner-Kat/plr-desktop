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
using System.ComponentModel;

namespace PlrDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для LocationEdit.xaml
    /// </summary>
    public partial class LocationEdit : Window, IPlrCardWindow
    {
        private ApiClient _api;
        private Location _location;
        private RtbTextHandler _rtbTextHandler;
        private IWindowsManager _windowsManager;

        private ObservableCollection<Location> _avalibleParentLocs = new();
        private CollectionViewSource _avParentLocsView = new();

        private bool _addMode = true;

        public LocationEdit(IApiClients apiClients, IWindowsManager windowsManager, Location location)
        {
            InitializeComponent();

            _rtbTextHandler = new RtbTextHandler(LocDescField);

            _api = apiClients.Default;
            _windowsManager = windowsManager;
            _location = location;
        }

        private async Task<List<Location>> GetAllLocations()
        {
            List<Location> locations = await _api.Methods.Locs.List(null);

            if (_location is not null)
            {
                var selfLoc = locations.FirstOrDefault(loc => loc.Id == _location.Id);
                locations.Remove(selfLoc);
            }

            return locations;
        }

        private void SetParentLocationsList()
        {
            var locs = Task.Run(() => GetAllLocations()).Result;

            if (locs is not null)
            {
                _avalibleParentLocs.Clear();
                foreach (var loc in locs)
                {
                    _avalibleParentLocs.Add(loc);
                }

                if (_location is not null && _location.ParentLoc is not null)
                {
                    var parentLoc = _avalibleParentLocs.FirstOrDefault(loc => loc.Id == _location.ParentLoc.Id);
                    ParentLocComboBox.SelectedItem = parentLoc;
                }
            }
        }

        private void LocationEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateCardData();

            _avParentLocsView.Source = _avalibleParentLocs;
            _avParentLocsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            _avParentLocsView.Filter += AvaliableParentLocs_Filter;
            ParentLocComboBox.ItemsSource = _avParentLocsView.View;
        }

        private void AvaliableParentLocs_Filter(object sender, FilterEventArgs e)
        {
            Location loc = e.Item as Location;

            if (loc is not null)
            {
                if (loc.Name.ToLower().Contains(ParentLocFindTextBox.Text.ToLower()))
                    e.Accepted = true;
                else
                    e.Accepted = false;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var editedLocation = new Location()
            {
                Name = LocNameTextBox.Text,
                Desc = _rtbTextHandler.GetAsString()
            };

            var selectedParentLoc = ParentLocComboBox.SelectedItem;
            editedLocation.ParentLocId = selectedParentLoc is not null ? ((Location)selectedParentLoc).Id : null;

            var result = false;
            if (_addMode)
            {
                result = Task.Run(() => _api.Methods.Locs.Add(editedLocation)).Result;
            }
            else
            {
                editedLocation.Id = _location.Id;
                result = Task.Run(() => _api.Methods.Locs.Change(editedLocation)).Result;
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
            return _location is not null ? _location.Id : null;
        }

        private void ClearParentLocSelection_Click(object sender, RoutedEventArgs e)
        {
            ParentLocComboBox.SelectedIndex = -1;
        }

        public void UpdateCardData()
        {
            if (_location is not null)
            {
                LocationEditWindow.Title = _location.Name + " – изменение";
                _addMode = false;

                LocNameTextBox.Text = _location.Name;
                LocDescField.Document.Blocks.Clear();
                if (_rtbTextHandler.SetFromString(_location.Desc) is not null)
                    RtbTextHandler.ShowError(_rtbTextHandler.LastException);
            }

            SetParentLocationsList();
        }

        private void ParentLocFindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _avParentLocsView.View.Refresh();
            ParentLocComboBox.UpdateLayout();
        }
    }
}
