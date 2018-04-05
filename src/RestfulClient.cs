// <copyright file="RestfulClient.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjectBlackmagic.RestfulClient
{
    /// <summary>
    /// The Restful Client
    /// </summary>
    public class RestfulClient : IRestfulClient
    {
        /// <summary>
        /// Gets or sets the base URL. All requests made by an instance of the client will be prefixed with this value.
        /// </summary>
        public Uri BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets instance of underlying HttpClient.This object will actually perform the request.
        /// </summary>
        public HttpClient Client { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Base constructor.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        public RestfulClient(string apiEndpoint)
        {
            BaseUrl = new Uri(apiEndpoint);
            Client = new HttpClient();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Constructor with custom message handler.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        /// <param name="messageHandler">Custom message handler.</param>
        public RestfulClient(string apiEndpoint, HttpMessageHandler messageHandler)
        {
            BaseUrl = new Uri(apiEndpoint);
            Client = CreateClient(messageHandler);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Constructor with custom message handler and additional delegating handlers.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        /// <param name="messageHandler">Custom message handler.</param>
        /// <param name="delegatingHandlers">Custom delegating handlers.</param>
        public RestfulClient(string apiEndpoint, HttpMessageHandler messageHandler, params DelegatingHandler[] delegatingHandlers)
        {
            BaseUrl = new Uri(apiEndpoint);
            Client = CreateClient(messageHandler, delegatingHandlers);
        }

        #region Sync Methods

        /// <summary>
        /// Performs HTTP Get method
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public T Get<T>(string action, Dictionary<string, string> headers = null)
        {
            return Task.Run(() => GetAsync<T>(action, headers)).Result;
        }

        /// <summary>
        /// Performs HTTP Post method
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <param name="contentType">Content type of the payload.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public T Post<T>(string action, object content, Dictionary<string, string> headers = null, ContentType contentType = ContentType.JSON)
        {
            return Task.Run(() => PostAsync<T>(action, content, headers, contentType)).Result;
        }

        /// <summary>
        /// Performs HTTP Put method
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <param name="contentType">Content type of the payload.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public T Put<T>(string action, object content, Dictionary<string, string> headers = null, ContentType contentType = ContentType.JSON)
        {
            return Task.Run(() => PutAsync<T>(action, content, headers, contentType)).Result;
        }

        /// <summary>
        /// Performs HTTP Patch method
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <param name="contentType">Content type of the payload.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public T Patch<T>(string action, object content, Dictionary<string, string> headers = null, ContentType contentType = ContentType.JSON)
        {
            return Task.Run(() => PatchAsync<T>(action, content, headers, contentType)).Result;
        }

        /// <summary>
        /// Performs HTTP Delete method
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public T Delete<T>(string action, Dictionary<string, string> headers = null)
        {
            return Task.Run(() => DeleteAsync<T>(action, headers)).Result;
        }

        #endregion

        #region Async Methods

        /// <summary>
        /// Performs HTTP Get method (async)
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public async Task<T> GetAsync<T>(string action, Dictionary<string, string> headers = null)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, BuildUri(action));
            return await PerformRequestAsync<T>(httpRequest, headers);
        }

        /// <summary>
        /// Performs HTTP Post method (async)
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <param name="contentType">Content type of the payload.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public async Task<T> PostAsync<T>(string action, object content, Dictionary<string, string> headers = null, ContentType contentType = ContentType.JSON)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, BuildUri(action))
            {
                Content = RestfulContent.GetHttpContent(content, contentType)
            };
            return await PerformRequestAsync<T>(httpRequest, headers);
        }

        /// <summary>
        /// Performs HTTP Put method (async)
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <param name="contentType">Content type of the payload.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public async Task<T> PutAsync<T>(string action, object content, Dictionary<string, string> headers = null, ContentType contentType = ContentType.JSON)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Put, BuildUri(action))
            {
                Content = RestfulContent.GetHttpContent(content, contentType)
            };
            return await PerformRequestAsync<T>(httpRequest, headers);
        }

        /// <summary>
        /// Performs HTTP Patch method (async)
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <param name="contentType">Content type of the payload.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public async Task<T> PatchAsync<T>(string action, object content, Dictionary<string, string> headers = null, ContentType contentType = ContentType.JSON)
        {
            var method = new HttpMethod("PATCH");
            var httpRequest = new HttpRequestMessage(method, BuildUri(action))
            {
                Content = RestfulContent.GetHttpContent(content, contentType)
            };
            return await PerformRequestAsync<T>(httpRequest, headers);
        }

        /// <summary>
        /// Performs HTTP Delete method (async)
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public async Task<T> DeleteAsync<T>(string action, Dictionary<string, string> headers = null)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, BuildUri(action));
            return await PerformRequestAsync<T>(httpRequest, headers);
        }
        #endregion

        #region Other Methods

        /// <summary>
        /// Performs the request.
        /// </summary>
        /// <param name="httpRequest">Request object.</param>
        /// <param name="headers">Request handlers.</param>
        /// <typeparam name="T">The type to use when deserializing the response.</typeparam>
        /// <returns>Response.</returns>
        protected virtual async Task<T> PerformRequestAsync<T>(HttpRequestMessage httpRequest, Dictionary<string, string> headers = null)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    try
                    {
                        httpRequest.Headers.Add(header.Key, header.Value);
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format(
                                "Error adding header \"{0}\" with value \"{1}\" to outgoing request to \"{2}\". Check for null-value.",
                                header.Key,
                                header.Value,
                                httpRequest.RequestUri);

                        throw new RestfulClientException(message, httpRequest, HttpStatusCode.InternalServerError, ex);
                    }
                }
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            var response = await Client.SendAsync(httpRequest);
            sw.Stop();

            // Result
            if (!response.IsSuccessStatusCode)
            {
                throw new RestfulClientException("RestfulClient received unsuccessful response", response);
            }
            else if (typeof(T) == typeof(Empty)) // if only success acknowledgment is requested
            {
                var result = Activator.CreateInstance(typeof(T));

                ((Empty)result).IsSuccessStatusCode = response.IsSuccessStatusCode;
                ((Empty)result).ReasonPhrase = response.ReasonPhrase;
                ((Empty)result).StatusCode = response.StatusCode;

                return (T)result;
            }
            else // if result needs to be parsed
            {
                var readContentTask = response.Content?.ReadAsStringAsync() ?? Task.FromResult(string.Empty);
                var responseContent = await readContentTask;

                if (!string.IsNullOrEmpty(responseContent))
                {
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
                else
                {
                    return default(T);
                }
            }
        }

        private Uri BuildUri(string action)
        {
            return new Uri(BaseUrl, action);
        }

        // Code origin: https://github.com/mono/aspnetwebstack/blob/master/src/System.Net.Http.Formatting/HttpClientFactory.cs
        private static HttpClient CreateClient(params DelegatingHandler[] handlers)
        {
            return CreateClient(new HttpClientHandler(), handlers);
        }

        private static HttpClient CreateClient(HttpMessageHandler innerHandler, params DelegatingHandler[] handlers)
        {
            HttpMessageHandler pipeline = CreatePipeline(innerHandler, handlers);
            return new HttpClient(pipeline);
        }

        private static HttpMessageHandler CreatePipeline(HttpMessageHandler innerHandler, IEnumerable<DelegatingHandler> handlers)
        {
            if (innerHandler == null)
            {
                throw new ArgumentNullException("innerHandler");
            }

            if (handlers == null)
            {
                return innerHandler;
            }

            // Wire handlers up in reverse order starting with the inner handler
            HttpMessageHandler pipeline = innerHandler;
            IEnumerable<DelegatingHandler> reversedHandlers = handlers.Reverse();
            foreach (DelegatingHandler handler in reversedHandlers)
            {
                if (handler == null)
                {
                    throw new Exception("Delegating handler array contains null item");
                }

                if (handler.InnerHandler != null)
                {
                    throw new Exception("Delegating handler array has non null InnerHandler");
                }

                handler.InnerHandler = pipeline;
                pipeline = handler;
            }

            return pipeline;
        }

        #endregion
    }
}