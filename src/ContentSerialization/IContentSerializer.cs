// <copyright file="IContentSerializer.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Net.Http;

namespace ProjectBlackmagic.RestfulClient.ContentSerialization
{
    /// <summary>
    /// IContentSerializer interface defines how to serialize and deserialize content.
    /// </summary>
    /// <typeparam name="T">Type of content being serialized</typeparam>
    public interface IContentSerializer<T>
    {
        /// <summary>
        /// Serializes content of type <typeparamref name="T"/> to type <see cref="HttpContent"/>.
        /// </summary>
        /// <param name="content">Content</param>
        /// <returns>Deserialized content</returns>
        HttpContent Serialize(T content);
    }
}
