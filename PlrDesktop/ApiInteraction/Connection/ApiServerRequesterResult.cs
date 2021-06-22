using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PlrDesktop.ApiInteraction.Connection
{
    public class ApiServerRequesterResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
        public HttpRequestMessage Request { get; set; }
    }
}
