using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DevOpsTools
{
    /// <summary>
    ///     Client for communicating with Azure DevOps (Visual Studio Team Services/VSTS)
    /// </summary>
    public class Client : IClient
    {
        #region Constructors

        public Client(
            string personalAccessToken
        )
        {
            if (string.IsNullOrWhiteSpace(personalAccessToken))
            {
                throw new ArgumentException(nameof(personalAccessToken));
            }

            _personalAccessToken = personalAccessToken;

            _client = new HttpClient();
            AddHeaders();
        }

        #endregion Constructors

        /// <summary>
        ///     Sends HTTP GET request using the base url with the provided suffix and headers.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<string> GetStringAsync(
            Uri uri
        )
        {
            AddHeaders();

            string output;

            try
            {
                output = await _client.GetStringAsync(uri).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                throw new ClientException("", exception);
            }

            return output;
        }

        /// <summary>
        ///     Sends HTTP POST with provided content
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostStringAsync(
            Uri uri,
            string content
        )
        {
            AddHeaders();

            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            var output = await _client.PostAsync(uri, httpContent).ConfigureAwait(false);

            await CheckStatusCode(output).ConfigureAwait(false);

            return output;
        }

        /// <summary>
        ///     Sends HTTP PUT with provided content
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PutStringAsync(
            Uri uri,
            string content
        )
        {
            AddHeaders();

            var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            var output = await _client.PutAsync(uri, httpContent).ConfigureAwait(false);

            await CheckStatusCode(output).ConfigureAwait(false);

            return output;
        }

        public async Task<HttpResponseMessage> DeleteAsync(
            Uri uri
        )
        {
            AddHeaders();

            var output = await _client.DeleteAsync(uri).ConfigureAwait(false);

            await CheckStatusCode(output).ConfigureAwait(false);

            return output;
        }

        public void AddHeaders()
        {
            _client.DefaultRequestHeaders.Clear();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(
                            $":{_personalAccessToken}"
                        )
                    )
                );

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        ///     Checks the status code and throws an exception if it does not indicate success.
        /// </summary>
        /// <param name="result"></param>
        private static async Task CheckStatusCode(HttpResponseMessage result)
        {
            if (!result.IsSuccessStatusCode)
            {
                var contentText = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new ClientException(
                    $"HTTP status code does not indicate success: {result.StatusCode} -- {contentText}");
            }
        }

        #region Private

        private readonly string _personalAccessToken;

        private readonly HttpClient _client;

        #endregion Private
    }
}