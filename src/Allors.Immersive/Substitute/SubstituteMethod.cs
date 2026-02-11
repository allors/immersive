// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstituteMethod.cs" company="allors bvba">
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
    using dnlib.DotNet;

    public class SubstituteMethod
    {
        public SubstituteMethod(ModuleWeaver moduleWeaver, MethodDef methodDef, CustomAttribute customAttribute)
        {
            this.ModuleWeaver = moduleWeaver;

            this.Definition = methodDef;

            var argValue = customAttribute.ConstructorArguments[0].Value;
            if (argValue is TypeDefOrRefSig typeSig)
            {
                this.FullTypeName = typeSig.FullName;
            }
            else if (argValue is ITypeDefOrRef typeRef)
            {
                this.FullTypeName = typeRef.FullName;
            }
            else
            {
                this.FullTypeName = argValue?.ToString();
            }

            this.MethodName = methodDef.Name;
            if (customAttribute.ConstructorArguments.Count > 1)
            {
                this.MethodName = customAttribute.ConstructorArguments[1].Value?.ToString();
            }

            this.ModuleWeaver.WriteInfo($"SubstituteMethod: {this.FullTypeName}.{this.MethodName}");
        }

        public ModuleWeaver ModuleWeaver { get; }

        public MethodDef Definition { get; }

        public string FullTypeName { get; }

        public string MethodName { get; }

        public MethodDef MethodDefinition => this.Definition;

        public override string ToString()
        {
            return this.Definition.ToString();
        }

        internal bool Matches(IMethod operand)
        {
            if (this.MethodName.Equals(operand.Name))
            {
                if (this.FullTypeName.Equals(operand.DeclaringType?.FullName))
                {
                    var operandSig = operand.MethodSig;
                    if (operandSig != null && this.Definition.Parameters.Count - (this.Definition.IsStatic ? 0 : 1) == operandSig.Params.Count)
                    {
                        var defParams = this.Definition.MethodSig.Params;
                        for (int i = 0; i < defParams.Count; i++)
                        {
                            if (!defParams[i].FullName.Equals(operandSig.Params[i].FullName))
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
