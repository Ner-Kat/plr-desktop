using PlrDesktop.ApiInteraction.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.ApiInteraction.Methods
{
    public class UsersMethods
    {
        private ApiServerConnection _server;
        private const string _methodsAddress = "accounts/";

        public UsersMethods(ApiServerConnection server)
        {
            _server = server;
        }
    }
}
