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
        /// <returns>Response received from the server</returns>
        Response Get(string url);
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
