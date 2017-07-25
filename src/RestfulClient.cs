// <copyright file="RestfulClient.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ProjectBlackmagic.RestfulClient.Configuration;

namespace ProjectBlackmagic.RestfulClient
{
    /// <summary>
    /// The Restful Client
    /// </summary>
    public class RestfulClient : IRestfulClient
    {
        private readonly HttpClient client;
        private readonly IRestfulConfiguration config;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Base constructor.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        public RestfulClient(string apiEndpoint)
            : this(apiEndpoint, new RestfulConfiguration())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Constructor with custom message handler.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        /// <param name="messageHandler">Custom message handler.</param>
        public RestfulClient(string apiEndpoint, HttpMessageHandler messageHandler)
            : this(apiEndpoint, new RestfulConfiguration(), messageHandler)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Constructor with custom message handler and additional delegating handlers.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        /// <param name="messageHandler">Custom message handler.</param>
        /// <param name="delegatingHandlers">Custom delegating handlers.</param>
        public RestfulClient(string apiEndpoint, HttpMessageHandler messageHandler, params DelegatingHandler[] delegatingHandlers)
            : this(apiEndpoint, new RestfulConfiguration(), messageHandler, delegatingHandlers)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Base constructor.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        /// <param name="config">RestfulClient configuration.</param>
        public RestfulClient(string apiEndpoint, IRestfulConfiguration config)
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(apiEndpoint)
            };
            this.config = config;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Constructor with custom message handler.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        /// <param name="config">RestfulClient configuration.</param>
        /// <param name="messageHandler">Custom message handler.</param>
        public RestfulClient(string apiEndpoint, IRestfulConfiguration config, HttpMessageHandler messageHandler)
        {
            client = CreateClient(messageHandler);
            client.BaseAddress = new Uri(apiEndpoint);

            this.config = config;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Constructor with custom message handler and additional delegating handlers.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        /// <param name="config">RestfulClient configuration.</param>
        /// <param name="messageHandler">Custom message handler.</param>
        /// <param name="delegatingHandlers">Custom delegating handlers.</param>
        public RestfulClient(string apiEndpoint, IRestfulConfiguration config, HttpMessageHandler messageHandler, params DelegatingHandler[] delegatingHandlers)
        {
            client = CreateClient(messageHandler, delegatingHandlers);
            client.BaseAddress = new Uri(apiEndpoint);

            this.config = config;
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
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public T Post<T>(string action, object content, Dictionary<string, string> headers = null)
        {
            return Task.Run(() => PostAsync<T>(action, content, headers)).Result;
        }

        /// <summary>
        /// Performs HTTP Put method
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public T Put<T>(string action, object content, Dictionary<string, string> headers = null)
        {
            return Task.Run(() => PutAsync<T>(action, content, headers)).Result;
        }

        /// <summary>
        /// Performs HTTP Patch method
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public T Patch<T>(string action, object content, Dictionary<string, string> headers = null)
        {
            return Task.Run(() => PatchAsync<T>(action, content, headers)).Result;
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
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, action);
            return await PerformRequestAsync<T>(httpRequest, headers);
        }

        /// <summary>
        /// Performs HTTP Post method (async)
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public async Task<T> PostAsync<T>(string action, object content, Dictionary<string, string> headers = null)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, action)
            {
                Content = config.RequestContentSerializer.Serialize(content)
            };

            return await PerformRequestAsync<T>(httpRequest, headers);
        }

        /// <summary>
        /// Performs HTTP Put method (async)
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public async Task<T> PutAsync<T>(string action, object content, Dictionary<string, string> headers = null)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Put, action)
            {
                Content = config.RequestContentSerializer.Serialize(content)
            };
            return await PerformRequestAsync<T>(httpRequest, headers);
        }

        /// <summary>
        /// Performs HTTP Patch method (async)
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        public async Task<T> PatchAsync<T>(string action, object content, Dictionary<string, string> headers = null)
        {
            var method = new HttpMethod("PATCH");
            var httpRequest = new HttpRequestMessage(method, action)
            {
                Content = config.RequestContentSerializer.Serialize(content)
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
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, action);
            return await PerformRequestAsync<T>(httpRequest, headers);
        }
        #endregion

        #region Other Methods
        protected void AddHeadersToRequest(HttpRequestMessage httpRequest, IDictionary<string, string> headers)
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
        }

        /// <summary>
        /// Performs the request.
        /// </summary>
        /// <param name="httpRequest">Request object.</param>
        /// <param name="headers">Request handlers.</param>
        /// <typeparam name="T">The type to use when deserializing the response.</typeparam>
        /// <returns>Response.</returns>
        protected virtual async Task<T> PerformRequestAsync<T>(HttpRequestMessage httpRequest, Dictionary<string, string> headers = null)
        {
            AddHeadersToRequest(httpRequest, config.CustomRequestHeaders);
            AddHeadersToRequest(httpRequest, headers);

            var response = await client.SendAsync(httpRequest, config.CompletionOption);

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
            else // if result needs to be deserialized
            {
                return await config.ResponseContentSerializer.Deserialize<T>(response);
            }
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