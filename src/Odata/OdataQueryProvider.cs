// <copyright file="OdataQueryProvider.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Linq;
using System.Linq.Expressions;

namespace ProjectBlackmagic.RestfulClient.Odata
{
    /// <summary>
    /// Linq to Odata query provider
    /// </summary>
    /// <seealso cref="System.Linq.IQueryProvider" />
    public class OdataQueryProvider : IQueryProvider
    {
        /// <inheritdoc/>
        public IQueryable CreateQuery(Expression expression)
        {
            Type elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return (IQueryable)Activator.CreateInstance(
                    typeof(OdataCollection<>).MakeGenericType(elementType),
                    new object[] { this, expression });
            }
            catch (System.Reflection.TargetInvocationException tie)
            {
                throw tie.InnerException;
            }
        }

        /// <inheritdoc/>
        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return new OdataCollection<TResult>(this, expression);
        }

        /// <inheritdoc/>
        public object Execute(Expression expression)
        {
            return OdataQueryContext.Execute(expression, false);
        }

        /// <inheritdoc/>
        public TResult Execute<TResult>(Expression expression)
        {
            bool isEnumerable = typeof(TResult).Name == "IEnumerable`1";
            return (TResult)OdataQueryContext.Execute(expression, isEnumerable);
        }
    }
}
