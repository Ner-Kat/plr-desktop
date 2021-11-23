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
    public class CharactersMethods : DataMethods
    {
        public override string MethodsAddress { get; } = "characters/";

        public CharactersMethods(ApiServerConnection server) : base(server)
        {
        }

        public async Task<Character> Get(int id)
        {
            var request = new RequestString(MethodsAddress, "get");
            request.AddParam("id", id);

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                Character character = JsonSerializer.Deserialize<Character>(result.Content);
                return character;
            }

            return null;
        }

        public async Task<List<Character>> List(int? count, int? from = 0)
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
                List<Character> characters = JsonSerializer.Deserialize<List<Character>>(result.Content);
                return characters;
            }

            return null;
        }

        public async Task<List<Character>> Find(string name)
        {
            var request = new RequestString(MethodsAddress, "find");
            request.AddParam("name", name);

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                List<Character> characters = JsonSerializer.Deserialize<List<Character>>(result.Content);
                return characters;
            }

            return null;
        }

        public async Task<bool> Add(Character character)
        {
            string url = MethodsAddress + "add";
            var result = await _server.PostAsync(url, character);

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> Change(Character character)
        {
            string url = MethodsAddress + "change";
            var result = await _server.PostAsync(url, character);

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> Remove(int id)
        {
            var request = new RequestString(MethodsAddress);
            request.AddParam("id", id);

            var result = await _server.GetAsync(request.GetUrl());

            return result.StatusCode == HttpStatusCode.OK;
        }

        public async Task<List<Character>> SortedList(int? count, int? from = 0)
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
                List<Character> characters = JsonSerializer.Deserialize<List<Character>>(result.Content);
                return characters;
            }

            return null;
        }

        public async Task<CharacterShort> GetShort(int id)
        {
            var request = new RequestString(MethodsAddress, "get");
            request.AddParam("id", id);

            var result = await _server.GetAsync(request.GetUrl());
            if (result.StatusCode == HttpStatusCode.OK)
            {
                CharacterShort character = JsonSerializer.Deserialize<CharacterShort>(result.Content);
                return character;
            }

            return null;
        }
    }
}
