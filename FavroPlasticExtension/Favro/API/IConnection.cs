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

using System.Collections.Specialized;

namespace FavroPlasticExtension.Favro.API
{
    internal interface IConnection
    {
        /// <summary>
        /// Email of the user to use for authentication
        /// </summary>
        string UserEmail { get; set; }
        /// <summary>
        /// Password of the user that will be used for communications athentication
        /// with the server
        /// </summary>
        string Password { set; }
        /// <summary>
        /// Sends an HTTP GET request to the API endpoint requested
        /// </summary>
        /// <param name="url">Endpoint where the request will be send to. Ommit the
        /// prefix with the hostname and API version</param>
        /// <param name="parameters">Collection of the parameters of the query to execute.</param>
        /// <returns>Response received from the server</returns>
        Response Get(string url, NameValueCollection parameters);
        /// <summary>
        /// Sends an HTTP POST request to the API endpoint requested
        /// </summary>
        /// <typeparam name="TData">Type of the object to send in the body of the request</typeparam>
        /// <param name="url">Endpoint where the request will be send to. Ommit the
        /// prefix with the hostname and API version</param>
        /// <param name="data">Object that will be serialized and sent in the request body. Set to
        /// if there is no information to be sent in the body.</param>
        /// <returns>Response received from the server</returns>
        Response Post<TData>(string url, TData data = null) where TData : class;
        /// <summary>
        /// Sends an HTTP PUT request to the API endpoint requested
        /// </summary>
        /// <typeparam name="TData">Type of the object to send in the body of the request</typeparam>
        /// <param name="url">Endpoint where the request will be send to. Ommit the
        /// prefix with the hostname and API version</param>
        /// <param name="data">Object that will be serialized and sent in the request body. Set to
        /// <c>null</c> if there is no information to be sent in the body</param>
        /// <returns>Response received from the server</returns>
        Response Put<TData>(string url, TData data = null) where TData : class;
        /// <summary>
        /// Sends and HTTP DELETE request to the API endpoint requested
        /// </summary>
        /// <typeparam name="TData">Type of the object to be send in the body of the request</typeparam>
        /// <param name="url">Endpoint where the request will be send to. Ommit the prefix
        /// with the hostname and API version</param>
        /// <param name="data">Object that will be sent in the body</param>
        /// <returns>Response received from the server</returns>
        Response Delete<TData>(string url, TData data = null) where TData : class;
    }
}
