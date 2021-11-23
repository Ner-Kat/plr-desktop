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

namespace PlrDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
            
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IApiClients, ApiClients>();

            services.AddSingleton<MainWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            serviceProvider.GetService<IApiClients>().CreateClient(
                "default", "https://localhost:16500/api/", 
                new AuthInfo() { Login = "admin", Password = "admin" });

            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
