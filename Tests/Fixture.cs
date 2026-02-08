using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Immersive.Weaver;

namespace Immersive.Tests
{
    public static class Fixture
    {
        public static ModuleWeaver ModuleWeaver;

        public static Assembly Assembly;

        public static string AssemblyPath;

        public static Assembly BeforeAssembly;

        static Fixture()
        {
            var sourcePath = new DirectoryInfo(".").GetFiles("AssemblyToProcess.dll").First().FullName;
            BeforeAssembly = System.Reflection.Assembly.LoadFrom(sourcePath);

            // Copy to temp directory for weaving
            var tempDir = Path.Combine(Path.GetTempPath(), "Immersive_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            var sourceDir = Path.GetDirectoryName(sourcePath);
            foreach (var file in Directory.GetFiles(sourceDir, "*.dll"))
            {
                File.Copy(file, Path.Combine(tempDir, Path.GetFileName(file)), true);
            }

            AssemblyPath = Path.Combine(tempDir, "AssemblyToProcess.dll");

            ModuleWeaver = new ModuleWeaver { WriteInfo = Console.WriteLine };
            ModuleWeaver.Execute(AssemblyPath, new[] { tempDir });

            Assembly = System.Reflection.Assembly.LoadFrom(AssemblyPath);
        }
    }
}
