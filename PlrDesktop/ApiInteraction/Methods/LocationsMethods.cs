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
            var result = await _server.GetAsync(MethodsAddress + $"get?id={id}");
            if (result.StatusCode == HttpStatusCode.OK)
            {
                Location location = JsonSerializer.Deserialize<Location>(result.Content);
                return location;
            }

            return null;
        }
    }
}
