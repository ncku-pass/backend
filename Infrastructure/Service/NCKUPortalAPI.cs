using Infrastructure.Service.Interface;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
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
            httpClient.BaseAddress = new Uri("https://i.ncku.edu.tw/");

            
            string boundary = DateTime.Now.Ticks.ToString("X");
            var formData = new MultipartFormDataContent(boundary);
            formData.Add(new StringContent(key), "key");
            formData.Add(new StringContent(keyval), "keyval");

            var response = await httpClient.PostAsync($"ncku/oauth/eportfolio/getdata.php?type={type}", formData);
            var responseStr = UnicodeConvert(await response.Content.ReadAsStringAsync());
            return responseStr;
        }
        string UnicodeConvert(string inputstr)
        {
            inputstr = Regex.Replace(inputstr, "\\\\u\\w{4}",
                delegate (Match m)
                {
                    return ((char)(int.Parse(m.Value.Substring(2), System.Globalization.NumberStyles.HexNumber))).ToString();
                });
            return inputstr;
        }
    }
}