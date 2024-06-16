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
/// Represents a bishop piece in a chess game.
/// </summary>
public class Bishop : PieceBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Bishop"/> class with the specified color.
    /// </summary>
    /// <param name="color">The color of the queen.</param>
    public Bishop(Color color) : base(PieceType.Bishop, color)
    {
    }

    public override ulong GetPieceAttacks(ulong position, Board board)
    {
        ulong pieceAttacks = this.GetPieceMovesOrAttacks(
            position,
            MovingDirections.Bishop,
            board,
            attacks: true);
        return this.PurgeIlegalMoves(position, pieceAttacks, board);
    }

    public override ulong GetPieceMoves(ulong position, Board board)
    {
        ulong pieceMoves = this.GetPieceMovesOrAttacks(
            position,
            MovingDirections.Bishop,
            board);
        return this.PurgeIlegalMoves(position, pieceMoves, board);
    }
}
