using PlrDesktop.ApiInteraction.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.ApiInteraction.Methods
{
    public abstract class DataMethods
    {
        protected ApiServerConnection _server;
        public virtual string MethodsAddress { get; }

        public DataMethods(ApiServerConnection server)
        {
            _server = server;
        }

        protected string GetFullPath()
        {
            return _server.ApiServerInfo.Address + MethodsAddress;
        }
    }
}
