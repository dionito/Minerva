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

using System.Text.RegularExpressions;

namespace Minerva;

/// <summary>
/// Represents the Forsyth-Edwards Notation (FEN), a standard notation for describing
/// a particular board position of a chess game.
/// https://www.chessprogramming.org/Forsyth-Edwards_Notation#FEN_Syntax
/// </summary>
public static class ForsythEdwardsNotation
{
    private static readonly char[] FenSplitChars = new[] { ' ' };
    private static readonly char[] RankSplitChars = new[] { '/' };

    /// <summary>
    /// Generates a chess board from a given Forsyth-Edwards Notation (FEN) string.
    /// </summary>
    /// <param name="fen">The FEN string representing the chess board position.</param>
    /// <returns>A <see cref="Board"/>> object representing the chess board position.</returns>
    public static Board GenerateBoard(string fen)
    {
        if (fen == null)
        {
            throw new ArgumentNullException(nameof(fen));
        }

        Board board = new Board();

        string[] fenParts = fen.Split(FenSplitChars);
        if (fenParts.Length != 6)
        {
            throw new ArgumentException("Invalid FEN string. FEN should always have 6 parts.", nameof(fen));
        }

        string[] ranks = fenParts[0].Split(RankSplitChars);
        if (ranks.Length != 8)
        {
            throw new ArgumentException(
                "Invalid FEN string. Ranks should always have 8 parts.",
                nameof(fen));
        }

        board.SetPieces(ranks);

        // The active color is the color that has the next move. It is either 'w' for white or 'b' for black.
        string activeColor = fenParts[1];
        if (activeColor != "w" && activeColor != "b")
        {
            throw new ArgumentException("Invalid active color in FEN string.", nameof(fen));
        }
        
        board.SetActiveColor(activeColor[0]);

        // The castling rights is a string representing the castling rights of both players.
        // If neither side can castle, the symbol '-' is used, otherwise each of four individual
        // castling rights for king and queen castling for both sides are indicated by a sequence
        // of one to four letters. KQkq means all four castling rights are available.
        string castlingRights = fenParts[2];
        if (castlingRights != "-" && !Regex.IsMatch(castlingRights, "^[KQkq]+$") || castlingRights.Length > 4)
        {
            throw new ArgumentException("Invalid castling rights in FEN string.", nameof(fen));
        }

        board.SetCastlingRights(castlingRights);

        string enPassantTargetSquare = fenParts[3];
        string halfmoveClock = fenParts[4];
        string fullmoveNumber = fenParts[5];



        return board;
    }


    /// <summary>
    /// Sets the pieces on the board based on the ranks part of a Forsyth-Edwards Notation (FEN) string.
    /// </summary>
    /// <param name="board">The board on which to set the pieces.</param>
    /// <param name="ranks">An <see cref="string[]"/>> representing the ranks part of the FEN string split by /.</param>
    static void SetPieces(this Board board, string[] ranks)
    {
        // Iterates over each rank in the FEN string (from 8 to 1)
        for (int ranksPart = 0; ranksPart < 8; ranksPart++)
        {
            // The file is initially set to 1 (files are labeled a-h, which correspond to 1-8)
            int file = 1;

            // The rank is calculated by subtracting the current rank part from 8
            // This is because the ranks in the FEN string are given from rank 8 to rank 1
            int rank = 8 - ranksPart;

            // Iterates over each character in the current rank part
            foreach (char piece in ranks[ranksPart])
            {
                if (char.IsDigit(piece))
                {
                    // If the character is a digit, it represents the number of empty squares
                    // The file is incremented by this number
                    file += piece - '0';
                }
                else if (char.IsLetter(piece))
                {
                    // If the character is not a digit, it represents a piece
                    board.SetPieceAt(file, rank, piece);
                    file++;
                }
                else
                {
                    throw new ArgumentException("Invalid character in FEN string ranks.", nameof(ranks));
                }
            }
        }
    }
}
