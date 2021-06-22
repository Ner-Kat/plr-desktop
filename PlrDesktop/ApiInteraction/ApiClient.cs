using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.ApiInteraction.Connection;
using PlrDesktop.ApiInteraction.Methods;

namespace PlrDesktop.ApiInteraction
{
    class ApiClient
    {
        ApiServerInfo _serverInfo;
        ApiServerConnection _server;

        public ApiMethods Methods { get; }


        public ApiClient(ApiServerInfo apiServer)
        {
            _serverInfo = apiServer;
            _server = new ApiServerConnection(_serverInfo);
            Methods = new ApiMethods(_server);
        }
    }
}
