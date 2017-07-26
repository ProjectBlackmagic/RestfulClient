// <copyright file="FormContentSerializer.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectBlackmagic.RestfulClient.Content
{
    /// <summary>
    /// Serializes and deserializes form url encoded content.
    /// </summary>
    public class FormContentSerializer : BaseContentSerializer, IRestfulContentSerializer
    {
        /// <inheritdoc/>
        public override HttpContent Serialize(object content)
        {
            var formContent = (Dictionary<string, string>)content;

            if (content != null && formContent == null)
            {
                throw new Exception($"FormContentSerializer cannot serialize content of type {content.GetType()}. Expecting {typeof(Dictionary<string, string>)}");
            }

            return new FormUrlEncodedContent(formContent);
        }

        /// <inheritdoc/>
        public override Task<T> Deserialize<T>(HttpResponseMessage responseMessage)
        {
            throw new NotImplementedException();
        }
    }
}
