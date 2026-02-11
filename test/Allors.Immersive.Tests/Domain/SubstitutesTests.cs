// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstitutesTests.cs" company="allors bvba">
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
    using System.IO;

    using Allors.Immersive.Weaver;

    using dnlib.DotNet;

    using Xunit;

    public class SubstitutesTests
    {
        private readonly Substitutes substitutes;

        public SubstitutesTests()
        {
            var assemblyPath = new DirectoryInfo("Allors.Immersive.AssemblyToImmerse.dll").FullName;
            var moduleDef = ModuleDefMD.Load(File.ReadAllBytes(assemblyPath));

            this.substitutes = new Substitutes(Fixture.ModuleWeaver, moduleDef);
        }

        [Fact]
        public void Default()
        {
            Assert.Equal(5, this.substitutes.SubstituteClasses.Count);

            var formSubstitute = this.substitutes.SubstituteClasses["Allors.Immersive.AssemblyToImmerse.Form"];
            var buttonSubstitute = this.substitutes.SubstituteClasses["Allors.Immersive.AssemblyToImmerse.Button"];
            var sealedSingleSubstitute = this.substitutes.SubstituteClasses["Allors.Immersive.AssemblyToImmerse.SealedSingle"];
            var sealedHierarchySubstitute = this.substitutes.SubstituteClasses["Allors.Immersive.AssemblyToImmerse.SealedHierarchy"];
            var sealedAbstractClassSubstitute = this.substitutes.SubstituteClasses["Allors.Immersive.AssemblyToImmerse.SealedAbstractClass"];

            Assert.NotNull(formSubstitute);
            Assert.NotNull(buttonSubstitute);
            Assert.NotNull(sealedSingleSubstitute);
            Assert.NotNull(sealedHierarchySubstitute);
            Assert.NotNull(sealedAbstractClassSubstitute);
        }
    }
}
