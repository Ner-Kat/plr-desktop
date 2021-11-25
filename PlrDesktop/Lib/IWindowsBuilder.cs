using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PlrDesktop.Datacards.MainCards;

namespace PlrDesktop.Lib
{
    public interface IWindowsBuilder
    {
        public Window CreateLocationDetailsWindow(int id);
        public Window CreateLocationEditWindow(Location location);
    }
}
