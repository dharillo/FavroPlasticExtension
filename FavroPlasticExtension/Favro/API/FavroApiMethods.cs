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

namespace FavroPlasticExtension.Favro.API
{
    internal class ApiFacade
    {
        private const string ENDPOINT_USERS = "/users";
        private const string ENDPOINT_ORGANIZATIONS = "/organizations";
        private const string ENDPOINT_COLUMNS = "/columns";
        private const string ENDPOINT_CARDS = "/cards";
        private readonly IFavroConnection connection;
        private readonly ILog logger;
        private const NameValueCollection NO_PARAMS = null;

        public ApiFacade(IFavroConnection connection, ILog logger)
        {
            this.connection = connection;
            this.logger = logger;
        }

        public List<User> GetAllUsers()
        {
            CheckOrganizationSelected();
            var response = connection.Get(ENDPOINT_USERS, NO_PARAMS);
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
            CheckOrganizationSelected();
            var response = connection.Get($"{ENDPOINT_USERS}/{userId}", NO_PARAMS);
            User user = null;
            if (response.Error != null)
            {
                logger.Fatal($"Unable to retrieve user with ID={userId}", response.Error);
            }
            else
            {
                user = GetEntries<User>(response).FirstOrDefault();
            }
            return user;
        }

        public List<Organization> GetAllOrganizations()
        {
            CheckOrganizationSelected();
            var response = connection.Get($"{ENDPOINT_ORGANIZATIONS}", NO_PARAMS);
            var organizations = GetEntries<Organization>(response);
            while (response.HasMorePages())
            {
                response = connection.GetNextPage(ENDPOINT_ORGANIZATIONS, response, NO_PARAMS);
                organizations.AddRange(GetEntries<Organization>(response));
            }
            return organizations;
        }

        public Organization GetOrganization(string organizationId)
        {
            CheckOrganizationSelected();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("organizationId", organizationId);
            var response = connection.Get($"{ENDPOINT_ORGANIZATIONS}", parameters);
            var organizations = GetEntries<Organization>(response);
            if (organizations != null && organizations.Count > 0)
                return organizations[0];
            else
                return null;
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
                parameters.Add("todoList", "true");
            else if (widgetCommonId != "")
                parameters.Add("widgetCommonId", widgetCommonId);
            else if (collectionId != "")
                parameters.Add("collectionId", collectionId);

            var response = connection.Get($"{ENDPOINT_CARDS}", parameters);
            var cards = GetEntries<Card>(response);
            while (response.HasMorePages())
            {
                response = connection.GetNextPage(ENDPOINT_CARDS, response, parameters);
                cards.AddRange(GetEntries<Card>(response));
            }

            if (cards.Count == 0 && (collectionId != "" || widgetCommonId != ""))
                return GetAssignedCards("", "");
            else
                return cards.Where(card => card.Assignments.Count > 0 && card.ColumnId != null && card.ColumnId != "").ToList();
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
            CheckOrganizationSelected();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("cardSequentialId", $"{sequentialId}");
            var response = connection.Get($"{ENDPOINT_CARDS}", parameters);
            var cards = GetEntries<Card>(response);
            if (cards != null && cards.Count > 0)
            {
                var cardWithColumn = cards.Find(card => card.ColumnId != null);
                if (cardWithColumn != null)
                    return cardWithColumn;
                else
                    return cards[0];
            }
            else
                return null;
        }

        public Card CompleteCard(string cardCommonId)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public CardComment AddCommentToCard(string cardCommonId, string comment)
        {
            throw new NotImplementedException("Method not implemented");
        }

        public void MoveCardToColumn(Card card, Column column)
        {
            throw new NotImplementedException("Method not implemented");
        }

        private List<TEntry> GetEntries<TEntry>(Response response)
        {
            return JsonConvert.DeserializeObject<List<TEntry>>(response.GetEntitiesString());
        }
    }
}