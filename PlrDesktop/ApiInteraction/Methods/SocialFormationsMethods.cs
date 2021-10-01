using PlrDesktop.ApiInteraction.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.ApiInteraction.Methods
{
    public class SocialFormationsMethods
    {
        private ApiServerConnection _server;
        private const string _methodsAddress = "socforms/";

        public SocialFormationsMethods(ApiServerConnection server)
        {
            _server = server;
        }
    }
}
