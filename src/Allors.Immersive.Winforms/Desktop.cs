// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Desktop.cs" company="allors bvba">
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
namespace Allors.Immersive.Winforms
{
    using System;
    using System.Runtime.InteropServices;
#if !NETFRAMEWORK
    using System.Runtime.Versioning;
#endif
    using System.Windows.Forms;

#if !NETFRAMEWORK
    [SupportedOSPlatform("windows")]
#endif
    public class Desktop : IDisposable
    {
        private readonly IntPtr windowsDesktopHandle;
        private readonly IntPtr allorsDesktopHandle;
        private bool isDisposed;

        #region Access
        private const uint DESKTOP_CREATEMENU = 0x0004; // Required to create a menu on the desktop.
        private const uint DESKTOP_CREATEWINDOW = 0x0002; // Required to create a window on the desktop. 
        private const uint DESKTOP_ENUMERATE = 0x0040; // Required for the desktop to be enumerated. 
        private const uint DESKTOP_HOOKCONTROL = 0x0008; // Required to establish any of the window hooks. 
        private const uint DESKTOP_JOURNALPLAYBACK = 0x0020; // Required to perform journal playback on a desktop. 
        private const uint DESKTOP_JOURNALRECORD = 0x0010; // Required to perform journal recording on a desktop. 
        private const uint DESKTOP_READOBJECTS = 0x0001; // Required to read objects on the desktop. 
        private const uint DESKTOP_SWITCHDESKTOP = 0x0100; // Required to activate the desktop using the SwitchDesktop function. 
        private const uint DESKTOP_WRITEOBJECTS = 0x0080; // Required to write objects on the desktop. 

        private const uint GENERIC_ALL =
            DESKTOP_CREATEMENU | DESKTOP_CREATEWINDOW | DESKTOP_ENUMERATE | DESKTOP_HOOKCONTROL |
            DESKTOP_JOURNALPLAYBACK | DESKTOP_JOURNALRECORD | DESKTOP_READOBJECTS | DESKTOP_SWITCHDESKTOP |
            DESKTOP_WRITEOBJECTS;
        #endregion

        public Desktop()
        {
            this.isDisposed = false;
            if (this.UseDesktop)
            {
                this.windowsDesktopHandle = GetThreadDesktop(GetCurrentThreadId());
                this.allorsDesktopHandle = CreateDesktop("allors", IntPtr.Zero, IntPtr.Zero, 0, GENERIC_ALL, IntPtr.Zero);

                if (this.allorsDesktopHandle == IntPtr.Zero)
                {
                    throw new Exception("Could not create desktop");
                }

                if (!SetThreadDesktop(this.allorsDesktopHandle))
                {
                    throw new Exception("Could not set thread on desktop");
                }

                if (!SwitchDesktop(this.allorsDesktopHandle))
                {
                    throw new Exception("Could not switch to desktop");
                }

            }
        }

        ~Desktop()
        {
            this.Dispose();
        }

        public bool UseDesktop
        {
            get
            {
                return !SystemInformation.UserInteractive;
            }
        }

        [DllImport("user32")]
        private static extern IntPtr CreateDesktop(string lpszDesktop, IntPtr lpszDevice, IntPtr pDevmode, int dwFlags, uint dwDesiredAccess, IntPtr lpsa);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseDesktop(IntPtr hDesktop);

        [DllImport("user32")]
        private static extern IntPtr GetThreadDesktop(int dwThreadId);

        [DllImport("kernel32")]
        private static extern int GetCurrentThreadId();

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetThreadDesktop(IntPtr hDesktop);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SwitchDesktop(IntPtr hDesktop);

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.isDisposed = true;
                if (this.UseDesktop)
                {
                    if (this.allorsDesktopHandle != IntPtr.Zero)
                    {
                        try
                        {
                            SetThreadDesktop(this.windowsDesktopHandle);
                        }
                        finally
                        {
                            try
                            {
                                SwitchDesktop(this.windowsDesktopHandle);
                            }
                            finally
                            {
                                CloseDesktop(this.allorsDesktopHandle);
                            }
                        }
                    }
                }
            }

            GC.SuppressFinalize(this);
        }
    }
}