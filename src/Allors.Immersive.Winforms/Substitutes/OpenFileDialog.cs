// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenFileDialog.cs" company="allors bvba">
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
    using System.IO;

    using Allors.Immersive.Attributes;

    [SubstituteClass(typeof (System.Windows.Forms.OpenFileDialog))]
    public sealed class OpenFileDialog : FileDialog, ISubstitute
    {
        #region Target Members

        public override string SubstituteName
        {
            get { return "OpenFileDialog"; }
        }

        #region OpenFileDialog

        private bool multiselect;
        private bool readOnlyChecked;
        private bool showReadOnly;

        public bool Multiselect
        {
            get { return multiselect; }
            set { multiselect = value; }
        }

        public bool ReadOnlyChecked
        {
            get { return readOnlyChecked; }
            set { readOnlyChecked = value; }
        }

        public bool ShowReadOnly
        {
            get { return showReadOnly; }
            set { showReadOnly = value; }
        }


        public Stream OpenFile()
        {
            return new FileStream(FileName, FileMode.Open, FileAccess.Read);
        }

        #endregion

        #endregion
    }
}