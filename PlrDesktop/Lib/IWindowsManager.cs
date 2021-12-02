using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PlrDesktop.Datacards;
using PlrDesktop.Windows;

namespace PlrDesktop.Lib
{
    public interface IWindowsManager
    {
        public MainWindow MainWindow { get; set; }

        public Window CreateLocationDetailsWindow(int id);
        public Window CreateLocationEditWindow(Location location);
        public Window CreateLocationAddWindow();


        public Window CreateRaceDetailsWindow(int id);
        public Window CreateRaceEditWindow(Race race);
        public Window CreateRaceAddWindow();


        public Window CreateSocFormDetailsWindow(int id);
        public Window CreateSocFormEditWindow(SocialFormation socForm);
        public Window CreateSocFormAddWindow();
    }
}
