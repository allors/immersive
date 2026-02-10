// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstituteClass.cs" company="allors bvba">
//   Copyright 2008-2014 Allors bvba.
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

namespace Allors.Immersive.Weaver
{
    using System;
    using System.Linq;
    using dnlib.DotNet;

    public class SubstituteClass
    {
        public SubstituteClass(ModuleWeaver moduleWeaver, TypeDef typeDef)
        {
            this.ModuleWeaver = moduleWeaver;
            this.TypeDefinition = typeDef;

            var attribute = this.TypeDefinition.CustomAttributes.FirstOrDefault(v => v.AttributeType.FullName.Equals(Attributes.SubstituteClassAttribute));

            if (attribute != null)
            {
                if (attribute.HasConstructorArguments)
                {
                    this.IsBaseSubstitution = false;

                    var argValue = attribute.ConstructorArguments[0].Value;
                    if (argValue is TypeDefOrRefSig typeSig)
                    {
                        this.SubstitutableFullName = typeSig.FullName;
                    }
                    else if (argValue is ITypeDefOrRef typeRef)
                    {
                        this.SubstitutableFullName = typeRef.FullName;
                    }
                    else
                    {
                        this.SubstitutableFullName = argValue?.ToString();
                    }
                }
                else
                {
                    if (typeDef.BaseType == null)
                    {
                        throw new Exception($"Base type is required for {typeDef.FullName}");
                    }

                    this.IsBaseSubstitution = true;
                    this.SubstitutableFullName = typeDef.BaseType.FullName;
                }
            }

            this.ModuleWeaver.WriteInfo($"SubstituteClass: {this.SubstitutableFullName}");
        }

        public ModuleWeaver ModuleWeaver { get; }

        public TypeDef TypeDefinition { get; }

        public string SubstitutableFullName { get; }

        public bool IsBaseSubstitution { get; }

        public MethodDef Constructor => Helper.GetFirstConstructor(this.TypeDefinition);

        public override string ToString()
        {
            return this.TypeDefinition.ToString();
        }
    }
}
