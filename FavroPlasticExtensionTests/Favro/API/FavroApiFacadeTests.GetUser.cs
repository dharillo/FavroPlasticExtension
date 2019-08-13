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
using System.Linq;
using System.Net;
using System.Net.Http;
using FavroPlasticExtension.Favro;
using FavroPlasticExtension.Favro.API;
using NUnit.Framework;

namespace FavroPlasticExtensionTests.Favro.API
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
            var sut = PrepareGetUser();
            mockConnection.OrganizationId = invalidOrganization;
            // Assert:
            Assert.DoesNotThrow(() => sut.GetUser(USER_ID_FULL_MEMBER));
        }

        [TestCase(null, typeof(ArgumentNullException), Category = CATEGORY_GET_USER)]
        [TestCase("", typeof(ArgumentException), Category = CATEGORY_GET_USER)]
        [TestCase(" ", typeof(ArgumentException), Category = CATEGORY_GET_USER)]
        public void GetUser_InvalidUserId_ShouldThrow(string userId, Type expectedException)
        {
            // Arrange:
            var sut = CreateFacade();
            // Assert:
            Assert.Throws(expectedException, () => sut.GetUser(userId));
        }

        [TestCase(Category = CATEGORY_GET_USER)]
        public void GetUser_ResponseError_ShouldReturnNull()
        {
            // Arrange:
            var sut = CreateFacade();
            var response = responseFactory.GetErrorResponse(new WebException("User not found"));
            mockConnection.SetNextResponse(response);
            // Act:
            var user = sut.GetUser(USER_ID_FULL_MEMBER);
            // Assert:
            Assert.IsNull(user);
        }

        [TestCase(Category = CATEGORY_GET_USER)]
        public void GetUser_ValidUser_ShouldUseCorrectMethod()
        {
            // Arrange:
            var sut = PrepareGetUser();
            // Act:
            sut.GetUser(USER_ID_FULL_MEMBER);
            // Assert:
            var request = mockConnection.RequestsProcessed.FirstOrDefault();
            Assert.IsNotNull(request);
            Assert.AreEqual(HttpMethod.Get, request.Method);
        }

        [TestCase(Category = CATEGORY_GET_USER)]
        public void GetUser_ValidUser_ShouldUseNullParameters()
        {
            // Arrange:
            var sut = PrepareGetUser();
            // Act:
            sut.GetUser(USER_ID_FULL_MEMBER);
            // Assert:
            var request = mockConnection.RequestsProcessed.FirstOrDefault();
            Assert.IsNotNull(request);
            Assert.IsNull(request.Parameters);
        }

        [TestCase(USER_ID_ADMINISTRATOR, Category = CATEGORY_GET_USER)]
        [TestCase(USER_ID_FULL_MEMBER, Category = CATEGORY_GET_USER)]
        [TestCase(USER_ID_GUEST, Category = CATEGORY_GET_USER)]
        public void GetUser_ValidUser_ShouldBuildCorrectUrl(string userId)
        {
            // Arrange:
            var sut = PrepareGetUser(userId);
            // Act:
            sut.GetUser(userId);
            // Assert:
            var request = mockConnection.RequestsProcessed.FirstOrDefault();
            Assert.IsNotNull(request);
            Assert.AreEqual($"{FavroApiFacade.ENDPOINT_USERS}/{userId}", request.Url);
        }

        [TestCase(Category = CATEGORY_GET_USER)]
        public void GetUser_ValidUser_ShouldDeserializeData()
        {
            // Arrange:
            var sut = PrepareGetUser();
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
            var sut = PrepareGetUser(userId);
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
            var sut = PrepareGetUser(userId);
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
            var sut = PrepareGetUser(userId);
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
            var sut = PrepareGetUser(userId);
            // Act:
            var user = sut.GetUser(userId);
            // Assert:
            Assert.AreEqual(expectedName, user.Name);
        }

        private FavroApiFacade PrepareGetUser(string userId = USER_ID_FULL_MEMBER)
        {
            var sut = CreateFacade();
            mockConnection.SetNextResponse(GetUserResponse(userId));
            return sut;
        }

        private Response GetUserResponse(string userId)
        {
            return responseFactory.GetResponseFromFile($"Responses.{userId}.json");
        }
    }
}
