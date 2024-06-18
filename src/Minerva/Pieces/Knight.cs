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
/// Represents a knight piece in a game of chess.
/// </summary>
public class Knight : PieceBase
{
    private static readonly MovingDirections[] KnightMoves = new MovingDirections[]
    {
        MovingDirections.Up | MovingDirections.UpLeft,
        MovingDirections.Up | MovingDirections.UpRight,
        MovingDirections.Down | MovingDirections.DownRight,
        MovingDirections.Down | MovingDirections.DownLeft,
        MovingDirections.Right | MovingDirections.UpRight,
        MovingDirections.Right | MovingDirections.DownRight,
        MovingDirections.Left | MovingDirections.UpLeft,
        MovingDirections.Left | MovingDirections.DownLeft,
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="Knight"/> class with the specified color.
    /// </summary>
    /// <param name="color">The color of the knight.</param>
    public Knight(Color color) : base(PieceType.Knight, color)
    {
    }

    public override ulong GetPieceAttacks(ulong position, Board board)
    {
        return this.GetKnightMovesOrAttacks(position, board, attacks: true);
    }

    public override ulong GetPieceMoves(ulong position, Board board)
    {
        return this.GetKnightMovesOrAttacks(position, board);
    }

    /// <summary>
    /// Gets all the valid moves for the knight from a given position on a given board.
    /// </summary>
    /// <param name="position">The current position of the knight.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>An enumerable collection of squares representing all the valid moves for the knight.</returns>
    protected ulong GetKnightMovesOrAttacks(ulong position, Board board, bool attacks = false)
    {
        // Possible perf. gains here by using the bit shifts directly, rather than
        // calling the Move method with KnightMoves. However, the Move method is more readable.
        ulong result = 0;
        foreach (MovingDirections knightMove in KnightMoves)
        {
            ulong newPosition = position.Move(knightMove);
            if (newPosition != 0)
            {
                if (attacks || (board.OccupiedBitBoard & newPosition) == 0 || 
                    board.SquareContainPieceOfColor(newPosition, this.Color.Opposite()))
                {
                    result |= newPosition;
                }
            }
        }

        if (!attacks)
        {
            result = this.PurgeIlegalMoves(position, result, board);
        }

        return result;
    }
}
