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
using System.Collections.Specialized;
using System.Linq;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FavroPlasticExtension.Favro.API
{
    internal class FavroApiFacade
    {
        public const string ENDPOINT_USERS = "/users";
        private readonly IFavroConnection connection;
        private readonly ILog logger;
        private const NameValueCollection NO_PARAMS = null;

        public FavroApiFacade(IFavroConnection connection, ILog logger)
        {
            this.connection = connection;
            this.logger = logger;
        }

        public List<User> GetAllUsers()
        {
            CheckOrganizationSelected();
            var response = connection.Get(ENDPOINT_USERS, NO_PARAMS);
            if (response.Error != null)
            {
                logger.Fatal("Unexpected error while retrieving users", response.Error);
                return new List<User>();
            }
            var users = GetEntries<User>(response);
            while (response.HasMorePages())
            {
                response = connection.GetNextPage(ENDPOINT_USERS, response, NO_PARAMS);
                users.AddRange(GetEntries<User>(response));
            }
            return users;
        }

        private void CheckOrganizationSelected()
        {
            if (string.IsNullOrEmpty(connection.OrganizationId))
            {
                throw new InvalidOperationException("An organization ID must be selected before retrieving the list of users");
            }
        }

        public User GetUser(string userId)
        {
            var response = connection.Get($"{ENDPOINT_USERS}/{userId}", NO_PARAMS);
            User user = null;
            if (response.Error != null)
            {
                logger.Fatal($"Unable to retrieve user with ID={userId}", response.Error);
            }
            else
            {
                user = JsonConvert.DeserializeObject<User>(response.Content);
            }
            return user;
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

        internal void CreateComment(string comment, string cardCommonId)
        {
            throw new NotImplementedException();
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
            var deserializedContent = JObject.Parse(response.Content);
            return deserializedContent["entities"].Select(entry => JsonConvert.DeserializeObject<TEntry>(entry.ToString())).ToList();
        }
    }
}