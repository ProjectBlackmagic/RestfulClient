// <copyright file="IRequestConfiguration.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using ProjectBlackmagic.RestfulClient.Content;

namespace ProjectBlackmagic.RestfulClient.Configuration
{
    /// <summary>
    /// Interface for request configurations for RestfulClient.
    /// </summary>
    public interface IRequestConfiguration
    {
        /// <summary>
        /// Gets or sets the completion option for the restful request(s).
        /// </summary>
        HttpCompletionOption? CompletionOption { get; set; }

        /// <summary>
        /// Gets or sets the cancellation token for the restful request(s).
        /// </summary>
        CancellationToken? CancellationToken { get; set; }

        /// <summary>
        /// Gets or sets the custom request headers added to each request.
        /// </summary>
        IDictionary<string, string> CustomRequestHeaders { get; set; }

        /// <summary>
        /// Gets or sets the request content serializer.
        /// </summary>
        IRestfulContentSerializer RequestContentSerializer { get; set; }

        /// <summary>
        /// Gets or sets the response content deserializer.
        /// </summary>
        IRestfulContentSerializer ResponseContentDeserializer { get; set; }

        /// <summary>
        /// Applies a base configuration to the current configuration.
        /// </summary>
        /// <param name="baseConfig">Base restful configuration</param>
        void ApplyBaseConfiguration(IRequestConfiguration baseConfig);
    }
}