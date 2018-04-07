// <copyright file="UrlFormStringContentSerializer.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBlackmagic.RestfulClient.ContentSerialization
{
    /// <summary>
    /// UrlFormStringContentSerializer defines how to serialize and deserialize form urlencoded content from a string.
    /// </summary>
    public class UrlFormStringContentSerializer : IContentSerializer<string>
    {
        /// <inheritdoc/>
        public HttpContent Serialize(string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            var normalizedContent = string.IsNullOrWhiteSpace(content)
                ? string.Empty
                : content;

            return new StringContent(normalizedContent, Encoding.UTF8, "application/x-www-form-urlencoded");
        }

        /// <inheritdoc/>
        public Task<string> Deserialize(HttpContent content)
        {
            throw new NotImplementedException();
        }
    }
}
