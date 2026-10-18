---
title: "Test an Avalonia application"
description: "Weave an Avalonia application with the Avalonia library, run it on the headless platform, and find and drive its controls from tests."
audience: "Users"
kind: "How-to guide"
status: "Current"
---

> **Status: Current.** How-to guide for users. `Allors.Immersive.Avalonia` is in the repository
> and builds with it, but it is not published to NuGet and its tests are not part of the build's
> `Test` target. Its 66 tests passed locally on 2026-10-18.

`Allors.Immersive.Avalonia` follows the design of the Windows Forms library for Avalonia 11: it
substitutes 49 controls of `Avalonia.Controls` with subclasses that register in a `Session`,
report events, and are addressed through testers by their `Name`. Read
[Test a Windows Forms application](winforms.md) first; this page describes the differences.

To use the library today, reference the project `src/Allors.Immersive.Avalonia` from a checkout
or pack it yourself; [Build and release](internals/build-and-release.md) describes the build. The
library targets .NET 10 and references `Avalonia` 11.2.

## Weave and set up

The marker derives from `Allors.Immersive.Avalonia.Marker`, and the weave is the same build
target as for Windows Forms. The tests run Avalonia on the headless platform, so no window
system is needed, with the Fluent theme, so that controls have templates. This is the essence of
the repository's base class:

```csharp
using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Themes.Fluent;
using Allors.Immersive.Avalonia;
using Allors.Immersive.Avalonia.Domain;

public abstract class AvaloniaTest : IDisposable
{
    private readonly List<Window> windows = new();
    protected readonly Session test;

    static AvaloniaTest()
    {
        AppBuilder.Configure<Application>()
            .UseHeadless(new AvaloniaHeadlessPlatformOptions())
            .SetupWithoutStarting();
        Application.Current.Styles.Add(new FluentTheme());
    }

    protected AvaloniaTest()
    {
        Session.Singleton.Reset();
        this.test = Session.Singleton;
        this.test.TestingAvaloniaEvent += this.EventOccurred;
    }

    protected void ShowWindow(Window window)
    {
        this.windows.Add(window);
        window.Show();
    }

    public virtual void Dispose()
    {
        this.test.TestingAvaloniaEvent -= this.EventOccurred;
        foreach (var window in this.windows)
        {
            try { window.Close(); } catch (Exception) { }
        }
    }

    private void EventOccurred(object sender, TestingAvaloniaEventArgs args)
    {
        if (args.Kind == TestingAvaloniaEventKind.Click)
        {
            this.OnClick(args);
        }
    }

    protected virtual void OnClick(TestingAvaloniaEventArgs args)
    {
    }
}
```

The test project references `Avalonia.Headless` and `Avalonia.Themes.Fluent` in addition to the
library. Run the tests sequentially, because the session is process-wide.

## Find and drive controls

```csharp
using Allors.Immersive.Avalonia.Testers;

public class ButtonTest : AvaloniaTest
{
    private readonly DefaultWindow window;

    public ButtonTest()
    {
        this.window = new DefaultWindow();
        this.ShowWindow(this.window);
    }

    [Fact]
    public void ClickFiresEvent()
    {
        var fired = false;
        this.OnClickAction = _ => fired = true;

        new ButtonTester("button1").Click();

        Assert.True(fired);
    }

    private Action<TestingAvaloniaEventArgs> OnClickAction { get; set; }

    protected override void OnClick(TestingAvaloniaEventArgs args) => this.OnClickAction?.Invoke(args);
}
```

Names come from the `Name` attribute in XAML or the `Name` property; the window of the
repository's fixture declares `Name="defaultWindow"`, and `new WindowTester("defaultWindow")`
finds it. The parent chain the session follows is `StyledElement.Parent`, and the session skips
controls whose `IsVisible` is false, so an invisible control is not found.

Every substitute overrides `StyleKeyOverride` to return the original control type, so that the
theme's styles and templates still apply to the substituted control. Events are reported for the
kinds in `TestingAvaloniaEventKind`: `Click`, `TextChanged`, `IsCheckedChanged`, `Opened`,
`Closed`, `SelectionChanged`, `ValueChanged` and `IsExpandedChanged`; the
[reference](controls.md#avalonia) shows which controls raise which.

The library substitutes controls only. Avalonia's file and folder pickers go through the storage
provider and are not substituted.
