using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PlrDesktop.Lib;
using PlrDesktop.ApiInteraction.Connection;
using PlrDesktop.Windows;
using System.Windows.Media.Animation;

namespace PlrDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IApiClients, ApiClients>();
            services.AddSingleton<IWindowsManager, WindowsManager>();

            services.AddSingleton<MainWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            _serviceProvider.GetService<IApiClients>().CreateClient(
                "default", "https://localhost:16500/api/", 
                new AuthInfo() { Login = "admin", Password = "admin" });

            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
