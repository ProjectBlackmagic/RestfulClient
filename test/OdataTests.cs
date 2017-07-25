// <copyright file="OdataTests.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectBlackmagic.RestfulClient.Test
{
    [TestClass]
    public class OdataTests
    {
        private class Person
        {
            public string Name { get; set; }

            public string LastName { get; set; }

            public int Age { get; set; }
        }

        [TestMethod]
        public void OdataTest_Simple()
        {
            var collection = new Odata.OdataCollection<Person>();
            var list = collection.ToList();
        }
    }
}
