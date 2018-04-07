// <copyright file="JsonContentSerializerTests.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ProjectBlackmagic.RestfulClient.ContentSerialization;

namespace ProjectBlackmagic.RestfulClient.Test.ContentSerialization
{
    [TestClass]
    public class JsonContentSerializerTests
    {
        private static TestObject testObject;

        [ClassInitialize]
        public static void InitClass(TestContext context)
        {
            testObject = new TestObject()
            {
                Guid = default(Guid),
                IsActive = false,
                Name = "TestName",
                Tags = new List<string>() { "first", "second" },
                Friends = new List<TestSubObject>()
                {
                    new TestSubObject()
                    {
                        Id = 1,
                        Name = "TestSubName",
                    }
                }
            };
        }

        [TestMethod]
        public void Serialize_TestContentTypeHeader()
        {
            var contentSerializer = new JsonContentSerializer<TestObject>();
            var content = contentSerializer.Serialize(testObject);

            Assert.AreEqual("application/json", content.Headers.ContentType.MediaType);
        }

        [TestMethod]
        public void Serialize_TestSerializedContentType()
        {
            var contentSerializer = new JsonContentSerializer<TestObject>();
            var content = contentSerializer.Serialize(testObject);

            Assert.IsInstanceOfType(content, typeof(StringContent));
        }

        [TestMethod]
        public void Serialize_TestNullContent()
        {
            var contentSerializer = new JsonContentSerializer<TestObject>();

            try
            {
                contentSerializer.Serialize(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("content", ex.ParamName);
            }
        }

        [TestMethod]
        public async Task Serialize_TestEmptyObjectContent()
        {
            var contentSerializer = new JsonContentSerializer<TestObject>();
            var content = contentSerializer.Serialize(new TestObject());
            var contentString = await content.ReadAsStringAsync();

            Assert.AreEqual("{\"Guid\":\"00000000-0000-0000-0000-000000000000\",\"IsActive\":false,\"Age\":0}", contentString);
        }

        [TestMethod]
        public async Task Serialize_TestSimpleContent()
        {
            var contentSerializer = new JsonContentSerializer<TestObject>();
            var content = contentSerializer.Serialize(testObject);
            var contentString = await content.ReadAsStringAsync();

            Assert.AreEqual("{\"Guid\":\"00000000-0000-0000-0000-000000000000\",\"IsActive\":false,\"Age\":0,\"Name\":\"TestName\",\"Tags\":[\"first\",\"second\"],\"Friends\":[{\"Id\":1,\"Name\":\"TestSubName\"}]}", contentString);
        }

        [TestMethod]
        public async Task Serialize_TestCustomSerializerSettings()
        {
            var customSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include,
            };

            var contentSerializer = new JsonContentSerializer<TestObject>(customSettings);
            var content = contentSerializer.Serialize(testObject);
            var contentString = await content.ReadAsStringAsync();

            Assert.AreEqual("{\"Guid\":\"00000000-0000-0000-0000-000000000000\",\"IsActive\":false,\"Age\":0,\"EyeColor\":null,\"Name\":\"TestName\",\"Gender\":null,\"Email\":null,\"About\":null,\"Tags\":[\"first\",\"second\"],\"Friends\":[{\"Id\":1,\"Name\":\"TestSubName\"}]}", contentString);
        }
    }
}
