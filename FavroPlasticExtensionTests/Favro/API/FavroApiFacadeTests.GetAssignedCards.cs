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
        private const string CATEGORY_GET_ASSIGNED_CARDS = "GetAssignedCards,FavroApiFacade";
        private const string ENDPOINT_GET_CARDS = "/cards";

        [TestCase(Category = CATEGORY_GET_ASSIGNED_CARDS)]
        public void GetAssignedCards_OrganizationIdNotSet_ShouldThrow()
        {
            // Arrange
            organization = string.Empty;
            // Assert
            var exception = Assert.Throws<InvalidOperationException>(() => sut.GetAssignedCards("", ""));
            Assert.AreEqual("An organization ID must be selected before retrieving information from Favro", exception.Message);
            connectionMock.VerifyGet(x => x.OrganizationId, Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_ASSIGNED_CARDS)]
        public void GetAssignedCards_ErrorResponse_ShouldNotThrow()
        {
            // Arrange
            StubConnectionWithError<WebException>();
            // Act
            sut.GetAssignedCards("", "");
            // Assert
            connectionMock.Verify(x => x.Get(ENDPOINT_GET_CARDS, It.Is<NameValueCollection>(p => HasAssignedCardsParameters(p))), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_ASSIGNED_CARDS)]
        public void GetAssignedCards_ErrorResponse_ShouldLogError()
        {
            // Arrange
            StubConnectionWithError<WebException>();
            // Act
            sut.GetAssignedCards("", "");
            // Assert
            logMock.Verify(x => x.Error("Unexpected error while retrieving assigned cards", It.IsAny<WebException>()), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_ASSIGNED_CARDS)]
        public void GetAssignedCards_ShouldRetrieveCards()
        {
            // Arrange
            StubConnectionWithResponses(GetAssignedCardsResponse());
            // Act
            var result = sut.GetAssignedCards("", "");
            // Assert
            connectionMock.Verify(x => x.Get(ENDPOINT_GET_CARDS, It.Is<NameValueCollection>(p => HasAssignedCardsParameters(p))), Times.Once);
            connectionMock.Verify(x => x.GetNextPage(ENDPOINT_GET_CARDS, It.IsAny<Response>(), It.Is<NameValueCollection>(p => HasAssignedCardsParameters(p))), Times.Once);
            Assert.That(result, Has.Count.EqualTo(3));
        }

        private IEnumerable<Response> GetAssignedCardsResponse()
        {
            yield return dataFaker.GetResponseFromFile("Responses.assigned_cards_page0.json");
            yield return dataFaker.GetResponseFromFile("Responses.assigned_cards_page1.json");
        }

        private bool HasAssignedCardsParameters(NameValueCollection captured)
        {
            return captured != null && captured.Get("todoList") == "true" && captured.Get("unique") == "true";
        }
    }
}
