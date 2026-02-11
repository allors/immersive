// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestForm.cs" company="allors bvba">
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
namespace Allors.Immersive.AssemblyToProcess
{
    public partial class TestForm : Allors.Immersive.AssemblyReferenced.Form
    {
        public bool constructorCalled = false;

        private Allors.Immersive.AssemblyReferenced.Button button1;
        private Allors.Immersive.AssemblyReferenced.TextBox textBox1;
        private Allors.Immersive.AssemblyReferenced.Nada nada;
        private Allors.Immersive.AssemblyReferenced.SealedSingle sealedSingle;
        private Allors.Immersive.AssemblyReferenced.SealedHierarchy sealedHierarchy;

        public TestForm()
        {
            constructorCalled = true;

            button1 = new Allors.Immersive.AssemblyReferenced.Button();
            textBox1 = new Allors.Immersive.AssemblyReferenced.TextBox();
            nada = new Allors.Immersive.AssemblyReferenced.Nada();
            sealedSingle = new Allors.Immersive.AssemblyReferenced.SealedSingle();
            sealedHierarchy = new Allors.Immersive.AssemblyReferenced.SealedHierarchy();
        }

        public static string ShowMessageBox(bool boolean)
        {
            return Allors.Immersive.AssemblyReferenced.MessageBox.Show(boolean);
        }

        public static string ShowMessageBox(string text)
        {
            return Allors.Immersive.AssemblyReferenced.MessageBox.Show(text);
        }

        public static string ShowMessageBox(int integer)
        {
            return Allors.Immersive.AssemblyReferenced.MessageBox.Show(integer);
        }

        public static string ShowMessageBox(string text, int integer)
        {
            return Allors.Immersive.AssemblyReferenced.MessageBox.Show(text, integer);
        }


        public static string ShowMessageBox2(bool boolean)
        {
            return Allors.Immersive.AssemblyReferenced.MessageBox.Show2(boolean);
        }

        public static string ShowMessageBox2(string text)
        {
            return Allors.Immersive.AssemblyReferenced.MessageBox.Show2(text);
        }

        public static string ShowMessageBox2(int integer)
        {
            return Allors.Immersive.AssemblyReferenced.MessageBox.Show2(integer);
        }

        public static string ShowMessageBox2(string text, int integer)
        {
            return Allors.Immersive.AssemblyReferenced.MessageBox.Show2(text, integer);
        }

        #region Properties
        public Allors.Immersive.AssemblyReferenced.Button Button1
        {
            get { return button1; }
        }

        public Allors.Immersive.AssemblyReferenced.TextBox TextBox1
        {
            get { return textBox1; }
        }

        public Allors.Immersive.AssemblyReferenced.Nada Nada
        {
            get { return nada; }
        }

        public Allors.Immersive.AssemblyReferenced.SealedSingle SealedSingle
        {
            get { return sealedSingle; }
        }

        public Allors.Immersive.AssemblyReferenced.SealedHierarchy SealedHierarchy
        {
            get { return sealedHierarchy; }
        }
        #endregion

        public string CallShowDialog()
        {
            return this.ShowDialog();
        }
    }
}