// <copyright file="RequestConfiguration.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using ProjectBlackmagic.RestfulClient.Content;

namespace ProjectBlackmagic.RestfulClient.Configuration
{
    /// <summary>
    /// Basic request configuration logic for RestfulClient.
    /// </summary>
    public class RequestConfiguration : IRequestConfiguration
    {
        /// <inheritdoc/>
        public virtual HttpCompletionOption? CompletionOption { get; set; }

        /// <inheritdoc/>
        public virtual CancellationToken? CancellationToken { get; set; }

        /// <inheritdoc/>
        public virtual IDictionary<string, string> CustomRequestHeaders { get; set; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        public virtual IRestfulContentSerializer RequestContentSerializer { get; set; } = new JsonContentSerializer();

        /// <inheritdoc/>
        public virtual IRestfulContentSerializer ResponseContentDeserializer { get; set; } = new JsonContentSerializer();

        /// <inheritdoc/>
        public virtual void ApplyBaseConfiguration(IRequestConfiguration baseConfig)
        {
            if (baseConfig == null)
            {
                return;
            }

            CompletionOption = CompletionOption ?? baseConfig.CompletionOption;

            // Merge custom request headers
            if (CustomRequestHeaders != null && baseConfig.CustomRequestHeaders != null)
            {
                baseConfig.CustomRequestHeaders
                    .ToList()
                    .ForEach(x => CustomRequestHeaders.Add(x.Key, x.Value));
            }
            else
            {
                CustomRequestHeaders = CustomRequestHeaders ?? baseConfig.CustomRequestHeaders;
            }

            RequestContentSerializer = RequestContentSerializer ?? baseConfig.RequestContentSerializer;
            ResponseContentDeserializer = ResponseContentDeserializer ?? baseConfig.ResponseContentDeserializer;
        }
    }
}
