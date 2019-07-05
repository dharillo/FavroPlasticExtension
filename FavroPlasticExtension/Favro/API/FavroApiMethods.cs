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