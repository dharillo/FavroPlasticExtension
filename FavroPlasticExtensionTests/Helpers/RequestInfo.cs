//  Favro Plastic Extension
//  Copyright(C) 2020  David Harillo Sánchez
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

using System.Collections.Specialized;
using System.Net.Http;

namespace FavroPlasticExtension.Helpers
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
