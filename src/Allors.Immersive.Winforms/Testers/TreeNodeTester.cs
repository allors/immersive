// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeNodeTester.cs" company="allors bvba">
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

    public class TreeNodeTester
    {
        private TreeNode target;
        private TreeViewTester treeViewTester;

        public TreeNodeTester(TreeViewTester treeViewTester, TreeNode treeNode)
        {
            this.treeViewTester = treeViewTester;
            target = treeNode;
        }

        public TreeNode Target
        {
            get { return target; }
        }

        public TreeViewTester TreeViewTester
        {
            get { return treeViewTester; }
        }

        public TreeNodeTester this[int index]
        {
            get
            {
                if (index < Target.Nodes.Count)
                {
                    return new TreeNodeTester(treeViewTester, Target.Nodes[index]);
                }
                else
                {
                    return null;
                }
            }
        }

        public TreeNodeTester[] FindByTagType(Type type)
        {
            List<TreeNodeTester> nodeTesters = new List<TreeNodeTester>();
            foreach (TreeNode nodeTester in Target.Nodes)
            {
                object tag = nodeTester.Tag;
                if (tag != null && tag.GetType().Equals(type))
                {
                    nodeTesters.Add(new TreeNodeTester(treeViewTester, nodeTester));
                }
            }

            return nodeTesters.ToArray();
        }

        public void Select()
        {
            treeViewTester.SelectNode(this);
        }

        public void Click()
        {
            treeViewTester.Target.PerformClick(Target);
        }
    }
}