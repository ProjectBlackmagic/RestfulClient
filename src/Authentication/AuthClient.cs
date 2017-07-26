// <copyright file="AuthClient.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ProjectBlackmagic.RestfulClient.Configuration;

namespace ProjectBlackmagic.RestfulClient.Authentication
{
    /// <summary>
    /// Restful client that supports an authentication mechanism.
    /// </summary>
    public class AuthClient : RestfulClient, IRestfulClient
    {
        private readonly IAuthenticator authenticator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthClient" /> class.
        /// Base constructor.
        /// </summary>
        /// <param name="authenticator">Authenticator</param>
        /// <param name="apiEndpoint">API Endpoint</param>
        public AuthClient(IAuthenticator authenticator, string apiEndpoint)
            : base(apiEndpoint)
        {
            this.authenticator = authenticator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthClient" /> class.
        /// Constructor with custom client handler and additional delegating handlers.
        /// </summary>
        /// <param name="authenticator">Authenticator</param>
        /// <param name="apiEndpoint">API Endpoint</param>
        /// <param name="clientHandler">Custom client handler</param>
        /// <param name="delegatingHandlers">Delegating handlers</param>
        public AuthClient(IAuthenticator authenticator, string apiEndpoint, HttpClientHandler clientHandler, params DelegatingHandler[] delegatingHandlers)
            : base(apiEndpoint, clientHandler, delegatingHandlers)
        {
            this.authenticator = authenticator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthClient" /> class.
        /// </summary>
        /// <param name="authenticator">Authenticator</param>
        /// <param name="apiEndpoint">API Endpoint</param>
        /// <param name="baseConfig">Base request configuration.</param>
        public AuthClient(IAuthenticator authenticator, string apiEndpoint, IRequestConfiguration baseConfig)
            : base(apiEndpoint, baseConfig)
        {
            this.authenticator = authenticator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthClient" /> class.
        /// Constructor with base request configuration, custom client handler, and additional delegating handlers.
        /// </summary>
        /// <param name="authenticator">Authenticator</param>
        /// <param name="apiEndpoint">API Endpoint</param>
        /// <param name="baseConfig">Base request configuration.</param>
        /// <param name="clientHandler">Custom client handler</param>
        /// <param name="delegatingHandlers">Delegating handlers</param>
        public AuthClient(IAuthenticator authenticator, string apiEndpoint, IRequestConfiguration baseConfig, HttpClientHandler clientHandler, params DelegatingHandler[] delegatingHandlers)
            : base(apiEndpoint, baseConfig, authenticator.EnhanceClientHandler(clientHandler), delegatingHandlers)
        {
            this.authenticator = authenticator;
        }

        /// <inheritdoc/>
        protected override async Task<T> PerformRequestAsync<T>(HttpRequestMessage httpRequest, IRequestConfiguration requestConfig = null)
        {
            try
            {
                var authValue = await authenticator.GetAuthValue();

                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Authorization", authValue);
            }
            catch (Exception ex)
            {
                throw new RestfulClientException("AuthClient could not obtain and/or add the authentication token", httpRequest, HttpStatusCode.Unauthorized, ex);
            }

            try
            {
                return await base.PerformRequestAsync<T>(httpRequest, requestConfig);
            }
            catch (RestfulClientException ex)
            {
                // If the request was unauthorized, clear the offending token from the cache to force a new token to be obtained on the next request
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    authenticator.ClearAuthValue();
                }

                throw ex;
            }
        }
    }
}
