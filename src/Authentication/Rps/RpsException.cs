// <copyright file="RpsException.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using Newtonsoft.Json;

namespace ProjectBlackmagic.RestfulClient.Authentication.Rps
{
    /// <summary>
    /// Object that contains the details of a server error response
    /// </summary>
    public class RpsException
    {
        /// <summary>
        /// Gets or sets the error string.
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the server description of the error.
        /// </summary>
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Gets or sets the WLID Error Code associated with this error.
        /// </summary>
        public string WlidErrorCode { get; set; }
    }
}
