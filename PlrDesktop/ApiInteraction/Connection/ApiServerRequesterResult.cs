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

        private HttpRequestMessage _request;
        public HttpRequestMessage Request { 
            get 
            {
                return _request;
            }
            set 
            {
                _request = new HttpRequestMessage()
                {
                    Method = value.Method,
                    Content = value.Content,
                    RequestUri = value.RequestUri
                };

                foreach(var header in value.Headers)
                {
                    _request.Headers.Add(header.Key, header.Value);
                }
            }
        }
    }
}
