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

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using Moq;
using NUnit.Framework;

namespace FavroPlasticExtension.Favro.API
{
    public partial class FavroApiFacadeTests
    {
        private const string CATEGORY_GET_ORGANIZATIONS = "GetOrganizations,FavroApiFacade";

        [TestCaseSource(nameof(NullAndEmptyStrings), Category = CATEGORY_GET_ORGANIZATIONS)]
        public void GetOrganizations_InvalidOrgnization_ShouldNotThrow(string invalidOrganization)
        {
            // Arrange:
            organization = invalidOrganization;
            StubConnectionWithResponses(GetOrganizationsResponses());
            // Assert:
            Assert.DoesNotThrow(() => sut.GetAllOrganizations());
        }

        [TestCase(Category = CATEGORY_GET_ORGANIZATIONS)]
        public void GetOrganizations_ResponseError_ShouldReturnEmptyList()
        {
            // Arrange:
            organization = null;
            StubConnectionWithError<WebException>();
            // Act:
            var organizations = sut.GetAllOrganizations();
            // Assert:
            Assert.IsNotNull(organizations);
            Assert.IsEmpty(organizations);
        }

        [TestCase(Category = CATEGORY_GET_ORGANIZATIONS)]
        public void GetOrganizations_ResponseError_ShouldLogError()
        {
            // Arrange:
            organization = null;
            StubConnectionWithError<WebException>();
            // Act:
            var organizations = sut.GetAllOrganizations();
            // Assert:
            logMock.Verify(x => x.Error("Unexpected error while retrieving organizations", It.IsAny<WebException>()), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_ORGANIZATIONS)]
        public void GetOrganizations_ShouldUseCorrectHttpMethod()
        {
            // Arrange:
            StubConnectionWithResponses(GetOrganizationsResponses());
            // Act:
            sut.GetAllOrganizations();
            // Assert:
            connectionMock.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<NameValueCollection>()), Times.Once);
            connectionMock.Verify(x => x.GetNextPage(It.IsAny<string>(), It.IsNotNull<Response>(), It.IsAny<NameValueCollection>()), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_ORGANIZATIONS)]
        public void GetOrganizations_ShouldUseCorrectEndpoint()
        {
            // Arrange:
            StubConnectionWithResponses(GetOrganizationsResponses());
            // Act:
            sut.GetAllOrganizations();
            // Assert:
            connectionMock.Verify(x => x.Get(FavroApiFacade.ENDPOINT_ORGANIZATIONS, It.IsAny<NameValueCollection>()), Times.Once);
            connectionMock.Verify(x => x.GetNextPage(FavroApiFacade.ENDPOINT_ORGANIZATIONS, It.IsNotNull<Response>(), It.IsAny<NameValueCollection>()), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_ORGANIZATIONS)]
        public void GetOrganizations_ShouldUseNullParameters()
        {
            // Arrange:
            StubConnectionWithResponses(GetOrganizationsResponses());
            // Act:
            sut.GetAllOrganizations();
            // Assert:
            connectionMock.Verify(x => x.Get(It.IsAny<string>(), null), Times.Once);
            connectionMock.Verify(x => x.GetNextPage(It.IsAny<string>(), It.IsNotNull<Response>(), It.IsAny<NameValueCollection>()), Times.Once);
        }

        private IEnumerable<Response> GetOrganizationsResponses()
        {
            yield return dataFaker.GetResponseFromFile("Responses.organizations_page1.json");
            yield return dataFaker.GetResponseFromFile("Responses.organizations_page2.json");
        }
    }
}
