// Copyright (C) 2024 dionito
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>

namespace Minerva.Extensions;

/// <summary>
/// Provides extension methods for the Color enumeration.
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// Gets the opposite color of the provided Color enumeration.
    /// </summary>
    /// <param name="color">The Color enumeration to get the opposite of.</param>
    /// <returns>The opposite Color enumeration.</returns>
    public static Color Opposite(this Color color) => color switch
    {
        Color.Black => Color.White,
        Color.White => Color.Black,
        _ => Color.None,
    };

    /// <summary>
    /// Converts the Color enumeration to its corresponding char representation.
    /// </summary>
    /// <param name="color">The Color enumeration to convert.</param>
    /// <returns>The char representation of the Color enumeration.</returns>
    public static char ToChar(this Color color) => (char)color;

    /// <summary>
    /// Converts a char representation of a Color to its corresponding Color enumeration.
    /// </summary>
    /// <param name="color">The char representation of a Color to convert.</param>
    /// <returns>The Color enumeration corresponding to the char representation.</returns>
    public static Color ToColor(this char color)
    {
        if (Enum.IsDefined(typeof(Color), (int)color))
        {
            return (Color)color;
        }

        throw new ArgumentOutOfRangeException(nameof(color), color, "Invalid color");
    }
}
