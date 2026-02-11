// --------------------------------------------------------------------------------------------------------------------
// <copyright file="B.cs" company="allors bvba">
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
namespace AllorsTestWindowsAssembly
{
    using System.ComponentModel;

    public class B
    {
        private C c;
        private string firstX;
        private string firstY;
        private bool iAmInvisible;
        private decimal second;

        public B()
        {
            iAmInvisible = true;
            firstX = "B First X";
            firstY = "B First Y";
            second = 2.2m;

            c = new C();
        }

        [Browsable(false)]
        public bool IAmInvisible
        {
            get { return iAmInvisible; }
            set { iAmInvisible = value; }
        }

        [Category("First")]
        public string FirstX
        {
            get { return firstX; }
            set { firstX = value; }
        }

        [Category("First")]
        public string FirstY
        {
            get { return firstY; }
            set { firstY = value; }
        }

        [Category("Second")]
        public decimal Second
        {
            get { return second; }
            set { second = value; }
        }

        [Browsable(true)]
        [TypeConverter(typeof (ExpandableObjectConverter))]
        public C C
        {
            get { return c; }
            set { c = value; }
        }
    }
}