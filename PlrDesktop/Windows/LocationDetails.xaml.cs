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
using PlrDesktop.Datacards.MainCards;

namespace PlrDesktop.Windows
{
    /// <summary>
    /// Логика взаимодействия для LocationDetails.xaml
    /// </summary>
    public partial class LocationDetails : Window
    {
        private ApiClient _api;
        private int _locId;
        private Location _location;

        public LocationDetails(IApiClients apiClients, int locId)
        {
            InitializeComponent();

            _api = apiClients.Default;
            _locId = locId;
        }

        private async Task<Location> GetLocation(int id)
        {
            if (id < 0)
                return null;

            Location location = await _api.Methods.Locs.Get(id);
            return location;
        }

        private async void LocationDetailsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _location = await GetLocation(_locId);

            if (_location is not null)
            {
                LocationNameLabel.Content = _location.Name;

                LocationDescription.Document.Blocks.Clear();
                LocationDescription.Document.Blocks.Add(new Paragraph(new Run(_location.Desc)));

                if (_location.ParentLoc is not null)
                    ParentLocationLabel.Content = "Является частью локации: " + _location.ParentLoc.Name;
                else
                    ParentLocationLabel.Content = "Корневая локация";

                SublocationsList.Items.Clear();
                foreach (var loc in _location.Children)
                {
                    SublocationsList.Items.Add(loc.Name);
                }
            }
        }
    }
}
