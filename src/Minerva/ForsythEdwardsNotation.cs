// 
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
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// 

using System.Text;
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

        // The en passant target square is a square in algebraic notation where an en passant capture can occur.
        //  Other moves than double pawn pushes imply the symbol '-' for this FEN field.
        string enPassantTargetSquare = fenParts[3];
        if (enPassantTargetSquare != "-" && !Regex.IsMatch(enPassantTargetSquare, "^[a-h][36]$"))
        {
            throw new ArgumentException("Invalid en passant target square in FEN string.", nameof(fen));
        }

        board.SetEnPassantTargetSquare(enPassantTargetSquare);

        // The halfmove clock specifies a decimal number of half moves with respect to the 50 move draw rule.
        // It is reset to zero after a capture or a pawn move and incremented otherwise.
        if (!int.TryParse(fenParts[4], out int halfmoveClock))
        {
            throw new ArgumentException("Invalid halfmove clock in FEN string.", nameof(fen));
        }

        if (halfmoveClock is < 0 or > 50)
        {
            throw new ArgumentException("Halfmove clock out of range in FEN string.", nameof(fen));
        }

        board.SetHalfmoveClock(halfmoveClock);

        // The fullmove number is the number of the full moves in a game.
        // It starts at 1, and is incremented after a black move.
        if (!int.TryParse(fenParts[5], out int fullmoveNumber))
        {
            throw new ArgumentException("Invalid fullmove counter in FEN string.", nameof(fen));
        }

        if (fullmoveNumber < 1)
        {
            throw new ArgumentException("Fullmove counter must be 1 or greater.", nameof(fen));
        }
        
        board.SetFullmoveNumber(fullmoveNumber);
        return board;
    }

    public static string GenerateFen(this Board board)
    {
        if (board == null) { throw new ArgumentNullException(nameof(board)); }

        StringBuilder fen = GeneratePiecePlacements(board);

        fen.Append(' ')
            .Append(board.ActiveColor)
            .Append(' ')
            .Append(board.CastlingRights)
            .Append(' ')
            .Append(board.EnPassantTargetSquare)
            .Append(' ')
            .Append(board.HalfmoveClock)
            .Append(' ')
            .Append(board.FullmoveNumber);

        return fen.ToString();
    }

    private static StringBuilder GeneratePiecePlacements(Board board)
    {
        StringBuilder fen = new StringBuilder();
        // The ranks part of the FEN string is generated by iterating over each rank from 8 to 1
        for (int rank = 8; rank >= 1; rank--)
        {
            // The file part of the FEN string is generated by iterating over each file from 1 to 8
            for (int file = 1; file <= 8; file++)
            {
                // The piece at the current file and rank is retrieved
                char piece = board.GetPieceAt(file, rank);

                // If the piece is empty, the empty square count is incremented
                if (piece == Board.EmptySquare)
                {
                    int emptySquareCount = 1;

                    // The empty square count is incremented while the next square is empty
                    while (file + emptySquareCount <= 8 &&
                           board.GetPieceAt(file + emptySquareCount, rank) == Board.EmptySquare)
                    {
                        emptySquareCount++;
                    }

                    // The empty square count is appended to the FEN string
                    fen.Append(emptySquareCount);
                    file += emptySquareCount - 1;
                }
                else
                {
                    // If the piece is not empty, the piece is appended to the FEN string
                    fen.Append(piece);
                }
            }

            // If the rank is not the last rank, a / is appended to the FEN string
            if (rank != 1)
            {
                fen.Append('/');
            }
        }

        return fen;
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
