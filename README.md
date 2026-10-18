# Immersive

[![Allors.Immersive](https://img.shields.io/nuget/v/Allors.Immersive?label=Allors.Immersive)](https://www.nuget.org/packages/Allors.Immersive)
[![Allors.Immersive.Attributes](https://img.shields.io/nuget/v/Allors.Immersive.Attributes?label=Allors.Immersive.Attributes)](https://www.nuget.org/packages/Allors.Immersive.Attributes)
[![Allors.Immersive.MSBuild](https://img.shields.io/nuget/v/Allors.Immersive.MSBuild?label=Allors.Immersive.MSBuild)](https://www.nuget.org/packages/Allors.Immersive.MSBuild)
[![Allors.Immersive.Tool](https://img.shields.io/nuget/v/Allors.Immersive.Tool?label=Allors.Immersive.Tool)](https://www.nuget.org/packages/Allors.Immersive.Tool)
[![Allors.Immersive.Winforms](https://img.shields.io/nuget/v/Allors.Immersive.Winforms?label=Allors.Immersive.Winforms)](https://www.nuget.org/packages/Allors.Immersive.Winforms)

Immerse a library inside another library.

Immersive is an IL weaver for .NET. After a project compiles, it rewrites the project's assembly
so that the classes and methods it uses from one library are replaced by substitutes from another
assembly. The source code does not change, and a Release build can stay exactly as compiled.

Allors uses it to test desktop applications. The `Allors.Immersive.Winforms` library substitutes
the Windows Forms controls, forms, dialogs and message boxes with subclasses that a test can find
by name and drive, while the application runs its real forms. A library for Avalonia is in the
repository as well.

## Documentation

The [documentation](https://github.com/allors/immersive/blob/main/docs/src/content/docs/index.md)
is written for users and for maintainers. Start with
[Getting started](https://github.com/allors/immersive/blob/main/docs/src/content/docs/getting-started.md),
a tutorial that builds three small projects and weaves one of them, and
[How weaving works](https://github.com/allors/immersive/blob/main/docs/src/content/docs/weaving.md).
Agent instructions are in `AGENTS.md`.

## Example

A library has a dialog, and an application derives from it:

```csharp
// The referenced library
namespace Desktop;

public class Dialog
{
    public string ShowDialog() => "A window appeared on screen";
}
```

```csharp
// The immerse assembly: substitutes marked with attributes, plus a marker class
namespace Desktop.Testing;

using Allors.Immersive.Attributes;

public class Marker { }

[SubstituteClass]
public class Dialog : Desktop.Dialog
{
    [SubstituteMethod(typeof(Desktop.Dialog), "ShowDialog")]
    public string ShowDialogSubstitute() => "No window: the substitute answered";
}
```

```csharp
// The application: unchanged, apart from the marker that names the immerse assembly
public class MainForm : Desktop.Dialog
{
    public string Run() => this.ShowDialog();
}

#if DEBUG
public class ImmersiveMarker : Desktop.Testing.Marker { }
#endif
```

After `immerse App.dll`, `MainForm` derives from the substitute `Dialog` and `Run()` returns
`No window: the substitute answered`. The tutorial shows every step and its output.

## Install

The packages are published as previews from `main`, so `--prerelease` is required.

```sh
dotnet add package Allors.Immersive.Attributes --prerelease   # in the immerse assembly
dotnet tool install Allors.Immersive.Tool --prerelease         # the immerse command
dotnet add package Allors.Immersive.Winforms --prerelease      # substitutes for Windows Forms
dotnet add package Allors.Immersive --prerelease               # the weaving engine, as an API
```

Weave the compiled assembly after each build, for example from a target in the project file:

```xml
<Target Name="Immerse" AfterTargets="Build">
  <Exec Command="dotnet immerse &quot;$(TargetPath)&quot; -v" />
</Target>
```

`Allors.Immersive.MSBuild` packages the weaver as an MSBuild task, but `2.1.10-preview`, the
version published when this was checked on 2026-10-18, fails a fresh `dotnet build`, because the
task runs before the output assembly exists. Use the tool until a corrected version ships; see
[Run the weaver](https://github.com/allors/immersive/blob/main/docs/src/content/docs/run-the-weaver.md).

## Building

The project uses [Nuke](https://nuke.build) as its build system and needs Windows, the .NET 10
SDK and the .NET Framework 4.8 developer pack.

```sh
./build.cmd Compile        # restore and build
./build.cmd Test           # run the engine and Windows Forms tests
./build.cmd Pack           # create NuGet packages in output/
./build.cmd Clean Compile  # clean, then build
```

[Build and release](https://github.com/allors/immersive/blob/main/docs/src/content/docs/internals/build-and-release.md)
describes the build, the test suites, versioning and publication.

## Solution structure

| Project | Description |
|---|---|
| `src/Allors.Immersive` | The weaving engine, built on dnlib |
| `src/Allors.Immersive.Attributes` | `SubstituteClassAttribute` and `SubstituteMethodAttribute` |
| `src/Allors.Immersive.MSBuild` | MSBuild task that invokes the weaver |
| `src/Allors.Immersive.Tool` | The `immerse` command, a .NET tool |
| `src/Allors.Immersive.Winforms` | Windows Forms substitutes, testers and session |
| `src/Allors.Immersive.Avalonia` | Avalonia substitutes, testers and session; built, not yet packaged |
| `test/Allors.Immersive.AssemblyReferenced` | Test fixture: the referenced library |
| `test/Allors.Immersive.AssemblyToImmerse` | Test fixture: the substitutes |
| `test/Allors.Immersive.AssemblyToProcess` | Test fixture: the target assembly to weave |
| `test/Allors.Immersive.Tests` | Engine tests on the woven fixture |
| `test/Allors.Immersive.Winforms.AssemblyToProcess`, `test/Allors.Immersive.Winforms.Tests` | Windows Forms fixture application and tests |
| `test/Allors.Immersive.Avalonia.AssemblyToProcess`, `test/Allors.Immersive.Avalonia.Tests` | Avalonia fixture application and tests |
| `docs` | The documentation website |

## Licence

LGPL-3.0-only
