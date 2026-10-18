---
title: "Packages, tool and task"
description: "The NuGet packages and their target frameworks, the immerse command, the MSBuild task, the ModuleWeaver API and the messages the weaver writes."
audience: "Users"
kind: "Reference"
status: "Current"
---

> **Status: Current.** Reference for users. Package versions were checked on nuget.org on
> 2026-10-18.

## Packages

| Package | Contents | Target frameworks | Depends on |
| --- | --- | --- | --- |
| `Allors.Immersive` | The weaving engine: `ModuleWeaver` and its supporting classes. | `net48`, `net10.0` | `dnlib` 4.5.0 |
| `Allors.Immersive.Attributes` | `SubstituteClassAttribute` and `SubstituteMethodAttribute`; strong-named. | `net48`, `net10.0` | nothing |
| `Allors.Immersive.Tool` | The `immerse` command, as a .NET tool with the engine bundled. | `net10.0`, rolling forward to later major runtimes | nothing |
| `Allors.Immersive.MSBuild` | `ImmersiveTask` and a `.targets` file, with the engine and `dnlib` bundled under `lib/`. | `net48` task for MSBuild on .NET Framework, `net10.0` task for the `dotnet` CLI | nothing declared |
| `Allors.Immersive.Winforms` | Substitutes, testers and the session for Windows Forms. | `net48`, `net10.0-windows` | `Allors.Immersive.Attributes` |

`Allors.Immersive.Avalonia` is built from the repository but is neither packed nor published;
see [Test an Avalonia application](avalonia.md).

Versions come from [Nerdbank.GitVersioning](https://github.com/dotnet/Nerdbank.GitVersioning)
and `version.json`: every push to `main` publishes `2.1.<git height>-preview` of each package.
The latest version on 2026-10-18 was `2.1.10-preview`; `Allors.Immersive.Winforms` also has
`1.0.0` to `1.0.3` from the earlier, Fody-based generation of the project. Use `--prerelease`
with `dotnet add package` and `dotnet tool install`. All packages are licensed under
`LGPL-3.0-only`.

## The immerse command

```text
immerse <assembly-path> [options]

Arguments:
  <assembly-path>            Path to the assembly to weave

Options:
  -s, --search-dir <dir>     Search directory for assemblies (repeatable)
  -v, --verbose              Print weaver info messages
  -h, --help                 Show this help
```

| Behaviour | Detail |
| --- | --- |
| Assembly path | Required, exactly one. A relative path is resolved against the current directory. The file is rewritten in place. |
| Search directories | Each `-s` adds a directory in which the immerse assembly and the referenced assemblies are resolved; a file path contributes its directory. Without `-s`, the directory of the assembly. The assembly's own directory is also tried last for the immerse assembly. |
| Output | With `-v`, the weaver's messages on standard output. Without it, nothing on success. Errors go to standard error as `Error: <reason>`. |
| Exit code | `0` after a weave or `--help`; `1` for a missing or repeated assembly path, an unknown option, `-s` without a value, a file that does not exist, or an exception during the weave. |

Install it with `dotnet tool install Allors.Immersive.Tool --prerelease`, in a tool manifest or
with `-g`. The tool needs a .NET 10 or later runtime.

## The MSBuild task

`Allors.Immersive.MSBuild.ImmersiveTask` is an MSBuild task with two parameters:

| Parameter | Required | Meaning |
| --- | --- | --- |
| `AssemblyPath` | Yes | The assembly to weave in place. |
| `SearchDirectories` | No | Items whose paths are search directories, or files whose directories are. |

The task logs the weaver's messages at normal importance, which `dotnet build` shows at
verbosity `normal` or higher, and logs an exception as an error with its stack trace, which fails
the build. The package's `build/Allors.Immersive.MSBuild.targets` registers the task from
`lib/net48` when MSBuild runs on .NET Framework and from `lib/net10.0` otherwise, and defines:

```xml
<Target Name="Immersive" AfterTargets="Compile">
  <ImmersiveTask AssemblyPath="$(TargetPath)" SearchDirectories="@(ReferencePath)" />
</Target>
```

[Run the weaver](run-the-weaver.md#the-msbuild-package) records that this target fails a fresh
`dotnet build`, because `$(TargetPath)` is written after `Compile`.

## Weaver API

`Allors.Immersive.Weaver.ModuleWeaver` in the `Allors.Immersive` package:

| Member | Meaning |
| --- | --- |
| `Action<string> WriteInfo { get; set; }` | Receives the messages below. Defaults to doing nothing. |
| `ModuleDef ModuleDefinition { get; set; }` | The dnlib module of the target while `Execute` runs; `null` afterwards. |
| `void Execute(string assemblyPath, string[] searchDirectories)` | Weaves the assembly at `assemblyPath` in place, resolving assemblies in `searchDirectories`, which are directories or files standing for their directories. Returns without writing when there is no marker. Throws `Exception("Could not resolve assembly '<name>'")` when the immerse assembly is not found, and lets dnlib errors propagate. |

The classes `Substitutables`, `Substitutes`, `SubstituteClasses` and `SubstituteMethods` are
public and used by the repository's tests; [Weaver internals](internals/weaver.md) describes
them. They are not a stable API.

## Messages

| Message | When |
| --- | --- |
| `Substitutables: <target assembly name>` | The target was loaded. |
| `Substitutable: <type>` | For each class of the target that will be visited. |
| `No immersive assembly found` | No top-level class named `ImmersiveMarker`; the weave ends without changes. |
| `Substitutes: <immerse assembly name>` | The immerse assembly was loaded. |
| `SubstituteClass: <substitutable type>` | For each class with `SubstituteClassAttribute`. |
| `SubstituteMethod: <type>.<method>` | For each method with `SubstituteMethodAttribute`. |
| `<immerse>.dll immersed in <target>.dll.` | The assembly was rewritten and saved. |
