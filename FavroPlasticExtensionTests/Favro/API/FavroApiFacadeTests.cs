using System;
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

        private FavroApiFacade CreateFacade()
        {
            return new FavroApiFacade(mockConnection, mockLog);
        }
    }
}
