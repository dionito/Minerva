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

namespace Minerva;

/// <summary>
/// Represents a square on a chess board using algebraic notation.
/// </summary>
/// <seealso href="https://en.wikipedia.org/wiki/Algebraic_notation_(chess)"/>
public readonly struct Square
{
    /// <summary>
    /// Gets the file (column) of the square.
    /// </summary>
    public char File { get; }

    /// <summary>
    /// Gets the rank (row) of the square.
    /// </summary>
    public int Rank { get; }

    /// <summary>
    /// Gets the BitBoard representation of the square.
    /// </summary>
    public ulong BitBoard { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Square"/> class.
    /// </summary>
    /// <param name="file">The file (column) of the square.</param>
    /// <param name="rank">The rank (row) of the square.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the file is not
    /// a lowercase letter from 'a' to 'h' or the rank is not an integer from 1 to 8.</exception>
    public Square(char file, int rank)
    {
        if (file is < 'a' or > 'h')
        {
            throw new ArgumentOutOfRangeException(nameof(file));
        }

        if (rank is <= 0 or > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(rank));
        }

        this.File = file;
        this.Rank = rank;
        this.BitBoard = Board.Files[file - 'a'] & Board.Ranks[rank - 1];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Square"/> class.
    /// </summary>
    /// <param name="square">The square in string format.</param>
    /// <exception cref="ArgumentException">Thrown when the square is not a two-character string.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the file is not a lowercase
    /// letter from 'a' to 'h' or the rank is not a digit from '1' to '8'.</exception>
    public Square(string square)
    {
        if (square.Length != 2)
        {
            throw new ArgumentException("Square must be two characters long.", nameof(square));
        }

        char file = square[0];
        int rank = square[1] - '0';
        if (file is < 'a' or > 'h')
        {
            throw new ArgumentOutOfRangeException(nameof(file));
        }

        if (rank is <= 0 or > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(rank));
        }

        this.File = file;
        this.Rank = rank;
        this.BitBoard = Board.Files[file - 'a'] & Board.Ranks[rank - 1];
    }

    /// <summary>
    /// Returns a string that represents the current square.
    /// </summary>
    /// <returns>A string that represents the current square using algebraic notation.</returns>
    public override string ToString() => $"{this.File}{this.Rank}";

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj) =>
        obj is Square square && this.File == square.File && this.Rank == square.Rank;

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => this.File.GetHashCode() ^ this.Rank.GetHashCode();

    /// <summary>
    /// Determines whether two specified instances of Square are equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>true if left and right represent the same square; otherwise, false.</returns>
    public static bool operator ==(Square left, Square right) => left.Equals(right);

    /// <summary>
    /// Determines whether two specified instances of Square are not equal.
    /// </summary>
    /// <param name="left">The first instance to compare.</param>
    /// <param name="right">The second instance to compare.</param>
    /// <returns>true if left and right do not represent the same square; otherwise, false.</returns>
    public static bool operator !=(Square left, Square right) => !(left == right);
}
