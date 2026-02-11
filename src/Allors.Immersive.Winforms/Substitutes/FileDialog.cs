// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileDialog.cs" company="allors bvba">
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

    [SubstituteClass(typeof (System.Windows.Forms.FileDialog))]
    public abstract class FileDialog : CommonDialog, ISubstitute
    {
        private const DialogResult DefaultDialogResult = DialogResult.None;

        private DialogResult dialogResult;
        private readonly Handle handle;

        public FileDialog()
        {
            DialogResult = DefaultDialogResult;
            this.handle = Session.Singleton.Create(this);
        }

        public DialogResult DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; }
        }

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

        #region FileDialog

        private bool addExtension;
        private bool checkFileExists;
        private bool checkPathExists;
        private string defaultExt;
        private bool dereferenceLinks;
        private string fileName;
        private string[] fileNames;
        private string filter;
        private int filterIndex;
        private string initialDirectory;
        private bool restoreDirectory;
        private bool showHelp;
        private bool supportMultiDottedExtensions;
        private string title;
        private bool validateNames;

        public bool AddExtension
        {
            get { return addExtension; }
            set { addExtension = value; }
        }

        public bool CheckFileExists
        {
            get { return checkFileExists; }
            set { checkFileExists = value; }
        }

        public bool CheckPathExists
        {
            get { return checkPathExists; }
            set { checkPathExists = value; }
        }

        public string DefaultExt
        {
            get { return defaultExt; }
            set { defaultExt = value; }
        }

        public bool DereferenceLinks
        {
            get { return dereferenceLinks; }
            set { dereferenceLinks = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public string[] FileNames
        {
            get { return fileNames; }
            set { fileNames = value; }
        }

        public string Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        public int FilterIndex
        {
            get { return filterIndex; }
            set { filterIndex = value; }
        }

        public string InitialDirectory
        {
            get { return initialDirectory; }
            set { initialDirectory = value; }
        }

        public bool RestoreDirectory
        {
            get { return restoreDirectory; }
            set { restoreDirectory = value; }
        }

        public bool ShowHelp
        {
            get { return showHelp; }
            set { showHelp = value; }
        }

        public bool SupportMultiDottedExtensions
        {
            get { return supportMultiDottedExtensions; }
            set { supportMultiDottedExtensions = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public bool ValidateNames
        {
            get { return validateNames; }
            set { validateNames = value; }
        }

        #endregion

        #region Target Members

        public abstract string SubstituteName { get; }

        #endregion
    }
}