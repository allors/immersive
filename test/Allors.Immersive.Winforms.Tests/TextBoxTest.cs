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
    using System.Windows.Forms;

    using Allors.Immersive.Winforms.Testers;

    using AllorsTestWindowsAssembly;

    using Xunit;

    
    public class TextBoxTest : WinformsTest
    {
        private DefaultForm form;
        public TextBoxTest()
        {
            this.form = new DefaultForm();
            this.form.Show();
        }

        [Fact]
        public void FindTesterByName()
        {
            var tester = new TextBoxTester(this.form.Name, "textBox1");
            Assert.NotNull(tester.Target);
            Assert.IsAssignableFrom<TextBox>(tester.Target);
        }

        [Fact]
        public void MultipleReference()
        {
            var textBox1a = new TextBoxTester(this.form.Name, "textBox1");
            var textBox1b = new TextBoxTester(this.form.Name, "textBox1");


            textBox1a.Target.Text = "Ok!";

            Assert.Equal("Ok!", textBox1a.Target.Text);
            Assert.Equal("Ok!", textBox1b.Target.Text);
        }

        [Fact]
        public void Read()
        {
            var textBox1 = new TextBoxTester(this.form.Name, "textBox1");
            var textBox2 = new TextBoxTester("textBox2");
            var button1 = new ButtonTester("button1");

            Assert.Equal(string.Empty, textBox1.Target.Text);
            Assert.Equal(string.Empty, textBox2.Target.Text);

            textBox1.Target.Text = "Ok!";
            button1.Click();

            Assert.Equal("Ok!", textBox1.Target.Text);
            Assert.Equal("Ok!", textBox2.Target.Text);
        }

        [Fact]
        public void AddTextBox()
        {
            // Adds a textbox to the controls collection
            var button = new ButtonTester("buttonAddTextBox");
            button.Click();

            var textBox = new TextBoxTester(this.form.Name, "textBoxHelloAllors");
            Assert.Equal(@"I'm added to the controls collection", textBox.Target.Text);

            var panel2 = new PanelTester("panel2");
            foreach (Control targetControl in panel2.Target.Controls)
            {
                if (targetControl is TextBox)
                {
                    Assert.Equal(@"I'm added to the panels collection", targetControl.Text);
                }
            }
        }
    }
}