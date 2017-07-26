// <copyright file="AadAuthenticator.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace ProjectBlackmagic.RestfulClient.Authentication.Aad
{
    /// <summary>
    /// Handler and token logic for AAD authentication.
    /// </summary>
    /// <seealso cref="IAuthenticator" />
    public class AadAuthenticator : IAuthenticator
    {
        private readonly string resource;
        private readonly AuthenticationContext authContext;
        private readonly ClientAssertionCertificate clientCert;

        /// <summary>
        /// Initializes a new instance of the <see cref="AadAuthenticator"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public AadAuthenticator(IAadConfig config)
        {
            resource = config.Resource;
            authContext = new AuthenticationContext(config.Authority);
            clientCert = new ClientAssertionCertificate(config.ClientId, config.SiteCertificate);
        }

        /// <inheritdoc/>
        public async Task<string> GetAuthValue()
        {
            var authResult = await authContext.AcquireTokenAsync(resource, clientCert);
            return $"Bearer {authResult.AccessToken}";
        }

        /// <inheritdoc/>
        public void ClearAuthValue()
        {
            authContext.TokenCache?.Clear();
        }

        /// <inheritdoc/>
        public HttpClientHandler EnhanceClientHandler(HttpClientHandler clientHandler)
        {
            return clientHandler ?? new HttpClientHandler();
        }
    }
}
