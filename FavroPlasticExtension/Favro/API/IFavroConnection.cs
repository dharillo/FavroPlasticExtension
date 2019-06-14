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
    }
}
