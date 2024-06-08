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
/// Represents a knight piece in a game of chess.
/// </summary>
public class Knight : PieceBase
{
    /// <summary>
    /// The possible moves for a knight.
    /// </summary>
    private static readonly Move[] KnightMoves =
    {
        2 * Move.Up + Move.Left,
        2 * Move.Up + Move.Right,
        2 * Move.Down + Move.Left,
        2 * Move.Down + Move.Right,
        Move.Down + 2 * Move.Left,
        Move.Down + 2 * Move.Right,
        Move.Up + 2 * Move.Left,
        Move.Up + 2 * Move.Right,
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="Knight"/> class with the specified color.
    /// </summary>
    /// <param name="color">The color of the knight.</param>
    public Knight(Color color) : base(PieceType.Knight, color)
    {
    }

    /// <summary>
    /// Gets all the possible moves for the knight from a given position on a given board.
    /// </summary>
    /// <param name="position">The current position of the knight.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>An array of squares representing all the possible moves for the knight.</returns>
    public override Square[] GetPossibleMoves(Square position, Board board)
    {
        return this.GetKnightMoves(position, board).ToArray();
    }

    /// <summary>
    /// Gets all the valid moves for the knight from a given position on a given board.
    /// </summary>
    /// <param name="position">The current position of the knight.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>An enumerable collection of squares representing all the valid moves for the knight.</returns>
    protected IEnumerable<Square> GetKnightMoves(Square position, Board board)
    {
        foreach (Move knightMove in KnightMoves)
        {
            if (position.TryMove(knightMove, out Square newPosition))
            {
                if (board.SquareContainPieceOfColor(newPosition, this.Color))
                {
                    continue;
                }

                yield return newPosition;
            }
        }
    }
}
