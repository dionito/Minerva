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

namespace Minerva.Extensions;

public static class BoardExtensions
{
    /// <summary>
    /// Calculates the bitboard representing all squares that are either empty or occupied by black pieces.
    /// </summary>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>A bitboard where 1s represent squares that are either empty or occupied by black pieces.</returns>
    public static ulong BlackOrEmpty(this Board board)
    {
        return ~board.WhitePiecesBitBoard;
    }

    /// <summary>
    /// Gets the piece at the specified square on the chess board.
    /// </summary>
    /// <param name="square">The bitboard representation of the square to check.</param>
    /// <param name="color">The color of the piece to find. If Color.None, any piece will
    /// be considered.</param>
    /// <returns>The character representing the piece at the given position, or the
    /// <see cref="EmptySquare"/> character (' ') if no piece is found.</returns>
    public static char GetPieceAt(this Board board, ulong square)
    {
        var pieces = (board.BlackPiecesBitBoard & square) != 0 ? board.BlackPieces :
            (board.WhitePiecesBitBoard & square) != 0 ? board.WhitePieces : null;

        if (pieces == null)
        {
            return BitBoards.EmptySquare;
        }

        foreach (var pieceType in pieces)
        {
            if ((pieceType.Value & square) != 0)
            {
                return pieceType.Key;
            }
        }

        return BitBoards.EmptySquare;
    }

    /// <summary>
    /// Initializes the chess board to the standard starting position.
    /// This method sets the bitboards for both black and white pieces.
    /// </summary>
    public static void InitializeGameStartingBoard(this Board board)
    {
        // Initialize black pieces
        // Rooks are placed on a8 and h8
        board.BlackPieces['r'] = BitBoards.Rank8 & BitBoards.FileA | BitBoards.Rank8 & BitBoards.FileH;

        // Knights are placed on b8 and g8
        board.BlackPieces['n'] = BitBoards.Rank8 & BitBoards.FileB | BitBoards.Rank8 & BitBoards.FileG;

        // Bishops are placed on c8 and f8
        board.BlackPieces['b'] = BitBoards.Rank8 & BitBoards.FileC | BitBoards.Rank8 & BitBoards.FileF;

        // Queen is placed on d8
        board.BlackPieces['q'] = BitBoards.Rank8 & BitBoards.FileD;

        // King is placed on e8
        board.BlackPieces['k'] = BitBoards.Rank8 & BitBoards.FileE;

        // Pawns are placed on a7 to h7
        board.BlackPieces['p'] = BitBoards.Rank7;

        // Initialize white pieces
        // Rooks are placed on a1 and h1
        board.WhitePieces['R'] = BitBoards.Rank1 & BitBoards.FileA | BitBoards.Rank1 & BitBoards.FileH;

        // Knights are placed on b1 and g1
        board.WhitePieces['N'] = BitBoards.Rank1 & BitBoards.FileB | BitBoards.Rank1 & BitBoards.FileG;

        // Bishops are placed on c1 and f1
        board.WhitePieces['B'] = BitBoards.Rank1 & BitBoards.FileC | BitBoards.Rank1 & BitBoards.FileF;

        // Queen is placed on d1
        board.WhitePieces['Q'] = BitBoards.Rank1 & BitBoards.FileD;

        // King is placed on e1
        board.WhitePieces['K'] = BitBoards.Rank1 & BitBoards.FileE;

        // Pawns are placed on a2 to h2
        board.WhitePieces['P'] = BitBoards.Rank2;

        board.UpdateBoardStatus();
    }

    /// <summary>
    /// Checks if a specified square on the chess board is empty.
    /// </summary>
    /// <param name="board">The current state of the chess board.</param>
    /// <param name="square">The square to check, represented as a bitboard.</param>
    /// <returns>True if the square is empty, false otherwise.</returns>
    public static bool IsEmptySquare(this Board board, ulong square)
    {
        return (board.OccupiedBitBoard & square) == 0;
    }

    /// <summary>
    /// Checks if a square contains a piece of the specified color.
    /// </summary>
    /// <param name="square">The square to check, represented as a bitboard with a single bit set.</param>
    /// <param name="color">The color to check for. Can be either 'w' for white or 'b' for black.</param>
    /// <returns>true if the square contains a piece of the specified color; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid color is provided.</exception>
    public static bool SquareContainPieceOfColor(this Board board, ulong square, char color)
    {
        return color switch
        {
            'w' => (board.WhitePiecesBitBoard & square) != 0,
            'b' => (board.BlackPiecesBitBoard & square) != 0,
            _ => throw new ArgumentException($"Invalid color: {color}. Valid colors are 'b' or 'w'.", nameof(color)),
        };
    }

    /// <summary>
    /// Calculates the bitboard representing all squares that are either empty or occupied by white pieces.
    /// </summary>
    /// <param name="board">The current state of the chess board.</param>
    /// <returns>A bitboard where 1s represent squares that are either empty or occupied by white pieces.</returns>
    public static ulong WhiteOrEmpty(this Board board)
    {
        return ~board.BlackPiecesBitBoard;
    }
}