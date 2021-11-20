using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PlrDesktop.Lib
{
    class GetRequestString
    {
        string _methodUrl = "";
        string _resultString = "";

        Dictionary<string, string> _params = new Dictionary<string, string>();

        public GetRequestString(string methodUrl)
        {
            _methodUrl = methodUrl;

            if (!_methodUrl.EndsWith('/'))
            {
                _methodUrl += '/';
            }
        }

        public GetRequestString(string url, string methodName)
        {
            if (!url.EndsWith('/'))
            {
                url += '/';
            }

            _methodUrl = url + methodName;
        }

        public string GetUrl()
        {
            return _resultString;
        }

        private void UpdateResultString()
        {
            _resultString = _methodUrl;
            if (_params.Count > 0)
            {
                _resultString += "?";

                foreach (var param in _params)
                {
                    string encodedValue = HttpUtility.UrlEncode(param.Value);
                    _resultString += $"{param.Key}={encodedValue}&";
                }
                _resultString = _resultString.Remove(_resultString.Length - 1);
            }
        }

        public GetRequestString AddParam(string name, int value)
        {
            return AddParam(name, value.ToString());
        }

        public GetRequestString AddParam(string name, string value)
        {
            AddParams(new Dictionary<string, string> { { name, value } });
            UpdateResultString();

            return this;
        }

        public string AddParams(Dictionary<string, string> parameters)
        {
            foreach(var param in parameters)
            {
                if (!_params.ContainsKey(param.Key))
                {
                    _params.Add(param.Key, param.Value);
                }
                else
                {
                    _params[param.Key] = param.Value;
                }
            }

            UpdateResultString();
            return GetUrl();
        }
    }
}
