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
using log4net;

namespace Codice.Client.IssueTracker.FavroExtension
{
    public class FavroExtension : IPlasticIssueTrackerExtension
    {
        internal static readonly ILog logger = LogManager.GetLogger("favroextension");

        internal const string KEY_USER = "User";
        internal const string KEY_PASSWORD = "Password";
        internal const string KEY_ORGANIZATION = "Organization";

        private const string EXTENSION_NAME = "Favro extension";
        private IssueTrackerConfiguration configuration;

        internal FavroExtension(IssueTrackerConfiguration configuration)
        {
            this.configuration = configuration;

            logger.Info("Favro issue tracker is initialized");
        }

        #region IPlasticIssueTrackerExtension implementation
        public string GetExtensionName()
        {
            return EXTENSION_NAME;
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public bool TestConnection(IssueTrackerConfiguration configuration)
        {
            throw new NotImplementedException();
        }

        public void LogCheckinResult(PlasticChangeset changeset, List<PlasticTask> tasks)
        {
            throw new NotImplementedException();
        }

        public void UpdateLinkedTasksToChangeset(PlasticChangeset changeset, List<string> tasks)
        {
            throw new NotImplementedException();
        }

        public PlasticTask GetTaskForBranch(string fullBranchName)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, PlasticTask> GetTasksForBranches(List<string> fullBranchNames)
        {
            throw new NotImplementedException();
        }

        public void OpenTaskExternally(string taskId)
        {
            throw new NotImplementedException();
        }

        public List<PlasticTask> LoadTasks(List<string> taskIds)
        {
            throw new NotImplementedException();
        }

        public List<PlasticTask> GetPendingTasks()
        {
            throw new NotImplementedException();
        }

        public List<PlasticTask> GetPendingTasks(string assignee)
        {
            throw new NotImplementedException();
        }

        public void MarkTaskAsOpen(string taskId, string assignee)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
