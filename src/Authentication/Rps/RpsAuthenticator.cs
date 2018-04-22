// <copyright file="RpsAuthenticator.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ProjectBlackmagic.RestfulClient.Authentication.Rps
{
    /// <summary>
    /// Handler and token logic for RPS (Live ID) authentication.
    /// </summary>
    /// <seealso cref="IAuthenticator" />
    public class RpsAuthenticator : IAuthenticator
    {
        private readonly IRestfulClient tokenClient;
        private readonly ReaderWriterLockSlim lockSlim;
        private readonly IMemoryCache cache;

        /// <summary>
        /// Gets or sets the configuration for RPS (Live ID) requests.
        /// </summary>
        public IRpsConfig Config { get; protected set; }

        /// <summary>
        /// Gets the query string used in RPS (Live ID) requests.
        /// </summary>
        public Dictionary<string, string> QueryString => new Dictionary<string, string>()
        {
            { "grant_type", "client_credentials" },
            { "client_id", Config.SiteId },
            { "scope", ServiceScope }
        };

        /// <summary>
        /// Gets the service scope used in RPS (Live ID) requests.
        /// </summary>
        public string ServiceScope => $"{Config.Scope}::{Config.Policy}";

        private string TokenCacheKey => $"rpsToken::{ServiceScope}";

        /// <summary>
        /// Initializes a new instance of the <see cref="RpsAuthenticator"/> class.
        /// Base constructor.
        /// </summary>
        /// <param name="config">Configuration for RPS (Live ID) configuration.</param>
        public RpsAuthenticator(IRpsConfig config)
            : this(config, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RpsAuthenticator"/> class.
        /// </summary>
        /// <param name="config">Configuration for RPS (Live ID) authentication.</param>
        /// <param name="clientHandler">Client handler for RPS auth requests.</param>
        /// <param name="delegatingHandlers">Delegating handlers for RPS auth requests.</param>
        public RpsAuthenticator(IRpsConfig config, HttpClientHandler clientHandler, params DelegatingHandler[] delegatingHandlers)
        {
            Config = config;

            tokenClient = new RestfulClient(config.AuthUrl, EnhanceClientHandler(clientHandler), delegatingHandlers);
            lockSlim = new ReaderWriterLockSlim();
            cache = new MemoryCache(new MemoryCacheOptions());
        }

        /// <inheritdoc/>
        public HttpClientHandler EnhanceClientHandler(HttpClientHandler clientHandler)
        {
            var handler = clientHandler ?? new HttpClientHandler();

            handler.SslProtocols = SslProtocols.Tls12;
            handler.ClientCertificates.Add(Config.SiteCertificate);

            return handler;
        }

        /// <inheritdoc/>
        public string Scheme => "Rps";

        /// <inheritdoc/>
        public async Task<string> GetAuthValue()
        {
            RpsToken token;

            lockSlim.EnterUpgradeableReadLock();
            try
            {
                token = cache.Get(TokenCacheKey) as RpsToken;

                if (token == null)
                {
                    lockSlim.EnterWriteLock();
                    try
                    {
                        // Perform the request for the token
                        var content = new FormUrlEncodedContent(QueryString);
                        token = await tokenClient.PostAsync<RpsToken>(string.Empty, content, null);

                        // Expire the token in the cache before the token actually expires, to be safe
                        var earlyExpiry = token.ExpiresIn * 0.95;

                        cache.Set(TokenCacheKey, token, DateTimeOffset.Now.AddSeconds(earlyExpiry));
                    }
                    finally
                    {
                        if (lockSlim.IsWriteLockHeld)
                        {
                            lockSlim.ExitWriteLock();
                        }
                    }
                }
            }
            finally
            {
                if (lockSlim.IsUpgradeableReadLockHeld)
                {
                    lockSlim.ExitUpgradeableReadLock();
                }
            }

            return $"app_ticket=\"{token.AccessToken}\"";
        }

        /// <inheritdoc/>
        public void ClearAuthValue()
        {
            lockSlim.EnterWriteLock();
            try
            {
                cache.Remove(TokenCacheKey);
            }
            finally
            {
                if (lockSlim.IsWriteLockHeld)
                {
                    lockSlim.ExitWriteLock();
                }
            }
        }
    }
}
