// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridViewTest.cs" company="allors bvba">
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

namespace Allors.Immersive.Winforms.Tests
{
    using System.Windows.Forms;

    using Allors.Immersive.Winforms.Testers;

    using AllorsTestWindowsAssembly;

    using Xunit;

    
    public class DataGridViewTest : WinformsTest
    {
        const string GridName = "dataGridView1";
        const string ButtonName = "button1";

        private DataGridViewForm form;
        public DataGridViewTest()
        {
            this.form = new DataGridViewForm();
            this.form.Show();
        }

        [Fact]
        public void FindTesterByName()
        {
            var tester = new DataGridViewTester("dataGridView1");
            Assert.NotNull(tester.Target);
            Assert.IsAssignableFrom<DataGridView>(tester.Target);
        }

        [Fact]
        public void FindRow()
        {
            var tester = new DataGridViewTester("dataGridView1");

            var row = tester.FindRow(v => v.Index == 0);
            Assert.NotNull(row);
            Assert.IsAssignableFrom<DataGridViewRow>(row);
        }

        [Fact]
        public void FindRows()
        {
            var tester = new DataGridViewTester("dataGridView1");

            var rows = tester.FindRows(v => v.Index > 0 && v.Index < 5);
            Assert.NotNull(rows);
            Assert.Equal(4, rows.Length);
        }


        [Fact]
        public void GetRowCount()
        {
            var dataGridView = new DataGridViewTester(GridName);
            Assert.Equal(10, dataGridView.Target.Rows.Count);
        }

        [Fact]
        public void GetColumnCount()
        {
            var dataGridView = new DataGridViewTester(GridName);
            Assert.Equal(2, dataGridView.Target.Columns.Count);
        }

        [Fact]
        public void CanTestCellContent()
        {
            var dataGridView = new DataGridViewTester(GridName);   
            Assert.Equal("person0", dataGridView.Target.Rows[0].Cells["Name"].Value);
        }

        [Fact]
        public void CanTestSelectedRows()
        {
            var dataGridView = new DataGridViewTester(GridName);
            var button1 = new ButtonTester(ButtonName);

            button1.Click();

            var selectedRows = dataGridView.Target.SelectedRows;
            Assert.Equal(5, selectedRows.Count);

            foreach(DataGridViewRow row in selectedRows)
            {
                Assert.Equal(row.Index % 2 == 0, row.Selected);
            }
        }

        [Fact]
        public void CanSetSelectedRows()
        {
            var dataGridView = new DataGridViewTester(GridName);
            
            foreach(DataGridViewRow row in dataGridView.Target.Rows)
            {
                // Select the odd rows here ...
                row.Selected = row.Index % 2 != 0;
            }

            var selectedRows = dataGridView.Target.SelectedRows;

            Assert.Equal(5, selectedRows.Count);

            foreach (DataGridViewRow row in selectedRows)
            {
                Assert.Equal(row.Index % 2 != 0, row.Selected);
            }
        }
    }
}