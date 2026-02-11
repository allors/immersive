// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeViewTester.cs" company="allors bvba">
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
    using System.Windows.Forms;

    using Allors.Immersive.Winforms.Domain;

    using TreeView = Allors.Immersive.Winforms.Substitutes.TreeView;

    public class TreeViewTester : Tester<TreeView>
    {
        public TreeViewTester(Handle handle) : base(handle)
        {
        }

        public TreeViewTester(params string[] names) : base(names)
        {
        }

        public void SelectNode(params int[] indeces)
        {
            SelectNode(FindNode(indeces));
        }

        public void SelectNode(TreeNodeTester treeNodeTester)
        {
            if (treeNodeTester == null)
            {
                throw new ArgumentException("No treeNode");
            }

            Target.SelectedNode = treeNodeTester.Target;
        }

        public TreeNodeTester FindNode(params int[] indeces)
        {
            if (indeces.Length > 0)
            {
                int index = indeces[0];
                if (Target.Nodes.Count <= index)
                {
                    return null;
                }

                TreeNode treeNode = Target.Nodes[index];
                for (int i = 1; i < indeces.Length; i++)
                {
                    index = indeces[i];
                    if (treeNode.Nodes.Count <= index)
                    {
                        return null;
                    }

                    treeNode = treeNode.Nodes[index];
                }

                return new TreeNodeTester(this, treeNode);
            }
            else
            {
                return null;
            }
        }

        public TreeNodeTester SelectedNode
        {
            get
            {
                return new TreeNodeTester(this, Target.SelectedNode);
            }
        }

        public TreeNodeTester this[string[] ancestors]
        {
            get
            {
                if (Target.Nodes.Count == 0) return null;
                TreeNode currentNode = null;

                foreach(string ancestorNodeName in ancestors)
                {
                    if(currentNode == null)
                    {
                        currentNode = Target.Nodes[ancestorNodeName];
                    }
                    else
                    {
                        currentNode = currentNode.Nodes[ancestorNodeName];
                    }
                }

                return currentNode == null ? null : new TreeNodeTester(this, currentNode);
            }
        }
    }
}