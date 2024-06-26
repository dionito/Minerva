﻿// Copyright (C) 2024 dionito
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
/// Represents a queen piece in a game of chess.
/// </summary>
public class Queen : PieceBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Queen"/> class with the specified color.
    /// </summary>
    /// <param name="color">The color of the queen.</param>
    public Queen(Color color) : base(PieceType.Queen, color)
    {
    }

    public override ulong GetPieceAttacks(ulong position, Board board)
    {
        return this.GetPieceMovesOrAttacks(
            position,
            MovingDirections.KingAndQueen,
            board,
            attacks: true);
    }

    public override ulong GetPieceMoves(ulong position, Board board)
    {
        return this.GetPieceMovesOrAttacks(
            position,
            MovingDirections.KingAndQueen,
            board);
    }
}
