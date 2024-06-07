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
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// 

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

    /// <summary>
    /// Gets all possible moves for the bishop from  a given position on a given board.
    /// </summary>
    /// <param name="position">The current position of the bishop.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>An array of squares representing all possible moves for the bishop.</returns>
    public override Square[] GetPossibleMoves(Square position, Board board)
    {
        return this.GetValidMoves(position, Move.UpLeft, board).Concat(
            this.GetValidMoves(position, Move.UpRight, board)).Concat(
            this.GetValidMoves(position, Move.DownLeft, board)).Concat(
            this.GetValidMoves(position, Move.DownRight, board)).ToArray();
    }
}
