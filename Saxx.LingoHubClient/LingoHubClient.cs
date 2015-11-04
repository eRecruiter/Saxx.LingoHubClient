using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// Fetch the data from the LingoHub API.
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>HTTP Response</returns>
        private async Task<HttpResponseMessage> Fetch(string url)
        {
            var response = await GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(string.Format("An error occured during request (HTTP {0}):\n{1}\n------", response.StatusCode, error));
        }

        /// <summary>
        /// Fetch the data from the LingoHub API as string.
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>Data as String</returns>
        private async Task<string> FetchAsString(string url)
        {
            return await Fetch(url).Result.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Fetch the data from the LingoHub API as byte array.
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>Data as String</returns>
        private async Task<byte[]> FetchAsByteArray(string url)
        {
            return await Fetch(url).Result.Content.ReadAsByteArrayAsync();
        }
    }
}