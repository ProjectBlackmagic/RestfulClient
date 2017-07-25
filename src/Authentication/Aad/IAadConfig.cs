// <copyright file="IAadConfig.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Security.Cryptography.X509Certificates;

namespace ProjectBlackmagic.RestfulClient.Authentication.Aad
{
    /// <summary>
    /// Interface for configuration for AAD authentication.
    /// </summary>
    public interface IAadConfig
    {
        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        string ClientId { get; }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <value>
        /// The resource.
        /// </value>
        string Resource { get; }

        /// <summary>
        /// Gets the authority.
        /// </summary>
        /// <value>
        /// The authority.
        /// </value>
        string Authority { get; }

        /// <summary>
        /// Gets the site certificate.
        /// </summary>
        /// <value>
        /// The site certificate.
        /// </value>
        X509Certificate2 SiteCertificate { get; }
    }
}
