// <copyright file="RpsConfig.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Security.Cryptography.X509Certificates;

namespace ProjectBlackmagic.RestfulClient.Authentication.Rps
{
    /// <summary>
    /// Configuration for RPS (Live ID) authentication.
    /// </summary>
    /// <seealso cref="AuthenticatorConfigBase" />
    /// <seealso cref="IRpsConfig"/>
    public class RpsConfig : AuthenticatorConfigBase, IRpsConfig
    {
        /// <inheritdoc/>
        public string AuthUrl { get; protected set; }

        /// <inheritdoc/>
        public string SiteId { get; protected set; }

        /// <inheritdoc/>
        public string Scope { get; protected set; }

        /// <inheritdoc/>
        public string Policy { get; protected set; }

        /// <inheritdoc/>
        public X509Certificate2 SiteCertificate { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RpsConfig" /> class.
        /// </summary>
        /// <param name="authUrl">MSA ClientCredential URL</param>
        /// <param name="targetSiteID">Target Site ID</param>
        /// <param name="targetScope">Target Scope</param>
        /// <param name="targetPolicy">The desired policy. This is usually S2S_24HOURS_MUTUALSSL</param>
        /// <param name="certificateName">Name of the certificate in the local, personal certificate store to be used for mutual TLS.</param>
        public RpsConfig(string authUrl, string targetSiteID, string targetScope, string targetPolicy, string certificateName)
            : this(authUrl, targetSiteID, targetScope, targetPolicy, GetCertificateFromStore(certificateName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RpsConfig" /> class.
        /// </summary>
        /// <param name="authUrl">MSA ClientCredential URL</param>
        /// <param name="targetSiteID">Target Site ID</param>
        /// <param name="targetScope">Target Scope</param>
        /// <param name="targetPolicy">The desired policy. This is usually S2S_24HOURS_MUTUALSSL</param>
        /// <param name="certificateName">Name of the certificate to be used for mutual TLS.</param>
        /// <param name="storeName">Name of the certificate store containing the certificate.</param>
        /// <param name="storeLocation">Location of the certificate store containing the certificate.</param>
        public RpsConfig(string authUrl, string targetSiteID, string targetScope, string targetPolicy, string certificateName, StoreName storeName, StoreLocation storeLocation)
            : this(authUrl, targetSiteID, targetScope, targetPolicy, GetCertificateFromStore(certificateName, storeName, storeLocation))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RpsConfig" /> class. Certificate is passed as binary.
        /// </summary>
        /// <param name="authUrl">MSA ClientCredential URL</param>
        /// <param name="targetSiteID">Target Site ID</param>
        /// <param name="targetScope">Target Scope</param>
        /// <param name="targetPolicy">The desired policy. This is usually S2S_24HOURS_MUTUALSSL</param>
        /// <param name="siteCertificate">X509Certificate2 to be used for mutual TLS</param>
        public RpsConfig(string authUrl, string targetSiteID, string targetScope, string targetPolicy, X509Certificate2 siteCertificate)
        {
            if (string.IsNullOrEmpty(authUrl))
            {
                throw new ArgumentNullException("loginAuthUrl");
            }

            if (string.IsNullOrEmpty(targetSiteID))
            {
                throw new ArgumentNullException("targetSiteID");
            }

            if (string.IsNullOrEmpty(targetScope))
            {
                throw new ArgumentNullException("targetScope");
            }

            if (string.IsNullOrEmpty(targetPolicy))
            {
                throw new ArgumentNullException("targetPolicy");
            }

            if (siteCertificate == null)
            {
                throw new ArgumentNullException("siteCertificate");
            }

            AuthUrl = authUrl;
            Policy = targetPolicy;
            Scope = targetScope;
            SiteId = targetSiteID;
            SiteCertificate = siteCertificate;
        }
    }
}
