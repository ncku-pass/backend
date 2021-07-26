using Infrastructure.Service.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class NCKUPortalAPI : INCKUPortalAPI
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public NCKUPortalAPI(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetExpRecordAsync(string key, string keyval, string type)
        {
            // 建立 HttpClient 實例
            var httpClient = _httpClientFactory.CreateClient();

            string boundary = DateTime.Now.Ticks.ToString("X");
            var formData = new MultipartFormDataContent(boundary);
            formData.Add(new StringContent(key), "key");
            formData.Add(new StringContent(keyval), "keyval");

            httpClient.BaseAddress = new Uri("https://i.ncku.edu.tw/");
            var response = await httpClient.PostAsync($"ncku/oauth/eportfolio/getdata.php?type={type}", formData);

            return await response.Content.ReadAsStringAsync();
        }

    }
}
