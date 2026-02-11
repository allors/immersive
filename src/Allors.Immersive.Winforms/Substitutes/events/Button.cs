// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Button.cs" company="allors bvba">
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

    using Allors.Immersive.Winforms.Domain;

    public partial class Button
    {
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Session.Singleton.OnTestingWinformsEvent(this.handle, TestingWinformsEventKind.Click);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            Session.Singleton.OnTestingWinformsEvent(this.handle, TestingWinformsEventKind.DoubleClick);
        }
    }
}