using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.ApiInteraction.Connection
{
    public class ApiServerInfo
    {
        private string _address;

        public string Address { get { return _address; } }

        public ApiServerInfo(string apiServerAddress)
        {
            _address = apiServerAddress;
        }
    }
}
