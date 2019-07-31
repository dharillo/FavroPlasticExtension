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
using System.Collections.Generic;

namespace FavroPlasticExtension.Favro.API
{
    internal class ApiFacade
    {
        private readonly IFavroConnection connection;

        public ApiFacade(IFavroConnection connection)
        {
            this.connection = connection;
        }

        public List<User> GetAllUsers()
        {
            if (string.IsNullOrEmpty(connection.OrganizationId))
            {
                throw new InvalidOperationException("An organization ID must be selected before retrieving the list of users");
            }
            var response = connection.Get("/users");
            var users = GetEntries<User>(response);
            while (response.HasMorePages())
            {
                response = connection.GetNextPage("/users", response);
                users.AddRange(GetEntries<User>(response));
            }
            return users;
        }


        public User GetUser(string userId)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public List<Organization> GetAllOrganizations()
        {
            throw new NotImplementedException("Method not implemented");
        }

        public Organization GetOrganization(string organizationId)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public List<Collection> GetAllCollections()
        {
            throw new NotImplementedException("Method not implemented");
        }

        public Collection GetCollection(string collectionId)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public List<Widget> GetAllWidgets(string collectionId = null, bool archived = false)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public List<Card> GetAssignedCards(bool onlyOpen = true)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public Card GetCard(string commonId)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public Card GetCard(int sequentialId)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public Card CompleteCard(string cardCommonId)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public CardComment AddCommentToCard(string cardCommonId, string comment)
        {
            throw new NotImplementedException("Method not implemented");
        }

        private List<TEntry> GetEntries<TEntry>(Response response)
        {
            throw new NotImplementedException();
        }
    }
}