// <copyright file="RestfulClientExceptionTests.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Net;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ProjectBlackmagic.RestfulClient.Test
{
    [TestClass]
    public class RestfulClientExceptionTests
    {
        private static string errorMessage = "Error message";
        private static HttpStatusCode errorCode = HttpStatusCode.NotFound;

        [TestMethod]
        public void ResponseContent_TestWithRequest()
        {
            var exception = new TestClientException(errorMessage, new HttpRequestMessage(), errorCode);

            Assert.IsNull(exception.ResponseContent);
        }

        [TestMethod]
        public void StatusCode_TestWithRequest()
        {
            var exception = new TestClientException(errorMessage, new HttpRequestMessage(), errorCode);

            Assert.AreEqual(errorCode, exception.StatusCode);
        }

        [TestMethod]
        public void Message_TestWithRequest()
        {
            var requestMock = new Mock<HttpRequestMessage>();
            requestMock
                .Setup(r => r.ToString())
                .Returns("MockRequestToString");

            var exception = new TestClientException(errorMessage, requestMock.Object, errorCode);

            exception.AssertMessageKeyAndValue("Message", errorMessage);
            exception.AssertMessageKeyAndValue("Request", "MockRequestToString");
            exception.AssertMessageKeyAndValue("StatusCode", errorCode.ToString());
            exception.AssertMissingMessageKey("Response");
            exception.AssertMissingMessageKey("ResponseContent");
        }

        [TestMethod]
        public void InnerException_TestWithRequestAndInnerException()
        {
            var innerException = new Exception("Inner exception message");

            var exception = new TestClientException(errorMessage, new HttpRequestMessage(), errorCode, innerException);

            Assert.AreSame(innerException, exception.InnerException);
        }

        [TestMethod]
        public void ResponseContent_TestWithResponse()
        {
            var content = new StringContent("MockContent");
            var response = new HttpResponseMessage()
            {
                Content = content,
            };

            var exception = new TestClientException(errorMessage, response);

            Assert.IsNotNull(exception.ResponseContent);
            Assert.IsInstanceOfType(exception.ResponseContent, typeof(StringContent));
        }

        [TestMethod]
        public void StatusCode_TestWithResponse()
        {
            var response = new HttpResponseMessage(errorCode);

            var exception = new TestClientException(errorMessage, response);

            Assert.AreEqual(errorCode, exception.StatusCode);
        }

        [TestMethod]
        public void Message_TestWithResponse()
        {
            var requestMock = new Mock<HttpRequestMessage>();
            requestMock
                .Setup(r => r.ToString())
                .Returns("MockRequestToString");

            var responseMock = new Mock<HttpResponseMessage>(errorCode);
            responseMock
                .Setup(r => r.ToString())
                .Returns("MockResponseToString");
            responseMock.Object.Content = new StringContent("MockContent");
            responseMock.Object.RequestMessage = requestMock.Object;

            var exception = new TestClientException(errorMessage, responseMock.Object);

            exception.AssertMessageKeyAndValue("Message", errorMessage);
            exception.AssertMessageKeyAndValue("Request", "MockRequestToString");
            exception.AssertMessageKeyAndValue("StatusCode", exception.StatusCode.ToString());
            exception.AssertMessageKeyAndValue("Response", "MockResponseToString");
            exception.AssertMessageKeyAndValue("ResponseContent", "MockContent");
        }

        [TestMethod]
        public void InnerException_TestWithResponseAndInnerException()
        {
            var innerException = new Exception("Inner exception message");

            var exception = new TestClientException(errorMessage, new HttpResponseMessage(), innerException);

            Assert.AreSame(innerException, exception.InnerException);
        }
    }
}
