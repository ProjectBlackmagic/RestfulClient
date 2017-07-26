// <copyright file="TestObject.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ProjectBlackmagic.RestfulClient.Test
{
    public class TestObject
    {
        public Guid Guid { get; set; }

        public bool IsActive { get; set; }

        public int Age { get; set; }

        public string EyeColor { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; }

        public string Email { get; set; }

        public string About { get; set; }

        public List<string> Tags { get; set; }

        public List<TestSubObject> Friends { get; set; }
    }

    public class TestSubObject
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
