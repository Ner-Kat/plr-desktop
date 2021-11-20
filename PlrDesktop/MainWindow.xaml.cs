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

namespace PlrDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Location> locationsOc;

        public MainWindow()
        {
            InitializeComponent();
            
            // locationsOc = new ObservableCollection<Location>(GetLocations().Result);
        }

        private async Task<List<Location>> GetLocations()
        {
            var info = new ApiServerInfo("https://localhost:16500/api/");
            var auth = new AuthInfo()
            {
                Login = "webUser",
                Password = "webPass"
            };
            ApiClient client = new ApiClient(info, auth);

            // List<Location> locs = await client.Methods.Locs.List(null);
            // MessageBox.Show($"{locs[0].Desc}");
            // return locs;

            Location loc = await client.Methods.Locs.Get(1);
            MessageBox.Show($"{loc.Desc}");
            return new List<Location>() { loc };
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var info = new ApiServerInfo("https://localhost:16500/api/");
            var auth = new AuthInfo()
            {
                Login = "webUser",
                Password = "webPass"
            };
            ApiClient client = new ApiClient(info, auth);

            Location loc = await client.Methods.Locs.Get(1);
            string outStr = $"Id = {loc.Id}\nName = {loc.Name}\nDesc = {loc.Desc}\n" +
                $"ParentLocId = {(loc.ParentLoc is not null ? loc.ParentLoc.Id : "")}\nParentLocName = {(loc.ParentLoc is not null ? loc.ParentLoc.Name : "")}\n" +
                $"Children: \n";
            foreach (var cloc in loc.Children)
            {
                outStr += $"\tId = {cloc.Id}, Name = {cloc.Name}\n";
            }
            MessageBox.Show(outStr);
        }
    }
}
