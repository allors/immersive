// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinformsTest.cs" company="allors bvba">
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

namespace Allors.Immersive.Winforms.Tests
{
    using System;
    using System.Linq;
    using System.Windows.Forms;

    using Allors.Immersive.Winforms.Domain;
    using Allors.Immersive.Winforms;

    public abstract class WinformsTest : IDisposable
    {
        static WinformsTest()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
        }

        private Desktop desktop;
        protected Session test;

        private void EventOccured(object sender, TestingWinformsEventArgs args)
        {
            switch (args.Kind)
            {
                case TestingWinformsEventKind.Shown:
                    OnShown(args);
                    break;
                case TestingWinformsEventKind.CheckedChanged:
                    OnCheckedChanged(args);
                    break;
                case TestingWinformsEventKind.MouseClick:
                    OnClick(args);
                    break;
                default:
                    break;
            }
        }

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
                    try
                    {
                        form.Dispose();
                    }
                    catch (ObjectDisposedException) { }
                }
            }
            finally
            {
                this.desktop.Dispose();
            }
        }

        protected virtual void OnClick(TestingWinformsEventArgs args)
        {

        }

        protected virtual void OnShown(TestingWinformsEventArgs args)
        {
        }

        protected virtual void OnCheckedChanged(TestingWinformsEventArgs args)
        {
        }

    }
}
