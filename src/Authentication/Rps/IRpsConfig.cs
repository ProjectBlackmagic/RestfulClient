// <copyright file="IRpsConfig.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Security.Cryptography.X509Certificates;

namespace ProjectBlackmagic.RestfulClient.Authentication.Rps
{
    /// <summary>
    /// Interface for configuration for RPS (Live ID) authentication.
    /// </summary>
    public interface IRpsConfig
    {
        /// <summary>
        /// Gets the authentication url.
        /// </summary>
        string AuthUrl { get; }

        /// <summary>
        /// Gets the site identifier.
        /// </summary>
        string SiteId { get; }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        string Scope { get; }

        /// <summary>
        /// Gets the policy.
        /// </summary>
        string Policy { get; }

        /// <summary>
        /// Gets the site certificate.
        /// </summary>
        X509Certificate2 SiteCertificate { get; }
    }
}
