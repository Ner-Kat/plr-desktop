using PlrDesktop.ApiInteraction.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.ApiInteraction.Methods
{
    public class CharactersMethods
    {
        private string _apiAddress;

        public CharactersMethods(ApiServerInfo apiServer)
        {
            _apiAddress = apiServer.Address;
        }
    }
}
