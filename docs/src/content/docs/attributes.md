---
title: "Attributes"
description: "SubstituteClassAttribute and SubstituteMethodAttribute: constructors, properties and the rules the weaver applies to them."
audience: "Users"
kind: "Reference"
status: "Current"
---

> **Status: Current.** Reference for users.

The `Allors.Immersive.Attributes` package declares the two attributes in the namespace
`Allors.Immersive.Attributes`. It targets .NET Framework 4.8 and .NET 10, is strong-named and
has no dependencies. The weaver recognizes the attributes by their full names,
`Allors.Immersive.Attributes.SubstituteClassAttribute` and
`Allors.Immersive.Attributes.SubstituteMethodAttribute`, and reads their constructor arguments
from the compiled metadata; it does not load the package. [How weaving works](weaving.md)
explains the rewrites; this page states the contract.

## SubstituteClassAttribute

Applies to classes, once per class. Marks the class as the substitute for one class of the
referenced assembly.

| Member | Meaning |
| --- | --- |
| `SubstituteClassAttribute()` | Base substitution. The attributed class must have a base type; that base type is the substitutable class. |
| `SubstituteClassAttribute(Type substitutableType)` | Explicit substitution of `substitutableType`. The attributed class need not derive from it. |
| `SubstitutableType` | The explicitly named type, or `null` for base substitution. |

Rules the weaver applies:

- The substitutable class is matched by its full name. The weaver fails with
  `Base type is required for <type>` when the parameterless form is used on a class without a
  base type.
- The first constructor declared by the attributed class replaces every constructor call and
  `newobj` of the substitutable class in the target; its parameters are not compared.
- With base substitution the weaver changes base types, constructor calls and matched method
  calls only. With explicit substitution it also retargets every other member reference of the
  substitutable class to the substitute, and changes field, local variable and return types.
- When several classes substitute the same full name, the first one found in the immerse
  assembly is used.

One example of each form, abridged from the repository's test fixture:

```csharp
[SubstituteClass]
public class Form : Allors.Immersive.AssemblyReferenced.Form
{
}

[SubstituteClass(typeof(Allors.Immersive.AssemblyReferenced.SealedSingle))]
public class SealedSingle
{
    public string Property { get; set; }
}
```

## SubstituteMethodAttribute

Applies to methods. Marks the method as the substitute for one method of the referenced
assembly.

| Member | Meaning |
| --- | --- |
| `SubstituteMethodAttribute(Type type)` | Substitutes the method of `type` that has the attributed method's own name. |
| `SubstituteMethodAttribute(Type type, string methodName)` | Substitutes the method of `type` named `methodName`. |
| `SubstitutableType` | The type that declares the substituted method. |
| `SubstitutableMethodName` | The explicit name, or `null` when the attributed method's name is used. |

Rules the weaver applies:

- A call matches when the called method's declaring type has the full name of the attribute's
  type, its name equals the substituted name, and its parameter types equal the attributed
  method's parameter types in number, order and full name. The return type is not compared, and
  the `call` or `callvirt` instruction keeps its opcode.
- Each overload of the substituted method needs its own attributed method.

Requirements the weaver does not check:

- A static method must substitute a static method, and an instance method an instance method,
  declared on the base substitute of the same class so that the receiver of the call is an
  instance of the substitute. The weaver compares neither the static modifier nor the declaring
  class of the attributed method; a mismatch produces invalid IL that fails at run time.

The attributed method can call the original method, for example through the inherited member.

Examples from the test fixture:

```csharp
[SubstituteClass]
public class Form : Allors.Immersive.AssemblyReferenced.Form
{
    [SubstituteMethod(typeof(Allors.Immersive.AssemblyReferenced.Form), "ShowDialog")]
    public string AllorsShowDialog() => "Substitute: " + ShowDialog();
}

public class MessageBox
{
    [SubstituteMethod(typeof(Allors.Immersive.AssemblyReferenced.MessageBox))]
    public static string Show(string text) => "Substitute: " + Allors.Immersive.AssemblyReferenced.MessageBox.Show(text);
}
```
