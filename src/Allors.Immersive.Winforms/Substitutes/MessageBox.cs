// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageBox.cs" company="allors bvba">
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
    using System.Windows.Forms;

    using Allors.Immersive.Attributes;
    using Allors.Immersive.Winforms.Domain;

    public class MessageBox : ISubstitute
    {
        private const DialogResult DefaultDialogResult = DialogResult.OK;

        private string caption;
        private DialogResult dialogResult;
        private string text;

        public MessageBox(string text, string caption)
        {
            this.text = text;
            this.caption = caption;
            dialogResult = DefaultDialogResult;
        }

        public string Text
        {
            get { return text; }
        }

        public string Caption
        {
            get { return caption; }
        }

        public DialogResult DialogResult
        {
            get { return dialogResult; }
            set { dialogResult = value; }
        }

        #region Target Members

        public string SubstituteName
        {
            get
            {
                if (caption != null)
                {
                    return caption;
                }
                else
                {
                    return text;
                }
            }
        }

        #endregion

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(string text)
        {
            return RaiseMessageBoxEvent(text, null);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(IWin32Window owner, string text)
        {
            return RaiseMessageBoxEvent(text, null);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(string text, string caption)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, 
                                        MessageBoxIcon icon)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, 
                                        MessageBoxDefaultButton defaultButton)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, 
                                        MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, 
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, 
                                        MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, 
                                        MessageBoxOptions options)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, 
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, 
                                        bool displayHelpButton)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, 
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, 
                                        string helpFilePath)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, 
                                        MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, 
                                        MessageBoxOptions options, string helpFilePath)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, 
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, 
                                        string helpFilePath, HelpNavigator navigator)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, 
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, 
                                        string helpFilePath, string keyword)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, 
                                        MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, 
                                        MessageBoxOptions options, string helpFilePath, HelpNavigator navigator)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, 
                                        MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, 
                                        MessageBoxOptions options, string helpFilePath, string keyword)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, 
                                        MessageBoxDefaultButton defaultButton, MessageBoxOptions options, 
                                        string helpFilePath, HelpNavigator navigator, object param)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        [SubstituteMethod(typeof (System.Windows.Forms.MessageBox))]
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, 
                                        MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, 
                                        MessageBoxOptions options, string helpFilePath, HelpNavigator navigator, 
                                        object param)
        {
            return RaiseMessageBoxEvent(text, caption);
        }

        private static DialogResult RaiseMessageBoxEvent(string text, string caption)
        {
            MessageBox messageBox = new MessageBox(text, caption);
            Handle handle = Session.Singleton.Create(messageBox);
            Session.Singleton.OnTestingWinformsEvent(handle, TestingWinformsEventKind.Shown);
            return messageBox.DialogResult;
        }
    }
}