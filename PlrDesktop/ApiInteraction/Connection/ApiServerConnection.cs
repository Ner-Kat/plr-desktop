using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PlrDesktop.ApiInteraction.Connection
{
    public class ApiServerConnection
    {
        private const string _authApiRelativePath = "accounts/";

        private ApiServerInfo _serverInfo;
        private HttpClient _httpClient;
        private ApiServerRequester _requester;
        private AuthProvider _authProvider;

        public ApiServerInfo ApiServerInfo 
        { 
            get
            {
                return _serverInfo;
            } 
        }

        private AuthInfo _authInfo;
        public AuthInfo AuthInfo
        {
            get
            {
                return _authInfo;
            }
            set
            {
                _authInfo = value;

                if (_authProvider != null)
                {
                    _authProvider.ChangeInfo(_authInfo);
                }
                else
                {
                    _authProvider = new AuthProvider(_authInfo, _serverInfo.Address + _authApiRelativePath, _requester);
                }
            }
        }

        public ApiServerConnection(ApiServerInfo serverInfo)
        {
            _serverInfo = serverInfo;

            var httpHandler = new HttpClientHandler();
            _httpClient = new HttpClient(httpHandler);

            _requester = new ApiServerRequester(_httpClient);
        }

        public ApiServerConnection(ApiServerInfo serverInfo, AuthInfo authInfo) : this(serverInfo)
        {
            _authProvider = new AuthProvider(authInfo, _serverInfo.Address + _authApiRelativePath, _requester);
        }

        public async Task<ApiServerRequesterResult> GetAsync(string path)
        {
            string fullRequestPath = _serverInfo.Address + path;

            if (_authProvider != null)
            {
                return await _authProvider.GetWithAuth(fullRequestPath);
            }

            return await SimpleGet(fullRequestPath);
        }

        public async Task<ApiServerRequesterResult> GetWithoutAuthAsync(string path)
        {
            string fullRequestPath = _serverInfo.Address + path;
            return await SimpleGet(fullRequestPath);
        }

        private async Task<ApiServerRequesterResult> SimpleGet(string address)
        {
            var request = _requester.FormNewRequest(HttpMethod.Get, new Uri(address));
            ApiServerRequesterResult result = await _requester.Send(request);
            return result;
        }

        public async Task<ApiServerRequesterResult> PostAsync(string path, object data)
        {
            string fullRequestPath = _serverInfo.Address + path;

            if (_authProvider != null)
            {
                return await _authProvider.PostWithAuth(fullRequestPath, data);
            }

            return await SimplePost(fullRequestPath, data);
        }

        public async Task<ApiServerRequesterResult> PostWithoutAuthAsync(string path, object data)
        {
            string fullRequestPath = _serverInfo.Address + path;
            return await SimplePost(fullRequestPath, data);
        }

        private async Task<ApiServerRequesterResult> SimplePost(string address, object data)
        {
            var request = _requester.FormNewRequest(HttpMethod.Post, new Uri(address));

            string jsonData = JsonSerializer.Serialize(data);
            _requester.AddData(request, jsonData);

            ApiServerRequesterResult result = await _requester.Send(request);
            return result;
        }
    }
}
