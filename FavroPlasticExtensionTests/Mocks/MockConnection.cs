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

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using FavroPlasticExtension.Favro.API;
using FavroPlasticExtensionTests.Helpers;

namespace FavroPlasticExtensionTests.Mocks
{
    public class MockConnection: IFavroConnection
    {
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
        /// <summary>
        /// Number of initial responses set that where finally used.
        /// </summary>
        public int ConsecutiveResponsesUsed { get; private set; }
        /// <summary>
        /// List of the requests received from the moment this instance was created.
        /// </summary>
        public List<RequestInfo> RequestsProcessed { get; }

        private List<Response> fakeResponses;

        public MockConnection()
        {
            fakeResponses = new List<Response>();
            RequestsProcessed = new List<RequestInfo>();
        }

        public Response Delete<TData>(string url, TData data = null) where TData : class
        {
            AddRequest(new RequestInfo(url, HttpMethod.Delete, null, data));
            return GetNextResponse();
        }

        public Response Get(string url, NameValueCollection parameters)
        {
            AddRequest(new RequestInfo(url, HttpMethod.Get, parameters));
            return GetNextResponse();
        }

        public Response GetNextPage(string url, Response previousPageResponse, NameValueCollection parameters)
        {
            AddRequest(new RequestInfo(url, HttpMethod.Get, parameters));
            return GetNextResponse();
        }

        public Response Post<TData>(string url, TData data = null) where TData : class
        {
            AddRequest(new RequestInfo(url, HttpMethod.Post, null, data));
            return GetNextResponse();
        }

        public Response Put<TData>(string url, TData data = null) where TData : class
        {
            AddRequest(new RequestInfo(url, HttpMethod.Put, null, data));
            return GetNextResponse();
        }

        public void AddRequest(RequestInfo request)
        {
            RequestsProcessed.Add(request);
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
