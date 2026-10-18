---
title: "Test a Windows Forms application"
description: "Weave a Windows Forms application with the Winforms library, then find its controls by name and drive forms, dialogs and message boxes from tests."
audience: "Users"
kind: "How-to guide"
status: "Current"
---

> **Status: Current.** How-to guide for users. It describes `Allors.Immersive.Winforms` as built
> from `main`; the repository's tests in `test/Allors.Immersive.Winforms.Tests` are the worked
> example, and the examples below are taken from them.

`Allors.Immersive.Winforms` is an immerse assembly for `System.Windows.Forms`. It substitutes the
forms, controls, components and dialogs listed in
[Substituted controls and testers](controls.md#windows-forms) with subclasses that register
themselves in a session when they are created, and it replaces `MessageBox.Show` with a method
that does not block. A test can then find any control of a running form by its `Name`, read and
set its properties, click it, and respond when a form, dialog or message box is shown. The forms
are real: they are created and shown by the application's own code. When the test process has no
interactive desktop, as under a CI service, the library runs them on a private Windows desktop.

The package targets .NET Framework 4.8 and .NET 10 on Windows.

## 1. Weave the application assembly

Reference the package from the project that contains the forms, declare the marker, and weave
the output after each build:

```xml
<ItemGroup>
  <PackageReference Include="Allors.Immersive.Winforms" Version="2.1.10-preview" />
</ItemGroup>
<Target Name="Immerse" AfterTargets="Build">
  <Exec Command="dotnet immerse &quot;$(TargetPath)&quot; -v" />
</Target>
```

```csharp
#if DEBUG
public class ImmersiveMarker : Allors.Immersive.Winforms.Marker
{
}
#endif
```

[Run the weaver](run-the-weaver.md) explains the tool, the target and the `DEBUG` guard. The
repository's fixture application,
[`Allors.Immersive.Winforms.AssemblyToProcess`](../../../../test/Allors.Immersive.Winforms.AssemblyToProcess/Allors.Immersive.Winforms.AssemblyToProcess.csproj),
is set up this way with the tool built from source. After the weave, every `Form`, `Button`,
`TextBox` and other substituted type that the application creates is an instance of the
library's subclass, including the application's own classes that derive from them.

## 2. Set up the test class

Each test needs a `Desktop`, a reset `Session` and, when it reacts to events, a subscription.
The repository's tests share a base class; this is its essence:

```csharp
using System.Windows.Forms;
using Allors.Immersive.Winforms;
using Allors.Immersive.Winforms.Domain;

public abstract class WinformsTest : IDisposable
{
    static WinformsTest()
    {
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
    }

    private readonly Desktop desktop;
    protected readonly Session test;

    protected WinformsTest()
    {
        this.desktop = new Desktop();
        Session.Singleton.Reset();
        this.test = Session.Singleton;
        this.test.TestingWinformsEvent += this.EventOccured;
    }

    public virtual void Dispose()
    {
        this.test.TestingWinformsEvent -= this.EventOccured;
        try
        {
            foreach (var form in Application.OpenForms.Cast<Form>().ToList())
            {
                try { form.Dispose(); } catch (ObjectDisposedException) { }
            }
        }
        finally
        {
            this.desktop.Dispose();
        }
    }

    private void EventOccured(object sender, TestingWinformsEventArgs args)
    {
        if (args.Kind == TestingWinformsEventKind.Shown)
        {
            this.OnShown(args);
        }
    }

    protected virtual void OnShown(TestingWinformsEventArgs args)
    {
    }
}
```

`Desktop` creates and switches to a private desktop only when `SystemInformation.UserInteractive`
is false; on a developer's machine the forms appear on the normal desktop. `Session.Singleton` is
one process-wide session, so run the tests sequentially: the repository's test projects declare
`[assembly: CollectionBehavior(DisableTestParallelization = true)]`.

## 3. Find and drive controls

Create the form as the application does and show it; then address controls through testers:

```csharp
using Allors.Immersive.Winforms.Testers;

public class ButtonTest : WinformsTest
{
    private readonly DefaultForm form;

    public ButtonTest()
    {
        this.form = new DefaultForm();
        this.form.Show();
    }

    [Fact]
    public void Click()
    {
        var textBox1 = new TextBoxTester(this.form.Name, "textBox1");
        var textBox2 = new TextBoxTester("textBox2");
        var button1 = new ButtonTester("button1");

        textBox1.Target.Text = "Clicked!";
        button1.Click();

        Assert.Equal("Clicked!", textBox2.Target.Text);
    }
}
```

A tester is constructed with one or more names: the last is the `Name` of the control, and the
names before it are the names of its parents, nearest last, so
`new TextBoxTester("DefaultForm", "textBox1")` finds `textBox1` inside the form named
`DefaultForm`. The session searches the most recently created controls first, skips disposed
controls, and prefers a visible control over an invisible one with the same name. When nothing
matches, the tester's constructor throws an `ArgumentNullException` whose message says to check
the names and that the form has been shown.

`Target` is the control itself, typed as the substitute, so every property and method of the
real control is available, as in `textBox1.Target.Text`. A tester adds what a user would do that
a property cannot express: `ButtonTester.Click()` performs a click, `ComboBoxTester.Select`
selects an item by predicate, and `TreeViewTester.FindNode` and `PropertyGridTester.FindGridItem`
return testers for nodes and grid items. The [reference](controls.md#windows-forms) lists these
members. `Handle` gives access to the session's record of the control, including its `Parent`.

## 4. Respond to modal dialogs and message boxes

A modal dialog blocks the code that calls `ShowDialog` until it closes, so the test must act from
the `Shown` event, which the substitutes raise when a form is shown, a common dialog runs or
`MessageBox.Show` is called. Override the handler, recognize the dialog by its name and drive it
there:

```csharp
protected override void OnShown(TestingWinformsEventArgs args)
{
    if (args.Handle.Name == "DialogForm")
    {
        var textBox1 = new TextBoxTester("DialogForm", "textBox1");
        var button1 = new ButtonTester("DialogForm", "button1");

        textBox1.Target.Text = "Ok!";
        button1.Click();

        new ButtonTester("DialogForm", "okButton").Click();
    }
}

[Fact(Timeout = 30000)]
public void ShowDialog()
{
    new ButtonTester("button2").Click();   // the application opens DialogForm modally

    var dialogForm = new FormTester("DialogForm");
    Assert.Equal(DialogResult.OK, dialogForm.Target.DialogResult);
}
```

A message box is substituted differently, because `MessageBox` is not a control. The library
replaces each `Show` overload with a static method that records the text and caption, raises
`Shown` with a `MessageBox` handle named after the caption, or after the text when there is no
caption, and returns the `DialogResult` the handler chose, `OK` by default:

```csharp
protected override void OnShown(TestingWinformsEventArgs args)
{
    if (args.Handle.Name == "Hello")
    {
        new MessageBoxTester(args.Handle).Target.DialogResult = DialogResult.Cancel;
    }
}
```

The library substitutes the common dialogs `OpenFileDialog`, `SaveFileDialog`,
`FolderBrowserDialog`, `ColorDialog`, `FontDialog`, `PrintDialog` and `PageSetupDialog`
explicitly, because five of them are sealed in Windows Forms, with classes that derive from
`CommonDialog` and raise `Shown` from `RunDialog` instead of showing anything, under fixed names
such as `OpenFileDialog` and `FolderBrowser`. Set from the handler
the properties the application reads afterwards.

Give tests that open modal dialogs a timeout, as above, so that a dialog the handler fails to
close does not block the run.

## What is substituted, and what is not

The library substitutes 82 types of `System.Windows.Forms`; the
[reference](controls.md#windows-forms) lists them with the events they report. A control that is
not in the list is still created and shown, but the session does not know it, so no tester finds
it. A class of the application that derives from a substituted type, such as a
`MyButton : Button`, derives from the substitute after the weave and is found like any other.
