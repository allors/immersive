// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PanelTest.cs" company="allors bvba">
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
    using global::Avalonia.Controls;

    using Allors.Immersive.Avalonia.Testers;

    using AllorsTestAvaloniaAssembly;

    using Xunit;

    public class PanelTest : AvaloniaTest
    {
        private DefaultWindow window;

        public PanelTest()
        {
            this.window = new DefaultWindow();
            this.ShowWindow(this.window);
        }

        [Fact]
        public void FindTesterByName()
        {
            var tester = new PanelTester("panel1");
            Assert.NotNull(tester.Target);
            Assert.IsAssignableFrom<global::Avalonia.Controls.Panel>(tester.Target);
        }
    }
}
