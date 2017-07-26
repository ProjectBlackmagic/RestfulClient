// <copyright file="IRestfulContentSerializer.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectBlackmagic.RestfulClient.Content
{
    /// <summary>
    /// Interface for restful content serialization and deserialization.
    /// </summary>
    public interface IRestfulContentSerializer
    {
        /// <summary>
        /// Serializes the content into HttpContent
        /// </summary>
        /// <param name="content">Content to serialize</param>
        /// <returns>Instance of HttpContent to use with request</returns>
        HttpContent Serialize(object content);

        /// <summary>
        /// Deserializes the response content.
        /// </summary>
        /// <param name="responseMessage">The http response message</param>
        /// <returns>Stringified response content</returns>
        Task<string> Deserialize(HttpResponseMessage responseMessage);

        /// <summary>
        /// Deserializes the response content.
        /// </summary>
        /// <typeparam name="T">Target type for content destructuring</typeparam>
        /// <param name="responseMessage">The http response message</param>
        /// <returns>Instance of T</returns>
        Task<T> Deserialize<T>(HttpResponseMessage responseMessage);
    }
}
