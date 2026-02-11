// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuItemTest.cs" company="allors bvba">
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
#if NETFRAMEWORK
namespace Allors.Immersive.Winforms.Tests
{
    using System.Windows.Forms;

    using Allors.Immersive.Winforms.Testers;

    using AllorsTestWindowsAssembly;

    using Xunit;


    public class MenuItemTest : WinformsTest
    {
        private DefaultForm form;
        public MenuItemTest()
        {
            this.form = new DefaultForm();
            this.form.Show();
        }

        [Fact]
        public void FindTesterByName()
        {
            var tester = new MenuItemTester("MenuItem1");
            Assert.NotNull(tester.Target);
            Assert.IsAssignableFrom<MenuItem>(tester.Target);
        }

        [Fact]
        public void click()
        {
            var treeView = new TreeViewTester("treeView1");
            treeView.SelectNode(new[]{0});

            var menuItem1 = new MenuItemTester("MenuItem1");
            menuItem1.Target.PerformClick();

            var textBoxTester = new TextBoxTester(this.form.Name, "textBox1");
            Assert.Equal("MenuItem1", textBoxTester.Target.Text);
        }
    }
}
#endif