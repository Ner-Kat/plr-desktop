using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.ApiInteraction;
using PlrDesktop.ApiInteraction.Connection;

namespace PlrDesktop.Lib
{
    public interface IApiClients
    {
        public ApiClient Default { get; }

        public ApiClient GetClient(string clientName);

        public bool CreateClient(string clientName, string apiUrl, AuthInfo authInfo);

        public bool RemoveClient(string clientName);
    }
}
