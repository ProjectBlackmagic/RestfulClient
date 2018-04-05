// <copyright file="AuthClientTests.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using ProjectBlackmagic.RestfulClient.Authentication;

namespace ProjectBlackmagic.RestfulClient.Test.Authentication
{
    [TestClass]
    [DeploymentItem("TestData/payload.json")]
    public class AuthClientTests
    {
        private static string endpoint = "http://fakeEndpoint/test";
        private static TestObject payload;

        private AuthClient GetTestClient(IAuthenticator authenticator, string endpoint, HttpStatusCode httpStatusCode, object httpPayload = null)
        {
            var host = "http://fakeEndpoint";
            var clientHandler = new HttpClientHandler();
            var delegatingHandlers = new DelegatingHandler[]
            {
                FakeResponseHandler.Create(httpStatusCode, host + "/" + endpoint, httpPayload)
            };

            return new AuthClient(authenticator, host, clientHandler, delegatingHandlers);
        }

        [ClassInitialize]
        public static void AuthClientTest_InitClass(TestContext context)
        {
            payload = JsonConvert.DeserializeObject<TestObject>(File.ReadAllText("TestData/payload.json"));
        }

        [TestMethod]
        public async Task GetAsync_TestRequestAndResponse()
        {
            var authenticatorMock = new Mock<IAuthenticator>();

            authenticatorMock
                .SetupGet(p => p.Scheme)
                .Returns("fakeScheme");
            authenticatorMock
                .Setup(p => p.GetAuthValue())
                .ReturnsAsync("fakeAuthValue");
            authenticatorMock
                .Setup(p => p.EnhanceClientHandler(It.IsAny<HttpClientHandler>()))
                .Returns(new HttpClientHandler());

            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.OK, endpoint, payload);
            var client = new AuthClient(authenticatorMock.Object, endpoint, new HttpClientHandler(), messageHandler);

            var testObject = await client.GetAsync<TestObject>(endpoint);

            Assert.IsNotNull(testObject);
        }

        [TestMethod]
        public async Task GetAsync_TestAuthorizationHeaderSetup()
        {
            var authenticatorMock = new Mock<IAuthenticator>();

            authenticatorMock
                .SetupGet(p => p.Scheme)
                .Returns("fakeScheme");
            authenticatorMock
                .Setup(p => p.GetAuthValue())
                .ReturnsAsync("fakeAuthValue");
            authenticatorMock
                .Setup(p => p.EnhanceClientHandler(It.IsAny<HttpClientHandler>()))
                .Returns(new HttpClientHandler());

            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.OK, endpoint, payload);
            var client = new AuthClient(authenticatorMock.Object, endpoint, new HttpClientHandler(), messageHandler);

            var testObject = await client.GetAsync<TestObject>(endpoint);

            Assert.AreEqual(1, messageHandler.Requests.Count);
            var request = messageHandler.Requests.FirstOrDefault();

            Assert.AreEqual("fakeScheme", request.Headers.Authorization.Scheme);
            Assert.AreEqual("fakeAuthValue", request.Headers.Authorization.Parameter);

            authenticatorMock.Verify(p => p.EnhanceClientHandler(It.IsAny<HttpClientHandler>()), Times.Once);
            authenticatorMock.VerifyGet(p => p.Scheme, Times.Once);
            authenticatorMock.Verify(p => p.GetAuthValue(), Times.Once);
        }

        [TestMethod]
        public async Task GetAsync_TestAuthorizationFailed()
        {
            var authValueException = new Exception("Test auth token error");
            var authenticatorMock = new Mock<IAuthenticator>();

            authenticatorMock
                .Setup(p => p.GetAuthValue())
                .ThrowsAsync(authValueException);
            authenticatorMock
                .Setup(p => p.EnhanceClientHandler(It.IsAny<HttpClientHandler>()))
                .Returns(new HttpClientHandler());

            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.OK, endpoint, payload);
            var client = new AuthClient(authenticatorMock.Object, endpoint, new HttpClientHandler(), messageHandler);

            try
            {
                await client.GetAsync<TestObject>(endpoint);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(RestfulClientException));
                var restfulException = ex as RestfulClientException;

                Assert.IsTrue(restfulException.Message.Contains("AuthClient could not obtain and/or add the authentication token"));
                Assert.AreEqual(HttpStatusCode.Unauthorized, restfulException.StatusCode);
                Assert.AreSame(authValueException, restfulException.InnerException);
            }
            finally
            {
                authenticatorMock.Verify(a => a.GetAuthValue(), Times.Once);
                authenticatorMock.Verify(p => p.ClearAuthValue(), Times.Never);
            }
        }

        [TestMethod]
        public async Task GetAsync_TestRequestUnauthorized()
        {
            var authenticatorMock = new Mock<IAuthenticator>();

            authenticatorMock
                .SetupGet(p => p.Scheme)
                .Returns("fakeScheme");
            authenticatorMock
                .Setup(p => p.GetAuthValue())
                .ReturnsAsync("fakeAuthValue");
            authenticatorMock
                .Setup(p => p.EnhanceClientHandler(It.IsAny<HttpClientHandler>()))
                .Returns(new HttpClientHandler());

            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.Unauthorized, endpoint, payload);
            var client = new AuthClient(authenticatorMock.Object, endpoint, new HttpClientHandler(), messageHandler);

            try
            {
                await client.GetAsync<TestObject>(endpoint);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(RestfulClientException));
            }
            finally
            {
                authenticatorMock.Verify(a => a.GetAuthValue(), Times.Once);
                authenticatorMock.Verify(p => p.ClearAuthValue(), Times.Once);
            }
        }

        [TestMethod]
        public async Task GetAsync_TestRequestFailed()
        {
            var authenticatorMock = new Mock<IAuthenticator>();

            authenticatorMock
                .SetupGet(p => p.Scheme)
                .Returns("fakeScheme");
            authenticatorMock
                .Setup(p => p.GetAuthValue())
                .ReturnsAsync("fakeAuthValue");
            authenticatorMock
                .Setup(p => p.EnhanceClientHandler(It.IsAny<HttpClientHandler>()))
                .Returns(new HttpClientHandler());

            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.NotFound, endpoint, payload);
            var client = new AuthClient(authenticatorMock.Object, endpoint, new HttpClientHandler(), messageHandler);

            try
            {
                await client.GetAsync<TestObject>(endpoint);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(RestfulClientException));
            }
            finally
            {
                authenticatorMock.Verify(a => a.GetAuthValue(), Times.Once);
                authenticatorMock.Verify(p => p.ClearAuthValue(), Times.Never);
            }
        }
    }
}
