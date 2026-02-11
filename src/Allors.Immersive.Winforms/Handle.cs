// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Handle.cs" company="allors bvba">
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

namespace Allors.Immersive.Winforms.Domain
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public class Handle 
    {
        private readonly Session session;
        private readonly ISubstitute substitute;

        internal Handle(Session session, ISubstitute substitute)
        {
            this.session = session;
            this.substitute = substitute;
        }

        public ISubstitute Substitute
        {
            get { return this.substitute; }
        }

        public Component Component
        {
            get { return this.substitute as Component; }
        }

        public Control Control
        {
            get { return this.substitute as Control; }
        }

        public Handle Parent
        {
            get
            {
                if (this.Control != null)
                {
                    var parent = this.Control.Parent;
                    return this.session.FindHandle(parent);
                }

                return null;
            }
        }

        public string Name
        {
            get
            {
                return this.substitute.SubstituteName;
            }
        }

        public Session Session 
        {
            get
            {
                return this.session;
            }
        }

        public override string ToString()
        {
            return this.Name + "[" + this.Substitute.GetHashCode() + "] " + this.Substitute.GetType().Name;
        }
    }
}
