<one line to give the program's name and a brief idea of what it does.>
    Copyright(C) <year>  <name of author>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.If not, see<https://www.gnu.org/licenses/>

using FavroPlasticExtension.Favro;
using FavroPlasticExtension.Favro.API;
using FavroPlasticExtensionTests.Helpers;
using FavroPlasticExtensionTests.Mocks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Net;

namespace FavroPlasticExtensionTests.Favro.API
{
    [TestFixture]
    public class ResponseTests
    {
        private const string REQUEST_ID = "8cc57b1d8a218fa639c8a0fa";
        #region GetPageNumber
        [TestCase]
        public void GetPageNumber_HasError_ShouldThrow()
        {
            // Arrange:
            var sut = new Response();
            sut.Content = SerializedResponseContent(REQUEST_ID, 1, 100, 100, new List<object>());
            sut.Error = new WebException();
            // Assert:
            Assert.Throws<InvalidOperationException>(() => sut.GetPageNumber());
        }
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetPageNumber_NoContent_ShouldThrow(string content)
        {
            // Arrange:
            var sut = new Response();
            sut.Content = content;
            // Assert:
            Assert.Throws<InvalidOperationException>(() => sut.GetPageNumber());
        }
        [TestCase(int.MinValue)]
        [TestCase(0)]
        public void GetPageNumber_NoValidPageNumber_ShouldThrow(int invalidValue)
        {
            // Arrange:
            var sut = new Response();
            sut.Content = SerializedResponseContent(REQUEST_ID, invalidValue, 10, 100, new List<object>());
            // Assert:
            Assert.Throws<ArgumentException>(() => sut.GetPageNumber());
        }
        [TestCase(int.MaxValue)]
        [TestCase(1)]
        public void GetPageNumber_ValidValue_ReturnsValue(int validValue)
        {
            // Arrange:
            var sut = new Response();
            sut.Content = SerializedResponseContent(REQUEST_ID, validValue, 10, 100, new List<object>());
            // Act:
            var pageNumber = sut.GetPageNumber();
            // Assert:
            Assert.AreEqual(validValue, pageNumber);
        }
        #endregion
        #region HasMorePages
        [TestCase]
        public void HasMorePages_HasError_ReturnsFalse()
        {
            // Arrange:
            var sut = new Response();
            sut.Content = SerializedResponseContent(REQUEST_ID, 1, 100, 100, new List<object>());
            sut.Error = new WebException();
            // Act:
            var hasMorePages = sut.HasMorePages();
            // Assert:
            Assert.IsFalse(hasMorePages);
        }
        [TestCase]
        public void HasMorePages_NoPagedResponse_ReturnsFalse()
        {
            // Arrange:
            var sut = new Response();
            sut.Content = SerializedResponseContent(GetExampleUser());
            // Act:
            var hasMorePages = sut.HasMorePages();
            // Assert:
            Assert.IsFalse(hasMorePages);
        }
        [TestCase]
        public void HasMorePages_EmptyContent_ReturnsFalse()
        {
            // Arrange:
            var sut = new Response();
            sut.Content = string.Empty;
            // Act:
            var hasMorePages = sut.HasMorePages();
            // Assert:
            Assert.IsFalse(hasMorePages);
        }
        [TestCase]
        public void HasMorePages_NullContent_ReturnsFalse()
        {
            // Arrange:
            var sut = new Response();
            sut.Content = null;
            // Act:
            var hasMorePages = sut.HasMorePages();
            // Assert:
            Assert.IsFalse(hasMorePages);
        }
        [TestCase]
        public void HasMorePages_LastPage_ReturnsFalse()
        {
            // Arrange:
            var sut = new Response();
            sut.Content = SerializedResponseContent(REQUEST_ID, 1, 1, 100, new List<object>());
            // Act:
            var hasMorePages = sut.HasMorePages();
            // Assert:
            Assert.IsFalse(hasMorePages);
        }
        [TestCase(1, 2)]
        [TestCase(1, 100)]
        public void HasMorePages_PendingPages_ReturnsTrue(int currentPage, int numPages)
        {
            // Arrange:
            var sut = new Response();
            sut.Content = SerializedResponseContent(REQUEST_ID, currentPage, numPages, 100, new List<object>());
            // Act:
            var hasMorePages = sut.HasMorePages();
            // Assert:
            Assert.IsTrue(hasMorePages);
        }
        #endregion
        #region Constructor
        [TestCase]
        public void Constructor_CreatesEmptyHeaders()
        {
            // Arrange:
            var sut = new Response();
            // Assert:
            Assert.IsNotNull(sut.Headers);
            Assert.That(sut.Headers, Is.Empty);
        }
        [TestCase]
        public void Constructor_CreatesNullError()
        {
            // Arrange:
            var sut = new Response();
            // Assert:
            Assert.IsNull(sut.Error);
        }
        [TestCase]
        public void Constructor_CreatesEmptyContent()
        {
            // Arrange:
            var sut = new Response();
            // Assert:
            Assert.IsNull(sut.Content);
        }
        #endregion
        public string SerializedResponseContent<TEntry>(string requestId, int page, int numPages, int limit, List<TEntry> entries)
        {
            var content = new PagedResponse<TEntry>
            {
                RequestId = requestId,
                Page = page,
                Pages = numPages,
                Entries = entries,
                Limit = limit
            };
            return SerializedResponseContent(content);
        }

        public string SerializedResponseContent<TEntry>(TEntry entry)
        {
            return JsonConvert.SerializeObject(entry, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        public object GetExampleUser()
        {
            return new MockUser
            {
                UserId = "67973f72db34592d8fc96c48",
                Name = "Favro user",
                Email = "user@favro.com",
                OrganizationRole = OrganizationMember.ROLE_ADMINISTRATOR
            };
        }
    }
}
