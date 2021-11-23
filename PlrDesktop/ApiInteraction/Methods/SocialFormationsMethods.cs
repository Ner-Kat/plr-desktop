using PlrDesktop.ApiInteraction.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.Datacards.MainCards;
using PlrDesktop.Lib;
using System.Text.Json;
using System.Net;

namespace PlrDesktop.ApiInteraction.Methods
{
    public class SocialFormationsMethods : DataMethods
    {
        public override string MethodsAddress { get; } = "socforms/";

        public SocialFormationsMethods(ApiServerConnection server) : base(server)
        {
        }

        public async Task<SocialFormation> Get(int id)
        {
            var request = new RequestString(MethodsAddress, "get");
            request.AddParam("id", id);

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                SocialFormation socForm = JsonSerializer.Deserialize<SocialFormation>(result.Content);
                return socForm;
            }

            return null;
        }

        public async Task<List<SocialFormation>> List(int? count, int? from = 0)
        {
            var request = new RequestString(MethodsAddress, "list");
            if (count.HasValue)
            {
                request.AddParam("count", count.Value);
            }
            if (from.HasValue)
            {
                request.AddParam("from", from.Value);
            }

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                List<SocialFormation> socForms = JsonSerializer.Deserialize<List<SocialFormation>>(result.Content);
                return socForms;
            }

            return null;
        }

        public async Task<List<SocialFormation>> Find(string name)
        {
            var request = new RequestString(MethodsAddress, "find");
            request.AddParam("name", name);

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                List<SocialFormation> socForms = JsonSerializer.Deserialize<List<SocialFormation>>(result.Content);
                return socForms;
            }

            return null;
        }

        public async Task<bool> Add(SocialFormation socForm)
        {
            string url = MethodsAddress + "add";
            var result = await _server.PostAsync(url, socForm);

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> Change(SocialFormation socForm)
        {
            string url = MethodsAddress + "change";
            var result = await _server.PostAsync(url, socForm);

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> Remove(int id)
        {
            var request = new RequestString(MethodsAddress);
            request.AddParam("id", id);

            var result = await _server.GetAsync(request.GetUrl());

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<List<SocialFormation>> SortedList(int? count, int? from = 0)
        {
            var request = new RequestString(MethodsAddress);
            if (count.HasValue)
            {
                request.AddParam("count", count.Value);
            }
            if (from.HasValue)
            {
                request.AddParam("from", from.Value);
            }

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                List<SocialFormation> socForms = JsonSerializer.Deserialize<List<SocialFormation>>(result.Content);
                return socForms;
            }

            return null;
        }
    }
}
