// <copyright file="FormUrlEncodedStringContentTests.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectBlackmagic.RestfulClient.Content;

namespace ProjectBlackmagic.RestfulClient.Test.Content
{
    [TestClass]
    public class FormUrlEncodedStringContentTests
    {
        [TestMethod]
        public void Content_TestMediaTypeHeader()
        {
            var content = new FormUrlEncodedStringContent("testContent");

            Assert.AreEqual("application/x-www-form-urlencoded", content.Headers.ContentType.MediaType);
        }

        [TestMethod]
        public void Content_TestNull()
        {
            try
            {
                new FormUrlEncodedStringContent(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("content", ex.ParamName);
            }
        }

        [TestMethod]
        public async Task Content_TestWhitespace()
        {
            var content = new FormUrlEncodedStringContent("    ");
            var contentString = await content.ReadAsStringAsync();

            Assert.AreEqual("    ", contentString);
        }

        [TestMethod]
        public async Task Content_TestSimpleString()
        {
            var content = new FormUrlEncodedStringContent("testContent");
            var contentString = await content.ReadAsStringAsync();

            Assert.AreEqual("testContent", contentString);
        }
    }
}
