// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumericUpDownTest.cs" company="allors bvba">
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

namespace Allors.Immersive.Winforms.Tests
{
    using System.Windows.Forms;

    using Allors.Immersive.Winforms.Testers;

    using AllorsTestWindowsAssembly;

    using Xunit;

    
    public class NumericUpDownTest : WinformsTest
    {
        private DefaultForm form;
        public NumericUpDownTest()
        {
            this.form = new DefaultForm();
            this.form.Show();
        }

        [Fact]
        public void FindTesterByName()
        {
            var tester = new NumericUpDownTester("numericUpDown1");
            Assert.NotNull(tester.Target);
            Assert.IsAssignableFrom<NumericUpDown>(tester.Target);
        }

        [Fact]
        public void SetValue()
        {
            var numericUpDown = new NumericUpDownTester("numericUpDown1");
            numericUpDown.Target.Value = 42;
            Assert.Equal(42, numericUpDown.Target.Value);
        }

        [Fact]
        public void SetMinMax()
        {
            var numericUpDown = new NumericUpDownTester("numericUpDown1");
            numericUpDown.Target.Minimum = 5;
            numericUpDown.Target.Maximum = 500;
            numericUpDown.Target.Value = 250;

            Assert.Equal(5, numericUpDown.Target.Minimum);
            Assert.Equal(500, numericUpDown.Target.Maximum);
            Assert.Equal(250, numericUpDown.Target.Value);
        }
    }
}