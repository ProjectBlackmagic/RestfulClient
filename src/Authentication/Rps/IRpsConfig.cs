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
        /// <value>
        /// The authenication url.
        /// </value>
        string AuthUrl { get; }

        /// <summary>
        /// Gets the site identifier.
        /// </summary>
        /// <value>
        /// The site identifier.
        /// </value>
        string SiteId { get; }

        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        string Scope { get; }

        /// <summary>
        /// Gets the policy.
        /// </summary>
        /// <value>
        /// The policy.
        /// </value>
        string Policy { get; }

        /// <summary>
        /// Gets the site certificate.
        /// </summary>
        /// <value>
        /// The site certificate.
        /// </value>
        X509Certificate2 SiteCertificate { get; }
    }
}
