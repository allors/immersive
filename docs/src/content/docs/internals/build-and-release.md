---
title: "Build and release"
description: "Prerequisites, the Nuke targets, the test suites and which ones CI runs, packaging, versioning and publication to NuGet."
audience: "Maintainers"
kind: "How-to guide"
status: "Current"
---

> **Status: Current.** How-to guide for maintainers. The build and the tests were run locally on
> Windows 11 with the .NET SDK 10.0.402 on 2026-10-18.

## Prerequisites

- Windows. The Windows Forms projects target `net48` and `net10.0-windows`, and the engine and
  attributes also target `net48`, so the .NET Framework 4.8 developer pack is needed; it comes
  with Visual Studio or the Windows SDK.
- The .NET SDK named in `global.json`: 10.0.100, rolling forward to the latest 10.0 feature band.
- `PEVerify.exe` from the Windows SDK, under `%ProgramFiles(x86)%\Microsoft SDKs\Windows`, for
  `AssemblyTests.PeVerify`; without it the test fails with `Could not find path to PEVerify`.
- Node.js 24 and npm for the documentation website; see the
  [website instructions](../../../../README.md).

## Build with Nuke

`build.cmd`, `build.ps1` and `build.sh` run the Nuke build in `build/Build.cs` through
`dotnet run`. The configuration is `Debug` locally and `Release` on a build server.

| Target | What it does |
| --- | --- |
| `Clean` | Deletes `bin` and `obj` under `src` and `test`, and recreates `output`. |
| `Restore` | `dotnet restore` of `Allors.Immersive.slnx`. |
| `Compile` | `dotnet build` of the solution; the default target. Building the fixture applications weaves them with the tool from `src/Allors.Immersive.Tool`. |
| `Test` | `dotnet test` of `Allors.Immersive.Tests` and `Allors.Immersive.Winforms.Tests`. |
| `Pack` | `dotnet pack` of the five packages into `output/`. |
| `Publish` | `Test` and `Pack`, then pushes every `output/*.nupkg` to nuget.org with `NuGetApiKey`, skipping duplicates. |

```sh
./build.cmd Compile
./build.cmd Test
./build.cmd Pack
```

The Windows Forms tests show forms on the developer's desktop; `Desktop` creates a private
desktop only when the session is not interactive. The test projects disable xunit parallelism
because `Session.Singleton` is shared.

One test depends on the Windows display language. `BindingNavigatorTest.TesterHandlesEvents`
compares the text of the count item of a `BindingNavigator` with `of 3`; on .NET Framework,
Windows Forms localizes that text, so the `net48` run fails with `Actual: "van 3"` on a Dutch
Windows, while the `net10.0-windows` run passes. Observed with the display language `nl-NL` on
2026-10-19; the CI runner is English.

`Allors.Immersive.Avalonia.Tests` is not in the `Test` target. Run it directly; it uses the
headless platform and needs no desktop:

```sh
dotnet test test/Allors.Immersive.Avalonia.Tests/Allors.Immersive.Avalonia.Tests.csproj
```

## Packaging

`Directory.Build.props` sets the package metadata for every project: the authors, the licence
`LGPL-3.0-only`, the repository URL, the icon, `README.md` as the package readme, symbol packages
and Source Link; `Allors.Immersive.MSBuild` turns its symbol package off. The packed projects
are `Allors.Immersive`, `Allors.Immersive.Attributes`, `Allors.Immersive.MSBuild`,
`Allors.Immersive.Tool` and `Allors.Immersive.Winforms`; `Allors.Immersive.Avalonia` is built
but not packed.

`Allors.Immersive.MSBuild` packs by hand: it builds into `lib/<tfm>/` with its dependencies
copied locally, suppresses the default build output and the dependency list, and names the task,
engine and dnlib assemblies of both target frameworks and `build/Allors.Immersive.MSBuild.targets`
as package files. Change that list when a dependency changes.

## Versioning and publication

`version.json` sets the version `2.1-preview` for Nerdbank.GitVersioning, with `main` as the
public release branch, so a build on `main` produces `2.1.<git height>-preview`. Every push to
`main` runs the `CI` workflow on `windows-2022`: `./build.cmd Test Pack`, then a NuGet trusted
publishing login with the `NUGET_USER` secret and `./build.cmd Publish`. A pull request runs the
tests and pack without publishing. Changes limited to `docs/`, the documentation workflow or
`AGENTS.md` do not run `CI` and therefore publish nothing; a change to `README.md` does run it,
because the readme is part of every package.

There are no tags or GitHub releases; nuget.org is the record of what was published.

## Documentation

The website in `docs/` has its own checks; the [website instructions](../../../../README.md)
list them, and the `Documentation` workflow runs them for changes to `docs/`, `AGENTS.md` and
`README.md`. Publication of the website is a manual step described there.
