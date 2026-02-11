// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxTest.cs" company="allors bvba">
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
    using Allors.Immersive.Winforms.Testers;

    using AllorsTestWindowsAssembly;

    using Xunit;

    
    public class UserControlTest : WinformsTest
    {
        private DefaultForm form;
        public UserControlTest()
        {
            this.form = new DefaultForm();
            this.form.Show();
        }

        [Fact]
        public void FindTesterByName()
        {
            var tester = new UserControlTester("defaultUserControl1");
            Assert.NotNull(tester.Target);
            Assert.IsAssignableFrom<DefaultUserControl>(tester.Target);
        }

        [Fact]
        public void SameNameOnDifferentParents()
        {
            var button1 = new ButtonTester("button1");

            var defaultUserControl = new UserControlTester("defaultUserControl1");

            var textBox1OnForm = new TextBoxTester(this.form.Name, "textBox1");
            var textBox2OnForm = new TextBoxTester(this.form.Name, "textBox2");
            var textBox1OnUserControl = new TextBoxTester(defaultUserControl.Target.Name, "textBox1");

            textBox1OnForm.Target.Text = "OkForm!";
            button1.Click();

            Assert.Equal("OkForm!", textBox1OnForm.Target.Text);
            Assert.Equal("OkForm!", textBox2OnForm.Target.Text);
            Assert.NotEqual("OkForm!", textBox1OnUserControl.Target.Text);
        }
    }
}