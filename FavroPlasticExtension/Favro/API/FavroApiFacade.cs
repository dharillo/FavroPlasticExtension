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
        public const string ENDPOINT_ORGANIZATIONS = "/organizations";
        public const string ENDPOINT_COLLECTIONS = "/collections";
        public const string ENDPOINT_WIDGETS = "/widgets";
        public const string ENDPOINT_CARDS = "/cards";
        public const string ENDPOINT_COMMENTS = "/comments";

        private readonly IFavroConnection connection;
        private readonly ILog log;
        private const NameValueCollection NO_PARAMS = null;

        public FavroApiFacade(IFavroConnection connection, ILog log)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public List<User> GetAllUsers()
        {
            CheckOrganizationSelected();
            return GetAllPagesFromEndpoint<User>(ENDPOINT_USERS, NO_PARAMS, "Unexpected error while retrieving users");
        }

        private void CheckOrganizationSelected()
        {
            if (string.IsNullOrWhiteSpace(connection.OrganizationId))
            {
                throw new InvalidOperationException("An organization ID must be selected before retrieving information from Favro");
            }
        }

        public User GetUser(string userId)
        {
            CheckUserParameter(userId);
            var response = connection.Get($"{ENDPOINT_USERS}/{userId}", NO_PARAMS);
            User user = null;
            if (response.Error != null)
            {
                log.Error($"Unable to retrieve the information of the user '{userId}'", response.Error);
            }
            else
            {
                user = JsonConvert.DeserializeObject<User>(response.Content);
            }
            return user;
        }

        public List<Organization> GetAllOrganizations()
        {
            return GetAllPagesFromEndpoint<Organization>(ENDPOINT_ORGANIZATIONS, NO_PARAMS, "Unexpected error while retrieving organizations");
        }

        public Organization GetOrganization(string organizationId)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public List<Collection> GetAllCollections()
        {
            CheckOrganizationSelected();
            return GetAllPagesFromEndpoint<Collection>(ENDPOINT_COLLECTIONS, NO_PARAMS, "Unexpected error while retrieving collections");
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
            CheckOrganizationSelected();
            var parameters = new NameValueCollection();
            parameters.Add("todoList", "true");
            parameters.Add("unique", "true");
            var cards = GetAllPagesFromEndpoint<Card>(ENDPOINT_CARDS, parameters, "Unexpected error while retrieving assigned cards");
            if (onlyOpen)
            {
                cards = cards.Where(x => x.TodoListCompleted == false).ToList();
            }
            return cards;
        }

        internal void CreateComment(string comment, string cardCommonId)
        {
            throw new NotImplementedException();
        }

        public Card GetCard(string commonId)
        {
            if (commonId == null)
            {
                throw new ArgumentNullException(nameof(commonId), "A card common identifier must be a non-empty string");
            }
            if (string.IsNullOrWhiteSpace(commonId))
            {
                throw new ArgumentException("A card common identifier must be a non-empty string", nameof(commonId));
            }
            NameValueCollection parameters = new NameValueCollection
            {
                { "cardCommonId", commonId }
            };
            return GetCard(parameters);
        }

        public Card GetCard(int sequentialId)
        {
            if (sequentialId < 0)
            {
                throw new ArgumentException("A card sequential identifier must be a positive integer", nameof(sequentialId));
            }
            NameValueCollection parameters = new NameValueCollection
            {
                { "cardSequentialId", sequentialId.ToString() }
            };
            return GetCard(parameters);
        }

        private Card GetCard(NameValueCollection parameters)
        {
            CheckOrganizationSelected();
            parameters.Add("unique", "true");
            return GetAllPagesFromEndpoint<Card>(ENDPOINT_CARDS, parameters, "Unexpected error while retrieving card by id").FirstOrDefault();

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

        private void CheckUserParameter(string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "The user identifier cannot be null");
            }
            else if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("The user identifier cannot be an empty string", nameof(userId));
            }
        }

        private List<TEntry> GetAllPagesFromEndpoint<TEntry>(string endpoint, NameValueCollection paramenters, string errorMessage)
        {
            var response = connection.Get(endpoint, paramenters);
            if (response.Error != null)
            {
                log.Error(errorMessage, response.Error);
                return new List<TEntry>();
            }
            var entries = GetEntries<TEntry>(response);
            while (response.HasMorePages())
            {
                response = connection.GetNextPage(endpoint, response, paramenters);
                entries.AddRange(GetEntries<TEntry>(response));
            }
            return entries;
        }
    }
}