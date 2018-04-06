// <copyright file="JsonContentTests.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ProjectBlackmagic.RestfulClient.Content;

namespace ProjectBlackmagic.RestfulClient.Test.Content
{
    [TestClass]
    public class JsonContentTests
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
        public void Content_TestMediaTypeHeader()
        {
            var content = new JsonContent<TestObject>(testObject);

            Assert.AreEqual("application/json", content.Headers.ContentType.MediaType);
        }

        [TestMethod]
        public void Content_TestNull()
        {
            try
            {
                new JsonContent<TestObject>(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("content", ex.ParamName);
            }
        }

        [TestMethod]
        public async Task Content_TestEmptyObject()
        {
            var content = new JsonContent<TestObject>(new TestObject());
            var contentString = await content.ReadAsStringAsync();

            Assert.AreEqual("{\"Guid\":\"00000000-0000-0000-0000-000000000000\",\"IsActive\":false,\"Age\":0}", contentString);
        }

        [TestMethod]
        public async Task Content_TestSimpleObject()
        {
            var content = new JsonContent<TestObject>(testObject);
            var contentString = await content.ReadAsStringAsync();

            Assert.AreEqual("{\"Guid\":\"00000000-0000-0000-0000-000000000000\",\"IsActive\":false,\"Age\":0,\"Name\":\"TestName\",\"Tags\":[\"first\",\"second\"],\"Friends\":[{\"Id\":1,\"Name\":\"TestSubName\"}]}", contentString);
        }

        [TestMethod]
        public async Task Content_TestCustomSerializerSettings()
        {
            var customSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Include,
            };

            var content = new JsonContent<TestObject>(testObject, customSettings);
            var contentString = await content.ReadAsStringAsync();

            Assert.AreEqual("{\"Guid\":\"00000000-0000-0000-0000-000000000000\",\"IsActive\":false,\"Age\":0,\"EyeColor\":null,\"Name\":\"TestName\",\"Gender\":null,\"Email\":null,\"About\":null,\"Tags\":[\"first\",\"second\"],\"Friends\":[{\"Id\":1,\"Name\":\"TestSubName\"}]}", contentString);
        }
    }
}
