// /*
//  * Copyright (C) 2024 dionito
//  *
//  * This program is free software: you can redistribute it and/or modify
//  * it under the terms of the GNU General Public License as published by
//  * the Free Software Foundation, either version 3 of the License, or
//  * (at your option) any later version.
//  *
//  * This program is distributed in the hope that it will be useful,
//  * but WITHOUT ANY WARRANTY; without even the implied warranty of
//  * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  * GNU General Public License for more details.
//  *
//  * You should have received a copy of the GNU General Public License
//  * along with this program.  If not, see <https://www.gnu.org/licenses/>.
//  */

namespace Minerva.Pieces;

public class King : PieceBase
{
    public King(Color color) : base(PieceType.King, color)
    {
    }

    public override ulong[] GetPossibleMoves(ulong position, Board board)
    {
        throw new NotImplementedException();
    }
}