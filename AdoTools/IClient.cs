using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DevOpsTools
{
    public interface IClient
    {
        void AddHeaders();

        Task<HttpResponseMessage> DeleteAsync(Uri uri);

        Task<string> GetStringAsync(Uri uri);

        Task<HttpResponseMessage> PostStringAsync(Uri uri, string content);

        Task<HttpResponseMessage> PutStringAsync(Uri uri, string content);
    }
}