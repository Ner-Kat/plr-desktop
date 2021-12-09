using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PlrDesktop.Lib
{
    public static class PlrWpfUtils
    {
        public static void ClearDataGridSelection(DataGrid dg)
        {
            if (dg.Items.Count > 0)
            {
                dg.SelectedIndex = 0;
                dg.SelectedIndex = -1;
            }
        }
    }
}
