// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BindingNavigatorTest.cs" company="allors bvba">
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
    using System.CodeDom;
    using System.Windows.Forms;

    using Allors.Immersive.Winforms.Testers;

    using AllorsTestWindowsAssembly;

    using Xunit;

    
    public class BindingNavigatorTest : WinformsTest
    {
        private DefaultForm form;
        public BindingNavigatorTest()
        {
            this.form = new DefaultForm();
            this.form.Show();
        }

        [Fact]
        public void FindTesterByName()
        {
            var tester = new BindingNavigatorTester("bindingNavigator1");
            Assert.NotNull(tester.Target);
            Assert.IsAssignableFrom<BindingNavigator>(tester.Target);
        }

        [Fact]
        public void TesterHandlesEvents()
        {
            var bindingNavigator = new BindingNavigatorTester("bindingNavigator1");
            
            var datagridview = new DataGridViewTester("dataGridView1");
            Assert.Equal(3, datagridview.Target.RowCount);

            Assert.Equal("bindingNavigator1", bindingNavigator.Target.Name);

            Assert.Equal("of 3", bindingNavigator.Target.CountItem.Text);
          
            bindingNavigator.AddNewItem();

            Assert.Equal(4, datagridview.Target.RowCount);
            Assert.Equal("of 4", bindingNavigator.Target.CountItem.Text);
        }

        [Fact]
        public void CustomConstructor()
        {
            var button = new ButtonTester("buttonAddBindingNavigator");
            button.Click();

            var panel = new PanelTester("emptyPanel");
            Assert.Equal(1, panel.Target.Controls.Count);
        }
    }
}