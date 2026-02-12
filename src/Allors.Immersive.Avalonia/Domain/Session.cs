// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Session.cs" company="allors bvba">
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

namespace Allors.Immersive.Avalonia.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    public class Session
    {
        public static readonly Session Singleton;

        private readonly List<Handle> handles;

        private TestingAvaloniaEventHandler testingAvaloniaEvent;

        static Session()
        {
            Singleton = new Session();
        }

        private Session()
        {
            this.handles = new List<Handle>();
        }

        public event TestingAvaloniaEventHandler TestingAvaloniaEvent
        {
            add
            {
                this.testingAvaloniaEvent += value;
            }

            remove
            {
                this.testingAvaloniaEvent -= value;
            }
        }

        public void Reset()
        {
            this.handles.Clear();
            this.testingAvaloniaEvent = null;
        }

        public Handle Create(ISubstitute substitute)
        {
            var handle = new Handle(this, substitute);
            this.handles.Add(handle);
            return handle;
        }

        public Handle FindHandle(params string[] names)
        {
            if (names != null && names.Length > 0)
            {
                var name = names[names.Length - 1];
                Handle fallback = null;

                for (var i = this.handles.Count - 1; i >= 0; --i)
                {
                    var handle = this.handles[i];

                    if (handle.Control is { IsVisible: false })
                    {
                        continue;
                    }

                    if (handle.Name != null && handle.Name.Equals(name))
                    {
                        var matched = true;
                        var parent = handle;
                        for (var j = names.Length - 2; j >= 0; j--)
                        {
                            parent = parent.Parent;
                            if (parent == null || !parent.Name.Equals(names[j]))
                            {
                                matched = false;
                                break;
                            }
                        }

                        if (matched)
                        {
                            if (handle.Control == null || handle.Control.IsVisible)
                            {
                                return handle;
                            }

                            fallback ??= handle;
                        }
                    }
                }

                return fallback;
            }

            return null;
        }

        public Handle FindHandle(object testedObject)
        {
            return this.handles.FirstOrDefault(tester => testedObject.Equals(tester.Substitute));
        }

        internal void OnTestingAvaloniaEvent(Handle source, TestingAvaloniaEventKind testingAvaloniaEventKind)
        {
            var eventHandler = this.testingAvaloniaEvent;
            if (eventHandler != null)
            {
                var args = new TestingAvaloniaEventArgs(source, testingAvaloniaEventKind);
                eventHandler(source, args);
            }
        }
    }
}
