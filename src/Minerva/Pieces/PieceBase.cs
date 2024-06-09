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
/// Represents a base class for all chess pieces.
/// </summary>
public abstract class PieceBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PieceBase"/> class.
    /// </summary>
    /// <param name="type">The type of the piece.</param>
    /// <param name="color">The color of the piece.</param>
    protected PieceBase(PieceType type, Color color)
    {
        if (color == Color.None)
        {
            throw new ArgumentException($"Invalid piece color: '{Color.None}'.", nameof(color));
        }

        this.PieceType = type;
        this.Color = color;
    }

    /// <summary>
    /// Gets or sets the color of the piece.
    /// </summary>
    public Color Color { get; protected set; }

    /// <summary>
    /// Gets or sets the type of the piece.
    /// </summary>
    public PieceType PieceType { get; protected set; }

    /// <summary>
    /// When overridden in a derived class, gets the possible moves for the piece.
    /// </summary>
    /// <param name="position">The current position of the piece.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>An array of squares representing the possible moves for the piece.</returns>
    public abstract Square[] GetPossibleMoves(Square position, Board board);

    /// <summary>
    /// Gets the bitboard representation of the possible moves for the piece.
    /// </summary>
    /// <param name="position">The current position of the piece.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>A bitboard (ulong) where each bit represents a square on the
    /// chess board. A set bit indicates that the piece can move to that square.</returns>
    public ulong GetPossibleMovesBitBoard(Square position, Board board)
    {
        return this.GetPossibleMoves(position, board).Aggregate(0ul, (acc, square) => acc | square.BitBoard);
    }

    /// <summary>
    /// Gets the valid moves for the piece in a given direction.
    /// </summary>
    /// <param name="position">The current position of the piece.</param>
    /// <param name="direction">The direction of the move.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>An enumerable collection of squares representing the valid
    /// moves for the piece in the given direction.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the provided board is null.</exception>
    protected virtual IEnumerable<Square> GetValidMoves(Square position, Move direction, Board board)
    {
        while (position.TryMove(direction, out Square newPosition))
        {
            position = newPosition;
            if ((board.OccupiedBitBoard & newPosition.BitBoard) == 0ul)
            {
                yield return newPosition;
                continue;
            }

            if (board.SquareContainPieceOfColor(newPosition, this.Color.Opposite()))
            {
                yield return newPosition;
            }

            yield break;
        }
    }
}