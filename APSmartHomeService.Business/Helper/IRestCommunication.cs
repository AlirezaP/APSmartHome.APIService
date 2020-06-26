using System;
using System.Threading.Tasks;

namespace APSmartHomeService.Business.Business.Common
{
    public interface IRestCommunication
    {
        Task<T> GetRequest<T>(string uri, bool allowAny = false);
        Task<T> GetRequest<T>(string uri, string query, bool allowAny = false);
        Task<T> GetRequest<T>(Uri uri, bool allowAny = false);
        Task<T> PostRequest<T>(string uri, object obj, bool allowAny = false);
        Task<T> PostRequest<T>(string uri, string jsonObject, bool allowAny = false);
        Task<T> PostRequestWithRedirect<T>(string uri, string jsonObject, bool allowAny = false);
    }
}