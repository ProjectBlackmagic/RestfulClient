// <copyright file="RestfulClientException.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Net;
using System.Net.Http;

namespace ProjectBlackmagic.RestfulClient
{
    /// <summary>
    /// Exception thrown by the client in case of unsuccessful request.
    /// </summary>
    public class RestfulClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClientException"/> class.
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="request">Request message.</param>
        /// <param name="statusCode">Request status code.</param>
        public RestfulClientException(string message, HttpRequestMessage request, HttpStatusCode statusCode)
        : this(message, request, statusCode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClientException"/> class.
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="request">Request message.</param>
        /// <param name="statusCode">Request status code.</param>
        /// <param name="innerException">Inner exception.</param>
        public RestfulClientException(string message, HttpRequestMessage request, HttpStatusCode statusCode, Exception innerException)
        : base(ToMessage(message, request, statusCode), innerException)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClientException"/> class.
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="response">Response message.</param>
        public RestfulClientException(string message, HttpResponseMessage response)
        : this(message, response, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClientException"/> class.
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="response">Response message.</param>
        /// <param name="innerException">Inner exception.</param>
        public RestfulClientException(string message, HttpResponseMessage response, Exception innerException)
        : base(ToMessage(message, response), innerException)
        {
            StatusCode = response.StatusCode;
            ResponseContent = response.Content;
        }

        /// <summary>
        /// Gets response content.
        /// </summary>
        public HttpContent ResponseContent { get; private set; }

        /// <summary>
        /// Gets or sets status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Transforms request object into string message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="request">Request object.</param>
        /// <param name="statusCode">Status code.</param>
        /// <returns>Formated message.</returns>
        public static string ToMessage(string message, HttpRequestMessage request, HttpStatusCode? statusCode)
        {
            return $"Message: {message}{Environment.NewLine}"
                + $"Request: {request}{Environment.NewLine}"
                + $"StatusCode: {statusCode}{Environment.NewLine}";
        }

        /// <summary>
        /// Transforms request object into string message.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="response">Response object.</param>
        /// <returns>Formated message.</returns>
        public static string ToMessage(string message, HttpResponseMessage response)
        {
            return ToMessage(message, response?.RequestMessage, response?.StatusCode)
                + $"Response: {response}{Environment.NewLine}"
                + $"ResponseContent: {response?.Content?.ReadAsStringAsync().Result}{Environment.NewLine}";
        }
    }
}