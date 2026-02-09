# Immersive

Immerse a library inside another library.

Immersive is an IL weaver that substitutes types and methods from a referenced assembly with implementations from an "immerse" assembly.
At compile time, it rewrites IL so that the target assembly uses the substitute types and method bodies — without changing source code.

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

Reference the `Immersive.Weaver` package in your project. It includes a `.targets` file that runs the weaver automatically after compilation:

```xml
<Target Name="Immersive" AfterTargets="Compile">
  <ImmersiveTask AssemblyPath="$(TargetPath)" SearchDirectories="@(ReferencePath)" />
</Target>
```

### CLI tool

```
immersive <assembly-path> [options]

Arguments:
  <assembly-path>            Path to the assembly to weave

Options:
  -s, --search-dir <dir>     Search directory for assemblies (repeatable)
  -v, --verbose              Print weaver info messages
  -h, --help                 Show this help
```

If no search directories are specified, the directory containing the assembly is used.

## Attributes

The `Immersive` package provides two attributes for the immerse assembly.

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
| `src/Immersive` | Attribute definitions (`SubstituteClassAttribute`, `SubstituteMethodAttribute`) |
| `src/Immersive.Weaver.Core` | Weaving engine (dnlib-based IL rewriting) |
| `src/Immersive.Weaver` | MSBuild task that invokes the weaver after compilation |
| `src/Immersive.Weaver.Tool` | CLI tool for standalone weaving |
| `test/AssemblyReferenced` | Test fixture — the referenced library |
| `test/AssemblyToImmerse` | Test fixture — substitute implementations |
| `test/AssemblyToProcess` | Test fixture — target assembly to weave |
| `test/Tests` | Integration tests validating the weaving results |

## License

LGPL-3.0
