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
        private const string CATEGORY_GET_ALL_USERS = "GetAllUsers,FavroApiFacade";

        [TestCaseSource(nameof(NullAndEmptyStrings), Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_InvalidOrganization_ShouldThrow(string invalidOrganization)
        {
            // Arrange:
            organization = invalidOrganization;
            // Assert:
            Assert.Throws<InvalidOperationException>(() => sut.GetAllUsers());
            connectionMock.VerifyGet(x => x.OrganizationId, Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_ShouldUseCorrectHttpMethod()
        {
            // Arrange:
            StubConnectionWithResponses(GetUserInfoResponses());
            // Act:
            sut.GetAllUsers();
            // Assert:
            connectionMock.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<NameValueCollection>()), Times.Once);
            connectionMock.Verify(x => x.GetNextPage(It.IsAny<string>(), It.IsNotNull<Response>(), It.IsAny<NameValueCollection>()), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_ShouldUseCorrectEndpoint()
        {
            // Arrange:
            StubConnectionWithResponses(GetUserInfoResponses());
            // Act:
            sut.GetAllUsers();
            // Assert:
            connectionMock.Verify(x => x.Get(FavroApiFacade.ENDPOINT_USERS, It.IsAny<NameValueCollection>()), Times.Once);
            connectionMock.Verify(x => x.GetNextPage(FavroApiFacade.ENDPOINT_USERS, It.IsNotNull<Response>(), It.IsAny<NameValueCollection>()), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_ShouldUseNullParameters()
        {
            // Arrange:
            StubConnectionWithResponses(GetUserInfoResponses());
            // Act:
            sut.GetAllUsers();
            // Assert:
            connectionMock.Verify(x => x.Get(It.IsAny<string>(), null), Times.Once);
            connectionMock.Verify(x => x.GetNextPage(It.IsAny<string>(), It.IsNotNull<Response>(), null), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange:
            StubConnectionWithResponses(GetUserInfoResponses());
            // Act:
            var users = sut.GetAllUsers();
            // Assert:
            Assert.IsNotNull(users);
            Assert.That(users, Has.Count.EqualTo(10));
        }

        [TestCase]
        public void GetAllUsers_ResponseError_ShouldReturnEmptyList()
        {
            // Arrange:
            StubConnectionWithError<WebException>();
            // Act:
            var users = sut.GetAllUsers();
            // Assert:
            Assert.IsNotNull(users);
            Assert.IsEmpty(users);
        }

        [TestCase]
        public void GetAllUsers_ResponseError_ShouldLogError()
        {
            // Arrange:
            StubConnectionWithError<WebException>();
            // Act:
            var users = sut.GetAllUsers();
            // Assert:
            logMock.Verify(x => x.Error("Unexpected error while retrieving users", It.IsAny<WebException>()), Times.Once);
        }

        private IEnumerable<Response> GetUserInfoResponses()
        {
            yield return dataFaker.GetResponseFromFile("Responses.users_page1.json");
            yield return dataFaker.GetResponseFromFile("Responses.users_page2.json");
        }

	}
}
