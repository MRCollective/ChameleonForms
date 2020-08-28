/*
 * License for this code from https://github.com/alexmurari/Exprelsior

MIT License

Copyright (c) 2019 Alexandre Murari Junior

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ChameleonForms.Utils
{
    /// <summary>
    ///     Provides static methods to create <see cref="Expression{TDelegate}" /> instances.
    /// </summary>
    public static class ExpressionBuilder
    {
        /// <summary>
        ///     Creates a lambda expression that represents an accessor to a property from an object of type <typeparamref name="T" />.
        /// </summary>
        /// <param name="propertyNameOrPath">
        ///     The name or the path to the property to be accessed composed of simple dot-separated property access expressions.
        /// </param>
        /// <typeparam name="T">
        ///     The type that contains the property to be accessed.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type of the accessed property used as the delegate return type.
        /// </typeparam>
        /// <returns>
        ///     The built <see cref="Expression{TDelegate}" /> instance representing the property accessor.
        /// </returns>
        public static Expression<Func<T, TResult>> CreateAccessor<T, TResult>(string propertyNameOrPath)
        {
            if (string.IsNullOrWhiteSpace(propertyNameOrPath))
                throw new ArgumentNullException(nameof(propertyNameOrPath));

            var (parameter, accessor) = BuildAccessor<T>(propertyNameOrPath);
            //var conversion = Expression.Convert(accessor, typeof(TResult)); This was in the Exprelsior code, but breaks ChameleonForms, so removed it

            return Expression.Lambda<Func<T, TResult>>(accessor, parameter);
        }

        /// <summary>
        ///     Creates a <see cref="MemberExpression" /> that represents accessing a property from an object of type
        ///     <typeparamref name="T" />.
        /// </summary>
        /// <param name="propertyNameOrPath">
        ///     The name or the path to the property to be accessed composed of simple dot-separated property access expressions.
        /// </param>
        /// <typeparam name="T">
        ///     The type that contains the property to be accessed.
        /// </typeparam>
        /// <returns>
        ///     The <see cref="ParameterExpression" /> representing a parameter of the type that contains
        ///     the accessed property and the <see cref="MemberExpression" /> representing the accessor to the property.
        /// </returns>
        private static (ParameterExpression Parameter, MemberExpression Accessor) BuildAccessor<T>(string propertyNameOrPath)
        {
            if (string.IsNullOrWhiteSpace(propertyNameOrPath))
                throw new ArgumentNullException(nameof(propertyNameOrPath));

            var param = Expression.Parameter(typeof(T));
            var accessor = propertyNameOrPath.Split('.').Aggregate<string, MemberExpression>(
                null,
                (current, property) => Expression.Property((Expression)current ?? param, property.Trim()));

            return (param, accessor);
        }
    }
}
