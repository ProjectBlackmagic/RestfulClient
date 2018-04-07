// <copyright file="UrlFormStringContentSerializerTests.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectBlackmagic.RestfulClient.ContentSerialization;

namespace ProjectBlackmagic.RestfulClient.Test.ContentSerialization
{
    [TestClass]
    public class UrlFormStringContentSerializerTests
    {
        [TestMethod]
        public void Serialize_TestContentTypeHeader()
        {
            var contentSerializer = new UrlFormStringContentSerializer();
            var content = contentSerializer.Serialize("testContent");

            Assert.AreEqual("application/x-www-form-urlencoded", content.Headers.ContentType.MediaType);
        }

        [TestMethod]
        public void Serialize_TestSerializedContentType()
        {
            var contentSerializer = new UrlFormStringContentSerializer();
            var content = contentSerializer.Serialize("testContent");

            Assert.IsInstanceOfType(content, typeof(StringContent));
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
        public async Task Serialize_TestWhitespaceContent()
        {
            var contentSerializer = new UrlFormStringContentSerializer();
            var content = contentSerializer.Serialize("    ");
            var contentString = await content.ReadAsStringAsync();

            Assert.AreEqual(string.Empty, contentString);
        }

        [TestMethod]
        public async Task Serialize_TestSimpleContent()
        {
            var contentSerializer = new UrlFormStringContentSerializer();
            var content = contentSerializer.Serialize("testContent");
            var contentString = await content.ReadAsStringAsync();

            Assert.AreEqual("testContent", contentString);
        }
    }
}
