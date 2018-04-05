// <copyright file="RpsAuthenticatorTests.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ProjectBlackmagic.RestfulClient.Authentication;
using ProjectBlackmagic.RestfulClient.Authentication.Rps;

namespace ProjectBlackmagic.RestfulClient.Test.Authentication.Rps
{
    [TestClass]
    [DeploymentItem("TestData/payload.json")]
    [DeploymentItem("TestData/rpsPayloadSuccess.json")]
    [DeploymentItem("TestData/rpsPayloadError.json")]
    public class RpsAuthenticatorTests
    {
        private static RpsToken rpsPayloadSuccess;
        private static string rpsPayloadError;
        private static TestObject payload;

        private IAuthenticator GetTestAuthenticator(HttpStatusCode rpsStatusCode, object rpsPayload)
        {
            var authHost = "http://fakeAuthUrl";
            var config = new RpsConfig(authHost, "fakeSiteID", "fakeScope", "fakePolicy", new X509Certificate2());

            var rpsMessageHandler = new HttpClientHandler();
            var rpsDelegatingHandlers = new DelegatingHandler[]
            {
                FakeResponseHandler.Create(rpsStatusCode, authHost, rpsPayload)
            };
            return new RpsAuthenticator(config, rpsMessageHandler, rpsDelegatingHandlers);
        }

        [ClassInitialize]
        public static void RpsTest_InitClass(TestContext context)
        {
            rpsPayloadSuccess = JsonConvert.DeserializeObject<RpsToken>(File.ReadAllText("TestData/rpsPayloadSuccess.json"));
            rpsPayloadError = File.ReadAllText("TestData/rpsPayloadError.json");
            payload = JsonConvert.DeserializeObject<TestObject>(File.ReadAllText("TestData/payload.json"));
        }

        [TestMethod]
        public void RpsAuthProviderTest_AuthSucceeded()
        {
            // Setup
            var authProvider = GetTestAuthenticator(HttpStatusCode.OK, rpsPayloadSuccess);

            // Exec
            var authValue = authProvider.GetAuthValue().Result;

            // Assert
            Assert.AreEqual($"app_ticket=\"{rpsPayloadSuccess.AccessToken}\"", authValue);
        }

        [TestMethod]
        public void RpsAuthProviderTest_AuthFailed()
        {
            // Setup
            var authProvider = GetTestAuthenticator(HttpStatusCode.BadRequest, rpsPayloadError);

            try
            {
                var authValue = authProvider.GetAuthValue().Result;

                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(typeof(RestfulClientException), ex.InnerException.GetType());

                var responseException = ex.InnerException as RestfulClientException;
                Assert.AreEqual(responseException.StatusCode, HttpStatusCode.BadRequest);
            }
        }
    }
}