// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormTest.cs" company="allors bvba">
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
    using System;
    using System.Windows.Forms;

    using Allors.Immersive.Winforms;
    using Allors.Immersive.Winforms.Domain;
    using Allors.Immersive.Winforms.Testers;

    using AllorsTestWindowsAssembly;

    using Xunit;

    
    public class FormTest : WinformsTest
    {
        private DefaultForm form;
        public FormTest()
        {
            this.form = new DefaultForm();
            this.form.Show();
        }
        public override void Dispose()
        {
            this.form.Dispose();
            this.form = null;
            base.Dispose();
        }

        protected override void OnCheckedChanged(TestingWinformsEventArgs args)
        {
        }

        protected override void OnShown(TestingWinformsEventArgs args)
        {
            var tester = args.Handle;
            if (tester != null && tester.Name.Equals("DialogForm"))
            {
                var session = Session.Singleton;

                var textBox1 = new TextBoxTester("DialogForm", "textBox1");
                var textBox2 = new TextBoxTester("DialogForm", "textBox2");
                var button1 = new ButtonTester("DialogForm", "button1");

                Assert.Equal(string.Empty, textBox1.Target.Text);
                Assert.Equal(string.Empty, textBox2.Target.Text);

                textBox1.Target.Text = "Ok!";
                button1.Click();

                Assert.Equal("Ok!", textBox1.Target.Text);
                Assert.Equal("Ok!", textBox2.Target.Text);

                var okButton = new ButtonTester("DialogForm", "okButton");
                okButton.Click();
            }
        }

        [Fact(Timeout = 30000)]
        public void ShowDialog()
        {
            var button2 = new ButtonTester("button2");
            button2.Click();

            var dialogForm = new FormTester("DialogForm");
            Assert.Equal(DialogResult.OK, dialogForm.Target.DialogResult);

            var button4 = new ButtonTester("button4");
            button4.Click();

            dialogForm = new FormTester("DialogForm");
            Assert.Equal(DialogResult.OK, dialogForm.Target.DialogResult);
        }

        [Fact(Timeout = 30000)]
        public void ShowDialog2()
        {
            var button4 = new ButtonTester("button4");
            button4.Click();
            button4.Click();
        }
    }
}