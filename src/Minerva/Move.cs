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

namespace Minerva;

/// <summary>
/// Represents a move in a chess board.
/// </summary>
public readonly struct Move
{
    /// <summary>
    /// Represents a move in the Down direction. For example from rank 8 to rank 4.
    /// </summary>
    public static Move Down = new(0, -1);

    /// <summary>
    /// Represents a move in the Down-Left direction.
    /// For example, from file 'e' and rank 4 to file 'c' and rank 2.
    /// </summary>
    public static Move DownLeft = new(-1, -1);

    /// <summary>
    /// Represents a move in the the Down-Right direction.
    /// For example, from file 'e' and rank 4 to file 'g' and rank 2.
    /// </summary>
    public static Move DownRight = new(1, -1);

    /// <summary>
    /// Represents a move in the Left direction. For example from file 'e' to file 'a'.
    /// </summary>
    public static Move Left = new(-1, 0);

    /// <summary>
    /// Represents a move in the Right direction. For example from file 'e' to file 'h'.
    /// </summary>
    public static Move Right = new(1, 0);

    /// <summary>
    /// Represents a move in the Up direction. For example from rank 4 to rank 8.
    /// </summary>
    public static Move Up = new(0, 1);

    /// <summary>
    /// Represents a move in the Up-Left direction.
    /// For example from file 'e' and rank 4 to file 'c' and rank 6.
    /// </summary>
    public static Move UpLeft = new(-1, 1);

    /// <summary>
    /// Represents a move in the Up-Right direction.
    /// For example from file 'e' and rank 4 to file 'g' and rank 6.
    /// </summary>
    public static Move UpRight = new(1, 1);

    /// <summary>
    /// Gets the number of files for the move. Moves to the Right are represented by positive integers
    /// while moves in the Left direction are represented by negative integers.
    /// </summary>
    public int FileMove { get; }

    /// <summary>
    /// Gets the number of ranks for the move. Moves Up are represented by positive integers
    /// while moves Down are represented by negative integers.
    /// </summary>
    public int RankMove { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Move"/> class.
    /// </summary>
    /// <param name="fileMove">The number of files for the move. Moves to the Right are represented by positive
    /// integers while moves to the Left are represented by negative integers.</param>
    /// <param name="rankMove">The number of ranks for the move. Moves Up are represented by positive integers
    /// while moves Down are represented by negative integers.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the absolute value of <paramref name="fileMove"/>
    /// or <paramref name="rankMove"/> is greater than 7.</exception>
    public Move(int fileMove, int rankMove)
    {
        if (fileMove is < -7 or > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(fileMove));
        }

        if (rankMove is < -7 or > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(rankMove));
        }

        if (fileMove == 0 && rankMove == 0)
        {
            throw new ArgumentException("A move must be in at least one direction.", nameof(fileMove));
        }

        this.FileMove = fileMove;
        this.RankMove = rankMove;
    }

    /// <summary>
    /// Adds two <see cref="Move"/> objects.
    /// </summary>
    /// <param name="a">The first <see cref="Move"/> to add.</param>
    /// <param name="b">The second <see cref="Move"/> to add.</param>
    /// <returns>A <see cref="Move"/> that is the sum of the values of <c>a</c> and <c>b</c>.</returns>
    public static Move operator +(Move a, Move b) =>
        new(a.FileMove + b.FileMove, a.RankMove + b.RankMove);

    /// <summary>
    /// Multiplies the <see cref="Move"/> by a scalar.
    /// </summary>
    /// <param name="multiplier">The scalar to multiply.</param>
    /// <param name="move">The <see cref="Move"/> to multiply.</param>
    /// <returns>A <see cref="Move"/> that is the result of the multiplication.</returns>
    public static Move operator *(int multiplier, Move move) => new(
        multiplier * move.FileMove,
        multiplier * move.RankMove);
}