// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Helper.cs" company="allors bvba">
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

    using dnlib.DotNet;

    public static class Helper
    {
        public static List<TypeDef> GetAllTypes(ModuleDef moduleDef)
        {
            List<TypeDef> typeDefs = new List<TypeDef>();
            foreach (TypeDef typeDef in moduleDef.Types)
            {
                if (!"<Module>".Equals(typeDef.Name))
                {
                    typeDefs.Add(typeDef);
                    GetAllTypes(typeDefs, typeDef);
                }
            }

            return typeDefs;
        }

        private static void GetAllTypes(List<TypeDef> typeDefs, TypeDef parentTypeDef)
        {
            foreach (TypeDef typeDef in parentTypeDef.NestedTypes)
            {
                if (!"<Module>".Equals(typeDef.Name))
                {
                    typeDefs.Add(typeDef);
                    GetAllTypes(typeDefs, typeDef);
                }
            }
        }

        public static List<MethodDef> GetConstructors(TypeDef typeDef)
        {
            List<MethodDef> methodDefs = new List<MethodDef>();
            foreach (MethodDef methodDef in typeDef.Methods)
            {
                if (methodDef.IsConstructor)
                {
                    methodDefs.Add(methodDef);
                }
            }

            return methodDefs;
        }

        public static MethodDef GetFirstConstructor(TypeDef typeDef)
        {
            List<MethodDef> constructors = GetConstructors(typeDef);
            if (constructors.Count > 0)
            {
                return constructors[0];
            }

            return null;
        }
    }
}
