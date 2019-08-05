using System;
using FavroPlasticExtension.Favro.API;
using NUnit.Framework;

namespace FavroPlasticExtensionTests.Favro.API
{
    public partial class FavroApiFacadeTests
    {
        [TestCase(Category = "Constructor,FavroApiFacade")]
        public void Constructor_NullConnection_ShouldThrow()
        {
            //Assert:
            Assert.Throws<ArgumentNullException>(() => new FavroApiFacade(null, mockLog));
        }

        [TestCase(Category = "Constructor,FavroApiFacade")]
        public void Constructor_NullLogger_ShouldThrow()
        {
            // Assert:
            Assert.Throws<ArgumentNullException>(() => new FavroApiFacade(mockConnection, null));
        }
    }
}
