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
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Codice.Utils;
using FavroPlasticExtension.Favro.API;
using log4net;

namespace Codice.Client.IssueTracker.FavroExtension
{
    public class FavroExtension : IPlasticIssueTrackerExtension
    {
        internal const string KEY_USER = "User";
        internal const string KEY_PASSWORD = "Password";
        internal const string KEY_ORGANIZATION = "Organization";
        internal const string KEY_BRANCH_PREFIX = "Prefix";
        internal const string COMMENT_TEMPLATE = "Checkin repository: {0}<br />Checkin ID: {1}<br />Checkin GUID: {2}<br />Checkin comment:<p>{3}</p>";

        private const string EXTENSION_NAME = "Favro extension";
        private readonly IssueTrackerConfiguration configuration;
        private readonly ILog logger;
        private IFavroConnection connection;
        private ApiFacade apiMethods;
        private Organization organizationInfo;
        private string organizationShortName;

        internal FavroExtension(IssueTrackerConfiguration configuration, ILog logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            logger.Info("Favro issue tracker is initialized");
        }

        #region IPlasticIssueTrackerExtension implementation
        public string GetExtensionName()
        {
            return EXTENSION_NAME;
        }

        public void Connect()
        {
            connection = CreateConnection(configuration);
            var organization = configuration.GetValue(KEY_ORGANIZATION);
            connection.OrganizationId = organization;
            apiMethods = new ApiFacade(connection, logger);
            organizationInfo = apiMethods.GetOrganization(organization);
            organizationShortName = GetOrganizationShortName();
        }

        public void Disconnect()
        {
            connection = null;
            apiMethods = null;
        }

        public bool TestConnection(IssueTrackerConfiguration configuration)
        {
            var testConnection = CreateConnection(configuration);
            var testMethods = new ApiFacade(testConnection, logger);
            var userOrganizations = testMethods.GetAllOrganizations();
            return userOrganizations != null && userOrganizations.Count != 0;
        }

        public void LogCheckinResult(PlasticChangeset changeset, List<PlasticTask> tasks)
        {
            string comment = CreateComment(changeset);
            foreach (var task in tasks)
            {
                var card = apiMethods.GetCard(task.Id);
                apiMethods.CreateComment(comment, card.CardCommonId);
            }
        }

        private string CreateComment(PlasticChangeset changeset)
        {
            return string.Format(COMMENT_TEMPLATE, changeset.Repository, changeset.Id, changeset.Guid, changeset.Comment);
        }

        public void UpdateLinkedTasksToChangeset(PlasticChangeset changeset, List<string> tasks)
        {
            throw new NotImplementedException();
        }

        public PlasticTask GetTaskForBranch(string fullBranchName)
        {
            var branchName = GetBranchName(fullBranchName);
            var cardId = GetCardIdFromBranchName(branchName);
            return GetTaskFromCardId(cardId);
        }

        public Dictionary<string, PlasticTask> GetTasksForBranches(List<string> fullBranchNames)
        {
            Dictionary<string, PlasticTask> branchesTasks = new Dictionary<string, PlasticTask>();
            foreach (var fullBranchName in fullBranchNames)
            {
                var task = GetTaskForBranch(fullBranchName);
                if (task != null)
                {
                    branchesTasks.Add(fullBranchName, task);
                }
            }
            return branchesTasks;
        }

        public void OpenTaskExternally(string taskId)
        {
            Process.Start(GetExternalLink(taskId));
        }

        public List<PlasticTask> LoadTasks(List<string> taskIds)
        {
            var result = new List<PlasticTask>();
            foreach (var taskId in taskIds)
            {
                var task = GetTaskFromCardId(taskId);
                if (task != null)
                {
                    result.Add(task);
                }
            }
            return result;
        }

        public List<PlasticTask> GetPendingTasks()
        {
            return GetPendingTasks(connection.UserEmail);
        }

        public List<PlasticTask> GetPendingTasks(string assignee)
        {
            var pendingCards = apiMethods.GetAssignedCards();
            return pendingCards.Select(_ => ConvertToTask(_)).ToList();
        }

        public void MarkTaskAsOpen(string taskId, string assignee)
        {
            throw new NotImplementedException();
        }
        #endregion

        private string GetDecryptedPassword(string encryptedPassword)
        {
            return CryptoServices.GetDecryptedPassword(encryptedPassword);
        }

        private IFavroConnection CreateConnection(IssueTrackerConfiguration connectionConfiguration)
        {
            var user = connectionConfiguration.GetValue(KEY_USER);
            var password = connectionConfiguration.GetValue(KEY_PASSWORD);

            return new Connection(user, GetDecryptedPassword(password));
        }

        private string GetOrganizationShortName()
        {
            var organizationName = organizationInfo.Name.Replace(" ", "");
            return organizationName.Substring(0, 3);
        }

        private string GetBranchName(string fullBranchName)
        {
            var lastSeparatorIndex = fullBranchName.LastIndexOf('/');
            var branchName = string.Empty;
            if (lastSeparatorIndex < 0)
            {
                branchName = fullBranchName;
            }
            else if (lastSeparatorIndex < (fullBranchName.Length - 1))
            {
                branchName = fullBranchName.Substring(lastSeparatorIndex + 1);
            }
            return branchName;
        }

        private string GetCardIdFromBranchName(string branchName)
        {
            var prefix = GetPrefix();
            var regex = new Regex($"{prefix}(/d+).*");
            var match = regex.Match(branchName);
            string cardId = null;
            if (match.Success)
            {
                cardId = match.Groups[1].Value;
            }
            return cardId;
        }

        private string GetPrefix()
        {
            return configuration.GetValue(KEY_BRANCH_PREFIX);
        }

        private PlasticTask GetTaskFromCardId(string cardId)
        {
            PlasticTask result = null;
            if (!string.IsNullOrEmpty(cardId) && int.TryParse(cardId, out int cardSequentialId))
            {
                var card = apiMethods.GetCard(cardSequentialId);
                result = ConvertToTask(card);
            }
            return result;
        }

        private PlasticTask ConvertToTask(Card card)
        {
            PlasticTask result = null;
            if (card != null)
            {
                result = new PlasticTask
                {
                    Description = GetDescription(card),
                    Title = card.Name,
                    Owner = connection.UserEmail,
                    Id = card.SequentialId.ToString(),
                    Status = "unknown"
                };
            }
            return result;
        }

        private string GetDescription(Card card)
        {
            var cardLink = GetExternalLink(card);
            return $"{card.Name}: {cardLink}";
        }

        private string GetExternalLink(Card card)
        {
            return GetExternalLink(card.SequentialId.ToString());
        }

        private string GetExternalLink(string cardId)
        {
            return $"https://favro.com/organization/{connection.OrganizationId}/?card={organizationShortName}-{cardId}";
        }
    }
}
