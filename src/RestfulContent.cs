﻿// <copyright file="RestfulContent.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ProjectBlackmagic.RestfulClient
{
    /// <summary>
    /// Types of content supported by <see cref="RestfulContent"/> serializer.
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// JSON serialization
        /// </summary>
        JSON,

        /// <summary>
        /// UrlEncodedForm serialization
        /// </summary>
        UrlEncodedForm
    }

    /// <summary>
    /// ResfulContent class is used for serialization of the request content object.
    /// As of now, supported data types are Json and UrlencodedForm.
    /// </summary>
    public static class RestfulContent
    {
        /// <summary>
        /// Serializes content object into given content type.
        /// </summary>
        /// <param name="content">Object to be serialized.</param>
        /// <param name="contentType">Serialization format.</param>
        /// <returns>Serialized <see cref="HttpContent"/> object.</returns>
        public static HttpContent GetHttpContent(object content, ContentType contentType = ContentType.JSON)
        {
            HttpContent httpContent;

            switch (contentType)
            {
                case ContentType.JSON:
                    {
                        httpContent = new StringContent(
                            JsonConvert.SerializeObject(
                                content,
                                Formatting.None,
                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));

                        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        break;
                    }

                case ContentType.UrlEncodedForm:
                    {
                        Type t = content.GetType();
                        if (t.Equals(typeof(string)))
                        {
                            var stringContent = (string)content;
                            httpContent = new StringContent(stringContent);
                            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        }
                        else if (t.Equals(typeof(Dictionary<string, string>)))
                        {
                            var formContent = (Dictionary<string, string>)content;
                            httpContent = new FormUrlEncodedContent(formContent);
                        }
                        else
                        {
                            throw new Exception("This content not supported for type UrlEncodedForm");
                        }

                        break;
                    }

                default:
                    {
                        throw new Exception("Content Type not supported.");
                    }
            }

            return httpContent;
        }
    }
}