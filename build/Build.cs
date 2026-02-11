using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly string Configuration = IsLocalBuild ? "Debug" : "Release";

    [Solution] readonly Solution Solution;

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestDirectory => RootDirectory / "test";
    AbsolutePath OutputDirectory => RootDirectory / "output";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").DeleteDirectories();
            TestDirectory.GlobDirectories("**/bin", "**/obj").DeleteDirectories();
            OutputDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetProjectFile(TestDirectory / "Allors.Immersive.Tests" / "Allors.Immersive.Tests.csproj")
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild());

            DotNetTest(_ => _
                .SetProjectFile(TestDirectory / "Allors.Immersive.Winforms.Tests" / "Allors.Immersive.Winforms.Tests.csproj")
                .SetConfiguration(Configuration)
                .EnableNoRestore()
                .EnableNoBuild());
        });

    Target Pack => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            var packableProjects = new[]
            {
                SourceDirectory / "Allors.Immersive" / "Allors.Immersive.csproj",
                SourceDirectory / "Allors.Immersive.Attributes" / "Allors.Immersive.Attributes.csproj",
                SourceDirectory / "Allors.Immersive.MSBuild" / "Allors.Immersive.MSBuild.csproj",
                SourceDirectory / "Allors.Immersive.Tool" / "Allors.Immersive.Tool.csproj",
                SourceDirectory / "Allors.Immersive.Winforms" / "Allors.Immersive.Winforms.csproj",
            };

            packableProjects.ForEach(project =>
                DotNetPack(_ => _
                    .SetProject(project)
                    .SetConfiguration(Configuration)
                    .SetOutputDirectory(OutputDirectory)
                    .EnableNoRestore()
                    .EnableNoBuild()));
        });

    [Parameter("NuGet API key")] [Secret] readonly string NuGetApiKey;

    Target Publish => _ => _
        .DependsOn(Test, Pack)
        .Requires(() => NuGetApiKey)
        .Executes(() =>
        {
            OutputDirectory.GlobFiles("*.nupkg")
                .ForEach(package =>
                    DotNetNuGetPush(_ => _
                        .SetTargetPath(package)
                        .SetSource("https://api.nuget.org/v3/index.json")
                        .SetApiKey(NuGetApiKey)
                        .EnableSkipDuplicate()));
        });
}
