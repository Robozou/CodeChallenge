using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CodeChallengeV2.Models;

namespace CodeChallengeV2.Services
{
    public class GeocodeService : IGeocodeService
    {
        public async Task<string> FindCountryCode(GeocodePayload payload)
        {
            HttpClient httpClient = new HttpClient();
            string request = $"https://geocode.xyz/{payload.Lat},{payload.Long}?json=1&auth=637940924585023510678x2173";
            HttpResponseMessage httpResponseMessage = (await httpClient.GetAsync(request)).EnsureSuccessStatusCode();
            return ((string)JObject.Parse(await httpResponseMessage.Content.ReadAsStringAsync())["state"]).ToUpper();
        }
    }
}
