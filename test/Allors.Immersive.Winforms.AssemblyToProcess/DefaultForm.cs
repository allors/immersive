// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultForm.cs" company="allors bvba">
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
namespace AllorsTestWindowsAssembly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    public partial class DefaultForm : Form
    {
       
        public DefaultForm()
        {
            InitializeComponent();

#if NETFRAMEWORK
            this.contextMenu1.Name = "contextMenu1";

            var dataGrid = new DataGrid();
            dataGrid.Name = "dataGrid1";

            var statusBar = new StatusBar();
            statusBar.Name = "statusBar1";

            var toolBar = new ToolBar();
            toolBar.Name = "toolBar1";
#endif
            
            var toolStripDropDownMenu = new ToolStripDropDownMenu();
            toolStripDropDownMenu.Name = "toolStripDropDownMenu1";

            var toolStripDropDown = new ToolStripDropDown();
            toolStripDropDown.Name = "toolStripDropDown1";

            var toolstripPanel = new ToolStripPanel();
            toolstripPanel.Name = "toolStripPanel1";
            toolstripPanel.Dock = DockStyle.Top;

            propertyGrid1.SelectedObject = new A();

            this.ACollection = new[]
                                   {
                                       new A() { FirstX = "X1", FirstY = "Y1", Second = 11M },
                                       new A() { FirstX = "X2", FirstY = "Y2", Second = 12M },
                                       new A() { FirstX = "X3", FirstY = "Y3", Second = 13M }
                                   }.ToList();

            this.BCollection = new[]
                                   {
                                       new B() { FirstX = "B1", FirstY = "B1", Second = 11M },
                                       new B() { FirstX = "B2", FirstY = "B2", Second = 12M },
                                       new B() { FirstX = "B3", FirstY = "B3", Second = 13M }
                                   }.ToList();

            this.comboBox1.DisplayMember = "FirstX";
            this.comboBox1.DataSource = this.ACollection;

            foreach (var a in ACollection)
            {
                this.comboBox2.Items.Add(a.FirstX);
            }
            comboBox2.SelectedIndex = 0;

            this.comboBox1.SelectedIndexChanged += this.comboBox1_SelectedIndexChanged;
            this.comboBox2.SelectedIndexChanged += this.comboBox2_SelectedIndexChanged;

            this.dataGridView1.DataSource = this.bindingSource1;
            this.bindingSource1.Clear();
            this.bindingSource1.DataSource = this.ACollection;

            this.bindingSource2.Clear();
            this.bindingSource2.DataSource = this.BCollection;
            
        }

        public List<B> BCollection { get; set; }

        public List<A> ACollection { get; set; }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBox1.Text = e.Node.Text;
        }

        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            textBox1.Text = e.Node.Text;
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            textBox1.Text = e.Node.Text;
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.Text = e.Location.ToString();
        }

#if NETFRAMEWORK
        private void menuItem1_Click(object sender, EventArgs e)
        {
            textBox1.Text = menuItem1.Text;
        }
#endif

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogForm dialogForm = new DialogForm();
            dialogForm.ShowDialog();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox1.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = MessageBox.Show("Hello", "Hello").ToString();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "toolStripMenuItem1";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "toolStripButton1";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogForm dialogForm = new DialogForm();
            dialogForm.ShowDialog(this);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox1.Text = ((A)this.comboBox1.SelectedItem).FirstX;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox2.Text = (string)this.comboBox2.SelectedItem;
        }

        private void buttonAddTextBox_Click(object sender, EventArgs e)
        {
            var textbox = new TextBox();
            textbox.Name = "textBoxHelloAllors";
            textbox.Text = @"I'm added to the controls collection";
            this.Controls.Add(textbox);

            textbox = new TextBox();
            textbox.Text = @"I'm added to the panels collection";
            this.panel2.Controls.Add(textbox);

        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            this.bindingSource1.ResetBindings(false);
        }

        private void buttonAddBindingNavigator_Click(object sender, EventArgs e)
        {
            var bindingNavigator = new BindingNavigator();
            bindingNavigator.BindingSource = this.bindingSource2;
            this.emptyPanel.Controls.Clear();
            this.emptyPanel.Controls.Add(bindingNavigator);
            bindingNavigator.Dock = DockStyle.Top;
        }
    }
}