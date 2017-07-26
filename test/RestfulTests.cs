// <copyright file="RestfulTests.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ProjectBlackmagic.RestfulClient.Content;

namespace ProjectBlackmagic.RestfulClient.Test
{
    [TestClass]
    public class RestfulTests
    {
        private static TestObject payload;

        private RestfulClient TestClient(string endpoint, HttpStatusCode statusCode, object payload = null)
        {
            var host = "http://fakeEndpoint";
            var messageHandler = FakeResponseHandler.Create(statusCode, host + "/" + endpoint, payload, new JsonContentSerializer());

            return new RestfulClient(host, messageHandler);
        }

        [ClassInitialize]
        public static void RestfulTest_InitClass(TestContext context)
        {
            payload = JsonConvert.DeserializeObject<TestObject>(File.ReadAllText("TestData/payload.json"));
        }

        [TestMethod]
        public void RestfulTest_Get()
        {
            var endpoint = "api/test_get";

            var client = TestClient(endpoint, HttpStatusCode.OK, payload);

            var testObject = client.Get<TestObject>(endpoint);
            Assert.IsNotNull(testObject);
        }

        [TestMethod]
        public void RestfulTest_Put()
        {
            var endpoint = "api/test_put";

            var client = TestClient(endpoint, HttpStatusCode.OK, payload);

            var testObject = client.Put<TestObject>(endpoint, payload);
            Assert.IsNotNull(testObject);
        }

        [TestMethod]
        public void RestfulTest_Post()
        {
            var endpoint = "api/test_post";

            var client = TestClient(endpoint, HttpStatusCode.OK, payload);

            var testObject = client.Post<TestObject>(endpoint, payload);
            Assert.IsNotNull(testObject);
        }

        [TestMethod]
        public void RestfulTest_Delete()
        {
            var endpoint = "api/test_delete";

            var client = TestClient(endpoint, HttpStatusCode.OK);

            var emptyResult = client.Delete<Empty>(endpoint);
            Assert.IsNotNull(emptyResult);
        }

        [TestMethod]
        public void RestfulTest_NotOK_Empty()
        {
            var endpoint = "api/test_not_ok_empty";

            var client = TestClient(endpoint, HttpStatusCode.NotFound);

            try
            {
                var emptyResult = client.Delete<Empty>(endpoint);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(typeof(RestfulClientException), ex.InnerException.GetType());
                var clientEx = ex.InnerException as RestfulClientException;
                Assert.AreEqual(HttpStatusCode.NotFound, clientEx.StatusCode);
            }
        }

        [TestMethod]
        public void RestfulTest_NotOK_Object()
        {
            var endpoint = "api/test_not_ok_object";

            var client = TestClient(endpoint, HttpStatusCode.NotFound, payload);

            try
            {
                var emptyResult = client.Delete<TestObject>(endpoint);
            }
            catch (AggregateException ex)
            {
                Assert.AreEqual(typeof(RestfulClientException), ex.InnerException.GetType());
                var clientEx = ex.InnerException as RestfulClientException;
                Assert.AreEqual(HttpStatusCode.NotFound, clientEx.StatusCode);
            }
        }
    }
}
