---
title: "Weaver internals"
description: "The classes of the weaving engine, the order of its passes over a type, assembly resolution, the test fixture, the known limitations and the validated change to the MSBuild package."
audience: "Maintainers"
kind: "Explanation"
status: "Current"
---

> **Status: Current.** Explanation for maintainers of the engine in `src/Allors.Immersive`, which
> [How weaving works](../weaving.md) describes from the user's side.

## Projects

| Project | Responsibility |
| --- | --- |
| `src/Allors.Immersive` | The engine, namespace `Allors.Immersive.Weaver`, built on [dnlib](https://github.com/0xd4d/dnlib) 4.5.0. It replaced Fody and Mono.Cecil in commit `a561460`. |
| `src/Allors.Immersive.Attributes` | The two attributes. The engine matches them by full name, in `Substitute/Attributes.cs`, and does not reference this project. |
| `src/Allors.Immersive.Tool` | `Program.cs`: argument parsing and exit codes around `ModuleWeaver`. |
| `src/Allors.Immersive.MSBuild` | `ImmersiveTask` and the `.targets` file; packs the engine and dnlib for both MSBuild runtimes. |
| `src/Allors.Immersive.Winforms`, `src/Allors.Immersive.Avalonia` | Immerse assemblies for the UI frameworks, with their sessions, handles and testers. |
| `test/Allors.Immersive.AssemblyReferenced`, `…AssemblyToImmerse`, `…AssemblyToProcess` | The fixture trio for the engine tests: referenced, immerse and target assembly. |
| `test/Allors.Immersive.Tests` | The engine tests. |
| `test/Allors.Immersive.Winforms.AssemblyToProcess`, `test/Allors.Immersive.Avalonia.AssemblyToProcess` | Fixture applications, woven after their build with the tool built from source. |
| `test/Allors.Immersive.Winforms.Tests`, `test/Allors.Immersive.Avalonia.Tests` | The library tests, a class per tested control: 57 of the 82 Windows Forms substitutes and all 49 Avalonia substitutes have one. |

## Execution

`ModuleWeaver.Execute(assemblyPath, searchDirectories)` runs these steps:

1. Builds a dnlib `AssemblyResolver` whose `PreSearchPaths` are the search directories, or the
   directories of the search entries that are files, and a `ModuleContext` around it.
2. Loads the target from a byte array, so that the file is not locked, and sets `Location`.
3. Creates `Substitutables`: a `SubstitutableClass` for every `TypeDef` of the module that is not
   an interface, nested types included and `<Module>` excluded. This happens before the marker
   check, which is why `-v` lists the classes even when no marker is found.
4. Finds the first top-level type named `ImmersiveMarker` that is a class. Without it, writes
   `No immersive assembly found` and returns; the file is untouched.
5. Takes the assembly name from the scope of the marker's base type: the `AssemblyRef` name, or
   the scope name for a type in the same module. Looks for `<name>.dll` in the directory of each
   search entry in order, then in the target's directory, and loads the first hit from bytes.
   Throws `Could not resolve assembly '<name>'` otherwise.
6. Creates `Substitutes` from the immerse module: `SubstituteClasses` from the attributed types and
   `SubstituteMethods` from the attributed methods.
7. Calls `Substitutables.Substitute`, which calls `SubstitutableClass.Substitute` for every class.
8. Writes the module back to `assemblyPath` with `ModuleDef.Write` and default options, then
   disposes both modules in a `finally` block.

## The passes of SubstitutableClass

`SubstitutableClass.Substitute` creates one dnlib `Importer` for the target module and runs six
private methods in order. The importer turns a `TypeDef` or `MethodDef` of the immerse module into
a `TypeRef` or `MemberRef` of the target module.

| Method | What it changes | Base substitution | Explicit |
| --- | --- | --- | --- |
| `SubstituteConstructors` | In the class's constructors, `call` operands named `.ctor` whose declaring type is substitutable become the substitute's `Constructor`; in all constructors and methods, `newobj` operands of a substitutable type become the substitute's `Constructor`. | yes | yes |
| `SubstituteBaseType` | `BaseType` becomes the substitute when its full name is substitutable. | yes | yes |
| `SubstituteMethods` | For `call` and `callvirt`: a `SubstituteMethods.Lookup` hit replaces the operand; otherwise, when the declaring type has an explicit substitute and the operand is a `MemberRef`, its `Class` becomes the substitute type. `calli` is skipped. | matched calls only | both |
| `SubstituteFields` | `FieldType` of fields whose type is substitutable. | no | yes |
| `SubstituteLocalVariables` | The types of local variables in method bodies. | no | yes |
| `SubstituteReturnTypes` | `ReturnType` of constructors and methods. | no | yes |

`SubstituteClass.Constructor` is `Helper.GetFirstConstructor`: the first `MethodDef` with
`IsConstructor` in metadata order, parameters not compared. `SubstituteMethod.Matches` compares
the operand's name with the attribute's name or the method's own name, the declaring type's full
name with the attribute's type, and then the parameter count and each parameter type's full name,
allowing for the `this` parameter of an instance substitute method.

Nothing visits parameter types, property or event signatures, generic arguments, custom
attributes or the `<Module>` type.

## Design notes and limitations

- Matching is by full name string. The attributes' `typeof` arguments arrive as a
  `TypeDefOrRefSig` or an `ITypeDefOrRef` and are reduced to their full name on construction, so
  assembly identity plays no part in matching.
- The first constructor is used without checking its parameters. A substitute class whose first
  constructor does not fit the call in the target produces invalid IL. The libraries keep one
  parameterless instance constructor per substitute.
- An instance substitute method declared outside the base substitute of its class produces a
  call whose receiver type does not declare the method. The weaver does not detect this.
- The weaver does not read or write symbols: `ModuleDefMD.Load(byte[])` loads no PDB and `Write`
  with default options writes none, so a woven assembly has no matching `.pdb`. The fixture
  applications set `DebugType` to `none`.
- A second weave of a woven assembly finds no substitutable base type, constructor call or method
  call left and writes the same bytes again; observed on the Avalonia fixture on 2026-10-19.
  Incremental builds do not rely on this: `dotnet build` copies the unwoven intermediate assembly
  to the output directory again, because the woven file differs from it, and the weave target
  weaves that copy.

## MSBuild package

The published `.targets` runs `ImmersiveTask` after `Compile` on `$(TargetPath)`. In an SDK
project the output assembly is copied from the intermediate assembly later in the build, so on a
fresh build the task fails with `Could not find file`.
[Run the weaver](../run-the-weaver.md#the-msbuild-package) records the observation.

The change validated on 2026-10-18 weaves the intermediate assembly, which the build then copies
to the output directory:

```xml
<Target Name="Immersive" AfterTargets="Compile">
  <ImmersiveTask AssemblyPath="@(IntermediateAssembly->'%(FullPath)')" SearchDirectories="@(ReferencePath)" />
</Target>
```

With that target in a locally repacked copy of `2.1.10-preview`, the tutorial's `App` wove on a
fresh build, on an incremental build and through `dotnet run`, and a Release build stayed
unwoven. `@(ReferencePath)` holds the paths of the referenced assemblies, so the immerse assembly
resolves from its directory. The change is not applied on `main`.

## Tests

`Fixture` in `test/Allors.Immersive.Tests` runs once per test assembly. It loads the unwoven
`Allors.Immersive.AssemblyToProcess.dll` from the test output as `BeforeAssembly`, copies every
`.dll` of the output to a temporary directory, weaves the copy with `ModuleWeaver`, and loads the
woven copy as `Assembly`. On .NET Framework, `Assembly.LoadFrom` suffices because the woven copy
has a different path; on .NET the same identity would return the cached unwoven assembly, so the
fixture loads it in its own `AssemblyLoadContext` with a `Resolving` handler for the temporary
directory. The directory is deleted at process exit.

`Verifier` runs the newest `PEVerify.exe` found under the Windows SDK directory on two paths and
compares the reports with offsets removed; `AssemblyTests.PeVerify` calls it in Debug builds on
.NET Framework only. The test passes the directories of the test output and of the woven copy
rather than the two assembly files, so PEVerify answers `File not found or has bad headers` for
both and the comparison passes without verifying any IL; observed on 2026-10-19. Passing the two
assembly paths would make it effective. `SubstitutesTests` constructs `Substitutes` from the
immerse fixture directly and expects five substitute classes.

To cover a behaviour, add it to the fixture trio and assert it in `ClassTests` or `MethodTests`
through reflection on `Fixture.Assembly`. The fixture projects are plain projects without the
weaver; the weave happens only in the fixture.
