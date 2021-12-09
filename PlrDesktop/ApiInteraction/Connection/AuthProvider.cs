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

        public bool TokensExists()
        {
            return !string.IsNullOrEmpty(_tokens.AccessToken) && !string.IsNullOrEmpty(_tokens.RefreshToken);
        }

        public async Task<AuthenticationHeaderValue> GetAuthHeader()
        {
            if (!TokensExists())
            {
                if (!await UpdateTokens())
                {
                    return null;
                }
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
            _requester.AddAuthHeader(request, await GetAuthHeader());

            ApiServerRequesterResult response = await _requester.SendAsync(request);
            response = await EnsureAccess(response);

            return response;
        }

        public async Task<ApiServerRequesterResult> PostWithAuth(string address, object data)
        {
            var request = _requester.FormNewRequest(HttpMethod.Post, new Uri(address));
            _requester.AddAuthHeader(request, await GetAuthHeader());

            string jsonData = JsonSerializer.Serialize(data);
            _requester.AddData(request, jsonData);

            ApiServerRequesterResult response = await _requester.SendAsync(request);
            response = await EnsureAccess(response);

            return response;
        }

        private async Task<ApiServerRequesterResult> EnsureAccess(ApiServerRequesterResult response)
        {
            if (response.StatusCode != HttpStatusCode.Unauthorized)
            {
                return response;
            }

            if (await UpdateTokens())
            {
                var newResponse = await _requester.SendAsync(response.Request);
                return newResponse;
            }

            return new ApiServerRequesterResult { StatusCode = HttpStatusCode.Unauthorized };
        }

        private async Task<bool> Authenticate()
        {
            var request = _requester.FormNewRequest(HttpMethod.Post, new Uri(_authApiPath + "gettoken"));

            string requestData = JsonSerializer.Serialize(_authInfo.GetAsDictionary());
            _requester.AddData(request, requestData);

            ApiServerRequesterResult result = await _requester.SendAsync(request);
            try
            {
                _tokens = JsonSerializer.Deserialize<AuthTokens>(result.Content);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        private async Task<bool> UpdateTokens()
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

                ApiServerRequesterResult result = await _requester.SendAsync(request);
                _tokens = JsonSerializer.Deserialize<AuthTokens>(result.Content);

                if (!string.IsNullOrEmpty(_tokens.RefreshToken))
                {
                    return await Authenticate();
                }
            }
            else
            {
                return await Authenticate();
            }

            return false;
        }
    }
}
