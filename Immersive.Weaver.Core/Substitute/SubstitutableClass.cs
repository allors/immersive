// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubstitutableClass.cs" company="allors bvba">
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

namespace Immersive.Weaver
{
    using System.Collections.Generic;
    using System.Linq;

    using dnlib.DotNet;
    using dnlib.DotNet.Emit;

    public class SubstitutableClass
    {
        public SubstitutableClass(ModuleWeaver moduleWeaver, TypeDef typeDef)
        {
            this.ModuleWeaver = moduleWeaver;
            this.TypeDefinition = typeDef;

            this.ModuleWeaver.WriteInfo($"Substitutable: {this.TypeDefinition.FullName}");
        }

        public ModuleWeaver ModuleWeaver { get; }

        public ModuleDef ModuleDefinition => this.ModuleWeaver.ModuleDefinition;

        public TypeDef TypeDefinition { get; }

        public override string ToString()
        {
            return this.TypeDefinition.ToString();
        }

        internal void Substitute(Substitutes substitutes)
        {
            if (this.TypeDefinition.Name != "<Module>")
            {
                var importer = new Importer(this.ModuleDefinition);

                this.SubstituteConstructors(substitutes, importer);
                this.SubstituteBaseType(substitutes, importer);
                this.SubstituteMethods(substitutes, importer);
                this.SubstituteFields(substitutes, importer);
                this.SubstituteLocalVariables(substitutes, importer);
                this.SubstituteReturnTypes(substitutes, importer);
            }
        }

        private void SubstituteConstructors(Substitutes substitutes, Importer importer)
        {
            foreach (var constructor in Helper.GetConstructors(this.TypeDefinition))
            {
                if (constructor.HasBody)
                {
                    foreach (var instruction in constructor.Body.Instructions)
                    {
                        if (instruction.OpCode.Equals(OpCodes.Call))
                        {
                            if (instruction.Operand is IMethod operand)
                            {
                                var operandDeclaringType = operand.DeclaringType;

                                var substitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(operandDeclaringType?.FullName);

                                if (substitute != null)
                                {
                                    if (operand.Name.Equals(".ctor"))
                                    {
                                        var substituteConstructorDefinition = substitute.Constructor;
                                        var substituteConstructorReference = importer.Import(substituteConstructorDefinition);
                                        instruction.Operand = substituteConstructorReference;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var constructorsAndMethods = new List<MethodDef>(Helper.GetConstructors(this.TypeDefinition));
            constructorsAndMethods.AddRange(this.TypeDefinition.Methods);

            foreach (MethodDef method in constructorsAndMethods)
            {
                if (method.HasBody)
                {
                    foreach (var instruction in method.Body.Instructions)
                    {
                        if (instruction.OpCode.Equals(OpCodes.Newobj))
                        {
                            if (instruction.Operand is IMethod operand)
                            {
                                var operandDeclaringType = operand.DeclaringType;

                                var substitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(operandDeclaringType?.FullName);

                                if (substitute != null)
                                {
                                    var substituteConstructorDefinition = substitute.Constructor;
                                    var substituteConstructorReference = importer.Import(substituteConstructorDefinition);
                                    instruction.Operand = substituteConstructorReference;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SubstituteBaseType(Substitutes substitutes, Importer importer)
        {
            var baseTypeReference = this.TypeDefinition.BaseType;
            var substitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(baseTypeReference?.FullName);

            if (substitute != null)
            {
                var substituteTypeReference = importer.Import(substitute.TypeDefinition);
                this.TypeDefinition.BaseType = substituteTypeReference;
            }
        }

        private void SubstituteMethods(Substitutes substitutes, Importer importer)
        {
            var constructorsAndMethods = new List<MethodDef>(Helper.GetConstructors(this.TypeDefinition));
            constructorsAndMethods.AddRange(this.TypeDefinition.Methods);

            foreach (MethodDef method in constructorsAndMethods)
            {
                if (method.HasBody)
                {
                    foreach (var instruction in method.Body.Instructions)
                    {
                        // Calli intentionally excluded: uses function pointers, not method references
                        if (instruction.OpCode.Equals(OpCodes.Call) ||
                            instruction.OpCode.Equals(OpCodes.Callvirt))
                        {
                            if (instruction.Operand is IMethod methodRef)
                            {
                                var substitute = substitutes.SubstituteMethods.Lookup(methodRef);
                                if (substitute != null)
                                {
                                    var substituteMethodReference = importer.Import(substitute.MethodDefinition);
                                    instruction.Operand = substituteMethodReference;
                                }
                                else
                                {
                                    var declaringClassSubstitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(methodRef.DeclaringType?.FullName);
                                    if (declaringClassSubstitute != null && !declaringClassSubstitute.IsBaseSubstitution)
                                    {
                                        if (instruction.Operand is MemberRef memberRef)
                                        {
                                            var classSubstituteReference = importer.Import(declaringClassSubstitute.TypeDefinition);
                                            memberRef.Class = classSubstituteReference;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SubstituteFields(Substitutes substitutes, Importer importer)
        {
            foreach (var field in this.TypeDefinition.Fields)
            {
                var fieldTypeFullName = field.FieldType.FullName;
                var substitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(fieldTypeFullName);

                if (substitute != null && !substitute.IsBaseSubstitution)
                {
                    var substituteTypeReference = importer.Import(substitute.TypeDefinition);
                    field.FieldType = substituteTypeReference.ToTypeSig();
                }
            }
        }

        private void SubstituteLocalVariables(Substitutes substitutes, Importer importer)
        {
            var constructorsAndMethods = new List<MethodDef>(Helper.GetConstructors(this.TypeDefinition));
            constructorsAndMethods.AddRange(this.TypeDefinition.Methods);

            foreach (MethodDef method in constructorsAndMethods)
            {
                if (method.HasBody)
                {
                    foreach (var local in method.Body.Variables)
                    {
                        SubstituteClass substitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(local.Type.FullName);

                        if (substitute != null && !substitute.IsBaseSubstitution)
                        {
                            var substituteTypeReference = importer.Import(substitute.TypeDefinition);
                            local.Type = substituteTypeReference.ToTypeSig();
                        }
                    }
                }
            }
        }

        private void SubstituteReturnTypes(Substitutes substitutes, Importer importer)
        {
            var constructorsAndMethods = new List<MethodDef>(Helper.GetConstructors(this.TypeDefinition));
            constructorsAndMethods.AddRange(this.TypeDefinition.Methods);

            foreach (MethodDef method in constructorsAndMethods)
            {
                var substitute = substitutes.SubstituteClasses.LookupBySubstitutableFullName(method.ReturnType.FullName);

                if (substitute != null && !substitute.IsBaseSubstitution)
                {
                    var substituteTypeReference = importer.Import(substitute.TypeDefinition);
                    method.ReturnType = substituteTypeReference.ToTypeSig();
                }
            }
        }
    }
}
