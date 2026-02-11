using System;
using System.Collections.Generic;
using System.IO;
using Allors.Immersive.Weaver;

namespace Allors.Immersive.GlobalTool
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            string assemblyPath = null;
            var searchDirs = new List<string>();
            var verbose = false;

            for (var i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--help":
                    case "-h":
                        PrintUsage();
                        return 0;

                    case "--search-dir":
                    case "-s":
                        if (i + 1 >= args.Length)
                        {
                            Console.Error.WriteLine("Error: --search-dir requires a value.");
                            return 1;
                        }
                        searchDirs.Add(args[++i]);
                        break;

                    case "--verbose":
                    case "-v":
                        verbose = true;
                        break;

                    default:
                        if (args[i].StartsWith("-"))
                        {
                            Console.Error.WriteLine($"Error: Unknown option '{args[i]}'.");
                            PrintUsage();
                            return 1;
                        }
                        if (assemblyPath != null)
                        {
                            Console.Error.WriteLine("Error: Multiple assembly paths specified.");
                            return 1;
                        }
                        assemblyPath = args[i];
                        break;
                }
            }

            if (assemblyPath == null)
            {
                Console.Error.WriteLine("Error: Assembly path is required.");
                PrintUsage();
                return 1;
            }

            if (!File.Exists(assemblyPath))
            {
                Console.Error.WriteLine($"Error: File not found: {assemblyPath}");
                return 1;
            }

            if (searchDirs.Count == 0)
            {
                searchDirs.Add(Path.GetDirectoryName(Path.GetFullPath(assemblyPath)));
            }

            try
            {
                var weaver = new ModuleWeaver
                {
                    WriteInfo = verbose ? Console.WriteLine : _ => { }
                };
                weaver.Execute(Path.GetFullPath(assemblyPath), searchDirs.ToArray());
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return 1;
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: immerse <assembly-path> [options]");
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            Console.WriteLine("  <assembly-path>            Path to the assembly to weave");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -s, --search-dir <dir>     Search directory for assemblies (repeatable)");
            Console.WriteLine("  -v, --verbose              Print weaver info messages");
            Console.WriteLine("  -h, --help                 Show this help");
        }
    }
}
