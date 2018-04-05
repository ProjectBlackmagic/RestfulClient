// <copyright file="TestClientException.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProjectBlackmagic.RestfulClient.Test
{
    internal class TestClientException : RestfulClientException
    {
        internal TestClientException(string message, HttpRequestMessage request, HttpStatusCode statusCode)
        : base(message, request, statusCode)
        {
        }

        internal TestClientException(string message, HttpRequestMessage request, HttpStatusCode statusCode, Exception innerException)
        : base(message, request, statusCode, innerException)
        {
            StatusCode = statusCode;
        }

        internal TestClientException(string message, HttpResponseMessage response)
        : base(message, response)
        {
        }

        internal TestClientException(string message, HttpResponseMessage response, Exception innerException)
        : base(message, response, innerException)
        {
        }

        internal void AssertMessageKeyAndValue(string key, string value)
        {
            Assert.IsTrue(Message.Contains($"{key}: {value}"));
        }

        internal void AssertMissingMessageKey(string key)
        {
            Assert.IsFalse(Message.Contains($"{key}:"));
        }
    }
}
