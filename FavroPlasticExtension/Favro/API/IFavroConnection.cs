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
