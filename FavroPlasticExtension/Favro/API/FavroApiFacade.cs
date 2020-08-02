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
        private const string ENDPOINT_USERS = "/users";
        private const string ENDPOINT_ORGANIZATIONS = "/organizations";
		private const string ENDPOINT_COLUMNS = "/columns";
        private const string ENDPOINT_COLLECTIONS = "/collections";
        private const string ENDPOINT_WIDGETS = "/widgets";
        private const string ENDPOINT_CARDS = "/cards";
        private const string ENDPOINT_COMMENTS = "/comments";

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
            CheckOrganizationSelected();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("organizationId", organizationId);
            var response = connection.Get($"{ENDPOINT_ORGANIZATIONS}", parameters);
            Organization organization = null;
            if (response.Error != null)
            {
                log.Error($"Unable to retrieve the information of the organization '{organizationId}'", response.Error);
            }
            else
            {
                organization = GetEntries<Organization>(response).FirstOrDefault();
            }
            return organization;
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

        public List<Column> GetAllColumns(string widgetCommonId)
        {
            CheckOrganizationSelected();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("widgetCommonId", widgetCommonId);
            var response = connection.Get($"{ENDPOINT_COLUMNS}", parameters);
            return GetEntries<Column>(response);
        }

        public List<Card> GetAssignedCards(string collectionId, string widgetCommonId)
        {
            CheckOrganizationSelected();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("unique", "true");
            parameters.Add("archived", "false");
            if (widgetCommonId == "" && collectionId == "")
            {
                parameters.Add("todoList", "true");
            }
            else if (widgetCommonId != "")
            {
                parameters.Add("widgetCommonId", widgetCommonId);
            }
            else if (collectionId != "")
            {
                parameters.Add("collectionId", collectionId);
            }

            var cards = GetAllPagesFromEndpoint<Card>(ENDPOINT_CARDS, parameters, "Unexpected error while retrieving cards");
            if (cards.Count == 0 && (collectionId != "" || widgetCommonId != ""))
            {
                return GetAssignedCards("", "");
            }
            else
            {
                return cards.Where(card => card.Assignments.Count > 0 && !string.IsNullOrEmpty(card.ColumnId)).ToList();
            }
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

        private Card GetFirstCardWithColumn(List<Card> cards)
        {
            if (cards != null && cards.Count > 0)
            {
                var cardWithColumn = cards.Find(card => card.ColumnId != null);
                if (cardWithColumn != null)
                {
                    return cardWithColumn;
                }
                else
                {
                    return cards[0];
                }
            }
            else
                return null;
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
            return GetFirstCardWithColumn(GetAllPagesFromEndpoint<Card>(ENDPOINT_CARDS, parameters, "Unexpected error while retrieving card by id"));
        }

        public Card CompleteCard(string cardCommonId)
        {
            throw new NotImplementedException("Method not implemented");
        }

        internal void CreateComment(string comment, string cardCommonId)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("cardCommonId", cardCommonId);
            parameters.Add("comment", comment);
            connection.Post($"{ENDPOINT_COMMENTS}", parameters);
        }

        public CardComment AddCommentToCard(string cardCommonId, string comment)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public void MoveCardToColumn(Card card, Column column)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("widgetCommonId", card.WidgetCommonId);
            parameters.Add("columnId", column.ColumnId);
            connection.Put($"{ENDPOINT_CARDS}/{card.CardId}", parameters);
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