// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstituteMethod.cs" company="allors bvba">
//   Copyright 2008-2014 Allors bvba.
//   
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU Lesser General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//   
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU Lesser General Public License for more details.
//   
//   You should have received a copy of the GNU Lesser General Public License
//   along with this program.  If not, see http://www.gnu.org/licenses.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Immersive
{
    using System;

    /// <summary>
    /// Marks a method as a substitute for a method on another type. The weaver replaces the target method's body with this method's body.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SubstituteMethodAttribute : Attribute
    {
        private readonly Type _substitutableType;
        private readonly string _substituableMethodName;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubstituteMethodAttribute"/> class. The target method is matched by name.
        /// </summary>
        /// <param name="type">The type containing the method to substitute.</param>
        public SubstituteMethodAttribute(Type type)
        {
            this._substitutableType = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubstituteMethodAttribute"/> class with an explicit target method name.
        /// </summary>
        /// <param name="type">The type containing the method to substitute.</param>
        /// <param name="methodName">The name of the method to substitute.</param>
        public SubstituteMethodAttribute(Type type, string methodName) : this(type)
        {
            this._substituableMethodName = methodName;
        }

        /// <summary>
        /// Gets the type containing the method to substitute.
        /// </summary>
        public Type SubstitutableType
        {
            get { return this._substitutableType; }
        }

        /// <summary>
        /// Gets the name of the method to substitute. When <c>null</c>, the annotated method's own name is used.
        /// </summary>
        public string SubstituableMethodName
        {
            get { return this._substituableMethodName; }
        }
    }
}