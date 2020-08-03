//  Favro Plastic Extension
//  Copyright(C) 2020  David Harillo Sánchez
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
using System.Net;
using Moq;
using NUnit.Framework;

namespace FavroPlasticExtension.Favro.API
{
    public partial class FavroApiFacadeTests
    {
        private const string CATEGORY_GET_USER = "GetUser,FavroApiFacade";
        private const string USER_ID_FULL_MEMBER = "user-fullMember";
        private const string USER_ID_ADMINISTRATOR = "user-administrator";
        private const string USER_ID_GUEST = "user-guest";
        private const string EMAIL_ADMINISTRATOR = "administrator@example.com";
        private const string EMAIL_FULL_MEMBER = "fullmember@example.com";
        private const string EMAIL_GUEST = "guest@example.com";
        private const string NAME_ADMINISTRATOR = "Test User Administrator";
        private const string NAME_FULL_MEMBER = "Test User Fullmember";
        private const string NAME_GUEST = "Test User Guest";

        [TestCaseSource(nameof(NullAndEmptyStrings), Category = CATEGORY_GET_USER)]
        public void GetUser_InvalidOrganization_ShouldNotThrow(string invalidOrganization)
        {
            // Arrange:
            StubConnectionWithResponses(GetUserResponse());
            organization = invalidOrganization;
            // Assert:
            Assert.DoesNotThrow(() => sut.GetUser(USER_ID_FULL_MEMBER));
        }

        [TestCase(null, typeof(ArgumentNullException), Category = CATEGORY_GET_USER)]
        [TestCase("", typeof(ArgumentException), Category = CATEGORY_GET_USER)]
        [TestCase(" ", typeof(ArgumentException), Category = CATEGORY_GET_USER)]
        public void GetUser_InvalidUserId_ShouldThrow(string userId, Type expectedException)
        {
            // Arrange:
            StubConnectionWithResponses(GetUserResponse());
            // Assert:
            Assert.Throws(expectedException, () => sut.GetUser(userId));
        }

        [TestCase(Category = CATEGORY_GET_USER)]
        public void GetUser_ResponseError_ShouldReturnNull()
        {
            // Arrange:
            StubConnectionWithError<WebException>();
            // Act:
            var user = sut.GetUser(USER_ID_FULL_MEMBER);
            // Assert:
            connectionMock.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<NameValueCollection>()), Times.Once);
            Assert.IsNull(user);
        }

        [TestCase(Category = CATEGORY_GET_USER)]
        public void GetUser_ResponseError_ShouldLogError()
        {
            // Arrange:
            StubConnectionWithError<WebException>();
            // Act:
            var user = sut.GetUser(USER_ID_FULL_MEMBER);
            // Assert:
            logMock.Verify(x => x.Error($"Unable to retrieve the information of the user '{USER_ID_FULL_MEMBER}'", It.IsAny<WebException>()), Times.Once);
            Assert.IsNull(user);
        }

        [TestCase(Category = CATEGORY_GET_USER)]
        public void GetUser_ValidUser_ShouldUseCorrectMethod()
        {
            // Arrange:
            StubConnectionWithResponses(GetUserResponse());
            // Act:
            sut.GetUser(USER_ID_FULL_MEMBER);
            // Assert:
            connectionMock.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<NameValueCollection>()), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_USER)]
        public void GetUser_ValidUser_ShouldUseNullParameters()
        {
            // Arrange:
            StubConnectionWithResponses(GetUserResponse());
            // Act:
            sut.GetUser(USER_ID_FULL_MEMBER);
            // Assert:
            connectionMock.Verify(x => x.Get(It.IsAny<string>(), null), Times.Once);
        }

        [TestCase(USER_ID_ADMINISTRATOR, Category = CATEGORY_GET_USER)]
        [TestCase(USER_ID_FULL_MEMBER, Category = CATEGORY_GET_USER)]
        [TestCase(USER_ID_GUEST, Category = CATEGORY_GET_USER)]
        public void GetUser_ValidUser_ShouldBuildCorrectUrl(string userId)
        {
            // Arrange:
            StubConnectionWithResponses(GetUserResponse(userId));
            // Act:
            sut.GetUser(userId);
            // Assert:
            connectionMock.Verify(x => x.Get($"{FavroApiFacade.ENDPOINT_USERS}/{userId}", It.IsAny<NameValueCollection>()), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_USER)]
        public void GetUser_ValidUser_ShouldDeserializeData()
        {
            // Arrange:
            StubConnectionWithResponses(GetUserResponse());
            // Act:
            var user = sut.GetUser(USER_ID_FULL_MEMBER);
            // Assert:
            Assert.IsNotNull(user);
        }

        [TestCase(USER_ID_ADMINISTRATOR, OrganizationMember.ROLE_ADMINISTRATOR, Category = CATEGORY_GET_USER)]
        [TestCase(USER_ID_FULL_MEMBER, OrganizationMember.ROLE_FULL_MEMBER, Category = CATEGORY_GET_USER)]
        [TestCase(USER_ID_GUEST, OrganizationMember.ROLE_GUEST, Category = CATEGORY_GET_USER)]
        public void GetUser_ValidUser_ParseCorrectUserRole(string userId, string expectedRole)
        {
            // Arrange:
            StubConnectionWithResponses(GetUserResponse(userId));
            // Act:
            var user = sut.GetUser(userId);
            // Assert:
            Assert.AreEqual(expectedRole, user.OrganizationRole);
        }

        [TestCase(USER_ID_ADMINISTRATOR, Category = CATEGORY_GET_USER)]
        [TestCase(USER_ID_FULL_MEMBER, Category = CATEGORY_GET_USER)]
        [TestCase(USER_ID_GUEST, Category = CATEGORY_GET_USER)]
        public void GetUser_ValidUser_ParseCorrectUserId(string userId)
        {
            // Arrange:
            StubConnectionWithResponses(GetUserResponse(userId));
            // Act:
            var user = sut.GetUser(userId);
            // Assert:
            Assert.AreEqual(userId, user.UserId);
        }

        [TestCase(USER_ID_ADMINISTRATOR, EMAIL_ADMINISTRATOR, Category = CATEGORY_GET_USER)]
        [TestCase(USER_ID_FULL_MEMBER, EMAIL_FULL_MEMBER, Category = CATEGORY_GET_USER)]
        [TestCase(USER_ID_GUEST, EMAIL_GUEST, Category = CATEGORY_GET_USER)]
        public void GetUser_ValidUser_ParseCorrectEmail(string userId, string expectedEmail)
        {
            // Arrange:
            StubConnectionWithResponses(GetUserResponse(userId));
            // Act:
            var user = sut.GetUser(userId);
            // Assert:
            Assert.AreEqual(expectedEmail, user.Email);
        }

        [TestCase(USER_ID_ADMINISTRATOR, NAME_ADMINISTRATOR, Category = CATEGORY_GET_USER)]
        [TestCase(USER_ID_FULL_MEMBER, NAME_FULL_MEMBER, Category = CATEGORY_GET_USER)]
        [TestCase(USER_ID_GUEST, NAME_GUEST, Category = CATEGORY_GET_USER)]
        public void GetUser_ValidUser_ParseCorrectName(string userId, string expectedName)
        {
            // Arrange:
            StubConnectionWithResponses(GetUserResponse(userId));
            // Act:
            var user = sut.GetUser(userId);
            // Assert:
            Assert.AreEqual(expectedName, user.Name);
        }

        private IEnumerable<Response> GetUserResponse(string userId = USER_ID_FULL_MEMBER)
        {
            yield return dataFaker.GetResponseFromFile($"Responses.{userId}.json");
        }
    }
}
