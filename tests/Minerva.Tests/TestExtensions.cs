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

namespace Minerva.Tests;

public static class TestExtensions
{
    /// <summary>
    /// This method returns the bitboard for a given square represented by
    /// its standard chess notation.
    /// </summary>
    /// <param name="square">The square in standard chess notation.</param>
    public static ulong GetSquareBitBoard(this Board board, string square)
    {
        if (square == null) { throw new ArgumentNullException(nameof(square)); }

        if (square.Length != 2)
        {
            throw new ArgumentException("Square notation must be 2 characters long.", nameof(square));
        }

        int file = char.ToLower(square[0]) - 'a';
        if (file is < 0 or > 7)
        {
            throw new ArgumentException("Invalid file.", nameof(square));
        }

        var rank = square[1] - '1';
        if (rank is < 0 or > 7)
        {
            throw new ArgumentException("Invalid rank.", nameof(square));
        }

        return Board.Files[file] & Board.Ranks[rank];
    }
}