// <copyright file="JsonContentSerializer.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace ProjectBlackmagic.RestfulClient.ContentSerialization
{
    /// <summary>
    /// JsonContentSerializer defines how to serialize and deserialize JSON content from a class object.
    /// </summary>
    /// <typeparam name="T">Type of content being serialized</typeparam>
    public class JsonContentSerializer<T> : IContentSerializer<T>
        where T : class
    {
        private readonly JsonSerializerSettings serializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContentSerializer{T}"/> class.
        /// Base constructor.
        /// </summary>
        public JsonContentSerializer()
        {
            this.serializerSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContentSerializer{T}"/> class.
        /// Constructor with custom serializer settings.
        /// </summary>
        /// /// <param name="serializerSettings">Custom serializer settings.</param>
        public JsonContentSerializer(JsonSerializerSettings serializerSettings)
        {
            this.serializerSettings = serializerSettings ?? throw new ArgumentNullException("serializerSettings");
        }

        /// <inheritdoc/>
        public HttpContent Serialize(T content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            var serializedObject = JsonConvert.SerializeObject(content, this.serializerSettings);

            return new StringContent(serializedObject, Encoding.UTF8, "application/json");
        }
    }
}
