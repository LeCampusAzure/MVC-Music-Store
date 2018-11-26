using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace MvcMusicStore.Helpers
{
    public class ApiHelper
    {
        IConfiguration config;
        string connStringName = "MvcMusicStoreService";

        public ApiHelper(IConfiguration _config)
        {
            config = _config;
        }


        public async Task<V> PostAsync<T,V>(string apiAddress, T value)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(config.GetConnectionString(connStringName));
            var response = await client.PostAsJsonAsync(apiAddress, value);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<V>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new Exception("Cannot get response from the API");
            }

        }


        public async Task PostAsync<T>(string apiAddress, T value)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(config.GetConnectionString(connStringName));
            var response = await client.PostAsJsonAsync(apiAddress, value);

            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                throw new Exception("Cannot get response from the API");
            }

        }


        public async Task<T> GetAsync<T>(string apiAddress)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(config.GetConnectionString(connStringName));
            var response = await client.GetAsync(apiAddress);

            if (response.IsSuccessStatusCode)
            {
                var str = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<T>(str);
            }
            else
            {
                throw new Exception("Cannot get response from the API");
            }

        }

        public T Get<T>(string apiAddress)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(config.GetConnectionString(connStringName));
            var response = Task.Run(() => client.GetAsync(apiAddress)).Result;
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new Exception("Cannot get response from the API");
            }

        }


        public async Task DeleteAsync(string apiAddress, int id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(config.GetConnectionString(connStringName));
            var response = await client.DeleteAsync(apiAddress + "/" + id);

            if (response.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                throw new Exception("Cannot get response from the API");
            }

        }
    }
}
