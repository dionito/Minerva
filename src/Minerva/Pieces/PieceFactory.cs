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
public static class PieceFactory
{
    private static readonly Dictionary<char, PieceBase> AllPieces = new()
    {
        { 'r', new Rook(Color.Black) },
        { 'n', new Knight(Color.Black) },
        { 'b', new Bishop(Color.Black) },
        { 'q', new Queen(Color.Black) },
        { 'k', new King(Color.Black) },
        { 'p', new Pawn(Color.Black) },
        { 'R', new Rook(Color.White) },
        { 'N', new Knight(Color.White) },
        { 'B', new Bishop(Color.White) },
        { 'Q', new Queen(Color.White) },
        { 'K', new King(Color.White) },
        { 'P', new Pawn(Color.White) },
        { ' ', new NoPiece() },
    };

    /// <summary>
    /// Creates a chess piece of the specified type and color.
    /// </summary>
    /// <param name="pieceType">The type of the piece to create.</param>
    /// <param name="pieceColor">The color of the piece to create.</param>
    /// <returns>A new chess piece of the specified type and color.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid piece type is provided.</exception>
    public static PieceBase GetPiece(PieceType pieceType, Color pieceColor)
    {
        var pieceChar = (char)pieceType;
        return pieceColor == Color.White
            ? AllPieces[char.ToUpper(pieceChar)]
            : AllPieces[pieceChar];
    }

    public static PieceBase GetPiece(char pieceType)
    {
        return AllPieces[pieceType];
    }
}
