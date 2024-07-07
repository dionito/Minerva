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

using System.Numerics;

namespace Minerva;

/// <summary>
/// Represents a square on a chess board using algebraic notation and BitBoards.
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
    /// Initializes a new instance of the <see cref="Square"/> struct with default values.
    /// </summary>
    /// <remarks>When initialized via this constructor, <see cref="Square"/> represents no square.</remarks>
    public Square()
    {
        this.File = '-';
        this.Rank = 0;
        this.BitBoard = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Square"/> struct using algegraic notation.
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
        this.BitBoard = BitBoards.Squares[$"{file}{rank}"];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Square"/> struct using a BitBoard representation.
    /// </summary>
    /// <param name="bitBoard">The BitBoard representation of the square.</param>
    /// <exception cref="ArgumentException">Thrown when the BitBoard does not have exactly one bit set.</exception>
    public Square(ulong bitBoard)
    {
        if (bitBoard == 0 || (bitBoard & (bitBoard - 1)) != 0)
        {
            throw new ArgumentException($"{nameof(bitBoard)} must have a single bit set.", nameof(bitBoard));
        }

        this.BitBoard = bitBoard;
        int index = BitOperations.TrailingZeroCount(bitBoard);
        this.File = (char)('h' - (index % 8));
        this.Rank = 1 + (index / 8);
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
        this.BitBoard = BitBoards.Squares[$"{file}{rank}"];
    }

    /// <summary>
    /// Moves from the curren square to square in the given direction.
    /// </summary>
    /// <param name="move">The direction and distance to move the square.</param>
    /// <returns>The new square after the move.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the move is off the board.</exception>
    public Square Move(Move move)
    {
        if (this.TryMove(move, out Square square))
        {
            return square;
        }

        throw new InvalidOperationException("The move is off the board.");
    }

    /// <summary>
    /// Tries to move from the curren square to the square in the given direction.
    /// </summary>
    /// <param name="move">The direction and distance to move the square.</param>
    /// <param name="square">When this method returns, contains the new square if the
    /// move was successful, or the default value of <see cref="Square"/> if the move
    /// was not successful.</param>
    /// <returns>true if the square was successfully moved; otherwise, false.</returns>
    public bool TryMove(Move move, out Square square)
    {
        // Check if the move is off the board horizontally.
        int fileIndex = this.File - 'a';
        if (fileIndex + move.FileMove < 0 || fileIndex + move.FileMove > 7)
        {
            square = default;
            return false;
        }

        // Check if the move is off the board vertically.
        int rankIndex = this.Rank - 1;
        if (rankIndex + move.RankMove < 0 || rankIndex + move.RankMove > 7)
        {
            square = default;
            return false;
        }

        ulong bitBoard = this.BitBoard;
        bitBoard = move.RankMove >= 0 ? bitBoard << 8 * move.RankMove : bitBoard >> -8 * move.RankMove;
        bitBoard = move.FileMove >= 0 ? bitBoard >> move.FileMove : bitBoard << -move.FileMove;
        square = new Square(bitBoard);
        return true;
    }

    /// <summary>
    /// Returns a string that represents the current square.
    /// </summary>
    /// <returns>A string that represents the current square using algebraic notation.</returns>
    public override string ToString() => this.File == '-' ? "-" : $"{this.File}{this.Rank}";

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj) =>
        obj is Square square && this.BitBoard == square.BitBoard;

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => this.BitBoard.GetHashCode();

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