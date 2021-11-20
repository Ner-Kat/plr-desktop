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
    public class RacesMethods : DataMethods
    {
        public override string MethodsAddress { get; } = "races/";

        public RacesMethods(ApiServerConnection server) : base(server)
        {
        }

        public async Task<Race> Get(int id)
        {
            var request = new GetRequestString(MethodsAddress, "get");
            request.AddParam("id", id);

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                Race race = JsonSerializer.Deserialize<Race>(result.Content);
                return race;
            }

            return null;
        }

        public async Task<List<Race>> List(int? count, int? from = 0)
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
                List<Race> races = JsonSerializer.Deserialize<List<Race>>(result.Content);
                return races;
            }

            return null;
        }

        public async Task<List<Race>> Find(string name)
        {
            var request = new GetRequestString(MethodsAddress, "find");
            request.AddParam("name", name);

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                List<Race> races = JsonSerializer.Deserialize<List<Race>>(result.Content);
                return races;
            }

            return null;
        }

        public async Task<bool> Add(Race race)
        {
            string url = MethodsAddress + "add";
            var result = await _server.PostAsync(url, race);

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> Change(Race race)
        {
            string url = MethodsAddress + "change";
            var result = await _server.PostAsync(url, race);

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> Remove(int id)
        {
            var request = new GetRequestString(MethodsAddress);
            request.AddParam("id", id);

            var result = await _server.GetAsync(request.GetUrl());

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<List<Race>> SortedList(int? count, int? from = 0)
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
                List<Race> races = JsonSerializer.Deserialize<List<Race>>(result.Content);
                return races;
            }

            return null;
        }
    }
}
