// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatePicker.cs" company="allors bvba">
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

namespace Allors.Immersive.Avalonia.Substitutes
{
    using global::Avalonia;

    using Allors.Immersive.Avalonia.Domain;

    public partial class DatePicker
    {
        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == global::Avalonia.Controls.DatePicker.SelectedDateProperty)
            {
                Session.Singleton.OnTestingAvaloniaEvent(this.handle, TestingAvaloniaEventKind.ValueChanged);
            }
        }
    }
}
