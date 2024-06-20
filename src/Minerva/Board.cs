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
using Minerva.Pieces;
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
    public const char EmptySquare = ' ';

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
    /// Represents the files of the chess board.
    /// </summary>
    public static readonly ulong[] Files = 
    {
        FileA, FileB, FileC, FileD, FileE, FileF, FileG, FileH,
    };

    /// <summary>
    /// Represents the ranks of the chess board.
    /// </summary>
    public static readonly ulong[] Ranks = 
    {
        Rank1, Rank2, Rank3, Rank4, Rank5, Rank6, Rank7, Rank8,
    };

    /// <summary>
    /// Gets the active color, which is the color that has the next move.
    /// </summary>
    /// <value>The active color is either 'w' for white or 'b' for black.</value>
    public char ActiveColor { get; private set; } = 'w';

    /// <summary>
    /// Represents the bitboards for the black pieces.
    /// </summary>
    public Dictionary<char, ulong> BlackPieces { get; } = new()
    {
        { 'b', 0ul },
        { 'n', 0ul },
        { 'r', 0ul },
        { 'q', 0ul },
        { 'k', 0ul },
        { 'p', 0ul },
    };

    /// <summary>
    /// Gets the bitboard representing all squares under attack by black pieces.
    /// This property is updated after each move to reflect the current state of the board.
    /// </summary>
    public ulong BlackAttacks { get; private set; }

    /// <summary>
    /// Gets the combined bitboard for all black pieces.
    /// This property performs a bitwise OR operation on the bitboards of all black pieces.
    /// </summary>
    public ulong BlackPiecesBitBoard { get; private set; }

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
    /// Gets a value indicating whether the current player is in check.
    /// This property is true if the current player's king is under attack by the opponent's pieces,
    /// indicating that the player must make a move to remove the threat to the king.
    /// </summary>
    public bool Check { get; private set; }

    /// <summary>
    /// Gets the en passant target square in Forsyth-Edwards Notation (FEN).
    /// The en passant target square is the square where a pawn can be captured en passant.
    /// This property is represented as a string with the following possible values:
    /// </summary>
    /// <value>
    /// "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3" - for white pawns
    /// "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6" - for black pawns
    /// "-" - if there is no en passant target square
    /// </value>
    public Square EnPassantTargetSquare { get; private set; } = new();

    /// <summary>
    /// Gets the fullmove number.
    /// </summary>
    public int FullmoveNumber { get; private set; }

    /// <summary>
    /// Gets or sets the halfmove clock.
    /// </summary>
    public int HalfmoveClock { get; set; }

    /// <summary>
    /// Gets a value indicating whether the current player can take the opposing king.
    /// This property is used to check for illegal moves where the king is left in check or
    /// to determine checkmate conditions.
    /// </summary>
    public bool IllegalCheck { get; private set; }

    /// <summary>
    /// Gets the combined bitboard for all pieces on the board.
    /// This property performs a bitwise OR operation on the bitboards of all black and white pieces.
    /// </summary>
    public ulong OccupiedBitBoard => this.BlackPiecesBitBoard | this.WhitePiecesBitBoard;

    /// <summary>
    /// Gets the bitboard representing all squares under attack by white pieces.
    /// This property is updated after each move to reflect the current state of the board.
    /// </summary>
    /// 
    public ulong WhiteAttacks { get; private set; }

    /// <summary>
    /// Represents the bitboards for the white pieces.
    /// </summary>
    public Dictionary<char, ulong> WhitePieces { get; } = new()
    {
        { 'B', 0ul },
        { 'N', 0ul },
        { 'R', 0ul },
        { 'Q', 0ul },
        { 'K', 0ul },
        { 'P', 0ul },
    };

    /// <summary>
    /// Gets the combined bitboard for all white pieces.
    /// This property performs a bitwise OR operation on the bitboards of all white pieces.
    /// </summary>
    public ulong WhitePiecesBitBoard { get; private set; }

    /// <summary>
    /// Gets the piece at the specified square on the chess board.
    /// </summary>
    /// <param name="file">The file of the target location. Must be between 1 and 8 inclusive.</param>
    /// <param name="rank">The rank of the target location. Must be between 1 and 8 inclusive.</param>
    /// <returns>The piece at the specified location. ' ' if the square is empty.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the file or rank is out of the valid range.
    /// </exception>
    public char GetPieceAt(int file, int rank)
    {
        if (file is <= 0 or > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(file), "Must be between 1 and 8 inclusive.");
        }

        if (rank is <= 0 or > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(file), "Must be between 1 and 8 inclusive.");
        }

        return this.GetPieceAt(Files[file - 1] & Ranks[rank - 1]);
    }

    /// <summary>
    /// Gets the piece at the specified square on the chess board.
    /// </summary>
    /// <param name="square">The square to check.</param>
    /// <returns>The piece at the specified square.</returns>
    public char GetPieceAt(Square square)
    {
        return this.GetPieceAt(square.BitBoard);
    }

    /// <summary>
    /// Gets the piece at the specified square on the chess board.
    /// </summary>
    /// <param name="square">The square to check in algebraic notation (e.g., "e4").</param>
    /// <returns>The piece at the specified square.</returns>
    public char GetPieceAt(string square)
    {
        return this.GetPieceAt(Files[square[0] - 'a'] & Ranks[square[1] - '1']);
    }

    /// <summary>
    /// Gets the piece at the specified square on the chess board.
    /// </summary>
    /// <param name="square">The bitboard representation of the square to check.</param>
    /// <param name="color">The color of the piece to find. If Color.None, any piece will
    /// be considered.</param>
    /// <returns>The character representing the piece at the given position, or the
    /// <see cref="EmptySquare"/> character (' ') if no piece is found.</returns>
    public char GetPieceAt(ulong square, Color color = Color.None)
    {
        if (color != Color.White && (this.BlackPiecesBitBoard & square) != 0)
        {
            foreach (KeyValuePair<char, ulong> pieceType in this.BlackPieces)
            {
                if ((pieceType.Value & square) != 0)
                {
                    return pieceType.Key;
                }
            }
        }

        if (color != Color.Black && (this.WhitePiecesBitBoard & square) != 0)
        {
            foreach (KeyValuePair<char, ulong> pieceType in this.WhitePieces)
            {
                if ((pieceType.Value & square) != 0)
                {
                    return pieceType.Key;
                }
            }
        }

        return EmptySquare;
    }

    /// <summary>
    /// Initializes the chess board to the standard starting position.
    /// This method sets the bitboards for both black and white pieces.
    /// </summary>
    public void InitializeGameStartingBoard()
    {
        // Initialize black pieces
        // Rooks are placed on a8 and h8
        this.BlackPieces['r'] = Rank8 & FileA | Rank8 & FileH;
        // Knights are placed on b8 and g8
        this.BlackPieces['n'] = Rank8 & FileB | Rank8 & FileG;
        // Bishops are placed on c8 and f8
        this.BlackPieces['b'] = Rank8 & FileC | Rank8 & FileF;
        // Queen is placed on d8
        this.BlackPieces['q'] = Rank8 & FileD;
        // King is placed on e8
        this.BlackPieces['k'] = Rank8 & FileE;
        // Pawns are placed on a7 to h7
        this.BlackPieces['p'] = Rank7;

        // Initialize white pieces
        // Rooks are placed on a1 and h1
        this.WhitePieces['R'] = Rank1 & FileA | Rank1 & FileH;
        // Knights are placed on b1 and g1
        this.WhitePieces['N'] = Rank1 & FileB | Rank1 & FileG;
        // Bishops are placed on c1 and f1
        this.WhitePieces['B'] = Rank1 & FileC | Rank1 & FileF;
        // Queen is placed on d1
        this.WhitePieces['Q'] = Rank1 & FileD;
        // King is placed on e1
        this.WhitePieces['K'] = Rank1 & FileE;
        // Pawns are placed on a2 to h2
        this.WhitePieces['P'] = Rank2;

        this.UpdateBoardStatus();
    }

    /// <summary>
    /// Checks if a square on the board is empty.
    /// </summary>
    /// <param name="file">The file of the square to check. Must be between a and h inclusive.</param>
    /// <param name="rank">The rank of the square to check. Must be between 1 and 8 inclusive.</param>
    /// <returns>True if the square is empty, false otherwise.</returns>
    public bool IsEmptySquare(char file, int rank)
    {
        return this.IsEmptySquare(new Square($"{file}{rank}"));
    }

    /// <summary>
    /// Checks if a square on the board is empty.
    /// </summary>
    /// <param name="square">The square to check.</param>
    /// <returns>True if the square is empty, false otherwise.</returns>
    public bool IsEmptySquare(Square square)
    {
        return this.IsEmptySquare(square.BitBoard);
    }

    public bool IsEmptySquare(ulong square)
    {
        return (this.OccupiedBitBoard & square) == 0;
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
    /// Sets the en passant target square on the chess board.
    /// </summary>
    /// <param name="enPassantTargetSquare">The en passant target square in Forsyth-Edwards Notation (FEN). 
    /// This is the square where a pawn can be captured en passant. 
    /// The value can be one of the following:
    /// "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3" - for white pawns
    /// "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6" - for black pawns
    /// "-" - if there is no en passant target square
    /// </param>
    /// <exception cref="ArgumentException">Thrown when an invalid en passant target square is provided.</exception>
    public void SetEnPassantTargetSquare(string enPassantTargetSquare)
    {
        if (enPassantTargetSquare == "-")
        {
            this.EnPassantTargetSquare = new Square();
            return;
        }

        if (enPassantTargetSquare.Length != 2 || enPassantTargetSquare[0] < 'a' || enPassantTargetSquare[0] > 'h' ||
            (enPassantTargetSquare[1] != '3' && enPassantTargetSquare[1] != '6'))
        {
            throw new ArgumentException(
                "Invalid en passant target square in FEN string.",
                nameof(enPassantTargetSquare));
        }

        // validate there is a pawn in the correct square for the en passant target
        int file = enPassantTargetSquare[0] - 'a';
        int rank = enPassantTargetSquare[1] - '1';
        rank = this.ActiveColor == 'w' ? rank - 1 : rank + 1;
        ulong targetBitBoard = Files[file] & Ranks[rank];
        if ((this.ActiveColor == Color.Black.ToChar() && (this.WhitePieces['P'] & targetBitBoard) == 0) ||
            (this.ActiveColor == Color.White.ToChar() && (this.BlackPieces['p'] & targetBitBoard) == 0))
        {
            throw new ArgumentException(
                "Invalid en passant target square in FEN string. No pawn to be taken en passant found.",
                nameof(enPassantTargetSquare));
        }

        this.EnPassantTargetSquare = new Square(enPassantTargetSquare);
    }

    /// <summary>
    /// Sets the fullmove number. The fullmove number is the number of the full moves in a game.
    /// It starts at 1, and is incremented after a black move.
    /// </summary>
    /// <param name="fullmoveNumber">The fullmove number. Must be 1 or greater.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the fullmove number
    /// is less than 1.</exception>
    public void SetFullmoveNumber(int fullmoveNumber)
    {
        if (fullmoveNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(fullmoveNumber), "Fullmove number must be 1 or greater.");
        }

        this.FullmoveNumber = fullmoveNumber;
    }

    /// <summary>
    /// Sets the halfmove clock.
    /// </summary>
    /// <param name="halfmoveClock">The halfmove clock. Must be between 0 and 50 inclusive.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the halfmove clock
    /// is out of the valid range.</exception>
    public void SetHalfmoveClock(int halfmoveClock)
    {
        if (halfmoveClock is < 0 or > 50)
        {
            throw new ArgumentOutOfRangeException(
                nameof(halfmoveClock),
                "Halfmove clock must be between 0 and 50.");
        }

        this.HalfmoveClock = halfmoveClock;
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

        ulong targetBitBoard = Files[file - 1] & Ranks[rank - 1];
        if (char.IsLower(piece))
        {
            // Clear the bit at the target location for all black pieces
            foreach (var key in this.BlackPieces.Keys.ToList())
            {
                this.BlackPieces[key] &= ~targetBitBoard;
            }

            // Set the bit at the target location for the specified piece
            this.BlackPieces[piece] |= targetBitBoard;
        }
        else
        {
            // Clear the bit at the target location for all white pieces
            foreach (var key in this.WhitePieces.Keys.ToList())
            {
                this.WhitePieces[key] &= ~targetBitBoard;
            }

            // Set the bit at the target location for the specified piece
            this.WhitePieces[piece] |= targetBitBoard;
        }

        this.UpdateBoardStatus();
    }

    /// <summary>
    /// Checks if a given square contains a piece of a specific color.
    /// </summary>
    /// <param name="file">The file of the square to check. Must be between 'a' and 'h' inclusive.</param>
    /// <param name="rank">The rank of the square to check. Must be between 1 and 8 inclusive.</param>
    /// <param name="color">The color of the piece to check for. 'w' for white and 'b' for black.</param>
    /// <returns>True if the square contains a piece of the specified color, false otherwise.</returns>
    public bool SquareContainPieceOfColor(char file, int rank, Color color)
    {
        return this.SquareContainPieceOfColor(new Square($"{file}{rank}"), (char)color);
    }

    /// <summary>
    /// Checks if a given square contains a piece of a specific color.
    /// </summary>
    /// <param name="square">The square to check.</param>
    /// <param name="color">The color of the piece to check for. 'w' for white and 'b' for black.</param>
    /// <returns>True if the square contains a piece of the specified color, false otherwise.</returns>
    public bool SquareContainPieceOfColor(Square square, Color color)
    {
        return this.SquareContainPieceOfColor(square, (char)color);
    }

    /// <summary>
    /// Checks if a given square contains a piece of a specific color.
    /// </summary>
    /// <param name="file">The file of the square to check. Must be between 'a' and 'h' inclusive.</param>
    /// <param name="rank">The rank of the square to check. Must be between 1 and 8 inclusive.</param>
    /// <param name="color">The color of the piece to check for. 'w' for white and 'b' for black.</param>
    /// <returns>True if the square contains a piece of the specified color, false otherwise.</returns>
    public bool SquareContainPieceOfColor(char file, int rank, char color)
    {
        return this.SquareContainPieceOfColor(new Square($"{file}{rank}"), color);
    }

    /// <summary>
    /// Checks if a given square contains a piece of a specific color.
    /// </summary>
    /// <param name="square">The square to check.</param>
    /// <param name="color">The color of the piece to check for. 'w' for white and 'b' for black.</param>
    /// <returns>True if the square contains a piece of the specified color, false otherwise.</returns>
    public bool SquareContainPieceOfColor(Square square, char color)
    {
        return this.SquareContainPieceOfColor(square.BitBoard, color);
    }

    /// <summary>
    /// Checks if a square contains a piece of the specified color.
    /// </summary>
    /// <param name="square">The square to check, represented as a bitboard with a single bit set.</param>
    /// <param name="color">The color to check for. Can be either 'w' for white or 'b' for black.</param>
    /// <returns>true if the square contains a piece of the specified color; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid color is provided.</exception>
    public bool SquareContainPieceOfColor(ulong square, Color color)
    {
        return this.SquareContainPieceOfColor(square, (char)color);
    }

    /// <summary>
    /// Checks if a square contains a piece of the specified color.
    /// </summary>
    /// <param name="square">The square to check, represented as a bitboard with a single bit set.</param>
    /// <param name="color">The color to check for. Can be either 'w' for white or 'b' for black.</param>
    /// <returns>true if the square contains a piece of the specified color; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid color is provided.</exception>
    public bool SquareContainPieceOfColor(ulong square, char color)
    {
        return color switch
        {
            'w' => (this.WhitePiecesBitBoard & square) != 0,
            'b' => (this.BlackPiecesBitBoard & square) != 0,
            _ => throw new ArgumentException($"Invalid color: {color}. Valid colors are 'b' or 'w'."
                , nameof(color)),
        };
    }

    private void UpdateAttacks()
    {
        this.BlackAttacks = 0;
        this.WhiteAttacks = 0;

        foreach (KeyValuePair<char, ulong> pieceKvp in this.BlackPieces.Where(p => p.Value != 0))
        {
            var piece = PieceFactory.GetPiece(pieceKvp.Key);
            this.BlackAttacks |= piece.GetPieceAttacks(pieceKvp.Value, this);
        }

        foreach (KeyValuePair<char, ulong> pieceKvp in this.WhitePieces.Where(p => p.Value != 0))
        {
            var piece = PieceFactory.GetPiece(pieceKvp.Key);
            this.WhiteAttacks |= piece.GetPieceAttacks(pieceKvp.Value, this);
        }
    }

    /// <summary>
    /// Updates the board status, including piece bitboards, attacks, and check status.
    /// Optionally toggles the active color and increments the full move number after a move.
    /// </summary>
    /// <param name="afterMove">Indicates whether the update is happening after a move, which
    /// will toggle the active color and may increment the full move number, or after an
    /// artificial setup of the board, which will not.</param>
    public void UpdateBoardStatus(bool afterMove = false)
    {
        if (afterMove)
        {
            if (this.ActiveColor == 'w')
            {
                this.ActiveColor = 'b';
            }
            else
            {
                this.ActiveColor = 'w';
                this.FullmoveNumber++;
            }
        }

        this.UpdatePiecesBitBoards();
        this.UpdateAttacks();
        this.UpdateCheckAndIllegalCheck();
    }

    /// <summary>
    /// Updates the check and illegal check status based on the current attacks against the kings.
    /// </summary>
    /// <remarks>Illegal check indicates whether the current player can take the opposing king.</remarks>
    void UpdateCheckAndIllegalCheck()
    {
        if (this.ActiveColor == 'w')
        {
            this.Check = (this.BlackAttacks & this.WhitePieces['K']) != 0;
            this.IllegalCheck = (this.WhiteAttacks & this.BlackPieces['k']) != 0;
        }
        else
        {
            this.Check = (this.WhiteAttacks & this.BlackPieces['k']) != 0;
            this.IllegalCheck = (this.BlackAttacks & this.WhitePieces['K']) != 0;
        }
    }

    /// <summary>
    /// Updates the bitboards representing all pieces for each color by aggregating the bitboards
    /// of individual piece types.
    /// </summary>
    private void UpdatePiecesBitBoards()
    {
        this.BlackPiecesBitBoard = 0;
        this.WhitePiecesBitBoard = 0;

        foreach (KeyValuePair<char, ulong> pieceKvp in this.BlackPieces.Where(p => p.Value != 0))
        {
            this.BlackPiecesBitBoard |= pieceKvp.Value;
        }

        foreach (KeyValuePair<char, ulong> pieceKvp in this.WhitePieces.Where(p => p.Value != 0))
        {
            this.WhitePiecesBitBoard |= pieceKvp.Value;
        }
    }
}
