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

using System.Collections.Generic;
using FavroPlasticExtension.Favro.API;
using FavroPlasticExtensionTests.Helpers;
using FavroPlasticExtensionTests.Mocks;
using NUnit.Framework;

namespace FavroPlasticExtensionTests.Favro.API
{
    [TestFixture]
    public partial class FavroApiFacadeTests
    {
        private const string ORGANIZATION = "test-organization";

        private MockConnection mockConnection;
        private MockLog mockLog;
        private FakeResponseFactory responseFactory;

        [OneTimeSetUp]
        public void Initialize()
        {
            responseFactory = new FakeResponseFactory();
        }

        [SetUp]
        public void PreTest()
        {
            mockConnection = new MockConnection();
            mockLog = new MockLog();
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

        private FavroApiFacade CreateFacade()
        {
            return new FavroApiFacade(mockConnection, mockLog);
        }
    }
}
