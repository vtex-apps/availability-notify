using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AvailabilityNotify.Tests
{
    /// <summary>
    /// Routes outgoing HTTP calls by request URI for deterministic unit tests.
    /// </summary>
    internal sealed class VtexApiTestHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;

        public VtexApiTestHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> responder)
        {
            _responder = responder ?? throw new ArgumentNullException(nameof(responder));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_responder(request));
        }

        public static HttpResponseMessage Ok(string content = "")
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(content)
            };
        }

        public static HttpResponseMessage Status(HttpStatusCode code, string content = "")
        {
            return new HttpResponseMessage(code)
            {
                Content = new StringContent(content)
            };
        }
    }
}
