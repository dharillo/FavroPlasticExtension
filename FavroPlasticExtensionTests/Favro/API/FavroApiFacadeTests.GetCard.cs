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
        private const string CATEGORY_GET_CARD = "GetCard,FavroApiFacade";
        private const string CARD_COMMON_ID = "918905e496715cbf70c7d1ac";
        private const int CARD_SEQUENTIAL_ID = 32864;

        [TestCase(Category = CATEGORY_GET_CARD)]
        public void GetCard_CommonId_OrganizationIdNotSet_ShouldThrow()
        {
            // Arrange
            organization = string.Empty;
            // Assert
            var exception = Assert.Throws<InvalidOperationException>(() => sut.GetCard(CARD_COMMON_ID));
            Assert.AreEqual("An organization ID must be selected before retrieving information from Favro", exception.Message);
            connectionMock.VerifyGet(x => x.OrganizationId, Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_CARD)]
        public void GetCard_SequentialId_OrganizationIdNotSet_ShouldThrow()
        {
            // Arrange
            organization = string.Empty;
            // Assert
            var exception = Assert.Throws<InvalidOperationException>(() => sut.GetCard(CARD_SEQUENTIAL_ID));
            Assert.AreEqual("An organization ID must be selected before retrieving information from Favro", exception.Message);
            connectionMock.VerifyGet(x => x.OrganizationId, Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_CARD)]
        public void GetCard_CommonId_NullId_ShouldThrow()
        {
            // Arrange
            organization = string.Empty;
            // Assert
            var exception = Assert.Throws<ArgumentNullException>(() => sut.GetCard(null));
            Assert.That(exception, Has.Message.StartsWith("A card common identifier must be a non-empty string"));
            Assert.AreEqual("commonId", exception.ParamName);
            connectionMock.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<NameValueCollection>()), Times.Never);
        }

        [TestCase("", Category = CATEGORY_GET_CARD)]
        [TestCase(" ", Category = CATEGORY_GET_CARD)]
        [TestCase("\t", Category = CATEGORY_GET_CARD)]
        [TestCase("\n", Category = CATEGORY_GET_CARD)]
        public void GetCard_CommonId_InvalidId_ShouldThrow(string _invalidId)
        {
            // Arrange
            organization = string.Empty;
            // Assert
            var exception = Assert.Throws<ArgumentException>(() => sut.GetCard(_invalidId));
            Assert.That(exception, Has.Message.StartsWith("A card common identifier must be a non-empty string"));
            Assert.AreEqual("commonId", exception.ParamName);
            connectionMock.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<NameValueCollection>()), Times.Never);
        }

        [TestCase(int.MinValue)]
        [TestCase(-1, Category = CATEGORY_GET_CARD)]
        public void GetCard_SequentialId_InvalidId_ShouldThrow(int _invalidId)
        {
            // Arrange
            organization = string.Empty;
            // Assert
            var exception = Assert.Throws<ArgumentException>(() => sut.GetCard(_invalidId));
            Assert.That(exception, Has.Message.StartsWith("A card sequential identifier must be a positive integer"));
            Assert.AreEqual("sequentialId", exception.ParamName);
            connectionMock.Verify(x => x.Get(It.IsAny<string>(), It.IsAny<NameValueCollection>()), Times.Never);
        }

        [TestCase(Category = CATEGORY_GET_CARD)]
        public void GetCard_CommonId_ErrorResponse_ShouldNotThrow()
        {
            // Arrange
            StubConnectionWithError<WebException>();
            // Act
            sut.GetCard(CARD_COMMON_ID);
            // Assert
            connectionMock.Verify(x => x.Get(ENDPOINT_GET_CARDS, It.Is<NameValueCollection>(p => HasGetCardByCommonIdParameters(p))), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_CARD)]
        public void GetCard_SequentialId_ErrorResponse_ShouldNotThrow()
        {
            // Arrange
            StubConnectionWithError<WebException>();
            // Act
            sut.GetCard(CARD_SEQUENTIAL_ID);
            // Assert
            connectionMock.Verify(x => x.Get(ENDPOINT_GET_CARDS, It.Is<NameValueCollection>(p => HasGetCardBySequentialIdParameters(p))), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_CARD)]
        public void GetCard_CommonId_ErrorResponse_ShouldLogError()
        {
            // Arrange
            StubConnectionWithError<WebException>();
            // Act
            sut.GetCard(CARD_COMMON_ID);
            // Assert
            logMock.Verify(x => x.Error("Unexpected error while retrieving card by id", It.IsAny<WebException>()), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_CARD)]
        public void GetCard_SequentialId_ErrorResponse_ShouldLogError()
        {
            // Arrange
            StubConnectionWithError<WebException>();
            // Act
            sut.GetCard(CARD_SEQUENTIAL_ID);
            // Assert
            logMock.Verify(x => x.Error("Unexpected error while retrieving card by id", It.IsAny<WebException>()), Times.Once);
        }

        [TestCase(Category = CATEGORY_GET_CARD)]
        public void GetCard_CommonId_ValidId_ShouldRetrieveCards()
        {
            // Arrange
            StubConnectionWithResponses(GetCardResponse());
            // Act
            var result = sut.GetCard(CARD_COMMON_ID);
            // Assert
            connectionMock.Verify(x => x.Get(ENDPOINT_GET_CARDS, It.Is<NameValueCollection>(p => HasGetCardByCommonIdParameters(p))), Times.Once);
            connectionMock.Verify(x => x.GetNextPage(It.IsAny<string>(), It.IsAny<Response>(), It.IsAny<NameValueCollection>()), Times.Never);
            Assert.NotNull(result);
            Assert.AreEqual(CARD_COMMON_ID, result.CardCommonId);
        }

        [TestCase(Category = CATEGORY_GET_CARD)]
        public void GetCard_SequentialId_ValidId_ShouldRetrieveCards()
        {
            // Arrange
            StubConnectionWithResponses(GetCardResponse());
            // Act
            var result = sut.GetCard(CARD_SEQUENTIAL_ID);
            // Assert
            connectionMock.Verify(x => x.Get(ENDPOINT_GET_CARDS, It.Is<NameValueCollection>(p => HasGetCardBySequentialIdParameters(p))), Times.Once);
            connectionMock.Verify(x => x.GetNextPage(It.IsAny<string>(), It.IsAny<Response>(), It.IsAny<NameValueCollection>()), Times.Never);
            Assert.NotNull(result);
            Assert.AreEqual(CARD_SEQUENTIAL_ID, result.SequentialId);
        }

        private IEnumerable<Response> GetCardResponse()
        {
            yield return dataFaker.GetResponseFromFile("Responses.card.json");
        }

        private bool HasGetCardByCommonIdParameters(NameValueCollection captured)
        {
            return captured != null && captured.Get("cardCommonId") == CARD_COMMON_ID && captured.Get("unique") == "true";
        }

        private bool HasGetCardBySequentialIdParameters(NameValueCollection captured)
        {
            return captured != null && captured.Get("cardSequentialId") == CARD_SEQUENTIAL_ID.ToString() && captured.Get("unique") == "true";
        }
    }
}
