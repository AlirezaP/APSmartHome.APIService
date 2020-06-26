using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APSmartHomeService.Business.Business.Common
{
    public class RestCommunication : IRestCommunication
    {
        private static RestHelper rest;
        public RestCommunication(string baseUrl)
        {
            if (rest == null || rest.BaseAddress != baseUrl)
            {
                rest = new RestHelper(baseUrl);
            }
        }
        private void AddHeader()
        {
            Dictionary<string, string> head = new Dictionary<string, string>();
            head.Add("Authorization", $"{Cache.Token}");
            head.Add("DeviceID", $"{Cache.DeviceID}");
            head.Add("AgentType", $"{Cache.Agent}");


            rest.AddHeader(head);
        }

        public async Task<T> GetRequest<T>(string uri, string query, bool allowAny = false)
        {
            if (!allowAny)
            {
                AddHeader();
            }
            return await rest.GetRequest<T>(uri, query);
        }

        public async Task<T> GetRequest<T>(Uri uri, bool allowAny = false)
        {
            if (!allowAny)
            {
                AddHeader();
            }
            return await rest.GetRequest<T>(uri);
        }

        public async Task<T> GetRequest<T>(string uri, bool allowAny = false)
        {
            if (!allowAny)
            {
                AddHeader();
            }
            return await rest.GetRequest<T>(uri);
        }

        public async Task<T> PostRequest<T>(string uri, string jsonObject, bool allowAny = false)
        {
            if (!allowAny)
            {
                AddHeader();
            }
            return await rest.PostRequest<T>(uri, jsonObject);
        }

        //public void PostRequest(string uri, object obj, bool allowAny = false)
        //{
        //    if (!allowAny)
        //    {
        //        AddHeader();
        //    }
        //    var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        //     rest.PostRequest(uri, jsonObject);

        //}

        public async Task<T> PostRequest<T>(string uri, object obj, bool allowAny = false)
        {
            if (!allowAny)
            {
                AddHeader();
            }
            var jsonObject = System.Text.Json.JsonSerializer.Serialize(obj);
            return await rest.PostRequest<T>(uri, jsonObject);
        }

        public async Task<T> PostRequestWithRedirect<T>(string uri, string jsonObject, bool allowAny = false)
        {
            if (!allowAny)
            {
                AddHeader();
            }
            return await rest.PostRequestWithRedirect<T>(uri, jsonObject);
        }
    }
}
