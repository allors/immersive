// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGrid.cs" company="allors bvba">
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
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    using Allors.Immersive.Attributes;
    using Allors.Immersive.Winforms.Domain;

    [SubstituteClass]
    public partial class PropertyGrid : System.Windows.Forms.PropertyGrid, ISubstitute
    {
        private readonly Handle handle;

        public PropertyGrid()
        {
            this.handle = Session.Singleton.Create(this);
        }

        #region Target Members

        public string SubstituteName
        {
            get { return Name; }
        }

        #endregion
    }

    public partial class EditorServiceProvider : ITypeDescriptorContext, IServiceProvider, IWindowsFormsEditorService
    {
        private Form form;
        private object instance;

        public EditorServiceProvider(object instance)
        {
            this.instance = instance;
        }

        #region ITypeDescriptorContext Members

        public object GetService(Type serviceType)
        {
            return this;
        }

        public IContainer Container
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public object Instance
        {
            get { return instance; }
        }

        public void OnComponentChanged()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool OnComponentChanging()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public PropertyDescriptor PropertyDescriptor
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IWindowsFormsEditorService Members

        public void CloseDropDown()
        {
            form?.Close();
        }

        public void DropDownControl(Control control)
        {
            control.Name = "EDITOR_CONTROL";

            using (form = new Form())
            {
                form.Name = "EDITOR";
                form.Controls.Add(control);
                form.ShowDialog();
            }
        }

        public DialogResult ShowDialog(System.Windows.Forms.Form dialog)
        {
            return dialog.ShowDialog();
        }

        #endregion
    }
}