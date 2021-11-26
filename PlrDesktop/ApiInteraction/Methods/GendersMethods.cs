using PlrDesktop.ApiInteraction.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.ApiInteraction.Lib;
using System.Net;
using System.Text.Json;
using PlrDesktop.Datacards.Utils;

namespace PlrDesktop.ApiInteraction.Methods
{
    public class GendersMethods : DataMethods
    {
        override public string MethodsAddress { get; } = "genders/";

        public GendersMethods(ApiServerConnection server) : base(server)
        {
        }

        public async Task<string> Get(int id)
        {
            var request = new RequestString(MethodsAddress, "get");
            request.AddParam("id", id);

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                string genderName = JsonSerializer.Deserialize<string>(result.Content);
                return genderName;
            }

            return null;
        }

        public async Task<List<Gender>> List()
        {
            string url = MethodsAddress + "list";

            var result = await _server.GetAsync(url);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                List<Gender> genders = JsonSerializer.Deserialize<List<Gender>>(result.Content);
                return genders;
            }

            return null;
        }

        public async Task<int?> GetId(string name)
        {
            var request = new RequestString(MethodsAddress, "getid");
            request.AddParam("name", name);

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                int id = JsonSerializer.Deserialize<int>(result.Content);
                return id;
            }

            return null;
        }
    }
}
