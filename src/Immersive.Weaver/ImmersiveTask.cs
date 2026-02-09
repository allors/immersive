using System;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Immersive.Weaver
{
    public class ImmersiveTask : Task
    {
        [Required]
        public string AssemblyPath { get; set; }

        public ITaskItem[] SearchDirectories { get; set; }

        public override bool Execute()
        {
            try
            {
                var weaver = new ModuleWeaver { WriteInfo = msg => Log.LogMessage(msg) };
                var dirs = SearchDirectories?.Select(d => d.ItemSpec).ToArray() ?? Array.Empty<string>();
                weaver.Execute(AssemblyPath, dirs);
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex, showStackTrace: true);
            }

            return !Log.HasLoggedErrors;
        }
    }
}
