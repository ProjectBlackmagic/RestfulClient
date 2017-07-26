// <copyright file="BaseContentSerializer.cs" company="ProjectBlackmagic">
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
    /// Base implementatation of <see cref="IRestfulContentSerializer"/>.
    /// </summary>
    public abstract class BaseContentSerializer : IRestfulContentSerializer
    {
        /// <inheritdoc/>
        public abstract HttpContent Serialize(object content);

        /// <inheritdoc/>
        public Task<string> Deserialize(HttpResponseMessage responseMessage)
        {
            return responseMessage.Content?.ReadAsStringAsync();
        }

        /// <inheritdoc/>
        public abstract Task<T> Deserialize<T>(HttpResponseMessage responseMessage);
    }
}
