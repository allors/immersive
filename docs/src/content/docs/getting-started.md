---
title: "Getting started"
description: "Weave a console application so that a library it uses is replaced by substitutes, first by hand with the immerse command and then as part of the build."
audience: "Users"
kind: "Tutorial"
status: "Current"
---

> **Status: Current.** Tutorial for users. Every step was run with the .NET SDK 10.0.402 and the
> `2.1.10-preview` packages on 2026-10-19; the output shown is the output observed.

In this tutorial you build three small projects: a library that opens a dialog, a library of
substitutes for it, and a console application that uses the first library. You then weave the
application so that it uses the substitutes, and finally make the weave part of its build. The
result is an application whose Debug build never opens the dialog, while its source and its
Release build are unchanged.

You need the [.NET SDK](https://dotnet.microsoft.com/download) 10 or later and a terminal. The
commands use the `dotnet` command line; an IDE works the same way once the projects exist.

## 1. Create the library to substitute

Create a directory for the tutorial and, inside it, the library:

```sh
dotnet new classlib -n Desktop -o Desktop -f net10.0
```

Delete `Desktop/Class1.cs` and add `Desktop/Dialog.cs`:

```csharp
namespace Desktop;

public class Dialog
{
    public string ShowDialog() => "A window appeared on screen";
}

public class MessageBox
{
    public static string Show(string text) => $"A message box showed '{text}'";
}
```

This stands in for a real UI library: `Dialog` is a class an application derives from, and
`MessageBox.Show` is a static method it calls.

## 2. Create the substitutes

```sh
dotnet new classlib -n Desktop.Testing -o Desktop.Testing -f net10.0
dotnet add Desktop.Testing reference Desktop/Desktop.csproj
dotnet add Desktop.Testing package Allors.Immersive.Attributes --prerelease
```

The packages are published as previews, so `--prerelease` is required. Delete
`Desktop.Testing/Class1.cs` and add `Desktop.Testing/Substitutes.cs`:

```csharp
namespace Desktop.Testing;

using Allors.Immersive.Attributes;

public class Marker
{
}

[SubstituteClass]
public class Dialog : Desktop.Dialog
{
    [SubstituteMethod(typeof(Desktop.Dialog), "ShowDialog")]
    public string ShowDialogSubstitute() => "No window: the substitute answered";
}

public class MessageBox
{
    [SubstituteMethod(typeof(Desktop.MessageBox))]
    public static string Show(string text) => $"No message box: the substitute received '{text}'";
}
```

Three things happen here. `Marker` is an ordinary empty class; the application will point at it
to name this assembly. `[SubstituteClass]` on `Dialog` substitutes its base class,
`Desktop.Dialog`: in a woven assembly, classes that derived from `Desktop.Dialog` derive from
this class instead. `[SubstituteMethod]` names a method of the library; calls to it are
redirected to the attributed method. `Show` takes its name from the attributed method, while
`ShowDialog` is named explicitly because the substitute method is called
`ShowDialogSubstitute`.

## 3. Create the application

```sh
dotnet new console -n App -o App -f net10.0
dotnet add App reference Desktop/Desktop.csproj Desktop.Testing/Desktop.Testing.csproj
```

Replace the content of `App/Program.cs`:

```csharp
using Desktop;

var form = new MainForm();
Console.WriteLine(form.Run());
Console.WriteLine(MessageBox.Show("Saved"));
Console.WriteLine("Base type: " + typeof(MainForm).BaseType);

public class MainForm : Dialog
{
    public string Run() => this.ShowDialog();
}

#if DEBUG
public class ImmersiveMarker : Desktop.Testing.Marker
{
}
#endif
```

The application uses the library as any application would. The only addition is
`ImmersiveMarker`: a class with exactly that name, whose base type tells the weaver which
assembly holds the substitutes. The `#if DEBUG` guard keeps the marker, and with it the weave,
out of Release builds.

Build and run it:

```sh
dotnet build App
dotnet App/bin/Debug/net10.0/App.dll
```

The output shows the unwoven behaviour:

```text
A window appeared on screen
A message box showed 'Saved'
Base type: Desktop.Dialog
```

## 4. Weave the application

Install the command-line tool into a local tool manifest, so that its version is recorded with
the tutorial:

```sh
dotnet new tool-manifest
dotnet tool install Allors.Immersive.Tool --prerelease
```

Weave the compiled application, with `-v` to see what the weaver finds:

```sh
dotnet immerse App/bin/Debug/net10.0/App.dll -v
```

```text
Substitutables: App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
Substitutable: Program
Substitutable: MainForm
Substitutable: ImmersiveMarker
Substitutes: Desktop.Testing, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
SubstituteClass: Desktop.Dialog
SubstituteMethod: Desktop.Dialog.ShowDialog
SubstituteMethod: Desktop.MessageBox.Show
Desktop.Testing.dll immersed in App.dll.
```

The weaver listed the classes of the application, found the marker, loaded `Desktop.Testing.dll`
from the application's output directory, found one substitute class and two substitute methods,
rewrote `App.dll` and saved it in place. Run the application again, without rebuilding it:

```sh
dotnet App/bin/Debug/net10.0/App.dll
```

```text
No window: the substitute answered
No message box: the substitute received 'Saved'
Base type: Desktop.Testing.Dialog
```

`MainForm` now derives from the substitute, `this.ShowDialog()` calls the substitute method, and
the static call goes to the substitute's `Show`. The source of `App` did not change.

## 5. Weave as part of the build

A build writes a fresh, unwoven `App.dll`, so the tool has to run after every build. Add this
target to `App/App.csproj`, inside the `<Project>` element:

```xml
<Target Name="Immerse" AfterTargets="Build" Condition="'$(IsCrossTargetingBuild)' != 'true'">
  <Exec Command="dotnet immerse &quot;$(TargetPath)&quot; -v" />
</Target>
```

The condition matters only for a project with several target frameworks;
[Run the weaver](run-the-weaver.md#weave-from-the-build) explains it.

`dotnet build App` now ends with the weaver's messages, and `dotnet run --project App` builds,
weaves and prints the substituted output; incremental builds weave again. A Release build has
no marker:

```sh
dotnet run --project App -c Release
```

The weaver reports `No immersive assembly found` in the build, leaves the assembly unchanged,
and the application prints the original output.

## Where to go next

[How weaving works](weaving.md) explains what the weaver rewrites and why the base type changed.
[Run the weaver](run-the-weaver.md) covers search directories, global installation of the tool,
the weaver's API and the state of the MSBuild package. For a Windows Forms or Avalonia
application the supplied libraries already contain the substitutes: see
[Test a Windows Forms application](winforms.md) and [Test an Avalonia application](avalonia.md).
