﻿//  Favro Plastic Extension
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

using System.Collections.Generic;
using log4net;

namespace Codice.Client.IssueTracker.FavroExtension
{
    public class FavroExtensionFactory: IPlasticIssueTrackerExtensionFactory
    {
        private const string ISSUE_TRACKER_NAME = "Favro Issue Tracker";
        #region IPlasticIssueTrackerExtensionFactory implementation
        public IssueTrackerConfiguration GetConfiguration(IssueTrackerConfiguration storedConfiguration)
        {
            var workingMode = GetWorkingMode(storedConfiguration);
            var parameters = new List<IssueTrackerConfigurationParameter>();
            parameters.Add(GetUserParameter(storedConfiguration));
            parameters.Add(GetPasswordParameter(storedConfiguration));

            parameters.Add(new IssueTrackerConfigurationParameter("WorkflowInfoSeparator", "Workflow configuration:", IssueTrackerConfigurationParameterType.Label, false));
            parameters.Add(GetBranchPrefixParameter(storedConfiguration));
            parameters.Add(GetBranchSuffixParameter(storedConfiguration));
            parameters.Add(GetDoingColumnNameParameter(storedConfiguration));

            parameters.Add(new IssueTrackerConfigurationParameter("FilterInfoSeparator", "Filters Favro Cards by:", IssueTrackerConfigurationParameterType.Label, false));
            parameters.Add(GetOrganizationParameter(storedConfiguration));
            parameters.Add(GetCollectionIdParameter(storedConfiguration));
            parameters.Add(GetWidgetCommonIdParameter(storedConfiguration));

            return new IssueTrackerConfiguration(workingMode, parameters);
        }

        public IPlasticIssueTrackerExtension GetIssueTrackerExtension(IssueTrackerConfiguration configuration)
        {
            return new FavroExtension(configuration, LogManager.GetLogger("favroextension"));
        }

        public string GetIssueTrackerName()
        {
            return ISSUE_TRACKER_NAME;
        }
        #endregion

        private ExtensionWorkingMode GetWorkingMode(IssueTrackerConfiguration configuration)
        {
            var mode = ExtensionWorkingMode.TaskOnBranch;
            if (configuration != null && configuration.WorkingMode != ExtensionWorkingMode.None)
            {
                mode = configuration.WorkingMode;
            }
            return mode;
        }

        private static IssueTrackerConfigurationParameter GetUserParameter(IssueTrackerConfiguration configuration)
        {
            string user = GetValidParameterValue(configuration, FavroExtension.KEY_USER, "username");
            return CreateUserParameter(user);
        }

        private static string GetValidParameterValue(IssueTrackerConfiguration configuration, string key, string defaultValue)
        {
            string result = configuration != null ? configuration.GetValue(key) : null;
            if (string.IsNullOrEmpty(result))
            {
                result = defaultValue;
            }
            return result;
        }

        private static IssueTrackerConfigurationParameter CreateUserParameter(string username)
        {
            return CreateLocalParameter(FavroExtension.KEY_USER, username, IssueTrackerConfigurationParameterType.User);
        }

        private static IssueTrackerConfigurationParameter CreateLocalParameter(string key, string value, IssueTrackerConfigurationParameterType type)
        {
            return new IssueTrackerConfigurationParameter
            {
                Name = key,
                Value = value,
                Type = type,
                IsGlobal = false
            };
        }

        private static IssueTrackerConfigurationParameter GetPasswordParameter(IssueTrackerConfiguration configuration)
        {
            string password = GetValidParameterValue(configuration, FavroExtension.KEY_PASSWORD, string.Empty);
            return CreatePasswordParameter(password);
        }

        private static IssueTrackerConfigurationParameter CreatePasswordParameter(string password)
        {
            return CreateLocalParameter(FavroExtension.KEY_PASSWORD, password, IssueTrackerConfigurationParameterType.Password);
        }

        private static IssueTrackerConfigurationParameter GetOrganizationParameter(IssueTrackerConfiguration configuration)
        {
            string organization = GetValidParameterValue(configuration, FavroExtension.KEY_ORGANIZATION, "");
            return CreateGlobalParameter(FavroExtension.KEY_ORGANIZATION, organization, IssueTrackerConfigurationParameterType.Text);
        }

        private static IssueTrackerConfigurationParameter GetDoingColumnNameParameter(IssueTrackerConfiguration configuration)
        {
            string doingColumnName = GetValidParameterValue(configuration, FavroExtension.KEY_DOING_COLUMN, "Doing");
            return CreateGlobalParameter(FavroExtension.KEY_DOING_COLUMN, doingColumnName, IssueTrackerConfigurationParameterType.Text);
        }

        private static IssueTrackerConfigurationParameter GetCollectionIdParameter(IssueTrackerConfiguration configuration)
        {
            string collectionId = GetValidParameterValue(configuration, FavroExtension.KEY_COLLECTION_ID, "");
            return CreateGlobalParameter(FavroExtension.KEY_COLLECTION_ID, collectionId, IssueTrackerConfigurationParameterType.Text);
        }

        private static IssueTrackerConfigurationParameter GetWidgetCommonIdParameter(IssueTrackerConfiguration configuration)
        {
            string widgetCommonId = GetValidParameterValue(configuration, FavroExtension.KEY_WIDGET_ID, "");
            return CreateGlobalParameter(FavroExtension.KEY_WIDGET_ID, widgetCommonId, IssueTrackerConfigurationParameterType.Text);
        }

        private static IssueTrackerConfigurationParameter CreateGlobalParameter(string key, string value, IssueTrackerConfigurationParameterType type)
        {
            return new IssueTrackerConfigurationParameter
            {
                Name = key,
                Value = value,
                Type = type,
                IsGlobal = true
            };
        }

        private static IssueTrackerConfigurationParameter GetBranchPrefixParameter(IssueTrackerConfiguration configuration)
        {
            string prefix = GetValidParameterValue(configuration, FavroExtension.KEY_BRANCH_PREFIX, "[GOL-");
            return CreateGlobalParameter(FavroExtension.KEY_BRANCH_PREFIX, prefix, IssueTrackerConfigurationParameterType.BranchPrefix);
        }

        private static IssueTrackerConfigurationParameter GetBranchSuffixParameter(IssueTrackerConfiguration configuration)
        {
            string suffix = GetValidParameterValue(configuration, FavroExtension.KEY_BRANCH_SUFFIX, "]");
            return CreateGlobalParameter(FavroExtension.KEY_BRANCH_SUFFIX, suffix, IssueTrackerConfigurationParameterType.BranchPrefix);
        }

    }
}
