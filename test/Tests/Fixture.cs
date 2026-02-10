using System;
using System.IO;
using System.Linq;
using System.Reflection;
#if !NETFRAMEWORK
using System.Runtime.Loader;
#endif
using Allors.Immersive.Weaver;

namespace Allors.Immersive.Tests
{
    public static class Fixture
    {
        public static ModuleWeaver ModuleWeaver;

        public static Assembly Assembly;

        public static string AssemblyPath;

        public static Assembly BeforeAssembly;

        static Fixture()
        {
            var outputDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var sourcePath = Directory.GetFiles(outputDir, "AssemblyToProcess.dll").First();
            BeforeAssembly = System.Reflection.Assembly.LoadFrom(sourcePath);

            // Copy to temp directory for weaving
            var tempDir = Path.Combine(Path.GetTempPath(), "Allors.Immersive_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            var sourceDir = Path.GetDirectoryName(sourcePath);
            foreach (var file in Directory.GetFiles(sourceDir, "*.dll"))
            {
                File.Copy(file, Path.Combine(tempDir, Path.GetFileName(file)), true);
            }

            AssemblyPath = Path.Combine(tempDir, "AssemblyToProcess.dll");

            ModuleWeaver = new ModuleWeaver { WriteInfo = Console.WriteLine };
            ModuleWeaver.Execute(AssemblyPath, new[] { tempDir });

#if NETFRAMEWORK
            Assembly = System.Reflection.Assembly.LoadFrom(AssemblyPath);
#else
            // On .NET Core, Assembly.LoadFrom deduplicates by assembly identity,
            // so loading the woven copy returns the cached unwoven assembly.
            // Use a separate AssemblyLoadContext to isolate the woven assembly.
            var alc = new AssemblyLoadContext("WovenAssembly");
            alc.Resolving += (context, name) =>
            {
                var candidate = Path.Combine(tempDir, name.Name + ".dll");
                if (File.Exists(candidate))
                    return context.LoadFromAssemblyPath(candidate);
                return null;
            };
            Assembly = alc.LoadFromAssemblyPath(AssemblyPath);
#endif

            // Clean up temp directory on process exit
            AppDomain.CurrentDomain.ProcessExit += (_, __) =>
            {
                try { Directory.Delete(tempDir, true); } catch { }
            };
        }
    }
}
