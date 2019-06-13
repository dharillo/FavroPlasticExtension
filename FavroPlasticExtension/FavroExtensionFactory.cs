using System;
using log4net;

namespace Codice.Client.IssueTracker.FavroExtension
{
    public class FavroExtensionFactory: IPlasticIssueTrackerExtensionFactory
    {
        private const string ISSUE_TRACKER_NAME = "Favro Issue Tracker";
        #region IPlasticIssueTrackerExtensionFactory implementation
        public IssueTrackerConfiguration GetConfiguration(IssueTrackerConfiguration storedConfiguration)
        {
            throw new NotImplementedException();
        }

        public IPlasticIssueTrackerExtension GetIssueTrackerExtension(IssueTrackerConfiguration configuration)
        {
            return new FavroExtension(configuration);
        }

        public string GetIssueTrackerName()
        {
            return ISSUE_TRACKER_NAME;
        }
        #endregion
    }
}
