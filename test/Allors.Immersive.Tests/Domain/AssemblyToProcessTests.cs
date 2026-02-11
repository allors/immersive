// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyToProcessTests.cs" company="allors bvba">
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
namespace Allors.Immersive.Tests
{
    using System;

    using Xunit;

    public class AssemblyToProcessTests
    {
        [Fact]
        public void Default()
        {
            var testFormType = Fixture.BeforeAssembly.GetType("Allors.Immersive.AssemblyToProcess.TestForm");
            var testForm = (dynamic)Activator.CreateInstance(testFormType);

            Assert.Equal("Allors.Immersive.AssemblyToProcess.TestForm", testForm.GetType().FullName);

            Assert.Equal("Allors.Immersive.AssemblyReferenced.Button", testForm.Button1.GetType().FullName);
            Assert.Equal("Allors.Immersive.AssemblyReferenced.TextBox", testForm.TextBox1.GetType().FullName);
            Assert.Equal("Allors.Immersive.AssemblyReferenced.SealedSingle", testForm.SealedSingle.GetType().FullName);
            Assert.Equal("Allors.Immersive.AssemblyReferenced.SealedHierarchy", testForm.SealedHierarchy.GetType().FullName);

            var nadaType = Fixture.BeforeAssembly.GetType("Allors.Immersive.AssemblyToProcess.Nada");
            var nada = (dynamic)Activator.CreateInstance(nadaType);

            Assert.Throws<Exception>(() => nada.TestSealedSingle("1"));
            Assert.Throws<Exception>(() => nada.TestSealedHierarchy("2"));
            Assert.Throws<Exception>(() => nada.TestSealedHierarchyAbstract("3"));
        }
    }
}