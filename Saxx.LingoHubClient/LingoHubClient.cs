using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Saxx.LingoHubClient
{
    public partial class LingoHubClient : HttpClient
    {
        public LingoHubClient(string username, string password)
        {
            var encodedCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password)));
            BaseAddress = new Uri("https://api.lingohub.com/v1/");
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
        }
    }
}