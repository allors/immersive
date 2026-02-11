// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressBarTest.cs" company="allors bvba">
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

    
    public class ProgressBarTest : WinformsTest
    {
        private DefaultForm form;
        public ProgressBarTest()
        {
            this.form = new DefaultForm();
            this.form.Show();
        }


        [Fact]
        public void FindTesterByName()
        {
            var tester = new ProgressBarTester("progressBar1");
            Assert.NotNull(tester.Target);
            Assert.IsAssignableFrom<ProgressBar>(tester.Target);
        }

        [Fact]
        public void SetValue()
        {
            var progressBar = new ProgressBarTester("progressBar1");
            progressBar.Target.Value = 50;
            Assert.Equal(50, progressBar.Target.Value);
        }

        [Fact]
        public void SetMinMax()
        {
            var progressBar = new ProgressBarTester("progressBar1");
            progressBar.Target.Minimum = 10;
            progressBar.Target.Maximum = 200;
            progressBar.Target.Value = 100;

            Assert.Equal(10, progressBar.Target.Minimum);
            Assert.Equal(200, progressBar.Target.Maximum);
            Assert.Equal(100, progressBar.Target.Value);
        }
    }
}