// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tester.cs" company="allors bvba">
//   Copyright 2008-2026 Allors bv.
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
namespace Allors.Immersive.Avalonia
{
    using System;

    using Allors.Immersive.Avalonia.Domain;

    public class Tester<T> where T : ISubstitute
    {
        private readonly Handle handle;

        public Tester(Handle handle)
        {
            this.handle = handle ?? throw new ArgumentNullException(nameof(handle), "No matching control found. Verify the control name(s) and that the window has been shown.");

            if (handle.Substitute is not T)
            {
                throw new InvalidCastException($"Handle substitute is {handle.Substitute.GetType().Name}, expected {typeof(T).Name}.");
            }
        }

        public Tester(params string[] names) : this(Session.Singleton.FindHandle(names))
        {
        }

        public Handle Handle
        {
            get { return this.handle; }
        }

        public T Target
        {
            get { return (T) this.handle.Substitute; }
        }
    }
}
