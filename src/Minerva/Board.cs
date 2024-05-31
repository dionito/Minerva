/*
 * Copyright (C) 2024 dionito
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Minerva;


/// <summary>
/// Represents a chess board and its pieces using a set of bitboards.
/// A bitboard is a 64-bit integer where each bit represents a square on the board. In our
/// case, a bit set to 1 means that a piece is present on that square.
///
/// A8 is represented by the most significant bit (MSB) and
/// H1 is represented by the least significant one (LSB)
/// 
/// 8 ▓ ▒ ▓ ▒ ▓ ▒ ▓ ▒ 
/// 7 ▒ ▓ ▒ ▓ ▒ ▓ ▒ ▓ 
/// 6 ▓ ▒ ▓ ▒ ▓ ▒ ▓ ▒ 
/// 5 ▒ ▓ ▒ ▓ ▒ ▓ ▒ ▓ 
/// 4 ▓ ▒ ▓ ▒ ▓ ▒ ▓ ▒ 
/// 3 ▒ ▓ ▒ ▓ ▒ ▓ ▒ ▓ 
/// 2 ▓ ▒ ▓ ▒ ▓ ▒ ▓ ▒ 
/// 1 ▒ ▓ ▒ ▓ ▒ ▓ ▒ ▓ 
///   a b c d e f g h
/// 
/// </summary>
public class Board
{
    public const ulong FileA = 0x8080808080808080ul;
    public const ulong FileB = 0x4040404040404040ul;
    public const ulong FileC = 0x2020202020202020ul;
    public const ulong FileD = 0x1010101010101010ul;
    public const ulong FileE = 0x0808080808080808ul;
    public const ulong FileF = 0x0404040404040404ul;
    public const ulong FileG = 0x0202020202020202ul;
    public const ulong FileH = 0x0101010101010101ul;

    public const ulong Rank1 = 0x00000000000000FFul;
    public const ulong Rank2 = 0x000000000000FF00ul;
    public const ulong Rank3 = 0x0000000000FF0000ul;
    public const ulong Rank4 = 0x00000000FF000000ul;
    public const ulong Rank5 = 0x000000FF00000000ul;
    public const ulong Rank6 = 0x0000FF0000000000ul;
    public const ulong Rank7 = 0x00FF000000000000ul;
    public const ulong Rank8 = 0xFF00000000000000ul;

    /// <summary>
    /// Represents the combined bitboard for all black pieces.
    /// This field should be updated whenever the black pieces change.
    /// </summary>
    ulong blackPiecesBitBoard;

    /// <summary>
    /// Represents the files of the chess board.
    /// </summary>
    readonly ulong[] files = 
    {
        FileA, FileB, FileC, FileD, FileE, FileF, FileG, FileH,
    };

    /// <summary>
    /// Represents the ranks of the chess board.
    /// </summary>
    readonly ulong[] ranks = 
    {
        Rank1, Rank2, Rank3, Rank4, Rank5, Rank6, Rank7, Rank8,
    };

    /// <summary>
    /// Represents the combined bitboard for all white pieces.
    /// This field should be updated whenever the white pieces change.
    /// </summary>
    ulong whitePiecesBitBoard;

    /// <summary>
    /// Represents the bitboards for the black pieces.
    /// </summary>
    public Dictionary<string, ulong> BlackPieces { get; } = new()
    {
        { "b", 0ul },
        { "n", 0ul },
        { "r", 0ul },
        { "q", 0ul },
        { "k", 0ul },
        { "p", 0ul },
    };

    /// <summary>
    /// Gets the combined bitboard for all black pieces.
    /// This property performs a bitwise OR operation on the bitboards of all black pieces.
    /// </summary>
    public ulong BlackPiecesBitBoard => this.blackPiecesBitBoard;

    /// <summary>
    /// Gets or sets the castling rights for both white and black.
    /// The rights are represented as a string with the following possible values:
    /// "K" - White can castle kingside
    /// "Q" - White can castle queenside
    /// "k" - Black can castle kingside
    /// "q" - Black can castle queenside
    /// "-"  - Neither side can castle
    /// </summary>
    public string CastlingRights { get; private set; } = "KQkq";

    /// <summary>
    /// Gets the combined bitboard for all pieces on the board.
    /// This property performs a bitwise OR operation on the bitboards of all black and white pieces.
    /// </summary>
    public ulong OccupiedBitBoard => this.BlackPiecesBitBoard | this.WhitePiecesBitBoard;

    /// <summary>
    /// Represents the bitboards for the white pieces.
    /// </summary>
    public Dictionary<string, ulong> WhitePieces { get; } = new()
    {
        { "B", 0ul },
        { "N", 0ul },
        { "R", 0ul },
        { "Q", 0ul },
        { "K", 0ul },
        { "P", 0ul },
    };

    /// <summary>
    /// Gets the combined bitboard for all white pieces.
    /// This property performs a bitwise OR operation on the bitboards of all white pieces.
    /// </summary>
    public ulong WhitePiecesBitBoard => this.whitePiecesBitBoard;

    /// <summary>
    /// Gets or sets the active color, which is the color that has the next move.
    /// </summary>
    /// <value>The active color is either 'w' for white or 'b' for black.</value>
    public char ActiveColor { get; private set; } = 'w';

    /// <summary>
    /// This method returns the bitboard for a given square represented by
    /// its standard chess notation.
    /// </summary>
    /// <param name="square">The square in standard chess notation.</param>
    public ulong GetSquareBitBoard(string square)
    {
        if (square == null)  { throw new ArgumentNullException(nameof(square)); }

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

        return this.files[file] & this.ranks[rank];
    }

    /// <summary>
    /// Initializes the chess board to the standard starting position.
    /// This method sets the bitboards for both black and white pieces.
    /// </summary>
    public void InitializeGameStartingBoard()
    {
        // Initialize black pieces
        // Rooks are placed on a8 and h8
        this.BlackPieces["r"] = Rank8 & FileA | Rank8 & FileH;
        // Knights are placed on b8 and g8
        this.BlackPieces["n"] = Rank8 & FileB | Rank8 & FileG;
        // Bishops are placed on c8 and f8
        this.BlackPieces["b"] = Rank8 & FileC | Rank8 & FileF;
        // Queen is placed on d8
        this.BlackPieces["q"] = Rank8 & FileD;
        // King is placed on e8
        this.BlackPieces["k"] = Rank8 & FileE;
        // Pawns are placed on a7 to h7
        this.BlackPieces["p"] = Rank7;

        // Initialize white pieces
        // Rooks are placed on a1 and h1
        this.WhitePieces["R"] = Rank1 & FileA | Rank1 & FileH;
        // Knights are placed on b1 and g1
        this.WhitePieces["N"] = Rank1 & FileB | Rank1 & FileG;
        // Bishops are placed on c1 and f1
        this.WhitePieces["B"] = Rank1 & FileC | Rank1 & FileF;
        // Queen is placed on d1
        this.WhitePieces["Q"] = Rank1 & FileD;
        // King is placed on e1
        this.WhitePieces["K"] = Rank1 & FileE;
        // Pawns are placed on a2 to h2
        this.WhitePieces["P"] = Rank2;

        this.UpdateBitBoards();
    }

    /// <summary>
    /// Sets the active color for the next move on the chess board.
    /// </summary>
    /// <param name="activeColor">The active color. 'w' for white and 'b' for black.</param>
    /// <exception cref="ArgumentException">Thrown when an invalid color is provided.</exception>
    public void SetActiveColor(char activeColor)
    {
        if (activeColor != 'w' && activeColor != 'b')
        {
            throw new ArgumentException("Invalid active color.", nameof(activeColor));
        }

        this.ActiveColor = activeColor;
    }

    /// <summary>
    /// Sets the castling rights for both white and black.
    /// The rights are represented as a string with the following possible values:
    /// "K" - White can castle kingside
    /// "Q" - White can castle queenside
    /// "k" - Black can castle kingside
    /// "q" - Black can castle queenside
    /// "-"  - Neither side can castle
    /// </summary>
    /// <param name="castlingRights">The string representing the castling rights.</param>
    /// <exception cref="ArgumentException">Thrown when an invalid castling rights string is provided.</exception>
    public void SetCastlingRights(string castlingRights)
    {
        if (castlingRights == "-")
        {
            this.CastlingRights = castlingRights;
            return;
        }

        if (castlingRights.Length > 4 || !Regex.IsMatch(castlingRights, "^[KQkq]+$"))
        {
            throw new ArgumentException("Invalid castling availability in FEN string.", nameof(castlingRights));
        }

        this.CastlingRights = castlingRights;
    }

    /// <summary>
    /// Sets a piece at the specified location on the chess board.
    /// </summary>
    /// <param name="file">The file of the target location. Must be between 1 and 8 inclusive.</param>
    /// <param name="rank">The rank of the target location. Must be between 1 and 8 inclusive.</param>
    /// <param name="piece">The piece to set. Lowercase for black pieces, uppercase for white pieces.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the file or rank is
    /// out of the valid range.</exception>
    /// <exception cref="ArgumentException">Thrown when an invalid piece is provided.</exception>
    public void SetPieceAt(int file, int rank, char piece)
    {
        if (file is <= 0 or > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(file));
        }

        if (rank is <= 0 or > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(rank));
        }

        if (!Regex.IsMatch(piece.ToString(), "^[rnbqkpRNBQKP]+$"))
        {
            throw new ArgumentException("Invalid piece.", nameof(piece));
        }

        ulong targetBitBoard = this.files[file - 1] & this.ranks[rank - 1];
        if (char.IsLower(piece))
        {
            // Clear the bit at the target location for all black pieces
            foreach (var key in this.BlackPieces.Keys.ToList())
            {
                this.BlackPieces[key] &= ~targetBitBoard;
            }

            // Set the bit at the target location for the specified piece
            this.BlackPieces[piece.ToString()] |= targetBitBoard;
        }
        else
        {
            // Clear the bit at the target location for all white pieces
            foreach (var key in this.WhitePieces.Keys.ToList())
            {
                this.WhitePieces[key] &= ~targetBitBoard;
            }

            // Set the bit at the target location for the specified piece
            this.WhitePieces[piece.ToString()] |= targetBitBoard;
        }

        this.UpdateBitBoards();
    }

    /// <summary>
    /// Updates the bitboards for black and white pieces.
    /// This method should be called whenever the pieces change.
    /// It performs a bitwise OR operation on the bitboards of all pieces of the same color.
    /// </summary>
    void UpdateBitBoards()
    {
        this.blackPiecesBitBoard = 0;
        foreach (var piece in this.BlackPieces.Values)
        {
            this.blackPiecesBitBoard |= piece;
        }

        this.whitePiecesBitBoard = 0;
        foreach (var piece in this.WhitePieces.Values)
        {
            this.whitePiecesBitBoard |= piece;
        }
    }
}
