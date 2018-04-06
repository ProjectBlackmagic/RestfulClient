// <copyright file="FormUrlEncodedStringContent.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System.Net.Http;
using System.Text;

namespace ProjectBlackmagic.RestfulClient.Content
{
    /// <summary>
    /// FormUrlEncodedStringContent defines a subtype of <see cref="StringContent"/> for JSON content.
    /// </summary>
    public class FormUrlEncodedStringContent : StringContent
    {
        private static readonly Encoding StringEncoding = Encoding.UTF8;
        private static readonly string MediaType = "application/x-www-form-urlencoded";

        /// <summary>
        /// Initializes a new instance of the <see cref="FormUrlEncodedStringContent"/> class.
        /// </summary>
        /// <param name="content">String content</param>
        public FormUrlEncodedStringContent(string content)
            : base(content, StringEncoding, MediaType)
        {
        }
    }
}
