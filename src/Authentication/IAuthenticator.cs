// <copyright file="IAuthenticator.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectBlackmagic.RestfulClient.Authentication
{
    /// <summary>
    /// Interface for an authenticator.
    /// </summary>
    public interface IAuthenticator
    {
        /// <summary>
        /// Enhances the client handler.
        /// </summary>
        /// <param name="clientHandler">The client handler.</param>
        /// <returns>The now-enhanced client handler.</returns>
        HttpClientHandler EnhanceClientHandler(HttpClientHandler clientHandler);

        /// <summary>
        /// Gets the authorization header scheme.
        /// </summary>
        string Scheme { get; }

        /// <summary>
        /// Gets the authorization value.
        /// </summary>
        /// <returns>Authorization header value</returns>
        Task<string> GetAuthValue();

        /// <summary>
        /// Clears the authentication value.
        /// </summary>
        void ClearAuthValue();
    }
}
