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
        private const string CATEGORY_GET_ORGANIZATIONS = "GetOrganizations,FavroApiFacade";

        [TestCaseSource(nameof(NullAndEmptyStrings), Category = CATEGORY_GET_ORGANIZATIONS)]
        public void GetOrganizations_InvalidOrgnization_ShouldNotThrow(string invalidOrganization)
        {
            // Arrange:
            var sut = CreateFacade();
            mockConnection.OrganizationId = invalidOrganization;
            // Assert:
            Assert.DoesNotThrow(() => sut.GetAllOrganizations());
        }

        [TestCase(Category = CATEGORY_GET_ORGANIZATIONS)]
        public void GetOrganizations_ResponseError_ShouldReturnEmptyList()
        {
            // Arrange:
            var sut = CreateFacade();
            var response = responseFactory.GetErrorResponse(new WebException("Invalid user"));
            mockConnection.SetNextResponse(response);
            // Act:
            var organizations = sut.GetAllOrganizations();
            // Assert:
            Assert.IsNotNull(organizations);
            Assert.IsEmpty(organizations);
        }

        [TestCase(Category = CATEGORY_GET_ORGANIZATIONS)]
        public void GetOrganizations_ShouldUseCorrectHttpMethod()
        {
            // Arrange:
            var sut = PrepareGetAllOrganizations();
            // Act:
            sut.GetAllOrganizations();
            // Assert:
            foreach (var requestInfo in mockConnection.RequestsProcessed)
            {
                Assert.AreEqual(HttpMethod.Get, requestInfo.Method);
            }
        }

        [TestCase(Category = CATEGORY_GET_ORGANIZATIONS)]
        public void GetOrganizations_ShouldUseCorrectEndpoint()
        {
            // Arrange:
            var sut = PrepareGetAllOrganizations();
            // Act:
            sut.GetAllOrganizations();
            // Assert:
            foreach (var requestInfo in mockConnection.RequestsProcessed)
            {
                Assert.AreEqual(FavroApiFacade.ENDPOINT_ORGANIZATIONS, requestInfo.Url);
            }
        }

        [TestCase(Category = CATEGORY_GET_ORGANIZATIONS)]
        public void GetOrganizations_ShouldUseNullParameters()
        {
            // Arrange:
            var sut = PrepareGetAllOrganizations();
            // Act:
            sut.GetAllOrganizations();
            // Assert:
            foreach (var requestInfo in mockConnection.RequestsProcessed)
            {
                Assert.IsNull(requestInfo.Parameters);
            }
        }

        private FavroApiFacade PrepareGetAllOrganizations()
        {
            var sut = CreateFacade();
            mockConnection.SetNextResponses(GetOrganizationsResponses());
            return sut;
        }

        private List<Response> GetOrganizationsResponses()
        {
            return new List<Response>
            {
                responseFactory.GetResponseFromFile("Responses.organizations_page1.json"),
                responseFactory.GetResponseFromFile("Responses.organizations_page2.json")
            };
        }
    }
}
