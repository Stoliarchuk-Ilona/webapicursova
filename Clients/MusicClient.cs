using firstWebAPI.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace firstWebAPI.Clients
{
    public class MusicClient
    {
        HttpClient _client;
        private static string _address;
        private static string _apikey;
        public MusicClient()
        {
            _address = Constants.address;
            _apikey = Constants.apikey;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(_address);
        }
        public async Task<Class1> GetInfo(string name)
        {
            var response = await _client.GetAsync($"?method=artist.getinfo&artist={name}&api_key={_apikey}&format=json");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Class1>(content);
            return result;
        }
        public async Task<Albums> GetTopAlbums(string name)
        {
            var response = await _client.GetAsync($"?method=artist.gettopalbums&artist={name}&api_key={_apikey}&format=json");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Albums>(content);
            return result;
        }
        public async Task<Tracks> GetTopTracks(string name)
        {
            var response = await _client.GetAsync($"?method=artist.gettoptracks&artist={name}&api_key={_apikey}&format=json");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Tracks>(content);
            return result;
        }
        public async Task<Similar> GetSimilar(string name)
        {
            var response = await _client.GetAsync($"?method=artist.getsimilar&artist={name}&api_key={_apikey}&format=json");
            response.EnsureSuccessStatusCode();
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<Similar>(content);
            return result;
        }

    }
}
