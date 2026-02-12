// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AvaloniaTest.cs" company="allors bvba">
//   Copyright 2008-2026 Allors bv.
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU Lesser General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU Lesser General Public License for more details.
//
//   You should have received a copy of the GNU Lesser General Public License
//   along with this program.  If not, see http://www.gnu.org/licenses.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Immersive.Avalonia.Tests
{
    using System;
    using System.Collections.Generic;

    using global::Avalonia;
    using global::Avalonia.Controls;
    using global::Avalonia.Headless;
    using global::Avalonia.Themes.Fluent;

    using Allors.Immersive.Avalonia.Domain;

    public abstract class AvaloniaTest : IDisposable
    {
        private readonly List<Window> windows;

        protected Session test;

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
            this.windows = new List<Window>();

            this.test.TestingAvaloniaEvent += this.EventOccurred;
        }

        public virtual void Dispose()
        {
            this.test.TestingAvaloniaEvent -= this.EventOccurred;

            foreach (var window in this.windows)
            {
                try
                {
                    window.Close();
                }
                catch (Exception) { }
            }
        }

        protected void ShowWindow(Window window)
        {
            this.windows.Add(window);
            window.Show();
        }

        private void EventOccurred(object sender, TestingAvaloniaEventArgs args)
        {
            switch (args.Kind)
            {
                case TestingAvaloniaEventKind.Opened:
                    OnOpened(args);
                    break;
                case TestingAvaloniaEventKind.Click:
                    OnClick(args);
                    break;
                case TestingAvaloniaEventKind.IsCheckedChanged:
                    OnIsCheckedChanged(args);
                    break;
                case TestingAvaloniaEventKind.TextChanged:
                    OnTextChanged(args);
                    break;
                case TestingAvaloniaEventKind.SelectionChanged:
                    OnSelectionChanged(args);
                    break;
                case TestingAvaloniaEventKind.ValueChanged:
                    OnValueChanged(args);
                    break;
                case TestingAvaloniaEventKind.IsExpandedChanged:
                    OnIsExpandedChanged(args);
                    break;
                case TestingAvaloniaEventKind.Closed:
                    OnClosed(args);
                    break;
                default:
                    break;
            }
        }

        protected virtual void OnOpened(TestingAvaloniaEventArgs args)
        {
        }

        protected virtual void OnClick(TestingAvaloniaEventArgs args)
        {
        }

        protected virtual void OnIsCheckedChanged(TestingAvaloniaEventArgs args)
        {
        }

        protected virtual void OnTextChanged(TestingAvaloniaEventArgs args)
        {
        }

        protected virtual void OnSelectionChanged(TestingAvaloniaEventArgs args)
        {
        }

        protected virtual void OnValueChanged(TestingAvaloniaEventArgs args)
        {
        }

        protected virtual void OnIsExpandedChanged(TestingAvaloniaEventArgs args)
        {
        }

        protected virtual void OnClosed(TestingAvaloniaEventArgs args)
        {
        }
    }
}
