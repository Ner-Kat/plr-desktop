﻿using System;
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
    /// Логика взаимодействия для LocationEdit.xaml
    /// </summary>
    public partial class LocationEdit : Window
    {
        private ApiClient _api;
        private Location _location;
        private ObservableCollection<Location> _avalibleParentLocs;

        private bool _addMode = true;

        public LocationEdit(IApiClients apiClients, Location location)
        {
            InitializeComponent();

            _api = apiClients.Default;
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

        private async Task<bool> SetParentLocationsList()
        {
            _avalibleParentLocs = new ObservableCollection<Location>(await GetAllLocations());

            if (_location is not null && _location.ParentLoc is not null)
            {
                var parentLoc = _avalibleParentLocs.FirstOrDefault(loc => loc.Id == _location.ParentLoc.Id);
                ParentLocComboBox.SelectedItem = parentLoc;
            }

            return true;
        }

        private async void LocationEditWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (_location is not null)
            {
                _addMode = false;

                LocNameTextBox.Text = _location.Name;
                LocDescField.Document.Blocks.Clear();
                LocDescField.Document.Blocks.Add(new Paragraph(new Run(_location.Desc)));
            }

            await SetParentLocationsList();
            ParentLocComboBox.ItemsSource = _avalibleParentLocs;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            TextRange descText = new TextRange(LocDescField.Document.ContentStart, LocDescField.Document.ContentEnd);
            var editedLocation = new Location()
            {
                Name = LocNameTextBox.Text,
                Desc = descText.Text,
            };

            var selectedParentLoc = ParentLocComboBox.SelectedItem;
            editedLocation.ParentLocId = selectedParentLoc is not null ? ((Location)selectedParentLoc).Id : null;

            if (_addMode)
            {
                await _api.Methods.Locs.Add(editedLocation);
            }
            else
            {
                editedLocation.Id = _location.Id;
                await _api.Methods.Locs.Change(editedLocation);
            }
            
            this.Close();
        }
    }
}
