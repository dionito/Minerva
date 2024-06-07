// 
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

using System.Diagnostics.CodeAnalysis;

namespace Minerva.Pieces;

public class King : PieceBase
{
    public King(Color color) : base(PieceType.King, color)
    {
    }

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

    protected override IEnumerable<Square> GetValidMoves(Square position, Move direction, [NotNull] Board board)
    {
        if (position.TryMove(direction, out Square newPosition))
        {
            if ((board.OccupiedBitBoard & newPosition.BitBoard) == 0ul)
            {
                yield return newPosition;
            }

            if (board.ContainsColorPiece(newPosition, this.Color.Opposite()))
            {
                yield return newPosition;
            }
        }
    }
}