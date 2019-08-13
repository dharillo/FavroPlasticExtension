using System;
using System.Collections.Specialized;
using System.Net.Http;

namespace FavroPlasticExtensionTests.Helpers
{
    public class RequestInfo
    {
        /// <summary>
        /// HTTP method used for the request
        /// </summary>
        public HttpMethod Method { get; }
        /// <summary>
        /// URL path used for the request
        /// </summary>
        public string Url { get; }
        /// <summary>
        /// Parameters used for the request
        /// </summary>
        public NameValueCollection Parameters { get; }
        /// <summary>
        /// Body of the requests
        /// </summary>
        public object Body { get; }

        public RequestInfo(string url, HttpMethod method, NameValueCollection parameters, object body = null)
        {
            Method = method;
            Url = url;
            Parameters = parameters;
            Body = body;
        }
    }
}
