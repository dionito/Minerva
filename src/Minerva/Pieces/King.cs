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

using Minerva.Extensions;

namespace Minerva.Pieces;

/// <summary>
/// Represents a king piece in a game of chess.
/// </summary>
public class King : PieceBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="King"/> class with the specified color.
    /// </summary>
    /// <param name="color">The color of the king.</param>
    public King(Color color) : base(PieceType.King, color)
    {
    }

    /// <summary>
    /// Gets all the possible moves for the king from a given position on a given board.
    /// </summary>
    /// <param name="position">The current position of the king.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>An array of squares representing all the possible moves for the king.</returns>
    public override Square[] GetPossibleMoves(Square position, Board board)
    {
        return this.GetValidMoves(position, Move.Up, board)
            .Union(this.GetValidMoves(position, Move.Down, board))
            .Union(this.GetValidMoves(position, Move.Right, board))
            .Union(this.GetValidMoves(position, Move.Left, board))
            .Union(this.GetValidMoves(position, Move.UpLeft, board))
            .Union(this.GetValidMoves(position, Move.UpRight, board))
            .Union(this.GetValidMoves(position, Move.DownLeft, board))
            .Union(this.GetValidMoves(position, Move.DownRight, board))
            .ToArray();
    }

    /// <summary>
    /// Gets all the valid moves for the king from a given position in a given direction on a given board.
    /// </summary>
    /// <param name="position">The current position of the king.</param>
    /// <param name="direction">The direction of the move.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>An enumerable collection of squares representing all the valid moves for the king
    /// in the given direction.</returns>
    protected override IEnumerable<Square> GetValidMoves(Square position, Move direction, Board board)
    {
        if (position.TryMove(direction, out Square newPosition))
        {
            if ((board.OccupiedBitBoard & newPosition.BitBoard) == 0ul)
            {
                yield return newPosition;
                yield break;
            }

            if (board.SquareContainPieceOfColor(newPosition, this.Color.Opposite()))
            {
                yield return newPosition;
            }
        }
    }
}
