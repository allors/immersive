// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SplitButtonTest.cs" company="allors bvba">
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

    public class SplitButtonTest : AvaloniaTest
    {
        private DefaultWindow window;

        public SplitButtonTest()
        {
            this.window = new DefaultWindow();
            this.ShowWindow(this.window);
        }

        [Fact]
        public void FindTesterByName()
        {
            var tester = new SplitButtonTester("splitButton1");
            Assert.NotNull(tester.Target);
            Assert.IsAssignableFrom<global::Avalonia.Controls.SplitButton>(tester.Target);
        }

        [Fact]
        public void ClickFiresEvent()
        {
            var fired = false;
            this.OnClickAction = _ => fired = true;

            var tester = new SplitButtonTester("splitButton1");
            tester.Click();

            Assert.True(fired);
        }

        private Action<TestingAvaloniaEventArgs> OnClickAction { get; set; }

        protected override void OnClick(TestingAvaloniaEventArgs args) => OnClickAction?.Invoke(args);
    }
}
