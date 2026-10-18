---
title: "Terminology"
description: "Agreed names for the assemblies, the marker, substitutions and the testing libraries."
audience: "Users and maintainers"
kind: "Reference"
status: "Current"
---

> **Status: Current.** Reference for users and maintainers.

Use these names consistently when discussing Immersive, in prose, code comments, diagrams and
pull requests. Exact identifiers keep their declared spelling. The linked pages own the
definitions.

## Weaving

| Term | Definition and usage |
| --- | --- |
| Weave, weaving | The rewrite of a compiled assembly by Immersive, as described in [How weaving works](weaving.md). Say that an assembly is woven; do not say *patched* or *instrumented*. |
| Immerse | The historical verb for the same operation, kept in the command `immerse`, in `ImmersiveTask` and in the message `<immerse> immersed in <target>`. Prefer *weave* in new prose. |
| Referenced assembly | The library whose classes and methods are substituted. It is unchanged by the weave. |
| Immerse assembly | The assembly that declares the substitutes and carries the attributes. Not *immersive assembly*, except in the message `No immersive assembly found`. |
| Target assembly | The assembly the weaver rewrites: the project that uses the referenced assembly and declares the marker. |
| Marker | The top-level class `ImmersiveMarker` in the target assembly, whose base type names the immerse assembly. The base class in the immerse assembly is conventionally called `Marker`. |
| Substitutable class | A class of the referenced assembly that a substitute class replaces. The weaver's `Substitutables` are the classes of the target that it visits; say *substitutable class* for the former to keep the two apart. |
| Substitute class | A class in the immerse assembly with `SubstituteClassAttribute`. |
| Base substitution | A substitute class that derives from the class it substitutes, declared with `[SubstituteClass]` without an argument. |
| Explicit substitution | A substitute class that names the class it substitutes with `[SubstituteClass(typeof(...))]`. |
| Substitute method | A method in the immerse assembly with `SubstituteMethodAttribute`. |

## Testing libraries

| Term | Definition and usage |
| --- | --- |
| Windows Forms | Microsoft's name for `System.Windows.Forms`. The package and project are named `Allors.Immersive.Winforms`; say *the Winforms library* only for them. |
| Avalonia | The cross-platform UI framework; `Allors.Immersive.Avalonia` is *the Avalonia library*. |
| Session | The process-wide `Session.Singleton` that records handles and raises testing events. |
| Handle | The session's record of one substitute instance: its name, its parent and the substitute itself. |
| Tester | A class that finds a handle by name and exposes the substitute as `Target`, with the actions a test performs on it. |
| Testing event | A `TestingWinformsEvent` or `TestingAvaloniaEvent` raised by a substitute, with its kind, so that a test can respond to a form, dialog or message box being shown and to changes. |
