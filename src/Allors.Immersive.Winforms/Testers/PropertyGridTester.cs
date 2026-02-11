// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGridTester.cs" company="allors bvba">
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
namespace Allors.Immersive.Winforms.Testers
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    using Allors.Immersive.Winforms.Domain;

    using PropertyGrid = Allors.Immersive.Winforms.Substitutes.PropertyGrid;

    public delegate void EditorMockDropDownEventHandler(object sender, EditorMockDropDownEventArgs args);

    public class EditorMockDropDownEventArgs
    {
        private Control control;

        public EditorMockDropDownEventArgs(Control control)
        {
            this.control = control;
        }

        public Control Control
        {
            get { return control; }
        }
    }

    public class EditorMock : ITypeDescriptorContext, IServiceProvider, IWindowsFormsEditorService
    {
        private object instance;

        public EditorMock(object instance)
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
        }

        public void DropDownControl(Control control)
        {
            EditorMockDropDownEventHandler editorMockDropDown = EditorMockDropDown;
            if (editorMockDropDown != null)
            {
                editorMockDropDown(this, new EditorMockDropDownEventArgs(control));
            }
        }

        public DialogResult ShowDialog(Form dialog)
        {
            dialog.ShowDialog();
            return dialog.DialogResult;
        }

        #endregion

        public event EditorMockDropDownEventHandler EditorMockDropDown;
    }

    public class PropertyGridTester : Tester<PropertyGrid>
    {
        public PropertyGridTester(Handle handle) : base(handle)
        {
        }

        public PropertyGridTester(params string[] names) : base(names)
        {
        }

        private GridItemTester RootGridItemTester
        {
            get
            {
                GridItem parentGridItem = Target.SelectedGridItem.Parent;
                while (parentGridItem.Parent != null)
                {
                    parentGridItem = parentGridItem.Parent;
                }

                GridItemTester rootGridItemTester = new GridItemTester(Target.SelectedObject, parentGridItem);
                return rootGridItemTester;
            }
        }

        public GridItemTester FindGridItem(params string[] names)
        {
            return RootGridItemTester.Find(names);
        }
    }
}