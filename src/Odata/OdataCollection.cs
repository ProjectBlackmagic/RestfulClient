// <copyright file="OdataCollection.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ProjectBlackmagic.RestfulClient.Odata
{
    /// <summary>
    /// IQueriable implementation for Odata.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <seealso cref="System.Linq.IOrderedQueryable{TData}" />
    public class OdataCollection<TData> : IOrderedQueryable<TData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdataCollection{TData}"/> class.
        /// </summary>
        public OdataCollection()
        {
            Provider = new OdataQueryProvider();
            Expression = Expression.Constant(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OdataCollection{TData}"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="expression">The expression.</param>
        /// <exception cref="System.ArgumentNullException">
        /// provider
        /// or
        /// expression
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">expression</exception>
        public OdataCollection(OdataQueryProvider provider, Expression expression)
        {
            if (expression == null || !typeof(IQueryable<TData>).IsAssignableFrom(expression.Type))
            {
                throw new ArgumentOutOfRangeException("expression");
            }

            Provider = provider ?? throw new ArgumentNullException("provider");
            Expression = expression;
        }

        /// <inheritdoc/>
        public Type ElementType => typeof(TData);

        /// <inheritdoc/>
        public Expression Expression { get; private set; }

        /// <inheritdoc/>
        public IQueryProvider Provider { get; private set; }

        /// <inheritdoc/>
        public IEnumerator<TData> GetEnumerator() => Provider.Execute<IEnumerable<TData>>(Expression).GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => Provider.Execute<IEnumerable>(Expression).GetEnumerator();
    }
}
