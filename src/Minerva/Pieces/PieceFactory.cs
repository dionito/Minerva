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

namespace Minerva.Pieces;

/// <summary>
/// Factory class for creating chess piece objects.
/// </summary>
public class PieceFactory
{
    /// <summary>
    /// Creates a chess piece of the specified type and color.
    /// </summary>
    /// <param name="pieceType">The type of the piece to create.</param>
    /// <param name="pieceColor">The color of the piece to create.</param>
    /// <returns>A new chess piece of the specified type and color.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid piece type is provided.</exception>
    public static PieceBase CreatePiece(PieceType pieceType, Color pieceColor)
    {
        return pieceType switch
        {
            PieceType.Bishop => new Bishop(pieceColor),
            PieceType.Queen => new Queen(pieceColor),
            PieceType.Rook => new Rook(pieceColor),
            PieceType.King => new King(pieceColor),
            PieceType.Knight => new Knight(pieceColor),
            PieceType.Pawn => new Pawn(pieceColor),
            PieceType.None => new NoPiece(),
            _ => throw new ArgumentException("Invalid piece type."),
        };
    }

    public static PieceBase CreatePiece(char pieceType, Color pieceColor)
    {
        pieceType = char.ToLower(pieceType);
        return pieceType switch
        {
            'b' => new Bishop(pieceColor),
            'q' => new Queen(pieceColor),
            'r' => new Rook(pieceColor),
            'k' => new King(pieceColor),
            'n' => new Knight(pieceColor),
            'p' => new Pawn(pieceColor),
            ' ' => new NoPiece(),
            _ => throw new ArgumentException("Invalid piece type."),
        };
    }
}
