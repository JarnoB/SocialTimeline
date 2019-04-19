using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
namespace SocialTimeline.OAuth
{
    /// <summary>
    /// Made by following this example: https://github.com/aspnet/samples/tree/master/samples/aspnet/HttpClient/TwitterSample/OAuth
    /// </summary>
    public class OAuthMessageHandler : DelegatingHandler
    {
        // Twitter API keys
        private static string apiKey = "d64EhqnumeFxyqtoDyUVejRXC";
        private static string apiKeySecret = "GyvqyjECjI6qgb1EI5rWc4tBwaGUTcGPAU6TyuJlcqQwhND1jK";
        private static string token = "1115220086208499712-IYfzHgSX0D142nFvHVZXFW2u83ChvT";
        private static string tokenSecret = "XkJI5HvD2N3YApxKENbdWEPme5J4Xhs87UgLijsuhLuBl";

        private OAuthBase _oAuthBase = new OAuthBase();

        public OAuthMessageHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Compute OAuth header
            string normalizedUri;
            string normalizedParameters;
            string authHeader;

            string signature = _oAuthBase.GenerateSignature(
                request.RequestUri,
                apiKey,
                apiKeySecret,
                token,
                tokenSecret,
                request.Method.Method,
                _oAuthBase.GenerateTimeStamp(),
                _oAuthBase.GenerateNonce(),
                out normalizedUri,
                out normalizedParameters,
                out authHeader);

            request.Headers.Authorization = new AuthenticationHeaderValue("OAuth", authHeader);
            return base.SendAsync(request, cancellationToken);
        }
    }
}