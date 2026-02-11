// --------------------------------------------------------------------------------------------------------------------
// <copyright file="A.cs" company="allors bvba">
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

    public class A
    {
        private B b;
        private string firstX;
        private string firstY;
        private bool iAmInvisible;
        private decimal second;

        public A()
        {
            iAmInvisible = true;
            firstX = "A First X";
            firstY = "A First Y";
            second = 1.1m;

            b = new B();
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
        public B B
        {
            get { return b; }
            set { b = value; }
        }
    }
}