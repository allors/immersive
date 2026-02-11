// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridItemTester.cs" company="allors bvba">
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
    using System.Collections.Generic;
    using System.Windows.Forms;

    public class GridItemTester
    {
        private GridItemTester[] children;
        private GridItem gridItem;
        private object propertyObject;

        internal GridItemTester(object propertyObject, GridItem gridItem)
        {
            this.propertyObject = propertyObject;
            this.gridItem = gridItem;

            List<GridItemTester> childList = new List<GridItemTester>();
            foreach (GridItem subGridItem in gridItem.GridItems)
            {
                AddChildren(childList, subGridItem);
            }

            children = childList.ToArray();
        }

        public GridItemTester[] Children
        {
            get { return children; }
        }

        public object Value
        {
            get
            {
                if (gridItem.GridItemType.Equals(GridItemType.Root))
                {
                    return propertyObject;
                }
                else
                {
                    object value = gridItem.PropertyDescriptor.GetValue(propertyObject);
                    if (value == null)
                    {
                        return null;
                    }
                    else
                    {
                        return value;
                    }
                }
            }

            set { gridItem.PropertyDescriptor.SetValue(propertyObject, value); }
        }

        private void AddChildren(List<GridItemTester> childList, GridItem subGridItem)
        {
            if (subGridItem.GridItemType.Equals(GridItemType.Property))
            {
                childList.Add(new GridItemTester(Value, subGridItem));
            }
            else
            {
                foreach (GridItem subSubChildGridItem in subGridItem.GridItems)
                {
                    AddChildren(childList, subSubChildGridItem);
                }
            }
        }

        public override string ToString()
        {
            return gridItem.PropertyDescriptor.Name + ":" + Value.ToString();
        }

        public void Edit()
        {
            // GridItem gridItem = FindGridItem(propertyName);

            // if (gridItem != null)
            // {
            // PropertyDescriptor descriptor = gridItem.PropertyDescriptor;
            // UITypeEditor editor = (UITypeEditor)descriptor.GetEditor(typeof(UITypeEditor));
            // EditorServiceProvider serviceProdvider = new EditorServiceProvider(Target.SelectedObject);
            // object value = descriptor.GetValue(Target.SelectedObject);
            // editor.EditValue(serviceProdvider, value);
            // }
        }

        internal GridItemTester Find(string[] names)
        {
            if (names != null && names.Length > 0)
            {
                foreach (GridItemTester gridItemTester in Children)
                {
                    GridItemTester result = gridItemTester.Find(names, 0);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        internal GridItemTester Find(string[] names, int i)
        {
            if (gridItem.PropertyDescriptor.Name.ToString().Equals(names[i]))
            {
                if (i < names.Length - 1)
                {
                    foreach (GridItemTester child in Children)
                    {
                        GridItemTester match = child.Find(names, i + 1);
                        if (match != null)
                        {
                            return match;
                        }
                    }
                }
                else
                {
                    return this;
                }
            }

            return null;
        }
    }
}