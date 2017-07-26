// <copyright file="AuthClientTests.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using ProjectBlackmagic.RestfulClient.Authentication;
using ProjectBlackmagic.RestfulClient.Content;

namespace ProjectBlackmagic.RestfulClient.Test
{
    [TestClass]
    public class AuthClientTests
    {
        private static TestObject payload;

        private AuthClient GetTestClient(IAuthenticator authenticator, string endpoint, HttpStatusCode httpStatusCode, object httpPayload = null)
        {
            var host = "http://fakeEndpoint";
            var clientHandler = new HttpClientHandler();
            var delegatingHandlers = new DelegatingHandler[]
            {
                FakeResponseHandler.Create(httpStatusCode, host + "/" + endpoint, httpPayload, new JsonContentSerializer())
            };

            return new AuthClient(authenticator, host, clientHandler, delegatingHandlers);
        }

        [ClassInitialize]
        public static void AuthClientTest_InitClass(TestContext context)
        {
            payload = JsonConvert.DeserializeObject<TestObject>(File.ReadAllText("TestData/payload.json"));
        }

        [TestMethod]
        public void AuthClientTest_AuthSucceeded()
        {
            // Setup
            var endpoint = "api/test_get";
            var authProvider = new Mock<IAuthenticator>();

            authProvider
                .Setup(p => p.GetAuthValue())
                .Returns(Task.FromResult("fakeAuthValue"));
            authProvider
                .Setup(p => p.EnhanceClientHandler(It.IsAny<HttpClientHandler>()))
                .Returns(new HttpClientHandler());

            var client = GetTestClient(authProvider.Object, endpoint, HttpStatusCode.OK, payload);

            // Exec
            var testObject = client.Get<TestObject>(endpoint);

            // Assert
            Assert.IsNotNull(testObject);
            authProvider.Verify(p => p.GetAuthValue(), Times.Once);
        }

        [TestMethod]
        public void AuthClientTest_GetAuthValueFailed()
        {
            // Setup
            var endpoint = "api/test_get";
            var authProvider = new Mock<IAuthenticator>();

            // Mock the get auth value call to be a bad request (any non-success code should work here)
            var authResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            authProvider
                .Setup(p => p.GetAuthValue())
                .ThrowsAsync(new RestfulClientException("Could not get token from auth provider", authResponse));
            authProvider
                .Setup(p => p.EnhanceClientHandler(It.IsAny<HttpClientHandler>()))
                .Returns(new HttpClientHandler());

            var client = GetTestClient(authProvider.Object, endpoint, HttpStatusCode.OK, payload);

            try
            {
                var testObject = client.Get<TestObject>(endpoint);

                // Fail the test if get did not throw an exception
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                // Top level exception is an RestfulClientException (Unauthorized) thrown by the AuthClient
                Assert.AreEqual(typeof(RestfulClientException), ex.InnerException.GetType());
                var authClientException = ex.InnerException as RestfulClientException;
                Assert.AreEqual(HttpStatusCode.Unauthorized, authClientException.StatusCode);

                // Inner exception should also be a RestfulClientException (thrown by the provider's GetAuthValue)
                Assert.AreEqual(typeof(RestfulClientException), authClientException.InnerException.GetType());
                var responseException = authClientException.InnerException as RestfulClientException;
                Assert.AreEqual(HttpStatusCode.BadRequest, responseException.StatusCode);
            }
            finally
            {
                authProvider.Verify(p => p.GetAuthValue(), Times.Once);

                // Because the auth value was never successfully obtained, the token should not have been cleared
                authProvider.Verify(p => p.ClearAuthValue(), Times.Never);
            }
        }

        [TestMethod]
        public void AuthClientTest_AuthFailure()
        {
            // Setup
            var endpoint = "api/test_get";
            var authProvider = new Mock<IAuthenticator>();

            authProvider
                .Setup(p => p.GetAuthValue())
                .Returns(Task.FromResult("fakeAuthValue"));
            authProvider
                .Setup(p => p.EnhanceClientHandler(It.IsAny<HttpClientHandler>()))
                .Returns(new HttpClientHandler());

            var client = GetTestClient(authProvider.Object, endpoint, HttpStatusCode.Unauthorized, null);

            try
            {
                var testObject = client.Get<TestObject>(endpoint);

                // Fail the test if get did not throw an exception
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                // Top level exception is an RestfulClientException re-thrown by the AuthClient
                Assert.AreEqual(typeof(RestfulClientException), ex.InnerException.GetType());
                var authClientException = ex.InnerException as RestfulClientException;
                Assert.AreEqual(HttpStatusCode.Unauthorized, authClientException.StatusCode);
            }
            finally
            {
                authProvider.Verify(p => p.GetAuthValue(), Times.Once);

                // Because the service call was unauthorized, the token should have been cleared
                authProvider.Verify(p => p.ClearAuthValue(), Times.Once);
            }
        }

        [TestMethod]
        public void AuthClientTest_GetFailure()
        {
            // Setup
            var endpoint = "api/test_get";
            var authProvider = new Mock<IAuthenticator>();

            authProvider
                .Setup(p => p.GetAuthValue())
                .Returns(Task.FromResult("fakeAuthValue"));
            authProvider
                .Setup(p => p.EnhanceClientHandler(It.IsAny<HttpClientHandler>()))
                .Returns(new HttpClientHandler());

            var client = GetTestClient(authProvider.Object, endpoint, HttpStatusCode.BadRequest, null);

            try
            {
                var testObject = client.Get<TestObject>(endpoint);

                // Fail the test if get did not throw an exception
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                // Top level exception is an RestfulClientException re-thrown by the AuthClient
                Assert.AreEqual(typeof(RestfulClientException), ex.InnerException.GetType());
                var authClientException = ex.InnerException as RestfulClientException;
                Assert.AreEqual(HttpStatusCode.BadRequest, authClientException.StatusCode);
            }
            finally
            {
                authProvider.Verify(p => p.GetAuthValue(), Times.Once);

                // Because the service call was failed for a reason other than unauthorized, the token should not have been cleared
                authProvider.Verify(p => p.ClearAuthValue(), Times.Never);
            }
        }
    }
}
