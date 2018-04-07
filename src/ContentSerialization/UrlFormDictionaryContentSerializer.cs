// <copyright file="UrlFormDictionaryContentSerializer.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Collections.Generic;
using System.Net.Http;

namespace ProjectBlackmagic.RestfulClient.ContentSerialization
{
    /// <summary>
    /// UrlFormDictionaryContentSerializer defines how to serialize and deserialize form urlencoded content from a dictionary.
    /// </summary>
    public class UrlFormDictionaryContentSerializer : IContentSerializer<Dictionary<string, string>>
    {
        /// <inheritdoc/>
        public HttpContent Serialize(Dictionary<string, string> content)
        {
            return new FormUrlEncodedContent(content);
        }
    }
}
