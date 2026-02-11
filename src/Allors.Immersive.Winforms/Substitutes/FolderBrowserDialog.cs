// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderBrowserDialog.cs" company="allors bvba">
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
    using System.Windows.Forms;

    using Allors.Immersive.Attributes;
    using Allors.Immersive.Winforms.Domain;

    [SubstituteClass(typeof (System.Windows.Forms.FolderBrowserDialog))]
    public class FolderBrowserDialog : CommonDialog, ISubstitute
    {
        private const DialogResult DefaultDialogResult = DialogResult.None;

        private string description;
        private DialogResult dialogResult;
        private Environment.SpecialFolder rootFolder;
        private string selectedPath;
        private bool showNewFolderButton;

        private readonly Handle handle;

        public FolderBrowserDialog()
        {
            DialogResult = DefaultDialogResult;
            this.handle = Session.Singleton.Create(this);
        }

        public DialogResult DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; }
        }

        #region FolderDialog

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public Environment.SpecialFolder RootFolder
        {
            get { return rootFolder; }
            set { rootFolder = value; }
        }

        public string SelectedPath
        {
            get { return selectedPath; }
            set { selectedPath = value; }
        }

        public bool ShowNewFolderButton
        {
            get { return showNewFolderButton; }
            set { showNewFolderButton = value; }
        }

        #endregion

        #region CommonDialog

        public override void Reset()
        {
        }

        protected override bool RunDialog(IntPtr hwndOwner)
        {
            Session.Singleton.OnTestingWinformsEvent(this.handle, TestingWinformsEventKind.Shown);
            return true;
        }

        #endregion

        #region Target Members

        public string SubstituteName
        {
            get { return "FolderBrowser"; }
        }

        #endregion
    }
}