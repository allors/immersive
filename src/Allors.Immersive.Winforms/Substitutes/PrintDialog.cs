// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrintDialog.cs" company="allors bvba">
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
namespace Allors.Immersive.Winforms.Substitutes
{
    using System;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    using Allors.Immersive.Attributes;
    using Allors.Immersive.Winforms.Domain;

    [SubstituteClass(typeof(System.Windows.Forms.PrintDialog))]
    public class PrintDialog : CommonDialog, ISubstitute
    {
        private const DialogResult DefaultDialogResult = DialogResult.None;

        private bool allowCurrentPage;
        private bool allowPrintToFile;
        private bool allowSelection;
        private bool allowSomePages;
        private DialogResult dialogResult;
        private PrintDocument document;
        private PrinterSettings printerSettings;
        private bool printToFile;
        private bool showHelp;
        private bool showNetwork;

        private readonly Handle handle;

        public PrintDialog()
        {
            DialogResult = DefaultDialogResult;
            this.allowPrintToFile = true;
            this.showNetwork = true;
            this.printerSettings = new PrinterSettings();
            this.handle = Session.Singleton.Create(this);
        }

        public DialogResult DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; }
        }

        public bool AllowCurrentPage
        {
            get { return allowCurrentPage; }
            set { allowCurrentPage = value; }
        }

        public bool AllowPrintToFile
        {
            get { return allowPrintToFile; }
            set { allowPrintToFile = value; }
        }

        public bool AllowSelection
        {
            get { return allowSelection; }
            set { allowSelection = value; }
        }

        public bool AllowSomePages
        {
            get { return allowSomePages; }
            set { allowSomePages = value; }
        }

        public PrintDocument Document
        {
            get { return document; }
            set { document = value; }
        }

        public PrinterSettings PrinterSettings
        {
            get { return printerSettings; }
            set { printerSettings = value; }
        }

        public bool PrintToFile
        {
            get { return printToFile; }
            set { printToFile = value; }
        }

        public bool ShowHelp
        {
            get { return showHelp; }
            set { showHelp = value; }
        }

        public bool ShowNetwork
        {
            get { return showNetwork; }
            set { showNetwork = value; }
        }

        public override void Reset()
        {
        }

        protected override bool RunDialog(IntPtr hwndOwner)
        {
            Session.Singleton.OnTestingWinformsEvent(this.handle, TestingWinformsEventKind.Shown);
            return true;
        }

        public string SubstituteName
        {
            get { return "PrintDialog"; }
        }
    }
}
