---
title: "Run the weaver"
description: "Run the immerse command by hand or from a build target, use the weaver's API, keep the marker out of Release builds, and know the state of the MSBuild package."
audience: "Users"
kind: "How-to guide"
status: "Current"
---

> **Status: Current.** How-to guide for users. The MSBuild package section records behaviour
> observed with `Allors.Immersive.MSBuild 2.1.10-preview` on 2026-10-18.

This guide assumes a target assembly with an `ImmersiveMarker` and an immerse assembly with
substitutes, as [Getting started](getting-started.md) builds them and
[How weaving works](weaving.md) explains them. It covers the ways to run the weaver over the
target.

## Run the command-line tool

`Allors.Immersive.Tool` is a .NET tool with the command `immerse`. It needs a .NET 10 or later
runtime. Install it into a tool manifest of the repository, so that its version is recorded with
the code, or globally:

```sh
dotnet new tool-manifest        # once per repository, if there is none
dotnet tool install Allors.Immersive.Tool --prerelease
dotnet immerse path/to/Target.dll -v
```

```sh
dotnet tool install -g Allors.Immersive.Tool --prerelease
immerse path/to/Target.dll -v
```

`--prerelease` is required while only preview versions are published. The command takes the path
of the assembly to weave and rewrites that file in place. Weave the compiled output after the
build, and not while another process holds the file open, because the weaver saves over it.

By default the weaver looks for the immerse assembly, and for the assemblies the target
references, in the directory of the target assembly, which is where a build output directory has
them. Use `-s`/`--search-dir` once per directory when they are elsewhere; a path to a file
counts as its directory. With `-v`/`--verbose` the tool prints each class it visits, each
substitute it finds and the final `<immerse>.dll immersed in <target>.dll.` line. Without `-v`
a successful weave prints nothing. The exit code is 0 on success and 1 when the path is missing
or repeated, an option is unknown, `-s` has no value, the file does not exist or the weave
throws; the reason is printed to standard error.

## Weave from the build

Run the tool after every build of the target project, because each build writes a fresh,
unwoven assembly. Add a target after `Build` to the project file:

```xml
<Target Name="Immerse" AfterTargets="Build" Condition="'$(IsCrossTargetingBuild)' != 'true'">
  <Exec Command="dotnet immerse &quot;$(TargetPath)&quot; -v" />
</Target>
```

The target runs for incremental builds too: the build copies the unwoven intermediate assembly
from `obj/` to the output directory again, because the woven file differs from it, and the
target weaves that fresh copy. For a multi-targeted project it runs once per target framework,
each with its own `$(TargetPath)`. The condition skips the outer build of an SDK-style project,
where `IsCrossTargetingBuild` is `true` and `TargetPath` is empty. It still runs for single-target
projects, including older .NET Framework project files. The repository's own fixture applications
also weave after `Build`, with a guard for their SDK-style targets and
the tool built from source; see
[`Allors.Immersive.Winforms.AssemblyToProcess.csproj`](../../../../test/Allors.Immersive.Winforms.AssemblyToProcess/Allors.Immersive.Winforms.AssemblyToProcess.csproj).

A project that references the woven project builds it first, and the woven output is what gets
copied next to the referencing project's own output. That is how the libraries' test projects
run against woven fixture applications.

## Weave in Debug builds only

Declare the marker under a condition, so that Release builds contain no marker and stay unwoven:

```csharp
#if DEBUG
public class ImmersiveMarker : Desktop.Testing.Marker
{
}
#endif
```

The project still references the immerse assembly in every configuration unless that reference
is conditional too. When the tool runs on a Release build it reports
`No immersive assembly found` and leaves the assembly as compiled.

## Use the weaver's API

The `Allors.Immersive` package contains the engine behind the tool and the task. Create a
`ModuleWeaver`, optionally receive its messages, and call `Execute` with the assembly path and
the search directories:

```csharp
using Allors.Immersive.Weaver;

var weaver = new ModuleWeaver { WriteInfo = Console.WriteLine };
weaver.Execute(assemblyPath, new[] { Path.GetDirectoryName(assemblyPath) });
```

`Execute` throws when the immerse assembly cannot be resolved or the weave fails. The test
fixture in `test/Allors.Immersive.Tests/Fixture.cs` uses the API this way: it copies the fixture
application and its dependencies to a temporary directory, weaves the copy and loads it in a
separate `AssemblyLoadContext`, because `Assembly.LoadFrom` would return the already loaded,
unwoven assembly with the same identity. [Packages, tool and task](packages.md#weaver-api) lists
the members.

## The MSBuild package

`Allors.Immersive.MSBuild` ships an MSBuild task and a `.targets` file that runs the task after
the `Compile` target on `$(TargetPath)`, the assembly in the output directory. In a project
built with `dotnet build`, that file is copied from the intermediate assembly later in the
build, so on a fresh build the package fails the build with the task's error
`Could not find file '…\bin\Debug\net10.0\App.dll'`. This was observed with version
`2.1.10-preview` and the .NET SDK 10.0.402 on the tutorial's `App` project on 2026-10-18. Use
the command-line tool from a build target, as above, until a version of the package weaves the
intermediate assembly instead; [Weaver internals](internals/weaver.md#msbuild-package) records
the validated change.

The package also places the task assemblies under `lib/`, so a plain package reference makes
them compile references of the project and copies them to its output. Reference the package with
`PrivateAssets="all" ExcludeAssets="compile;runtime"` to keep only its build logic.

## Troubleshooting

| Symptom | Cause and action |
| --- | --- |
| `No immersive assembly found` | The target has no top-level class named `ImmersiveMarker`, or the build configuration excluded it. Nothing was changed. |
| `Could not resolve assembly 'X'` | The immerse assembly `X.dll` is neither in a search directory nor next to the target. Pass `-s` with its directory. |
| The woven assembly fails at run time with an exception such as `InvalidProgramException` or `MissingMethodException` | A substitute does not match what the target uses: a constructor with different parameters, or an explicitly substituted member the substitute does not declare. See [Substitute classes](weaving.md#substitute-classes). |
| The output is unwoven after `dotnet run` | The build rewrote the assembly after the weave. Run the weaver from a target after `Build`, as above. |
| `dotnet immerse` is not found | The tool manifest is in another directory, the tool is not installed, or no .NET 10 or later runtime is present. |
