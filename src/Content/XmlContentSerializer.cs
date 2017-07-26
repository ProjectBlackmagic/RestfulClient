// <copyright file="XmlContentSerializer.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProjectBlackmagic.RestfulClient.Content
{
    /// <summary>
    /// Serializes and deserializes XML content using System.Xml.Serialization.
    /// </summary>
    public class XmlContentSerializer : BaseContentSerializer, IRestfulContentSerializer
    {
        /// <inheritdoc/>
        public override HttpContent Serialize(object content)
        {
            var serializer = new XmlSerializer(content.GetType());

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, content);

                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");

                return streamContent;
            }
        }

        /// <inheritdoc/>
        public override async Task<T> Deserialize<T>(HttpResponseMessage responseMessage)
        {
            var deserializer = new XmlSerializer(typeof(T));

            using (var stream = await responseMessage.Content?.ReadAsStreamAsync())
            {
                return (T)deserializer.Deserialize(stream);
            }
        }
    }
}
