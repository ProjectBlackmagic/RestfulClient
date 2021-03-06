﻿// <copyright file="AuthenticatorConfigBase.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Security.Cryptography.X509Certificates;

namespace ProjectBlackmagic.RestfulClient.Authentication
{
    /// <summary>
    /// Base class for authenticator configurations.
    /// </summary>
    public abstract class AuthenticatorConfigBase
    {
        /// <summary>
        /// Gets certificate from certificate store.
        /// </summary>
        /// <param name="certificateName">Certificate name</param>
        /// <param name="storeName">Name of certificate store containing the certificate. Defaults to personal store.</param>
        /// <param name="storeLocation">Location of the certificate store containing the certificate. Defaults to local machine store.</param>
        /// <returns>Certificate from store</returns>
        protected static X509Certificate2 GetCertificateFromStore(string certificateName, StoreName storeName = StoreName.My, StoreLocation storeLocation = StoreLocation.LocalMachine)
        {
            using (var store = new X509Store(storeName, storeLocation))
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection collection = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, "CN=" + certificateName, true);

                if (collection.Count != 1)
                {
                    throw new Exception("Unable to find unique certificate in store with name: " + certificateName);
                }
                else
                {
                    var certificate = collection[0];
                    try
                    {
                        // Try to get the private key to make sure we have access to it
                        var provider = certificate.GetRSAPrivateKey();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Unable to access private key for certificate with name: " + certificateName, ex);
                    }

                    return certificate;
                }
            }
        }
    }
}
