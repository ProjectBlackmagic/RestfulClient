// <copyright file="AadConfig.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Security.Cryptography.X509Certificates;

namespace ProjectBlackmagic.RestfulClient.Authentication.Aad
{
    /// <summary>
    /// Configuration for AAD authentication.
    /// </summary>
    /// <seealso cref="AuthenticatorConfigBase" />
    /// <seealso cref="IAadConfig"/>
    public class AadConfig : AuthenticatorConfigBase, IAadConfig
    {
        /// <inheritdoc/>
        public string ClientId { get; protected set; }

        /// <inheritdoc/>
        public string Resource { get; protected set; }

        /// <inheritdoc/>
        public string Authority { get; protected set; }

        /// <inheritdoc/>
        public X509Certificate2 SiteCertificate { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AadConfig"/> class.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="certificateName">Name of the certificate.</param>
        public AadConfig(string authority, string clientId, string resource, string certificateName)
            : this(authority, clientId, resource, GetCertificateFromStore(certificateName))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AadConfig"/> class.
        /// </summary>
        /// <param name="authority">The authority.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="resource">The resource.</param>
        /// <param name="certificate">The certificate.</param>
        public AadConfig(string authority, string clientId, string resource, X509Certificate2 certificate)
        {
            Authority = authority;
            ClientId = clientId;
            Resource = resource;
            SiteCertificate = certificate;
        }
    }
}
