using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.ApiInteraction;
using PlrDesktop.ApiInteraction.Connection;

namespace PlrDesktop.Lib
{
    public class ApiClients : IApiClients
    {
        private Dictionary<string, ApiClient> _clients;

        public ApiClient Default { 
            get
            {
                if (_clients.ContainsKey("default"))
                    return _clients["default"];
                else
                    return null;
            } 
        }

        public ApiClients()
        {
            _clients = new Dictionary<string,ApiClient>();
        }

        public ApiClient GetClient(string clientName)
        {
            if (_clients.ContainsKey(clientName))
                return _clients[clientName];

            return null;
        }

        public bool CreateClient(string clientName, string apiUrl, AuthInfo authInfo)
        {
            ApiClient newClient = new ApiClient(new ApiServerInfo(apiUrl), authInfo);
            _clients.Add(clientName, newClient);

            return true;
        }

        public bool RemoveClient(string clientName)
        {
            return _clients.Remove(clientName);
        }
    }
}
