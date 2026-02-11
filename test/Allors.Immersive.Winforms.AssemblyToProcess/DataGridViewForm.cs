// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGridViewForm.cs" company="allors bvba">
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
    using System.Windows.Forms;

    public partial class DataGridViewForm : Form
    {
        public static readonly DateTime OffSetDate = new DateTime(2008, 1, 1);

        public DataGridViewForm()
        {
            InitializeComponent();
        }

        private void DataGridViewForm_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = CreatePopulation();
        }

        private static IList<Person> CreatePopulation()
        {
            IList<Person> persons = new List<Person>(10);
            for(int personIndex = 0; personIndex < 10; personIndex++)
            {
                Person person = new Person();
                person.Name = "person" + personIndex;
                person.DateOfBirth = OffSetDate.AddYears(-(personIndex*5));
                persons.Add(person);
            }

            return persons;
        }

        private class Person
        {
            private string name;
            private DateTime dateOfBirth;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            public DateTime DateOfBirth
            {
                get { return dateOfBirth; }
                set { dateOfBirth = value; }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectEvenRows();
        }

        private void SelectEvenRows()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Selected = row.Index %2 == 0;
            }
        }
    }
}