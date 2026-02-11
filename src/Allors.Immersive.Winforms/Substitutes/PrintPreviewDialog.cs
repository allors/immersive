// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrintPreviewDialog.cs" company="allors bvba">
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
    using Allors.Immersive.Attributes;
    using Allors.Immersive.Winforms.Domain;

    /// <summary>
    /// Substitute for <see cref="System.Windows.Forms.PrintPreviewDialog" />.
    /// </summary>
    [SubstituteClass]
    public partial class PrintPreviewDialog : System.Windows.Forms.PrintPreviewDialog, ISubstitute
    {
        /// <summary>
        /// The Handle/
        /// </summary>
        private readonly Handle handle;

        /// <summary>
        /// Initializes a new instance of the PrintPreviewDialog class. Initializes a new instance of the.Class.
        /// </summary>
        public PrintPreviewDialog()
        {
            this.handle = Session.Singleton.Create(this);
        }

        /// <summary>
        /// Gets the name of the substitute.
        /// </summary>
        /// <value>
        /// The name of the substitute.
        /// </value>
        public string SubstituteName
        {
            get { return Name; }
        }
    }
}