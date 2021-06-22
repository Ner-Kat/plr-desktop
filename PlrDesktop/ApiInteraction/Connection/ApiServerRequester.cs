using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace PlrDesktop.ApiInteraction.Connection
{
    public class ApiServerRequester
    {
        private HttpClient _httpClient;


        public ApiServerRequester(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpRequestMessage FormNewRequest(HttpMethod method, Uri address, AuthenticationHeaderValue authHeader = null, string data = null)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = method,
                RequestUri = address
            };

            AddAuthHeader(request, authHeader);
            AddData(request, data);

            return request;
        }

        public void AddAuthHeader(HttpRequestMessage request, AuthenticationHeaderValue authHeader)
        {
            if (authHeader != null)
            {
                request.Headers.Authorization = authHeader;
            }
        }

        public void AddData(HttpRequestMessage request, string data)
        {
            if (data != null && request.Method == HttpMethod.Post)
            {
                request.Content = new StringContent(data, Encoding.UTF8, "application/json");
            }
        }

        public async Task<ApiServerRequesterResult> Send(HttpRequestMessage request)
        {
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            var result = new ApiServerRequesterResult()
            {
                StatusCode = response.StatusCode,
                Content = await response.Content.ReadAsStringAsync(),
                Request = request
            };
            return result;
        }
    }
}
