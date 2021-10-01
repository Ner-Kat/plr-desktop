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

namespace PlrDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            MessageBox.Show($"{loc.Name}, {loc.Desc}");
        }
    }
}
