//  Favro Plastic Extension
//  Copyright(C) 2019  David Harillo Sánchez
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published
//  by the Free Software Foundation, either version 3 of the License, or
//  any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU Lesser General Public License for more details in the project root.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program. If not, see<https://www.gnu.org/licenses/>

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Codice.Utils;

namespace FavroPlasticExtension.Favro.API
{
    internal class Connection: IFavroConnection
    {
        #region Constants
        private const int DEFAULT_REQUEST_LIMIT = 100;
        private const string API_URL = "https://favro.com/api/v1";
        private const string HEADER_ORGANIZATION = "organizationId";
        private const string MIME_TYPE_JSON = "application/json";
        #endregion
        #region Members
        private string organizatinoId;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates a new conne connection instance.
        /// </summary>
        /// <param name="userEmail">Email of the user that will be used for authentication in this connection</param>
        /// <param name="password"></param>
        public Connection(string userEmail, string password)
        {
            UserEmail = userEmail ?? throw new ArgumentNullException(nameof(userEmail), "The username can not be null");
            Password = password ?? throw new ArgumentNullException(nameof(password), "The password can not be null");
            RemainingRequest = DEFAULT_REQUEST_LIMIT;
            RequestLimit = DEFAULT_REQUEST_LIMIT;
            organizatinoId = string.Empty;
        }
        #endregion
        #region IConnection implementation
        /// <summary>
        /// Email of the user that will be used for authentication in all the communications
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// Password used in the communications authentication
        /// </summary>
        public string Password { private get; set; }
        /// <summary>
        /// Remaining request until the hourly limit is reached
        /// </summary>
        public int RemainingRequest { get; private set; }
        /// <summary>
        /// Maximum number of requests that the user of this connection can make each hour
        /// </summary>
        public int RequestLimit { get; private set; }
        /// <summary>
        /// ID of the organization where the information will be retrieved from.
        /// </summary>
        public string OrganizationId
        {
            get => organizatinoId;
            set => organizatinoId = value ?? throw new ArgumentNullException(nameof(value), "The organizationId can not be null");
        }
        /// <summary>
        /// Sends an HTTP GET request to the API endpoint requested
        /// </summary>
        /// <param name="url">Endpoint where the request will be send to. Ommit the
        /// prefix with the hostname and API version</param>
        /// <returns>Response received from the server</returns>
        public Response Get(string url, NameValueCollection parameters = null)
        {
            return SendRequest<object>(HttpMethod.Get, BuildRequestUrl(url, parameters));
        }

        /// <summary>
        /// Sends an HTTP POST request to the API endpoint requested
        /// </summary>
        /// <typeparam name="TData">Type of the object to send in the body of the request</typeparam>
        /// <param name="url">Endpoint where the request will be send to. Ommit the
        /// prefix with the hostname and API version</param>
        /// <param name="data">Object that will be serialized and sent in the request body. Set to
        /// if there is no information to be sent in the body.</param>
        public Response Post<TData>(string url, TData data = null) where TData : class
        {
            return SendRequest(HttpMethod.Post, url, data);
        }

        /// <summary>
        /// Sends an HTTP PUT request to the API endpoint requested
        /// </summary>
        /// <typeparam name="TData">Type of the object to send in the body of the request</typeparam>
        /// <param name="url">Endpoint where the request will be send to. Ommit the
        /// prefix with the hostname and API version</param>
        /// <param name="data">Object that will be serialized and sent in the request body. Set to
        /// <c>null</c> if there is no information to be sent in the body</param>
        /// <returns>Response received from the server</returns>
        public Response Put<TData>(string url, TData data = null) where TData : class
        {
            return SendRequest(HttpMethod.Put, url, data);
        }

        /// <summary>
        /// Sends and HTTP DELETE request to the API endpoint requested
        /// </summary>
        /// <typeparam name="TData">Type of the object to be send in the body of the request</typeparam>
        /// <param name="url">Endpoint where the request will be send to. Ommit the prefix
        /// with the hostname and API version</param>
        /// <param name="data">Object that will be sent in the body</param>
        /// <returns>Response received from the server</returns>
        public Response Delete<TData>(string url, TData data = null) where TData : class
        {
            return SendRequest(HttpMethod.Delete, url, data);
        }

        /// <summary>
        /// Gets the next page of a paginated response.
        /// </summary>
        /// <param name="url">API endpoint to query</param>
        /// <param name="previousPageResponse">The previous page response from where the next page should continue.</param>
        /// <returns>Next page of the pagination or <c>null</c> if there are no more pages available</returns>
        /// <exception cref="InvalidOperationException">The response given is not a pagination response</exception>
        public Response GetNextPage(string url, Response previousPageResponse, NameValueCollection parameters = null)
        {
            if (previousPageResponse == null)
            {
                throw new ArgumentNullException(nameof(previousPageResponse), "The previous response object cannot be null");
            }
            if (!previousPageResponse.HasMorePages())
            {
                return null;
            }
            var endPointError = CheckEndpoint(url);
            Response response;
            if (endPointError != null)
            {
                response = CreateErrorResponse(endPointError);
            }
            else
            {
                response = SendNextPageQuery(url, previousPageResponse, parameters);
            }
            return response;

        }
        #endregion

        private Response SendRequest<TData>(HttpMethod verb, string endpoint, TData data = null) where TData: class
        {
            Response response;
            Exception endpointError = CheckEndpoint(endpoint);
            if (endpointError == null)
            {
                string uri = $"{API_URL}{endpoint}";
                HttpWebRequest request = CreateRequest(verb, uri, data);
                response = ParseWebResponse(request);
            }
            else
            {
                response = CreateErrorResponse(endpointError);
            }
            return response;
        }

        private Exception CheckEndpoint(string endpoint)
        {
            Exception error = null;
            if (endpoint == null)
            {
                error = new ArgumentNullException(nameof(endpoint), "The endpoint of the API to use can not be null");
            }
            else if (string.IsNullOrWhiteSpace(endpoint))
            {
                error = new ArgumentException("The endpoint of the API to use can not be an empty string");
            }
            else if (!endpoint.StartsWith("/", StringComparison.Ordinal))
            {
                error = new ArgumentException("The endpoint of the API must start with an slash (/)");
            }
            return error;
        }

        private HttpWebRequest CreateRequest<TData>(HttpMethod verb, string uri, TData data) where TData : class
        {
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            SetRequestBasicHeaders(verb, request);
            if (data != null)
            {
                AddPayload(data, request);
            }
            return request;
        }

        private void SetRequestBasicHeaders(HttpMethod verb, HttpWebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = verb.Method;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            var encodedAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{UserEmail}:{Password}"));
            request.Headers.Set(HttpRequestHeader.Authorization.ToString(), $"Basic {encodedAuth}");
            if (!string.IsNullOrEmpty(organizatinoId))
            {
                request.Headers.Set(HEADER_ORGANIZATION, organizatinoId);
            }
        }

        private void AddPayload<TData>(TData data, HttpWebRequest request)
        {
            request.ContentType = MIME_TYPE_JSON;
            string serializedPayload = JsonConvert.SerializeObject(data);
            using (var stream = request.GetRequestStream())
            {
                var payloadBytes = Encoding.UTF8.GetBytes(serializedPayload);
                stream.Write(payloadBytes, 0, payloadBytes.Length);
            }
        }

        private Response ParseWebResponse(HttpWebRequest request)
        {
            Response response = new Response();
            try
            {
                using (var webResponse = request.GetResponse())
                {
                    response.Headers.Add(Response.HEADER_BACKEND_ID, webResponse.Headers[Response.HEADER_BACKEND_ID]);
                    response.Headers.Add(Response.HEADER_LIMIT_MAX, webResponse.Headers[Response.HEADER_LIMIT_MAX]);
                    response.Headers.Add(Response.HEADER_LIMIT_REMAINING, webResponse.Headers[Response.HEADER_LIMIT_REMAINING]);
                    response.Headers.Add(Response.HEADER_LIMIT_RESET, webResponse.Headers[Response.HEADER_LIMIT_RESET]);

                    using (var responseStream = webResponse.GetResponseStream())
                    using (var reader = new StreamReader(responseStream))
                    {
                        response.Content = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                response.Content = string.Empty;
                response.Error = e;
            }
            return response;
        }

        private string BuildRequestUrl(string url, NameValueCollection parameters)
        {
            var calculatedUrl = url;
            if (parameters != null)
            {
                var serializedParameters = (from key in parameters.AllKeys
                                            from value in parameters.GetValues(key)
                                            select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)));
                calculatedUrl = $"{url}?{string.Join("&", serializedParameters)}";
            }
            return calculatedUrl;
        }

        private Response SendNextPageQuery(string url, Response previousPageResponse, NameValueCollection parameters)
        {
            Response response;
            var previousPage = previousPageResponse.GetPageNumber();
            var requestId = previousPageResponse.GetRequestId();
            parameters.Add("page", (previousPage + 1).ToString());
            parameters.Add("requestId", requestId);
            var query = BuildRequestUrl(url, parameters);
            var request = CreateRequest<object>(HttpMethod.Get, $"{API_URL}{query}", null);
            request.Headers.Add(Response.HEADER_BACKEND_ID, previousPageResponse.Headers[Response.HEADER_BACKEND_ID]);
            response = ParseWebResponse(request);
            return response;
        }

        private static Response CreateErrorResponse(Exception error)
        {
            return new Response
            {
                Content = string.Empty,
                Error = error
            };
        }
    }
}
