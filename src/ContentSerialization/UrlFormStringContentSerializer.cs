// <copyright file="UrlFormStringContentSerializer.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Net.Http;
using System.Net.Http.Headers;

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
            var httpContent = new StringContent(content);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            return httpContent;
        }
    }
}
