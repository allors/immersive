# Immersive

[![Allors.Immersive](https://img.shields.io/nuget/v/Allors.Immersive?label=Allors.Immersive)](https://www.nuget.org/packages/Allors.Immersive)
[![Allors.Immersive.Attributes](https://img.shields.io/nuget/v/Allors.Immersive.Attributes?label=Allors.Immersive.Attributes)](https://www.nuget.org/packages/Allors.Immersive.Attributes)
[![Allors.Immersive.MSBuild](https://img.shields.io/nuget/v/Allors.Immersive.MSBuild?label=Allors.Immersive.MSBuild)](https://www.nuget.org/packages/Allors.Immersive.MSBuild)
[![Allors.Immersive.Tool](https://img.shields.io/nuget/v/Allors.Immersive.Tool?label=Allors.Immersive.Tool)](https://www.nuget.org/packages/Allors.Immersive.Tool)
[![Allors.Immersive.Winforms](https://img.shields.io/nuget/v/Allors.Immersive.Winforms?label=Allors.Immersive.Winforms)](https://www.nuget.org/packages/Allors.Immersive.Winforms)

Immerse a library inside another library.

Immersive is an IL weaver that substitutes types and methods from a referenced assembly with implementations from an "immerse" assembly.
At compile time, it rewrites IL so that the target assembly uses the substitute types and method bodies — without changing source code.

## Installation

### MSBuild task (recommended)

Automatically weaves your assembly after compilation:

```
dotnet add package Allors.Immersive.MSBuild
```

### CLI tool

For standalone weaving from the command line:

```
dotnet tool install -g Allors.Immersive.Tool
```

### Individual packages

```
dotnet add package Allors.Immersive              # Weaving engine API
dotnet add package Allors.Immersive.Attributes   # SubstituteClass/SubstituteMethod attributes
dotnet add package Allors.Immersive.Winforms     # WinForms substitute types and testers
```

## How it works

Three assemblies are involved:

1. **Referenced assembly** — the library your code depends on (e.g. `AssemblyReferenced`)
2. **Immerse assembly** — provides substitute types/methods annotated with `[SubstituteClass]` and `[SubstituteMethod]` (e.g. `AssemblyToImmerse`)
3. **Target assembly** — your project that references both; after weaving, all references to types in the referenced assembly are rewritten to use the substitutes

The target assembly signals which immerse assembly to use via a marker class:

```csharp
#if(DEBUG)
public class ImmersiveMarker : AssemblyToImmerse.Marker
{
}
#endif
```

The weaver discovers the immerse assembly from the marker's base type, then rewrites the target's IL accordingly.

## Usage

### MSBuild task (automatic)

Reference the `Allors.Immersive.MSBuild` package in your project. It includes a `.targets` file that runs the weaver automatically after compilation:

```xml
<Target Name="Immersive" AfterTargets="Compile">
  <ImmersiveTask AssemblyPath="$(TargetPath)" SearchDirectories="@(ReferencePath)" />
</Target>
```

### CLI tool

```
immerse <assembly-path> [options]

Arguments:
  <assembly-path>            Path to the assembly to weave

Options:
  -s, --search-dir <dir>     Search directory for assemblies (repeatable)
  -v, --verbose              Print weaver info messages
  -h, --help                 Show this help
```

If no search directories are specified, the directory containing the assembly is used.

## Attributes

The `Allors.Immersive.Attributes` package provides two attributes for the immerse assembly.

### `[SubstituteClass]`

Applied to a class in the immerse assembly. Two modes:

**Base substitution** — no arguments. The class to substitute is inferred from the base type:

```csharp
[SubstituteClass]
public class Form : AssemblyReferenced.Form
{
    // All references to AssemblyReferenced.Form in the target
    // will be rewritten to use this type's members
}
```

**Explicit substitution** — pass the type to substitute:

```csharp
[SubstituteClass(typeof(AssemblyReferenced.Button))]
public class MyButton
{
    // Substitutes AssemblyReferenced.Button
}
```

### `[SubstituteMethod]`

Applied to a method. Substitutes a specific method on a specific type:

```csharp
[SubstituteMethod(typeof(AssemblyReferenced.Form), "ShowDialog")]
public string AllorsShowDialog()
{
    return "Substitute: " + ShowDialog();
}
```

The first argument is the type containing the method to substitute.
The second (optional) argument is the method name — if omitted, the decorated method's own name is used.

## Example

**AssemblyReferenced** (the library):
```csharp
public class Form
{
    public string ShowDialog() => "Referenced: ShowDialog()";
}
```

**AssemblyToImmerse** (the substitutions):
```csharp
[SubstituteClass]
public class Form : AssemblyReferenced.Form
{
    [SubstituteMethod(typeof(AssemblyReferenced.Form), "ShowDialog")]
    public string AllorsShowDialog()
    {
        return "Substitute: " + ShowDialog();
    }
}
```

**AssemblyToProcess** (the target — before weaving):
```csharp
public class TestForm : AssemblyReferenced.Form
{
    public string CallShowDialog() => this.ShowDialog();
}
```

After weaving, `TestForm` inherits from the substitute `Form` and `CallShowDialog()` invokes the substituted method body.

## Building

The project uses [Nuke](https://nuke.build) as its build system.

```
./build.cmd Compile        # restore + build
./build.cmd Test           # run tests
./build.cmd Pack           # create NuGet packages in output/
./build.cmd Clean Compile  # clean then build
```

## Solution structure

| Project | Description |
|---|---|
| `src/Allors.Immersive` | Weaving engine (dnlib-based IL rewriting) — standalone NuGet package |
| `src/Allors.Immersive.Attributes` | Attribute definitions (`SubstituteClassAttribute`, `SubstituteMethodAttribute`) |
| `src/Allors.Immersive.MSBuild` | MSBuild task that invokes the weaver after compilation |
| `src/Allors.Immersive.Tool` | CLI global tool for standalone weaving |
| `src/Allors.Immersive.Winforms` | WinForms substitute types and testers |
| `test/AssemblyReferenced` | Test fixture — the referenced library |
| `test/AssemblyToImmerse` | Test fixture — substitute implementations |
| `test/AssemblyToProcess` | Test fixture — target assembly to weave |
| `test/Tests` | Integration tests validating the weaving results |

## License

LGPL-3.0
