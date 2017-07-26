// <copyright file="JsonContentSerializer.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjectBlackmagic.RestfulClient.Content
{
    /// <summary>
    /// Serializes and deserializes JSON content using Newtonsoft.
    /// </summary>
    public class JsonContentSerializer : BaseContentSerializer, IRestfulContentSerializer
    {
        private readonly Formatting formatting;
        private readonly JsonSerializerSettings serializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContentSerializer"/> class.
        /// Base constructor.
        /// </summary>
        public JsonContentSerializer()
        {
            formatting = Formatting.None;
            serializerSettings = new JsonSerializerSettings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonContentSerializer"/> class with formatting and settings.
        /// </summary>
        /// <param name="formatting">Serialization formatting type</param>
        /// <param name="serializerSettings">Serialization and deserialization settings.</param>
        public JsonContentSerializer(Formatting formatting, JsonSerializerSettings serializerSettings)
        {
            this.formatting = formatting;
            this.serializerSettings = serializerSettings;
        }

        /// <inheritdoc/>
        public override HttpContent Serialize(object requestContent)
        {
            var serializedContent = JsonConvert.SerializeObject(requestContent, formatting, serializerSettings);

            var stringContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");

            return stringContent;
        }

        /// <inheritdoc/>
        public override async Task<T> Deserialize<T>(HttpResponseMessage responseMessage)
        {
            var responseContent = await Deserialize(responseMessage);

            return string.IsNullOrEmpty(responseContent)
                ? default(T)
                : JsonConvert.DeserializeObject<T>(responseContent, serializerSettings);
        }
    }
}
