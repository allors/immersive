// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FontDialog.cs" company="allors bvba">
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
    using System.Drawing;
    using System.Windows.Forms;

    using Allors.Immersive.Attributes;
    using Allors.Immersive.Winforms.Domain;

    [SubstituteClass(typeof(System.Windows.Forms.FontDialog))]
    public class FontDialog : CommonDialog, ISubstitute
    {
        private const DialogResult DefaultDialogResult = DialogResult.None;

        private Color color;
        private DialogResult dialogResult;
        private Font font;
        private bool showColor;
        private bool showApply;
        private bool showEffects;
        private bool showHelp;

        private readonly Handle handle;

        public FontDialog()
        {
            DialogResult = DefaultDialogResult;
            this.showEffects = true;
            this.handle = Session.Singleton.Create(this);
        }

        public DialogResult DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        public bool ShowColor
        {
            get { return showColor; }
            set { showColor = value; }
        }

        public bool ShowApply
        {
            get { return showApply; }
            set { showApply = value; }
        }

        public bool ShowEffects
        {
            get { return showEffects; }
            set { showEffects = value; }
        }

        public bool ShowHelp
        {
            get { return showHelp; }
            set { showHelp = value; }
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
            get { return "FontDialog"; }
        }
    }
}
