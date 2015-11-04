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

        #region Request Methods

        public new async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return await AnalyzeLingoHubResponse(await base.PostAsync(requestUri, content));
        }

        public new async Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            return await AnalyzeLingoHubResponse(await base.PostAsync(requestUri, content));
        }

        public new async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return await AnalyzeLingoHubResponse(await base.GetAsync(requestUri));
        }

        public new async Task<string> GetStringAsync(string requestUri)
        {
            var response = await AnalyzeLingoHubResponse(await base.GetAsync(requestUri));
            return await response.Content.ReadAsStringAsync();
        }

        public new async Task<string> GetStringAsync(Uri requestUri)
        {
            var response = await AnalyzeLingoHubResponse(await base.GetAsync(requestUri));
            return await response.Content.ReadAsStringAsync();
        }


        public new async Task<byte[]> GetByteArrayAsync(string requestUri)
        {
            var response = await AnalyzeLingoHubResponse(await base.GetAsync(requestUri));
            return await response.Content.ReadAsByteArrayAsync();
        }

        public new async Task<byte[]> GetByteArrayAsync(Uri requestUri)
        {
            var response = await AnalyzeLingoHubResponse(await base.GetAsync(requestUri));
            return await response.Content.ReadAsByteArrayAsync();
        }

        #endregion

        /// <summary>
        /// Analyze the response from LingoHub to extract error messages.
        /// </summary>
        /// <param name="response">HTTP Response</param>
        /// <returns>HTTP Response</returns>
        private static async Task<HttpResponseMessage> AnalyzeLingoHubResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return response;

            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(string.Format("An error occured during request (HTTP {0}):\n{1}\n------", response.StatusCode, error));
        }
    }
}