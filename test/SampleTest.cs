// <copyright file="SampleTest.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectBlackmagic.RestfulClient.Test
{
    public class Post
    {
        public int UserId { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }

    [TestClass]
    public class SampleTest
    {
        [TestMethod]
        public void First()
        {
            Assert.IsTrue(true);

            var client = new RestfulClient("https://jsonplaceholder.typicode.com/");

            var result = client.Get<List<Post>>("posts");

            Assert.AreEqual(100, result.Count);
        }
    }
}