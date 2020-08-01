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
using Moq;
using NUnit.Framework;

namespace FavroPlasticExtension.Favro.API
{
	public partial class FavroApiFacadeTests
    {
        private const string CATEGORY_ADD_COMMENT_TO_CARD = "AddCommentToCard,FavroApiFacade";
        private const string ENDPOINT_COMMENT = "/comments";
        private const string VALID_CARD_ID = "abc123";
        private const string VALID_COMMENT = "this is a comment";

        [TestCase(Category = CATEGORY_ADD_COMMENT_TO_CARD)]
        public void AddCommentToCard_OrganizationNotSelected_ShouldThrow()
        {
            // Arrange
            organization = string.Empty;
            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => sut.AddCommentToCard(VALID_CARD_ID, VALID_COMMENT));
            // Assert
            Assert.AreEqual("An organization ID must be selected before retrieving information from Favro", exception.Message);
            connectionMock.VerifyGet(x => x.OrganizationId, Times.Once);
            connectionMock.Verify(x => x.Post(It.IsAny<string>(), It.IsAny<CardComment>()), Times.Never);
        }

        [TestCase(Category = CATEGORY_ADD_COMMENT_TO_CARD)]
        public void AddCommentToCard_NullCardId_ShouldThrow()
        {
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.AddCommentToCard(null, VALID_COMMENT));
            // Assert
            Assert.That(exception, Has.Message.StartsWith("A card common identifier must be a non-empty string"));
            Assert.AreEqual("commonId", exception.ParamName);
        }

        [TestCase("",Category = CATEGORY_ADD_COMMENT_TO_CARD)]
        [TestCase(" ", Category = CATEGORY_ADD_COMMENT_TO_CARD)]
        [TestCase("\t", Category = CATEGORY_ADD_COMMENT_TO_CARD)]
        [TestCase("\n", Category = CATEGORY_ADD_COMMENT_TO_CARD)]
        public void AddCommentToCard_InvalidCardId_ShouldThrow(string invalidId)
        {
            // Act
            var exception = Assert.Throws<ArgumentException>(() => sut.AddCommentToCard(invalidId, VALID_COMMENT));
            // Assert
            Assert.That(exception, Has.Message.StartsWith("A card common identifier must be a non-empty string"));
            Assert.AreEqual("commonId", exception.ParamName);
        }


        [TestCase(Category = CATEGORY_ADD_COMMENT_TO_CARD)]
        public void AddCommentToCard_NullComment_ShouldThrow()
        {
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => sut.AddCommentToCard(VALID_CARD_ID, null));
            // Assert
            Assert.That(exception, Has.Message.StartsWith("A card comment must be a non-empty string"));
            Assert.AreEqual("comment", exception.ParamName);
        }

        [TestCase("", Category = CATEGORY_ADD_COMMENT_TO_CARD)]
        [TestCase(" ", Category = CATEGORY_ADD_COMMENT_TO_CARD)]
        [TestCase("\t", Category = CATEGORY_ADD_COMMENT_TO_CARD)]
        [TestCase("\n", Category = CATEGORY_ADD_COMMENT_TO_CARD)]
        public void AddCommentToCard_InvalidComment_ShouldThrow(string invalidComment)
        {
            // Act
            var exception = Assert.Throws<ArgumentException>(() => sut.AddCommentToCard(VALID_CARD_ID, invalidComment));
            // Assert
            Assert.That(exception, Has.Message.StartsWith("A card comment must be a non-empty string"));
            Assert.AreEqual("comment", exception.ParamName);
        }
    }
}
