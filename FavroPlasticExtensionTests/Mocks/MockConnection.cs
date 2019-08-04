using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using FavroPlasticExtension.Favro.API;

namespace FavroPlasticExtensionTests.Mocks
{
    public class MockConnection: IFavroConnection
    {
        /// <summary>
        /// Endpoint path received in the latest use of this connection object
        /// </summary>
        public string RequestUrl { get; private set; }
        /// <summary>
        /// Parameters received in the latest use of this connection object
        /// </summary>
        public NameValueCollection RequestParameters { get; private set; }
        /// <summary>
        /// HTTP method used in the latest use of this connection object
        /// </summary>
        public HttpMethod RequestMethod { get; private set; }
        /// <summary>
        /// Body of the lastest use of this connection object
        /// </summary>
        public object RequestBody { get; private set; }
        /// <summary>
        /// Identifier of the organization in the Favro system where the cards exists
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// Number of remaining request within the hourly limit
        /// </summary>
        public int RemainingRequest { get; set; }
        /// <summary>
        /// Max amount of request per hour that the Favro user can do
        /// </summary>
        public int RequestLimit { get; set; }
        /// <summary>
        /// Email of the Favro user
        /// </summary>
        public string UserEmail { get; set; }
        /// <summary>
        /// Password of the favro user
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Fake response object to be returned in the next use of this connection
        /// </summary>
        public Response LatestFakeResponse { get; private set; }

        public int ConsecutiveResponsesUsed { get; private set; }

        private List<Response> fakeResponses;

        public MockConnection()
        {
            fakeResponses = null;
        }

        public Response Delete<TData>(string url, TData data = null) where TData : class
        {
            RequestUrl = url;
            RequestMethod = HttpMethod.Delete;
            RequestBody = data;
            RequestParameters = null;
            return GetNextResponse();
        }

        public Response Get(string url, NameValueCollection parameters)
        {
            RequestUrl = url;
            RequestMethod = HttpMethod.Get;
            RequestBody = null;
            RequestParameters = parameters;
            return GetNextResponse();
        }

        public Response GetNextPage(string url, Response previousPageResponse, NameValueCollection parameters)
        {
            RequestUrl = url;
            RequestMethod = HttpMethod.Get;
            RequestBody = null;
            RequestParameters = parameters;
            return GetNextResponse();
        }

        public Response Post<TData>(string url, TData data = null) where TData : class
        {
            RequestUrl = url;
            RequestBody = data;
            RequestParameters = null;
            RequestMethod = HttpMethod.Post;
            return GetNextResponse();
        }

        public Response Put<TData>(string url, TData data = null) where TData : class
        {
            RequestUrl = url;
            RequestBody = data;
            RequestParameters = null;
            RequestMethod = HttpMethod.Put;
            return GetNextResponse();
        }

        public void SetNextResponse(Response response)
        {
            SetNextResponses(new List<Response> { response });
        }

        public void SetNextResponses(List<Response> responses)
        {
            fakeResponses = responses;
            LatestFakeResponse = null;
            ConsecutiveResponsesUsed = 0;
        }

        private Response GetNextResponse()
        {
            if (fakeResponses == null || ConsecutiveResponsesUsed >= fakeResponses.Count)
            {
                return null;
            }
            LatestFakeResponse = fakeResponses[ConsecutiveResponsesUsed];
            ConsecutiveResponsesUsed++;
            return LatestFakeResponse;
        }
    }
}
