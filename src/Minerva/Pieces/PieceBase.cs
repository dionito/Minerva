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
    /// Initializes a new instance of the <see cref="PieceBase"/> class.
    /// </summary>
    protected PieceBase()
    {
    }

    /// <summary>
    /// Gets or sets the color of the piece.
    /// </summary>
    public Color Color { get; protected set; }

    /// <summary>
    /// Gets or sets the type of the piece.
    /// </summary>
    public PieceType PieceType { get; protected set; }

    public abstract ulong GetPieceAttacks(ulong position, Board board);

    /// <summary>
    /// Gets the bitboard representation of the possible moves for the piece.
    /// </summary>
    /// <param name="position">The current position of the piece.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>A bitboard (ulong) where each bit represents a square on the
    /// chess board. A set bit indicates that the piece can move to that square.</returns>
    public abstract ulong GetPieceMoves(ulong position, Board board);

    /// <summary>
    /// Gets the bitboard representation of the possible moves for the piece based on its current
    /// position and moving direction.
    /// </summary>
    /// <param name="position">The bitboard representing the current position of the piece.</param>
    /// <param name="direction">The moving direction(s) to calculate the moves for.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>A bitboard (ulong) where each bit represents a possible move destination square.
    /// A set bit indicates that the piece can move to that square.</returns>
    /// <remarks>This method works for pieces that can move an unlimited number of squares in the same
    /// direction (rook, bishop and queen). Overrided for other pieces.</remarks>
    protected virtual ulong GetPieceMovesOrAttacks(
        ulong position,
        MovingDirections direction,
        Board board,
        bool attacks = false)
    {
        ulong result = 0;
        ulong originalPosition = position;
        foreach (MovingDirections singleDirection in Enum.GetValues(typeof(MovingDirections)))
        {
            if (singleDirection == MovingDirections.None || singleDirection == MovingDirections.Rook ||
                singleDirection == MovingDirections.Bishop ||
                singleDirection == MovingDirections.KingAndQueen ||
                (direction & singleDirection) == 0)
            {
                continue; // Skip composite flags and None
            }

            position = originalPosition;

            ulong newPosition = position.Move(singleDirection);
            while (newPosition != 0)
            {
                if ((board.OccupiedBitBoard & newPosition) == 0)
                {
                    result |= newPosition;
                }
                else
                {
                    if (attacks || board.SquareContainPieceOfColor(newPosition, this.Color.Opposite().ToChar()))
                    {
                        result |= newPosition;
                    }

                    break;
                }

                position = newPosition;
                newPosition = position.Move(singleDirection);
            }
        }

        if (!attacks)
        {
            result = this.PurgeIlegalMoves(originalPosition, result, board);
        }

        return result;
    }

    /// <summary>
    /// Removes illegal moves from a set of potential move destinations.
    /// </summary>
    /// <param name="from">The bitboard representing the starting square of the move.</param>
    /// <param name="toLocations">The bitboard representing potential destination squares.</param>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>A bitboard (ulong) representing the legal move destinations after removing illegal ones.</returns>
    protected ulong PurgeIlegalMoves(
        ulong from,
        ulong toLocations,
        Board board)
    {
        return toLocations; // TODO: Implement this thing or delete.
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>
    /// A string that represents the current object. The string is the standard char representation
    /// of the piece type,  where uppercase represents white pieces and lowercase represents black
    /// pieces. 
    /// </returns>
    /// <example>"P" represents a white pawn, and "q" represents a black queen.</example>
    public override string ToString()
    {
        char pieceType = this.PieceType.ToChar();
        pieceType = this.Color == Color.White ? char.ToUpper(pieceType) : char.ToLower(pieceType);
        return $"{pieceType}";
    }
}

