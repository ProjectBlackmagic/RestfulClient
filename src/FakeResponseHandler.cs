// <copyright file="FakeResponseHandler.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectBlackmagic.RestfulClient
{
    /// <summary>
    /// Mock response handler. Useful for unit testing.
    /// </summary>
    public class FakeResponseHandler : DelegatingHandler
    {
        private readonly Dictionary<Uri, HttpResponseMessage> fakeResponses = new Dictionary<Uri, HttpResponseMessage>();

        /// <summary>
        /// Gets list of requests made via the handler. Primarily used to enable test assertions on outgoing requests.
        /// </summary>
        public List<HttpRequestMessage> Requests { get; private set; } = new List<HttpRequestMessage>();

        /// <summary>
        /// Add fake response identified by URL.
        /// </summary>
        /// <param name="uri">Url ("key") of the response.</param>
        /// <param name="responseMessage">Response message.</param>
        public void AddFakeResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            fakeResponses.Add(uri, responseMessage);
        }

        /// <summary>
        /// Mimics request execution by retrieving response from added mocs.
        /// </summary>
        /// <param name="request">Request object.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Mocked response.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);

            if (fakeResponses.ContainsKey(request.RequestUri))
            {
                return Task.FromResult(fakeResponses[request.RequestUri]);
            }
            else
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    RequestMessage = request
                });
            }
        }

        /// <summary>
        /// Creates a fake response handler
        /// </summary>
        /// <param name="code">Status code of the mocked response.</param>
        /// <param name="endpoint">Request endpoint.</param>
        /// <param name="content">Request content.</param>
        /// <returns>Mocked response handler.</returns>
        public static FakeResponseHandler Create(HttpStatusCode code, string endpoint, HttpContent content = null)
        {
            var fakeResponseMessage = new HttpResponseMessage(code);
            if (content != null)
            {
                fakeResponseMessage.Content = content;
            }

            var fakeResponseHandler = new FakeResponseHandler();
            fakeResponseHandler.AddFakeResponse(new Uri(endpoint), fakeResponseMessage);

            return fakeResponseHandler;
        }
    }
}