using PlrDesktop.ApiInteraction.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlrDesktop.Datacards.MainCards;
using PlrDesktop.Datacards.InputCards;
using System.Net;
using System.Windows;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using PlrDesktop.Lib;

namespace PlrDesktop.ApiInteraction.Methods
{
    public class LocationsMethods : DataMethods
    {
        override public string MethodsAddress { get; } = "locations/";

        public LocationsMethods(ApiServerConnection server) : base(server)
        {
        }

        public async Task<Location> Get(int id)
        {
            var request = new GetRequestString(MethodsAddress, "get");
            request.AddParam("id", id);

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Location location = JsonSerializer.Deserialize<Location>(result.Content, options);
                return location;
            }

            return null;
        }

        public async Task<List<Location>> List(int? count, int? from = 0)
        {
            var request = new GetRequestString(MethodsAddress, "list");
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
                List<Location> locations = JsonSerializer.Deserialize<List<Location>>(result.Content);
                return locations;
            }

            return null;
        }

        public async Task<List<Location>> Find(string name)
        {
            var request = new GetRequestString(MethodsAddress, "find");
            request.AddParam("name", name);

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                List<Location> locations = JsonSerializer.Deserialize<List<Location>>(result.Content);
                return locations;
            }

            return null;
        }

        public async Task<bool> Add(Location loc)
        {
            string url = MethodsAddress + "add";
            var result = await _server.PostAsync(url, loc);

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> Change(Location loc)
        {
            string url = MethodsAddress + "change";
            var result = await _server.PostAsync(url, loc);

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> Remove(int id)
        {
            var request = new GetRequestString(MethodsAddress);
            request.AddParam("id", id);

            var result = await _server.GetAsync(request.GetUrl());

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<List<Location>> SortedList(int? count, int? from = 0)
        {
            var request = new GetRequestString(MethodsAddress);
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
                List<Location> locations = JsonSerializer.Deserialize<List<Location>>(result.Content);
                return locations;
            }

            return null;
        }

        public async Task<List<Location>> Sublocations(int id)
        {
            var request = new GetRequestString(MethodsAddress);
            request.AddParam("id", id);

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                List<Location> locations = JsonSerializer.Deserialize<List<Location>>(result.Content);
                return locations;
            }

            return null;
        }

        public async Task<List<Location>> Rootlocations()
        {
            string url = MethodsAddress + "rootlocations";

            var result = await _server.GetAsync(url);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                List<Location> locations = JsonSerializer.Deserialize<List<Location>>(result.Content);
                return locations;
            }

            return null;
        }
    }
}
