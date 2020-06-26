using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APSmartHomeService.Business.Business.Common
{
    public class RestHelper
    {
        private System.Net.Http.HttpClient client;

        public string BaseAddress
        {
            get
            {
                if (client != null)
                {
                    return client.BaseAddress.ToString();
                }
                return "";
            }
        }
        public RestHelper(string baseurl)
        {
            var httpClientHandler = new HttpClientHandler();
            
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

            
            client = new System.Net.Http.HttpClient(httpClientHandler);
            client.BaseAddress = new Uri(baseurl);
        }

        public void AddHeader(Dictionary<string, string> head)
        {
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Clear();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var item in head)
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
            }
        }
        public async Task<T> GetRequest<T>(string uri, string query)
        {
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/json"));

            //var query = HttpUtility.ParseQueryString(string.Empty);
            //query["foo"] = "bar<>&-baz";
            //query["bar"] = "bazinga";
            //string queryString = query.ToString();

            var res = await client.GetStringAsync(uri + "?" + query);

            return System.Text.Json.JsonSerializer.Deserialize<T>(res);
        }

        public async Task<T> GetRequest<T>(Uri uri)
        {
            var res = await client.GetStringAsync(uri);

            return System.Text.Json.JsonSerializer.Deserialize<T>(res);
        }

        public async Task<T> GetRequest<T>(string uri, Dictionary<string, string> header = null)
        {
            //client.DefaultRequestHeaders.Clear();

            //if (header != null && header.Count > 0)
            //{
            //    foreach (var item in header)
            //    {
            //        client.DefaultRequestHeaders.Add(item.Key, item.Value);
            //    }
            //}

            var res = await client.GetStringAsync(uri);

            return System.Text.Json.JsonSerializer.Deserialize<T>(res);
        }

        //public async Task<T> PostRequest<T>(string uri, string jsonObject)
        //{
        //    return await PostRequest<T>(new Uri(uri), jsonObject);
        //}

        public async Task<T> PostRequest<T>(string uri, string jsonObject)
        {
            try
            {
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(uri, content);

                return System.Text.Json.JsonSerializer.Deserialize<T>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async void PostRequest(string uri, string jsonObject)
        {
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(uri, content);
        }

        public async Task<T> PostRequestWithRedirect<T>(string uri, string jsonObject)
        {
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(uri, content);

            var reuri = result.Headers.Location;

            return await GetRequest<T>(reuri);
        }
    }
}
