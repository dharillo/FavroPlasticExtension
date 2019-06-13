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
