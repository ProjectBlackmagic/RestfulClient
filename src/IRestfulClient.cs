// <copyright file="IRestfulClient.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectBlackmagic.RestfulClient
{
    /// <summary>
    /// The clients interface. Defines Restful methods
    /// GET, POST, PUT, PATCH and DELETE in both synchronous and async variants
    /// </summary>
    public interface IRestfulClient
    {
        #region Sync Methods

        /// <summary>
        /// Performs HTTP Get method
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.
        /// </returns>
        T Get<T>(string action, Dictionary<string, string> headers = null);

        /// <summary>
        /// Performs HTTP Post method
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        T Post<T>(string action, object content, Dictionary<string, string> headers = null);

        /// <summary>
        /// Performs HTTP Put method
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        T Put<T>(string action, object content, Dictionary<string, string> headers = null);

        /// <summary>
        /// Performs HTTP Patch method
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        T Patch<T>(string action, object content, Dictionary<string, string> headers = null);

        /// <summary>
        /// Performs HTTP Delete method
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        T Delete<T>(string action, Dictionary<string, string> headers = null);
        #endregion

        #region Async Methods

        /// <summary>
        /// Performs HTTP Get method (async)
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        Task<T> GetAsync<T>(string action, Dictionary<string, string> headers = null);

        /// <summary>
        /// Performs HTTP Post method (async)
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        Task<T> PostAsync<T>(string action, object content, Dictionary<string, string> headers = null);

        /// <summary>
        /// Performs HTTP Put method (async)
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        Task<T> PutAsync<T>(string action, object content, Dictionary<string, string> headers = null);

        /// <summary>
        /// Performs HTTP Patch method (async)
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="content">Content object to be serialized as request body.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        Task<T> PatchAsync<T>(string action, object content, Dictionary<string, string> headers = null);

        /// <summary>
        /// Performs HTTP Delete method (async)
        /// </summary>
        /// <param name="action">URL action value. It will be concatenated with base URL to create request URL.</param>
        /// <param name="headers">HTTP headers to be included in the request.</param>
        /// <typeparam name="T">The type to use when deserializing the response. Use type <see cref="Empty" /> if the response is not expected to have a body.</typeparam>
        /// <returns>Deserialized to type T object from response body.</returns>
        Task<T> DeleteAsync<T>(string action, Dictionary<string, string> headers = null);
        #endregion
    }
}