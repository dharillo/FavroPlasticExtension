using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using FavroPlasticExtension.Favro.API;
using NUnit.Framework;

namespace FavroPlasticExtensionTests.Favro.API
{
	public partial class FavroApiFacadeTests
	{
        private const string CATEGORY_GET_ALL_USERS = "GetAllUsers,FavroApiFacade";

        [TestCase(null, Category = CATEGORY_GET_ALL_USERS)]
        [TestCase("", Category = CATEGORY_GET_ALL_USERS)]
        [TestCase(" ", Category = CATEGORY_GET_ALL_USERS)]
        [TestCase("\t", Category = CATEGORY_GET_ALL_USERS)]
        [TestCase("\n", Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_InvalidOrganization_ShouldThrow(string invalidOrganization)
        {
            // Arrange:
            var sut = CreateFacade();
            mockConnection.OrganizationId = invalidOrganization;
            // Assert:
            Assert.Throws<InvalidOperationException>(() => sut.GetAllUsers());
        }

        [TestCase(Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_ShouldUseCorrectHttpMethod()
        {
            // Arrange:
            var sut = PrepareGetAllUsers();
            // Act:
            sut.GetAllUsers();
            // Assert:
            foreach (var requestInfo in mockConnection.RequestsProcessed)
            {
                Assert.AreEqual(HttpMethod.Get, requestInfo.Method);
            }
        }

        [TestCase(Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_ShouldUseCorrectEndpoint()
        {
            // Arrange:
            var sut = PrepareGetAllUsers();
            // Act:
            sut.GetAllUsers();
            // Assert:
            foreach (var requestInfo in mockConnection.RequestsProcessed)
            {
                Assert.AreEqual("/users", requestInfo.Url);
            }
        }

        [TestCase(Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_InitialRequest_ShouldUseNullParameters()
        {
            // Arrange:
            var sut = PrepareGetAllUsers();
            // Act:
            sut.GetAllUsers();
            // Assert:
            var firstRequest = mockConnection.RequestsProcessed.FirstOrDefault();
            Assert.IsNotNull(firstRequest);
            Assert.IsNull(firstRequest.Parameters);
        }

        [TestCase(Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_FollowingRequest_ShouldSetRequestIdParameter()
        {
            // Arrange:
            var sut = PrepareGetAllUsers();
            // Act:
            sut.GetAllUsers();
            // Assert:
            var requests = mockConnection.RequestsProcessed;
            for (int i = 1; i < requests.Count; i ++)
            {
                var request = requests[i];
                Assert.IsNotNull(request.Parameters);
                Assert.IsTrue(request.Parameters.HasKeys());
                Assert.AreEqual("users", request.Parameters["requestId"]);
            }
        }

        [TestCase(Category = CATEGORY_GET_ALL_USERS)]
        public void GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange:
            var sut = PrepareGetAllUsers();
            // Act:
            var users = sut.GetAllUsers();
            // Assert:
            Assert.IsNotNull(users);
            Assert.AreEqual(2, mockConnection.ConsecutiveResponsesUsed, "Should call twice to get all user pages");
            Assert.That(users, Has.Count.EqualTo(10));
        }

        [TestCase]
        public void GetAllUsers_ResponseError_ShouldReturnEmptyList()
        {
            // Arrange:
            var sut = CreateFacade();
            mockConnection.OrganizationId = ORGANIZATION;
            mockConnection.SetNextResponse(responseFactory.GetErrorResponse(new WebException("Invalid user")));
            // Act:
            var users = sut.GetAllUsers();
            // Assert:
            Assert.IsNotNull(users);
            Assert.IsEmpty(users);
        }

        private FavroApiFacade PrepareGetAllUsers()
        {
            var sut = CreateFacade();
            mockConnection.OrganizationId = ORGANIZATION;
            mockConnection.SetNextResponses(GetUserResponses());
            return sut;
        }

        private List<Response> GetUserResponses()
        {
            return new List<Response>
            {
                responseFactory.GetResponseFromFile("Responses.users_page1.json"),
                responseFactory.GetResponseFromFile("Responses.users_page2.json")
            };
        }
	}
}
