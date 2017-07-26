// <copyright file="RpsToken.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using Newtonsoft.Json;

namespace ProjectBlackmagic.RestfulClient.Authentication.Rps
{
    /// <summary>
    /// Token for RPS (Live ID)
    /// </summary>
    public class RpsToken
    {
        /// <summary>
        /// Gets or sets the TokenType of the response. This is usually "Bearer."
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the requested ticket.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds that the AccessToken is valid for.
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Returns summary string of RpsToken.
        /// </summary>
        /// <returns>Summary of Rpstoken</returns>
        public override string ToString()
        {
            return $"Token Type: {TokenType}, Access Token: {AccessToken}, Expires In: {ExpiresIn}";
        }
    }
}
