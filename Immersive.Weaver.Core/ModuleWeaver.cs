using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using dnlib.DotNet;

namespace Immersive.Weaver
{
    public class ModuleWeaver
    {
        public ModuleDef ModuleDefinition { get; set; }

        public Action<string> WriteInfo { get; set; } = _ => { };

        public void Execute(string assemblyPath, string[] searchDirectories)
        {
            var resolver = new AssemblyResolver();
            foreach (var dir in searchDirectories)
            {
                if (Directory.Exists(dir))
                {
                    resolver.PreSearchPaths.Add(dir);
                }
                else if (File.Exists(dir))
                {
                    var dirName = Path.GetDirectoryName(dir);
                    if (dirName != null && !resolver.PreSearchPaths.Contains(dirName))
                    {
                        resolver.PreSearchPaths.Add(dirName);
                    }
                }
            }

            var moduleContext = new ModuleContext(resolver);
            resolver.DefaultModuleContext = moduleContext;

            ModuleDef immerseModule = null;
            try
            {
                // Load from byte array to avoid file locking
                var assemblyBytes = File.ReadAllBytes(assemblyPath);
                this.ModuleDefinition = ModuleDefMD.Load(assemblyBytes, moduleContext);
                this.ModuleDefinition.Location = assemblyPath;

                var substitutableAssembly = new Substitutables(this);

                var immersiveType = this.ModuleDefinition.Types.FirstOrDefault(v => v.Name.Equals("ImmersiveMarker") && v.IsClass);
                if (immersiveType == null)
                {
                    this.WriteInfo("No immersive assembly found");
                    return;
                }

                var scope = immersiveType.BaseType.Scope;
                string assemblyName;
                if (scope is AssemblyRef asmRef)
                {
                    assemblyName = asmRef.Name;
                }
                else
                {
                    assemblyName = scope.ScopeName;
                }

                // Resolve the immerse assembly
                foreach (var dir in searchDirectories)
                {
                    string candidate = null;
                    if (Directory.Exists(dir))
                    {
                        candidate = Path.Combine(dir, assemblyName + ".dll");
                    }
                    else if (File.Exists(dir))
                    {
                        var dirName = Path.GetDirectoryName(dir);
                        if (dirName != null)
                        {
                            candidate = Path.Combine(dirName, assemblyName + ".dll");
                        }
                    }

                    if (candidate != null && File.Exists(candidate))
                    {
                        immerseModule = ModuleDefMD.Load(File.ReadAllBytes(candidate), moduleContext);
                        immerseModule.Location = candidate;
                        break;
                    }
                }

                if (immerseModule == null)
                {
                    // Try the same directory as the assembly being processed
                    var sameDir = Path.GetDirectoryName(assemblyPath);
                    var candidate = Path.Combine(sameDir, assemblyName + ".dll");
                    if (File.Exists(candidate))
                    {
                        immerseModule = ModuleDefMD.Load(File.ReadAllBytes(candidate), moduleContext);
                        immerseModule.Location = candidate;
                    }
                }

                if (immerseModule == null)
                {
                    throw new Exception($"Could not resolve assembly '{assemblyName}'");
                }

                var substitutes = new Substitutes(this, immerseModule);

                substitutableAssembly.Substitute(substitutes);

                this.WriteInfo(immerseModule.Name + " immersed in " + this.ModuleDefinition.Name + ".");

                this.ModuleDefinition.Write(assemblyPath);
            }
            finally
            {
                (this.ModuleDefinition as IDisposable)?.Dispose();
                (immerseModule as IDisposable)?.Dispose();
                this.ModuleDefinition = null;
            }
        }
    }
}
