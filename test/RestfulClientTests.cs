// <copyright file="RestfulClientTests.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace ProjectBlackmagic.RestfulClient.Test
{
    [TestClass]
    [DeploymentItem("TestData/payload.json")]
    public class RestfulClientTests
    {
        private static string endpoint = "http://fakeEndpoint/test";
        private static KeyValuePair<string, string> testHeader = new KeyValuePair<string, string>("TestHeader", "TestValue");
        private static TestObject payload;

        [ClassInitialize]
        public static void InitClass(TestContext context)
        {
            payload = JsonConvert.DeserializeObject<TestObject>(File.ReadAllText("TestData/payload.json"));
        }

        [TestMethod]
        public async Task GetAsync_TestRequestAndResponse()
        {
            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.OK, endpoint, payload);
            var client = new RestfulClient(endpoint, messageHandler);

            var testObject = await client.GetAsync<TestObject>(endpoint);

            Assert.IsNotNull(testObject);
            Assert.AreEqual(1, messageHandler.Requests.Count);

            var request = messageHandler.Requests.FirstOrDefault();

            Assert.AreEqual(HttpMethod.Get, request.Method);
            Assert.AreEqual(new Uri(endpoint), request.RequestUri);
            Assert.IsNull(request.Content);
        }

        [TestMethod]
        public async Task PostAsync_TestRequestAndResponse()
        {
            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.OK, endpoint, payload);
            var client = new RestfulClient(endpoint, messageHandler);

            var testObject = await client.PostAsync<TestObject>(endpoint, payload);

            Assert.IsNotNull(testObject);
            Assert.AreEqual(1, messageHandler.Requests.Count);

            var request = messageHandler.Requests.FirstOrDefault();

            Assert.AreEqual(HttpMethod.Post, request.Method);
            Assert.AreEqual(new Uri(endpoint), request.RequestUri);
            Assert.IsNotNull(request.Content);
        }

        [TestMethod]
        public async Task PutAsync_TestRequestAndResponse()
        {
            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.OK, endpoint, payload);
            var client = new RestfulClient(endpoint, messageHandler);

            var testObject = await client.PutAsync<TestObject>(endpoint, payload);

            Assert.IsNotNull(testObject);
            Assert.AreEqual(1, messageHandler.Requests.Count);

            var request = messageHandler.Requests.FirstOrDefault();

            Assert.AreEqual(HttpMethod.Put, request.Method);
            Assert.AreEqual(new Uri(endpoint), request.RequestUri);
            Assert.IsNotNull(request.Content);
        }

        [TestMethod]
        public async Task PatchAsync_TestRequestAndResponse()
        {
            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.OK, endpoint, payload);
            var client = new RestfulClient(endpoint, messageHandler);

            var testObject = await client.PatchAsync<TestObject>(endpoint, payload);

            Assert.IsNotNull(testObject);
            Assert.AreEqual(1, messageHandler.Requests.Count);

            var request = messageHandler.Requests.FirstOrDefault();

            Assert.AreEqual(new HttpMethod("PATCH"), request.Method);
            Assert.AreEqual(new Uri(endpoint), request.RequestUri);
            Assert.IsNotNull(request.Content);
        }

        [TestMethod]
        public async Task DeleteAsync_TestRequestAndResponse()
        {
            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.OK, endpoint, payload);
            var client = new RestfulClient(endpoint, messageHandler);

            var testObject = await client.DeleteAsync<TestObject>(endpoint);

            Assert.IsNotNull(testObject);
            Assert.AreEqual(1, messageHandler.Requests.Count);

            var request = messageHandler.Requests.FirstOrDefault();

            Assert.AreEqual(HttpMethod.Delete, request.Method);
            Assert.AreEqual(new Uri(endpoint), request.RequestUri);
            Assert.IsNull(request.Content);
        }

        [TestMethod]
        public async Task GetAsync_TestAdditionalRequestHeaders()
        {
            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.OK, endpoint, payload);
            var client = new RestfulClient(endpoint, messageHandler);
            var headers = new Dictionary<string, string>()
            {
                { testHeader.Key, testHeader.Value }
            };

            var testObject = await client.GetAsync<TestObject>(endpoint, headers);

            var request = messageHandler.Requests.FirstOrDefault();

            Assert.IsTrue(request.Headers.Contains(testHeader.Key));

            var testHeaderValues = request.Headers.GetValues(testHeader.Key);
            Assert.AreEqual(1, testHeaderValues.Count());
            Assert.AreEqual(testHeader.Value, testHeaderValues.FirstOrDefault());
        }

        // TO-DO: In practice, RestfulClient fails with null header value
        /*
        [TestMethod]
        public async Task GetAsync_TestMisconfiguredRequestHeaders()
        {
            var client = new RestfulClient(endpoint);
            var headers = new Dictionary<string, string>()
            {
                { testHeader.Key, null }
            };
            try
            {
                var testObject = await client.GetAsync<TestObject>(endpoint, headers);
                Assert.Fail();
            }
            catch (RestfulClientException ex)
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, ex.StatusCode);
                Assert.AreEqual($"Error adding header \"{ testHeader.Key }\" with value \"{ null }\" to outgoing request to \"{ endpoint }\". Check for null-value.", ex.Message);
            }
        }
        */

        [TestMethod]
        public async Task GetAsync_TestEmptyResponse()
        {
            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.OK, endpoint);
            var client = new RestfulClient(endpoint, messageHandler);

            var testObject = await client.GetAsync<Empty>(endpoint);

            Assert.IsTrue(testObject.IsSuccessStatusCode);
            Assert.AreEqual("OK", testObject.ReasonPhrase);
            Assert.AreEqual(HttpStatusCode.OK, testObject.StatusCode);
        }

        [TestMethod]
        public async Task GetAsync_TestDefaultResponse()
        {
            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.OK, endpoint, null);
            var client = new RestfulClient(endpoint, messageHandler);

            var testObject = await client.GetAsync<TestObject>(endpoint);

            Assert.AreEqual(default(TestObject), testObject);
        }

        [TestMethod]
        public async Task GetAsync_TestRequestFailure()
        {
            var messageHandler = FakeResponseHandler.Create(HttpStatusCode.NotFound, endpoint);
            var client = new RestfulClient(endpoint, messageHandler);

            try
            {
                await client.GetAsync<TestObject>(endpoint);
            }
            catch (RestfulClientException ex)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, ex.StatusCode);
                Assert.IsNull(ex.ResponseContent);
                Assert.IsTrue(ex.Message.Contains("RestfulClient received unsuccessful response"));
            }
        }
    }
}