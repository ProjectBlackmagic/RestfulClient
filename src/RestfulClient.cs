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
using System.Net.Http.Headers;
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
        private readonly IRequestConfiguration baseConfig;

        #region HttpClient properties

        /// <inheritdoc/>
        public HttpRequestHeaders DefaultRequestHeaders
        {
            get
            {
                return client.DefaultRequestHeaders;
            }
        }

        /// <inheritdoc/>
        public long MaxResponseContentBufferSize
        {
            get
            {
                return client.MaxResponseContentBufferSize;
            }

            set
            {
                client.MaxResponseContentBufferSize = value;
            }
        }

        /// <inheritdoc/>
        public TimeSpan Timeout
        {
            get
            {
                return client.Timeout;
            }

            set
            {
                client.Timeout = value;
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Base constructor.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        public RestfulClient(string apiEndpoint)
            : this(apiEndpoint, new RequestConfiguration())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Constructor with custom message handler.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        /// <param name="messageHandler">Custom message handler.</param>
        public RestfulClient(string apiEndpoint, HttpMessageHandler messageHandler)
            : this(apiEndpoint, new RequestConfiguration(), messageHandler)
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
            : this(apiEndpoint, new RequestConfiguration(), messageHandler, delegatingHandlers)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Constructor with base request configuration.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        /// <param name="baseConfig">Base request configuration.</param>
        public RestfulClient(string apiEndpoint, IRequestConfiguration baseConfig)
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(apiEndpoint)
            };
            this.baseConfig = baseConfig;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Constructor with base request configuration and custom message handler.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        /// <param name="baseConfig">Base request configuration.</param>
        /// <param name="messageHandler">Custom message handler.</param>
        public RestfulClient(string apiEndpoint, IRequestConfiguration baseConfig, HttpMessageHandler messageHandler)
        {
            client = CreateClient(messageHandler);
            client.BaseAddress = new Uri(apiEndpoint);

            this.baseConfig = baseConfig;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulClient"/> class.
        /// Constructor with base request configuration, custom message handler, and additional delegating handlers.
        /// </summary>
        /// <param name="apiEndpoint">Base endpoint URL(prefix).</param>
        /// <param name="baseConfig">Base request configuration.</param>
        /// <param name="messageHandler">Custom message handler.</param>
        /// <param name="delegatingHandlers">Custom delegating handlers.</param>
        public RestfulClient(string apiEndpoint, IRequestConfiguration baseConfig, HttpMessageHandler messageHandler, params DelegatingHandler[] delegatingHandlers)
        {
            client = CreateClient(messageHandler, delegatingHandlers);
            client.BaseAddress = new Uri(apiEndpoint);

            this.baseConfig = baseConfig;
        }
        #endregion

        #region Sync Methods

        /// <inheritdoc/>
        public T Get<T>(string action, IRequestConfiguration requestConfig = null)
        {
            return Task.Run(() => GetAsync<T>(action, requestConfig)).Result;
        }

        /// <inheritdoc/>
        public T Post<T>(string action, object content, IRequestConfiguration requestConfig = null)
        {
            return Task.Run(() => PostAsync<T>(action, content, requestConfig)).Result;
        }

        /// <inheritdoc/>
        public T Put<T>(string action, object content, IRequestConfiguration requestConfig = null)
        {
            return Task.Run(() => PutAsync<T>(action, content, requestConfig)).Result;
        }

        /// <inheritdoc/>
        public T Patch<T>(string action, object content, IRequestConfiguration requestConfig = null)
        {
            return Task.Run(() => PatchAsync<T>(action, content, requestConfig)).Result;
        }

        /// <inheritdoc/>
        public T Delete<T>(string action, IRequestConfiguration requestConfig = null)
        {
            return Task.Run(() => DeleteAsync<T>(action, requestConfig)).Result;
        }

        #endregion

        #region Async Methods

        /// <inheritdoc/>
        public async Task<T> GetAsync<T>(string action, IRequestConfiguration requestConfig = null)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, action);
            return await PerformRequestAsync<T>(httpRequest, requestConfig);
        }

        /// <inheritdoc/>
        public async Task<T> PostAsync<T>(string action, object content, IRequestConfiguration requestConfig = null)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, action)
            {
                Content = baseConfig.RequestContentSerializer.Serialize(content)
            };

            return await PerformRequestAsync<T>(httpRequest, requestConfig);
        }

        /// <inheritdoc/>
        public async Task<T> PutAsync<T>(string action, object content, IRequestConfiguration requestConfig = null)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Put, action)
            {
                Content = baseConfig.RequestContentSerializer.Serialize(content)
            };
            return await PerformRequestAsync<T>(httpRequest, requestConfig);
        }

        /// <inheritdoc/>
        public async Task<T> PatchAsync<T>(string action, object content, IRequestConfiguration requestConfig = null)
        {
            var method = new HttpMethod("PATCH");
            var httpRequest = new HttpRequestMessage(method, action)
            {
                Content = baseConfig.RequestContentSerializer.Serialize(content)
            };
            return await PerformRequestAsync<T>(httpRequest, requestConfig);
        }

        /// <inheritdoc/>
        public async Task<T> DeleteAsync<T>(string action, IRequestConfiguration requestConfig = null)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, action);
            return await PerformRequestAsync<T>(httpRequest, requestConfig);
        }
        #endregion

        #region Other Methods

        /// <summary>
        /// Adds a dictionary of headers to the given http request
        /// </summary>
        /// <param name="httpRequest">Http request to add the headers to</param>
        /// <param name="headers">Dictionary of headers</param>
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
                        var message = $"Error adding header \"{header.Key}\" "
                            + $"with value \"{header.Value}\" "
                            + $"to outgoing request to \"{httpRequest.RequestUri}\". "
                            + "Check for null-value.";

                        throw new RestfulClientException(message, httpRequest, HttpStatusCode.InternalServerError, ex);
                    }
                }
            }
        }

        /// <summary>
        /// Performs the request.
        /// </summary>
        /// <param name="httpRequest">Request object.</param>
        /// <param name="requestConfig">Restful configuration for this request. Overrides base configuration.</param>
        /// <typeparam name="T">The type to use when deserializing the response.</typeparam>
        /// <returns>Response.</returns>
        protected virtual async Task<T> PerformRequestAsync<T>(HttpRequestMessage httpRequest, IRequestConfiguration requestConfig = null)
        {
            // Merge configurations
            requestConfig?.ApplyBaseConfiguration(baseConfig);
            var mergedConfig = requestConfig ?? baseConfig;

            AddHeadersToRequest(httpRequest, mergedConfig.CustomRequestHeaders);

            var response = mergedConfig.CompletionOption.HasValue
                ? await client.SendAsync(httpRequest, mergedConfig.CompletionOption.Value)
                : await client.SendAsync(httpRequest);

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
                return await mergedConfig.ResponseContentDeserializer.Deserialize<T>(response);
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