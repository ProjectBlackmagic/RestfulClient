// <copyright file="JsonContent.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ProjectBlackmagic.RestfulClient.Content
{
    /// <summary>
    /// JsonContent defines a subtype of <see cref="StringContent"/> for JSON content.
    /// </summary>
    /// <typeparam name="T">Type of object being serialized</typeparam>
    public class JsonContent<T> : StringContent
    {
        /// <summary>
        /// Defines the string encoding for the content
        /// </summary>
        public static readonly Encoding StringEncoding = Encoding.UTF8;

        /// <summary>
        /// Defines the media type for the content
        /// </summary>
        public static readonly string MediaType = "application/json";

        private static readonly JsonSerializerSettings DefaultSerializerSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContent{T}"/> class.
        /// </summary>
        /// <param name="content">Content of type <typeparamref name="T"/></param>
        public JsonContent(T content)
            : this(content, DefaultSerializerSettings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContent{T}"/> class.
        /// </summary>
        /// <param name="content">Content of type <typeparamref name="T"/></param>
        /// <param name="serializerSettings">Serialization settings</param>
        public JsonContent(T content, JsonSerializerSettings serializerSettings)
            : base(Serialize(content, serializerSettings), StringEncoding, MediaType)
        {
        }

        private static string Serialize(T content, JsonSerializerSettings serializerSettings)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            return JsonConvert.SerializeObject(content, serializerSettings);
        }
    }
}
