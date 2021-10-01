using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PlrDesktop.ApiInteraction.Connection
{
    public class AuthProvider
    {
        private string _authApiPath;
        private ApiServerRequester _requester;

        private AuthInfo _authInfo;
        AuthTokens _tokens;


        public AuthProvider(AuthInfo authInfo, string authApiPath, ApiServerRequester requester)
        {
            _authApiPath = authApiPath;
            _authInfo = authInfo;
            _requester = requester;
            _tokens = new AuthTokens();
        }

        public void ChangeInfo(AuthInfo newInfo)
        {
            _authInfo = newInfo;
        }

        public void ChangeRequester(ApiServerRequester newRequester)
        {
            _requester = newRequester;
        }

        public bool IsTokensExists()
        {
            return !string.IsNullOrEmpty(_tokens.AccessToken) && !string.IsNullOrEmpty(_tokens.RefreshToken);
        }

        public AuthenticationHeaderValue GetAuthHeader()
        {
            if (!IsTokensExists())
            {
                UpdateTokens();
            }

            if (string.IsNullOrEmpty(_tokens.AccessToken))
            {
                return null;
            }

            return new AuthenticationHeaderValue("Bearer", _tokens.AccessToken);
        }

        public async Task<ApiServerRequesterResult> GetWithAuth(string address)
        {
            var request = _requester.FormNewRequest(HttpMethod.Get, new Uri(address));
            _requester.AddAuthHeader(request, GetAuthHeader());

            ApiServerRequesterResult response = await _requester.Send(request);
            response = await EnsureAccess(response);

            return response;
        }

        public async Task<ApiServerRequesterResult> PostWithAuth(string address, object data)
        {
            var request = _requester.FormNewRequest(HttpMethod.Post, new Uri(address));
            _requester.AddAuthHeader(request, GetAuthHeader());

            string jsonData = JsonSerializer.Serialize(data);
            _requester.AddData(request, jsonData);

            ApiServerRequesterResult response = await _requester.Send(request);
            response = await EnsureAccess(response);

            return response;
        }

        private async Task<ApiServerRequesterResult> EnsureAccess(ApiServerRequesterResult response)
        {
            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return response;
            }

            UpdateTokens();
            var newResponse = await _requester.Send(response.Request);
            return newResponse;
        }

        private async void Authenticate()
        {
            var request = _requester.FormNewRequest(HttpMethod.Post, new Uri(_authApiPath + "gettoken"));

            string requestData = JsonSerializer.Serialize(_authInfo.GetAsDictionary());
            _requester.AddData(request, requestData);

            ApiServerRequesterResult result = await _requester.Send(request);
            _tokens = JsonSerializer.Deserialize<AuthTokens>(result.Content);
        }

        private async void UpdateTokens()
        {
            if (!string.IsNullOrEmpty(_tokens.RefreshToken))
            {
                var request = _requester.FormNewRequest(HttpMethod.Post, new Uri(_authApiPath + "refreshtoken"));
                Dictionary<string, string> refreshTokenData = new Dictionary<string, string>()
                {
                    { "RefreshToken", _tokens.RefreshToken }
                };

                string requestData = JsonSerializer.Serialize(refreshTokenData);
                _requester.AddData(request, requestData);

                ApiServerRequesterResult result = await _requester.Send(request);
                _tokens = JsonSerializer.Deserialize<AuthTokens>(result.Content);

                if (!string.IsNullOrEmpty(_tokens.RefreshToken))
                {
                    Authenticate();
                }
            }
            else
            {
                Authenticate();
            }
        }
    }
}
