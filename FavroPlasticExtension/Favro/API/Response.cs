using System;
using System.Collections.Generic;

namespace FavroPlasticExtension.Favro.API
{
    /// <summary>
    /// Stores the information of an API response
    /// </summary>
    internal class Response
    {
        /// <summary>
        /// The maximum number of request permitted to make per hour
        /// </summary>
        public const string HEADER_LIMIT_MAX = "X-RateLimit-Limit";
        /// <summary>
        /// The number of request remaining in the current rate limit window
        /// </summary>
        public const string HEADER_LIMIT_REMAINING = "X-RateLimit-Remaining";
        /// <summary>
        /// The time at which the current rate limit window resets in UTC
        /// </summary>
        public const string HEADER_LIMIT_RESET = "X-RateLimit-Reset";
        /// <summary>
        /// The backend server that returned this response
        /// </summary>
        public const string HEADER_BACKEND_ID = "X-Favro-Backend-Identifier";
        public const string PATH_REQUEST_ID = "requestId";
        public const string PATH_ENTRIES = "entries";
        public const string PATH_PAGE = "page";
        public const string PATH_PAGES = "pages";
        public const string PATH_LIMIT = "limit";
        /// <summary>
        /// The special headers of the Favro API that where detected in the response
        /// </summary>
        public Dictionary<string, string> Headers { get; private set; }
        /// <summary>
        /// Content of the response
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// If some error happened, stores the response exception.
        /// </summary>
        public Exception Error { get; set; }

        public Response()
        {
            Headers = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the current page of the responses that use pagination. If this response
        /// has no pages.
        /// </summary>
        /// <returns>Page of information stored in this response object</returns>
        /// <exception cref="InvalidOperationException">If this response has no pages</exception>
        public int GetPageNumber()
        {
            throw new NotImplementedException("Method not implemented");
        }
        /// <summary>
        /// Indicates if there are more pages of information about the resources retrieved and
        /// stored in this response object.
        /// </summary>
        /// <returns><c>true</c> if there are more pages to be retrieved from the server;
        /// otherwise <c>false</c></returns>
        public bool HasMorePages()
        {
            throw new NotImplementedException("Method not implemented");
        }
    }
}
