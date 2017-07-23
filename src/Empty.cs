// <copyright file="Empty.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Net;

namespace ProjectBlackmagic.RestfulClient
{
    /// <summary>
    /// Represents a empty HTTP response. Used for methods that don't return a payload.
    /// </summary>
    public class Empty
    {
        /// <summary>
        /// Gets or sets a value indicating whether indicates wether <cref name="IsSuccessStatusCode" /> is a success status code.
        /// </summary>
        public bool IsSuccessStatusCode { get; set; }

        /// <summary>
        /// Gets or sets HTTP Status code value
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets reason phrase value returned by the server.
        /// </summary>
        public string ReasonPhrase { get; set; }
    }
}