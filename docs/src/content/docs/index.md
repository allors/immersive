---
title: "Documentation"
description: "Documentation for users of the Immersive IL weaver and for its maintainers."
audience: "Users and maintainers"
kind: "Index"
status: "Current"
---

> **Status: Current.**

Immersive is an IL weaver for .NET assemblies. After a project compiles, the weaver rewrites the
project's assembly so that its use of one library is redirected to substitutes from another
assembly, without a change to the source code. Allors uses it to test desktop applications: the
Windows Forms and Avalonia libraries in this repository substitute the UI controls, so a test can
find the controls of a real form by name and drive them.

Users weave their own assemblies, or test applications with the supplied libraries. Maintainers
change the weaver, the libraries, the build and these pages. [README.md](../../../../README.md)
introduces the project. The [writing guide](internals/writing-documentation.md) defines the rules
for these pages; [Terminology](terminology.md) records the agreed names. These pages are for
humans. Agents use the same technical content, following the instructions in `AGENTS.md`.

Immersive is one of Allors's supporting open-source tools. Its packages are licensed under
`LGPL-3.0-only`, as their package metadata states. Ask questions and report problems through
the [repository's issues](https://github.com/allors/immersive/issues).

## For users

| Page | Kind | Status | Content |
| --- | --- | --- | --- |
| [Getting started](getting-started.md) | Tutorial | Current | Build a library, a library of substitutes and a console application, weave the application with the command-line tool, and make the weave part of its build. |
| [How weaving works](weaving.md) | Explanation | Current | The three assemblies, the marker, base and explicit class substitution, method substitution, and what the weaver rewrites and leaves alone. |
| [Run the weaver](run-the-weaver.md) | How-to guide | Current | The command-line tool, a build target that runs it, the weaver's API, the marker in Debug builds only, troubleshooting, and the state of the MSBuild package. |
| [Test a Windows Forms application](winforms.md) | How-to guide | Current | Weave an application with the Winforms library, then find its controls by name and drive forms, dialogs and message boxes from tests. |
| [Test an Avalonia application](avalonia.md) | How-to guide | Current | The same approach for Avalonia on the headless platform; the library is built from source and is not yet published. |
| [Attributes](attributes.md) | Reference | Current | `SubstituteClassAttribute` and `SubstituteMethodAttribute`: constructors, properties and the matching rules the weaver applies. |
| [Packages, tool and task](packages.md) | Reference | Current | The NuGet packages and their target frameworks, the `immerse` command, the MSBuild task, the `ModuleWeaver` API and the messages the weaver writes. |
| [Substituted controls and testers](controls.md) | Reference | Current | Every substituted Windows Forms and Avalonia type, its tester, the events it reports and the tester's members. |
| [Terminology](terminology.md) | Reference | Current | Agreed names for the assemblies, the marker, substitutions and the testing libraries. |

## For maintainers

The pages under `internals/` describe how the weaver and the repository are built. A user must
not rely on them: they can change with any version.

| Page | Kind | Status | Content |
| --- | --- | --- | --- |
| [Weaver internals](internals/weaver.md) | Explanation | Current | The classes of the weaving engine, the order of its passes over a type, assembly resolution, the test fixture, the known limitations and the validated change to the MSBuild package. |
| [Build and release](internals/build-and-release.md) | How-to guide | Current | Prerequisites, the Nuke targets, the test suites and which ones CI runs, packaging, versioning and publication to NuGet. |
| [Writing documentation](internals/writing-documentation.md) | How-to guide | Current | Audience, Diátaxis, status, Oxford English, stable terminology, visual explanations and review. |
| [Documentation website](internals/documentation-website.md) | Decision record | Planned | The adoption of the Allors3 documentation standard, what was reused, validation and the unpublished state of the website. |

The writing guide defines [page kinds](internals/writing-documentation.md#choose-the-audience-and-purpose)
and [statuses](internals/writing-documentation.md#state-what-is-current). Audience, kind and
status describe different things: who a page serves, what it helps them do and whether it
describes current behaviour.
