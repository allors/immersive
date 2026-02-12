// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckBoxTest.cs" company="allors bvba">
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
namespace Allors.Immersive.Avalonia.Tests
{
    using System;

    using global::Avalonia.Controls;

    using Allors.Immersive.Avalonia.Testers;

    using AllorsTestAvaloniaAssembly;

    using Xunit;

    public class CheckBoxTest : AvaloniaTest
    {
        private DefaultWindow window;

        public CheckBoxTest()
        {
            this.window = new DefaultWindow();
            this.ShowWindow(this.window);
        }

        [Fact]
        public void FindTesterByName()
        {
            var tester = new CheckBoxTester("checkBox1");
            Assert.NotNull(tester.Target);
            Assert.IsAssignableFrom<CheckBox>(tester.Target);
        }

        [Fact]
        public void ToggleFiresEvent()
        {
            var fired = false;
            this.OnIsCheckedChangedAction = _ => fired = true;

            var tester = new CheckBoxTester("checkBox1");
            tester.Target.IsChecked = true;

            Assert.True(fired);
        }

        private Action<TestingAvaloniaEventArgs> OnIsCheckedChangedAction { get; set; }

        protected override void OnIsCheckedChanged(TestingAvaloniaEventArgs args) => OnIsCheckedChangedAction?.Invoke(args);
    }
}
