// <copyright file="OdataQueryContext.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ProjectBlackmagic.RestfulClient.Odata
{
    /// <summary>
    /// Odata Query Context
    /// </summary>
    public class OdataQueryContext
    {
        /// <summary>
        /// Executes an expression
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <param name="isEnumerable">Indicates when the returning object should be IEnummerable</param>
        /// <returns>Expression result</returns>
        internal static object Execute(Expression expression, bool isEnumerable)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);

            return Activator.CreateInstance(
                    typeof(OdataCollection<>).MakeGenericType(elementType),
                    new object[] { });
        }
    }
}
