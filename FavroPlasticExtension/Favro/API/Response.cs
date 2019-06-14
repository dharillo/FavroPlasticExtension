using System;
namespace FavroPlasticExtension.Favro.API
{
    /// <summary>
    /// Stores the information of an API response
    /// </summary>
    internal class Response
    {
        /// <summary>
        /// Content of the response
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// If some error happened, stores the response exception.
        /// </summary>
        public Exception Error { get; set; }
    }
}
