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
/// Represents a pawn piece in a game of chess.
/// </summary>
public class Pawn : PieceBase
{
    readonly MovingDirections standardMove;
    readonly MovingDirections[] captureMoves;
    readonly ulong initialRank;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pawn"/> class with the specified color.
    /// </summary>
    /// <param name="color">The color of the pawn.</param>
    public Pawn(Color color) : base(PieceType.Pawn, color)
    {
        this.standardMove = color == Color.White ? MovingDirections.Up : MovingDirections.Down;
        this.initialRank = color == Color.White ? Board.Rank2 : Board.Rank7;
        this.captureMoves = color == Color.White
            ? new[] { MovingDirections.UpLeft, MovingDirections.UpRight }
            : new[] { MovingDirections.DownLeft, MovingDirections.DownRight };
    }

    public override ulong GetPieceMoves(ulong position, Board board)
    {
        return this.GetPawnMoves(position, board);
    }

    protected ulong GetPawnMoves(ulong position, Board board)
    {
        ulong result = 0;

        // Try to move the pawn forward
        ulong newPosition = position.Move(this.standardMove);
        if (newPosition != 0)
        {
            // If the square in front is empty, the pawn can move there
            if (board.IsEmptySquare(newPosition))
            {
                result |= newPosition;

                // If the pawn is on its initial rank, it can also move two squares forward,
                // provided both squares in front are empty
                newPosition = newPosition.Move(this.standardMove);
                if ((position & this.initialRank) != 0 &&
                    newPosition != 0 &&
                    board.IsEmptySquare(newPosition))
                {
                    result |= newPosition;
                }
            }
        }

        // Check for capturing moves regardless of whether the pawn can move forward
        // A pawn can capture a piece if it is located diagonally in front of the pawn (to the left or right)
        foreach (var direction in this.captureMoves)
        {
            ulong capturePosition = position.Move(direction);
            if (position != 0 &&
                (board.SquareContainPieceOfColor(capturePosition, this.Color.Opposite()) ||
                    (board.EnPassantTargetSquare.BitBoard & capturePosition) != 0))
            {
                result |= capturePosition;
            }
        }

        return result;
    }
}
