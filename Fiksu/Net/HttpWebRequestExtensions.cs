using System.Net;
using System.Threading.Tasks;

namespace Fiksu.Net {
    public static class HttpWebRequestExtensions {
        public static HttpWebResponse GetAnyResponse(this HttpWebRequest request) {
            try {
                return (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wex) {
                if (wex.Response is HttpWebResponse response)
                    return response;
                throw;
            }
        }

        public static async Task<HttpWebResponse> GetAnyResponseAsync(this HttpWebRequest request) {
            try {
                var response = await request.GetResponseAsync().ConfigureAwait(false);
                return (HttpWebResponse)response;
            }
            catch (WebException wex) {
                if (wex.Response is HttpWebResponse response)
                    return response;
                throw;
            }
        }
    }
}
