using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.ApiInteraction.Connection;
using PlrDesktop.ApiInteraction.Methods;

namespace PlrDesktop.ApiInteraction
{
    public class ApiClient
    {
        ApiServerInfo _serverInfo;
        ApiServerConnection _server;

        public ApiMethods Methods { get; }


        public ApiClient(ApiServerInfo apiServerInfo)
        {
            _serverInfo = apiServerInfo;
            _server = new ApiServerConnection(_serverInfo);
            Methods = new ApiMethods(_server);
        }

        public ApiClient(ApiServerInfo apiServerInfo, AuthInfo authInfo) : this(apiServerInfo)
        {
            _server.AuthInfo = authInfo;
        }
    }
}
