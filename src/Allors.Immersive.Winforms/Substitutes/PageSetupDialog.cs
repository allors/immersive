// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageSetupDialog.cs" company="allors bvba">
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

    [SubstituteClass(typeof(System.Windows.Forms.PageSetupDialog))]
    public class PageSetupDialog : CommonDialog, ISubstitute
    {
        private const DialogResult DefaultDialogResult = DialogResult.None;

        private bool allowMargins;
        private bool allowOrientation;
        private bool allowPaper;
        private bool allowPrinter;
        private DialogResult dialogResult;
        private PrintDocument document;
        private bool enableMetric;
        private Margins minMargins;
        private PageSettings pageSettings;
        private PrinterSettings printerSettings;
        private bool showHelp;
        private bool showNetwork;

        private readonly Handle handle;

        public PageSetupDialog()
        {
            DialogResult = DefaultDialogResult;
            this.allowMargins = true;
            this.allowOrientation = true;
            this.allowPaper = true;
            this.allowPrinter = true;
            this.showNetwork = true;
            this.handle = Session.Singleton.Create(this);
        }

        public DialogResult DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; }
        }

        public bool AllowMargins
        {
            get { return allowMargins; }
            set { allowMargins = value; }
        }

        public bool AllowOrientation
        {
            get { return allowOrientation; }
            set { allowOrientation = value; }
        }

        public bool AllowPaper
        {
            get { return allowPaper; }
            set { allowPaper = value; }
        }

        public bool AllowPrinter
        {
            get { return allowPrinter; }
            set { allowPrinter = value; }
        }

        public PrintDocument Document
        {
            get { return document; }
            set { document = value; }
        }

        public bool EnableMetric
        {
            get { return enableMetric; }
            set { enableMetric = value; }
        }

        public Margins MinMargins
        {
            get { return minMargins; }
            set { minMargins = value; }
        }

        public PageSettings PageSettings
        {
            get { return pageSettings; }
            set { pageSettings = value; }
        }

        public PrinterSettings PrinterSettings
        {
            get { return printerSettings; }
            set { printerSettings = value; }
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
            get { return "PageSetupDialog"; }
        }
    }
}
