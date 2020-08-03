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
using System.Net;
using Moq;
using NUnit.Framework;

namespace FavroPlasticExtension.Favro.API
{
    public partial class FavroApiFacadeTests
    {
        private const string ENDPOINT_GET_ALL_COLLECTIONS = "/collections";

        [TestCase]
        public void GetAllCollections_OrganizationIdNotSet_ShouldThrow()
        {
            // Arrange
            organization = string.Empty;
            // Assert
            var exception = Assert.Throws<InvalidOperationException>(() => sut.GetAllCollections());
            Assert.AreEqual("An organization ID must be selected before retrieving information from Favro", exception.Message);
            connectionMock.VerifyGet(x => x.OrganizationId, Times.Once);
        }

        [TestCase]
        public void GetAllCollections_ErrorResponse_ShouldNotThrow()
        {
            // Arrange
            StubConnectionWithError<WebException>();
            // Act
            sut.GetAllCollections();
            // Assert
            connectionMock.Verify(x => x.Get(ENDPOINT_GET_ALL_COLLECTIONS, null), Times.Once);
        }

        [TestCase]
        public void GetAllCollections_ErrorResponse_ShouldLogError()
        {
            // Arrange
            StubConnectionWithError<WebException>();
            // Act
            sut.GetAllCollections();
            // Assert
            logMock.Verify(x => x.Error("Unexpected error while retrieving collections", It.IsAny<WebException>()), Times.Once);
        }

        [TestCase]
        public void GetAllCollections_ErrorResponse_ShouldReturnEmptyList()
        {
            // Arrange
            StubConnectionWithError<WebException>();
            // Act
            var collections = sut.GetAllCollections();
            // Assert
            Assert.IsNotNull(collections);
            Assert.IsEmpty(collections);
        }

        [TestCase]
        public void GetAllCollections_ShouldRetrieveCollections()
        {
            // Arrange
            StubConnectionWithResponses(GetAllCollectionsResponses());
            // Act
            var collections = sut.GetAllCollections();
            // Assert
            Assert.IsNotNull(collections);
            Assert.That(collections, Has.Count.EqualTo(51));
            Assert.AreEqual("3db52e216bc2a1661c6f98d0", collections[0].CollectionId);
            Assert.AreEqual("collection-test-1", collections[0].Name);
        }

        public IEnumerable<Response> GetAllCollectionsResponses()
        {
            yield return dataFaker.GetResponseFromFile("Responses.collections_page1.json");
        }
    }
}
