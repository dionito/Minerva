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
/// Represents a pawn piece in a game of chess.
/// </summary>
public class Pawn : PieceBase
{
    readonly Move standardMove;
    readonly int initialRank;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pawn"/> class with the specified color.
    /// </summary>
    /// <param name="color">The color of the pawn.</param>
    public Pawn(Color color) : base(PieceType.Pawn, color)
    {
        this.standardMove = color == Color.White ? Move.Up : Move.Down;
        this.initialRank = color == Color.White ? 2 : 7;
    }

    /// <summary>
    /// Gets all the possible moves for the pawn from a given position on a given board.
    /// </summary>
    /// <param name="position">The current position of the pawn.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>An array of squares representing all the possible moves for the pawn.</returns>
    public override Square[] GetPossibleMoves(Square position, Board board)
    {
        return this.GetPawnMoves(position, board).ToArray();
    }

    /// <summary>
    /// Gets all the valid moves for the pawn from a given position on a given board.
    /// </summary>
    /// <param name="position">The current position of the pawn.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>An enumerable collection of squares representing all the valid moves for the pawn.</returns>
    protected IEnumerable<Square> GetPawnMoves(Square position, Board board)
    {
        // Try to move the pawn forward
        if (position.TryMove(this.standardMove, out Square newPosition))
        {
            // If the square in front is empty, the pawn can move there
            if (board.IsEmptySquare(newPosition))
            {
                yield return newPosition;

                // If the pawn is on its initial rank, it can also move two squares forward,
                // provided both squares in front are empty
                if (position.Rank == this.initialRank &&
                    newPosition.TryMove(this.standardMove, out newPosition) &&
                    board.IsEmptySquare(newPosition))
                {
                    yield return newPosition;
                }
            }
        }

        // Check for capturing moves regardless of whether the pawn can move forward
        // A pawn can capture a piece if it is located diagonally in front of the pawn (to the left or right)
        foreach (var direction in new[] { this.standardMove + Move.Left, this.standardMove + Move.UpRight })
        {
            if (position.TryMove(direction, out Square capturePosition) &&
                board.SquareContainPieceOfColor(capturePosition, this.Color.Opposite()))
            {
                yield return capturePosition;
            }
        }
    }
}
