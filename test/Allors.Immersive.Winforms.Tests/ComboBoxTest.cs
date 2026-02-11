// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComboBoxTest.cs" company="allors bvba">
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
    using System.Linq;
    using System.Windows.Forms;

    using Allors.Immersive.Winforms.Testers;

    using AllorsTestWindowsAssembly;

    using Xunit;

    
    public class ComboBoxTest : WinformsTest
    {
        private DefaultForm form;
        public ComboBoxTest()
        {
            this.form = new DefaultForm();
            this.form.Show();
        }

        [Fact]
        public void FindTesterByName()
        {
            var tester = new ComboBoxTester("comboBox1");
            Assert.NotNull(tester.Target);
            Assert.IsAssignableFrom<ComboBox>(tester.Target);
        }

        [Fact]
        public void SelectItem()
        {
            var a1 = this.form.ACollection.First();
            var a3 = this.form.ACollection.Last();

            var comboBox = new ComboBoxTester("comboBox1");
            Assert.Equal(a1, comboBox.Target.SelectedItem);

            comboBox.Select<A>(v => Equals(a3, v));
            Assert.Equal(a3, comboBox.Target.SelectedItem);

            var textBox1 = new TextBoxTester(this.form.Name, "textBox1");
            Assert.Equal(a3.FirstX, textBox1.Target.Text);
        }

        [Fact]
        public void SelectText()
        {
            var a1 = this.form.ACollection.First();
            var a3 = this.form.ACollection.Last();

            var comboBox = new ComboBoxTester("comboBox2");
            Assert.Equal(a1.FirstX, comboBox.Target.SelectedItem);

            comboBox.Select<string>(v => Equals(a3.FirstX, v));
            Assert.Equal(a3.FirstX, comboBox.Target.SelectedItem);

            var textBox1 = new TextBoxTester("textBox2");
            Assert.Equal(a3.FirstX, textBox1.Target.Text);
        }
    }
}