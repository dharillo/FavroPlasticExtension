using System;

namespace FavroPlasticExtension.Favro.API
{
    internal interface IFavroConnection : IConnection
    {
        /// <summary>
        /// The ID of the organization where the information will come from
        /// </summary>
        string OrganizationId { get; set; }
        /// <summary>
        /// Remaining request until the hourly limit is reached
        /// </summary>
        int RemainingRequest { get; }
        /// <summary>
        /// Maximum number of requests that the user of this connection can make each hour
        /// </summary>
        int RequestLimit { get; }
        /// <summary>
        /// Gets the next page of a paginated response.
        /// </summary>
        /// <param name="url">API endpoint to query</param>
        /// <param name="previousPageResponse">The previous page response from where the next page should continue.</param>
        /// <returns>Next page of the pagination</returns>
        /// <exception cref="InvalidOperationException">The response given is not a pagination response</exception>
        Response GetNextPage(string url, Response previousPageResponse);
    }
}
