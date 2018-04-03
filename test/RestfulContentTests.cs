// <copyright file="RestfulContentTests.cs" company="ProjectBlackmagic">
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

namespace ProjectBlackmagic.RestfulClient.Test
{
    [TestClass]
    public class RestfulContentTests
    {
        [TestMethod]
        public async Task GetHttpContent_TestJSONContent()
        {
            var testObject = new TestObject()
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

            var content = RestfulContent.GetHttpContent(testObject);
            var contentString = await content.ReadAsStringAsync();

            Assert.IsInstanceOfType(content, typeof(StringContent));
            Assert.AreEqual("{\"Guid\":\"00000000-0000-0000-0000-000000000000\",\"IsActive\":false,\"Age\":0,\"Name\":\"TestName\",\"Tags\":[\"first\",\"second\"],\"Friends\":[{\"Id\":1,\"Name\":\"TestSubName\"}]}", contentString);

            Assert.AreEqual("application/json", content.Headers.ContentType.MediaType);
        }

        [TestMethod]
        public async Task GetHttpContent_TestUrlEncodedFormContentString()
        {
            var content = RestfulContent.GetHttpContent("testContent", ContentType.UrlEncodedForm);
            var contentString = await content.ReadAsStringAsync();

            Assert.IsInstanceOfType(content, typeof(StringContent));
            Assert.AreEqual(contentString, "testContent");

            Assert.AreEqual("application/x-www-form-urlencoded", content.Headers.ContentType.MediaType);
        }

        [TestMethod]
        public async Task GetHttpContent_TestUrlEncodedFormContentDictionary()
        {
            var testDict = new Dictionary<string, string>()
            {
                { "testKey", "testValue" },
                { "otherKey", "otherValue" }
            };

            var content = RestfulContent.GetHttpContent(testDict, ContentType.UrlEncodedForm);
            var contentString = await content.ReadAsStringAsync();

            Assert.IsInstanceOfType(content, typeof(FormUrlEncodedContent));
            Assert.AreEqual(contentString, "testKey=testValue&otherKey=otherValue");

            Assert.AreEqual("application/x-www-form-urlencoded", content.Headers.ContentType.MediaType);
        }

        [TestMethod]
        public void GetHttpContent_TestUrlEncodedFormContentInvalid()
        {
            var invalidContent = new List<string>();

            try
            {
                RestfulContent.GetHttpContent(invalidContent, ContentType.UrlEncodedForm);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(Exception));
                Assert.AreEqual("This content not supported for type UrlEncodedForm", ex.Message);
            }
        }
    }
}
