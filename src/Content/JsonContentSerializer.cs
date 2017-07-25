// <copyright file="JsonContentSerializer.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjectBlackmagic.RestfulClient.Content
{
    public class JsonContentSerializer : BaseContentSerializer, IRestfulContentSerializer
    {
        private readonly Formatting formatting;
        private readonly JsonSerializerSettings serializerSettings;

        public JsonContentSerializer()
        {
            formatting = Formatting.None;
            serializerSettings = new JsonSerializerSettings();
        }

        public JsonContentSerializer(object content, Formatting formatting, JsonSerializerSettings serializerSettings)
        {
            this.formatting = formatting;
            this.serializerSettings = serializerSettings;
        }

        public override HttpContent Serialize(object requestContent)
        {
            var serializedContent = JsonConvert.SerializeObject(requestContent, formatting, serializerSettings);

            var stringContent = new StringContent(serializedContent);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return stringContent;
        }

        public override async Task<T> Deserialize<T>(HttpResponseMessage responseMessage)
        {
            var responseContent = await Deserialize(responseMessage);

            return string.IsNullOrEmpty(responseContent)
                ? default(T)
                : JsonConvert.DeserializeObject<T>(responseContent, serializerSettings);
        }
    }
}
