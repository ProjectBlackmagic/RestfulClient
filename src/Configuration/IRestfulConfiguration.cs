// <copyright file="IRestfulConfiguration.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using ProjectBlackmagic.RestfulClient.Content;

namespace ProjectBlackmagic.RestfulClient.Configuration
{
    /// <summary>
    /// Interface for configurations for RestfulClient.
    /// </summary>
    public interface IRestfulConfiguration
    {
        /// <summary>
        /// Gets or sets the maximum buffer size for response content.
        /// </summary>
        //long MaxResponseContentBufferSize { get; set; }

        /// <summary>
        /// Gets or sets the default request timeout span.
        /// </summary>
        //TimeSpan Timeout { get; set; }

        /// <summary>
        /// Gets or sets the default http request headers added to each request.
        /// </summary>
        //HttpRequestHeaders DefaultRequestHeaders { get; set; }

        HttpCompletionOption CompletionOption { get; set; }

        /// <summary>
        /// Gets or sets the custom request headers added to each request.
        /// </summary>
        IDictionary<string, string> CustomRequestHeaders { get; set; }

        /// <summary>
        /// Gets or sets the request content serializer.
        /// </summary>
        IRestfulContentSerializer RequestContentSerializer { get; set; }

        /// <summary>
        /// Gets or sets the response content serializer.
        /// </summary>
        IRestfulContentSerializer ResponseContentSerializer { get; set; }
    }
}