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
using System.Net;
using System.Net.Http;
using FavroPlasticExtension.Favro.API;
using NUnit.Framework;

namespace FavroPlasticExtensionTests.Favro.API
{
	public partial class FavroApiFacadeTests
	{
        private const string CATEGORY_GET_ALL_USERS = "GetAllUsers,FavroApiFacade";

        [TestCase(null, Category = CATEGORY_GET_ALL_USERS)]
        [TestCase("", Category = CATEGORY_GET_ALL_USERS)]
        [TestCase(" ", Category = CATEGORY_GET_ALL_USERS)]
        [TestCase("\t", Category = CATEGORY_GET_ALL_USERS)]
        [TestCase("\n", Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_InvalidOrganization_ShouldThrow(string invalidOrganization)
        {
            // Arrange:
            var sut = CreateFacade();
            mockConnection.OrganizationId = invalidOrganization;
            // Assert:
            Assert.Throws<InvalidOperationException>(() => sut.GetAllUsers());
        }

        [TestCase(Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_ShouldUseCorrectHttpMethod()
        {
            // Arrange:
            var sut = PrepareGetAllUsers();
            // Act:
            sut.GetAllUsers();
            // Assert:
            foreach (var requestInfo in mockConnection.RequestsProcessed)
            {
                Assert.AreEqual(HttpMethod.Get, requestInfo.Method);
            }
        }

        [TestCase(Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_ShouldUseCorrectEndpoint()
        {
            // Arrange:
            var sut = PrepareGetAllUsers();
            // Act:
            sut.GetAllUsers();
            // Assert:
            foreach (var requestInfo in mockConnection.RequestsProcessed)
            {
                Assert.AreEqual(FavroApiFacade.ENDPOINT_USERS, requestInfo.Url);
            }
        }

        [TestCase(Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_ShouldUseNullParameters()
        {
            // Arrange:
            var sut = PrepareGetAllUsers();
            // Act:
            sut.GetAllUsers();
            // Assert:
            foreach (var request in mockConnection.RequestsProcessed)
            {
                Assert.IsNull(request.Parameters);
            }
        }

        [TestCase(Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange:
            var sut = PrepareGetAllUsers();
            // Act:
            var users = sut.GetAllUsers();
            // Assert:
            Assert.IsNotNull(users);
            Assert.AreEqual(2, mockConnection.ConsecutiveResponsesUsed, "Should call twice to get all user pages");
            Assert.That(users, Has.Count.EqualTo(10));
        }

        [TestCase]
        public void GetAllUsers_ResponseError_ShouldReturnEmptyList()
        {
            // Arrange:
            var sut = CreateFacade();
            mockConnection.OrganizationId = ORGANIZATION;
            mockConnection.SetNextResponse(responseFactory.GetErrorResponse(new WebException("Invalid user")));
            // Act:
            var users = sut.GetAllUsers();
            // Assert:
            Assert.IsNotNull(users);
            Assert.IsEmpty(users);
        }

        private FavroApiFacade PrepareGetAllUsers()
        {
            var sut = CreateFacade();
            mockConnection.OrganizationId = ORGANIZATION;
            mockConnection.SetNextResponses(GetUsersResponses());
            return sut;
        }

        private List<Response> GetUsersResponses()
        {
            return new List<Response>
            {
                responseFactory.GetResponseFromFile("Responses.users_page1.json"),
                responseFactory.GetResponseFromFile("Responses.users_page2.json")
            };
        }
	}
}
