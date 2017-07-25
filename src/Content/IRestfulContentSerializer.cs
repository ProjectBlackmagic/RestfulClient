// <copyright file="IRestfulContentSerializer.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectBlackmagic.RestfulClient.Content
{
    public interface IRestfulContentSerializer
    {
        HttpContent Serialize(object content);

        Task<string> Deserialize(HttpResponseMessage responseMessage);

        Task<T> Deserialize<T>(HttpResponseMessage responseMessage);
    }
}
