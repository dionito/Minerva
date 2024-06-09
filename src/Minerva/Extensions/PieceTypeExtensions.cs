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

using Minerva.Pieces;

namespace Minerva.Extensions;

public static class PieceTypeExtensions
{
    /// <summary>
    /// Converts a character to a PieceType.
    /// </summary>
    /// <param name="pieceType">The character to convert.</param>
    /// <returns>The converted PieceType.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the character does not
    /// represent a valid PieceType.</exception>
    public static PieceType ToPieceType(this char pieceType)
    {
        pieceType = char.ToLower(pieceType);
        if (Enum.IsDefined(typeof(PieceType), (int)pieceType))
        {
            return (PieceType)pieceType;
        }

        throw new ArgumentOutOfRangeException(nameof(pieceType), pieceType, "Invalid piece type.");
    }

    /// <summary>
    /// Converts a PieceType to a character.
    /// </summary>
    /// <param name="piece">The PieceType to convert.</param>
    /// <returns>The converted character.</returns>
    public static char ToChar(this PieceType piece) => (char)piece;
}
