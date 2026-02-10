// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstituteClass.cs" company="allors bvba">
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
    /// Marks a class as a substitute for another class. The weaver replaces the target class's members with those from the annotated class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SubstituteClassAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubstituteClassAttribute"/> class. The substitutable type is inferred by name.
        /// </summary>
        public SubstituteClassAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubstituteClassAttribute"/> class with an explicit target type.
        /// </summary>
        /// <param name="substitutableType">The type to substitute.</param>
        public SubstituteClassAttribute(Type substitutableType)
        {
            this.SubstitutableType = substitutableType;
        }

        /// <summary>
        /// Gets the type that this class substitutes.
        /// </summary>
        public Type SubstitutableType { get; }
    }
}