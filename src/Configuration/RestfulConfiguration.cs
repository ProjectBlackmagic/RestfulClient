// <copyright file="RestfulConfiguration.cs" company="ProjectBlackmagic">
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
    public class RestfulConfiguration : IRestfulConfiguration
    {
        #region HttpClient configurations
        //public long MaxResponseContentBufferSize { get; set; }

        //public TimeSpan Timeout { get; set; }

        //public HttpRequestHeaders DefaultRequestHeaders { get; set; }

        public HttpCompletionOption CompletionOption { get; set; } = HttpCompletionOption.ResponseContentRead;
        #endregion

        /// <inheritdoc/>
        public IDictionary<string, string> CustomRequestHeaders { get; set; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        public IRestfulContentSerializer RequestContentSerializer { get; set; } = new JsonContentSerializer();

        /// <inheritdoc/>
        public IRestfulContentSerializer ResponseContentSerializer { get; set; } = new JsonContentSerializer();
    }
}
