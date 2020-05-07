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
using FavroPlasticExtensionTests.Helpers;
using log4net;
using Moq;
using NUnit.Framework;

namespace FavroPlasticExtension.Favro.API
{
    [TestFixture]
    public partial class FavroApiFacadeTests
    {
        private const string ORGANIZATION = "test-organization";

        private Mock<IFavroConnection> connectionMock;
        private Mock<ILog> logMock;
        private FakeResponseFactory dataFaker;
        private string organization;
        private FavroApiFacade sut;

        [OneTimeSetUp]
        public void InitialSetup()
        {
            dataFaker = new FakeResponseFactory();
        }

        [SetUp]
        public void PreTest()
        {
            organization = ORGANIZATION;
            connectionMock = new Mock<IFavroConnection>();
            connectionMock
                .SetupGet(x => x.OrganizationId)
                .Returns(() => organization);
            logMock = new Mock<ILog>();
            sut = new FavroApiFacade(connectionMock.Object, logMock.Object);
        }
        
        private static IEnumerable<string> NullAndEmptyStrings
        {
            get
            {
                yield return null;
                yield return "";
                yield return " ";
                yield return "\t";
                yield return "\n";
            }
        }

        private void StubConnectionWithResponses(IEnumerable<Response> userInfoResponses)
        {
            IEnumerator<Response> responses = userInfoResponses.GetEnumerator();
            connectionMock
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<NameValueCollection>()))
                .Returns(responses.MoveNext() ? responses.Current : null);
            connectionMock
                .SetupSequence(x => x.GetNextPage(It.IsAny<string>(),It.IsAny<Response>(), It.IsAny<NameValueCollection>()))
                .Returns(() => responses.MoveNext() ? responses.Current : null);
        }

        private void StubConnectionWithError<TError>() where TError : Exception, new()
        {
            connectionMock
                .Setup(x => x.Get(It.IsAny<string>(), It.IsAny<NameValueCollection>()))
                .Returns(dataFaker.GetErrorResponse(new TError()));
        } 
    }
}
