// <copyright file="UrlFormDictionaryContentSerializerTests.cs" company="ProjectBlackmagic">
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
using ProjectBlackmagic.RestfulClient.ContentSerialization;

namespace ProjectBlackmagic.RestfulClient.Test.ContentSerialization
{
    [TestClass]
    public class UrlFormDictionaryContentSerializerTests
    {
        private static Dictionary<string, string> testDictionary;

        [ClassInitialize]
        public static void InitClass(TestContext context)
        {
            testDictionary = new Dictionary<string, string>()
            {
                { "testKey", "testValue" },
                { "otherKey", "otherValue" }
            };
        }

        [TestMethod]
        public void Serialize_TestContentTypeHeader()
        {
            var contentSerializer = new UrlFormDictionaryContentSerializer();
            var content = contentSerializer.Serialize(testDictionary);

            Assert.AreEqual("application/x-www-form-urlencoded", content.Headers.ContentType.MediaType);
        }

        [TestMethod]
        public void Serialize_TestSerializedContentType()
        {
            var contentSerializer = new UrlFormDictionaryContentSerializer();
            var content = contentSerializer.Serialize(testDictionary);

            Assert.IsInstanceOfType(content, typeof(FormUrlEncodedContent));
        }

        [TestMethod]
        public void Serialize_TestNullContent()
        {
            var contentSerializer = new UrlFormStringContentSerializer();

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
        public async Task Serialize_TestSimpleContent()
        {
            var contentSerializer = new UrlFormDictionaryContentSerializer();
            var content = contentSerializer.Serialize(testDictionary);
            var contentString = await content.ReadAsStringAsync();

            Assert.AreEqual("testKey=testValue&otherKey=otherValue", contentString);
        }
    }
}
