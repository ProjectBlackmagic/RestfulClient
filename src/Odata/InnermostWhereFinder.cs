// <copyright file="InnermostWhereFinder.cs" company="ProjectBlackmagic">
// RestfulClient
// Copyright (c) ProjectBlackmagic. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// Github: https://github.com/ProjectBlackmagic/RestfulClient.
// </copyright>

using System;
using System.Linq.Expressions;

namespace ProjectBlackmagic.RestfulClient.Odata
{
    /// <summary>
    /// Looks for innermost where
    /// </summary>
    internal class InnermostWhereFinder : ExpressionVisitor
    {
        private MethodCallExpression innermostWhereExpression;

        /// <summary>
        /// Gets innermost where
        /// </summary>
        /// <param name="expression">expression</param>
        /// <returns>Result</returns>
        public MethodCallExpression GetInnermostWhere(Expression expression)
        {
            Visit(expression);
            return innermostWhereExpression;
        }

        /// <summary>
        /// Visits expression node
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns>Results</returns>
        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            if (expression.Method.Name == "Where")
            {
                innermostWhereExpression = expression;
            }

            Visit(expression.Arguments[0]);

            return expression;
        }
    }
}
