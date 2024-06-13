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

    public override ulong GetPieceMoves(ulong position, Board board)
    {
        return this.GetValidMoves(position, MovingDirections.KingAndQueen, board);
    }

    protected override ulong GetValidMoves(ulong position, MovingDirections direction, Board board)
    {
        ulong result = 0;
        foreach (MovingDirections singleDirection in Enum.GetValues(typeof(MovingDirections)))
        {
            if (singleDirection == MovingDirections.None || singleDirection == MovingDirections.Rook ||
                singleDirection == MovingDirections.Bishop ||
                singleDirection == MovingDirections.KingAndQueen ||
                (direction & singleDirection) == 0)
            {
                continue; // Skip composite flags and None
            }

            ulong newPosition = position.Move(singleDirection);
            if (newPosition != 0)
            {
                if ((board.OccupiedBitBoard & newPosition) == 0)
                {
                    result |= newPosition;
                }

                if (board.SquareContainPieceOfColor(newPosition, this.Color.Opposite()))
                {
                    result |= newPosition;
                }
            }
        }

        return result;
    }
}
