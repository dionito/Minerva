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
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Gets the active color, which is the color that has the next move.
    /// </summary>
    /// <value>The active color is either 'w' for white or 'b' for black.</value>
    public char ActiveColor { get; private set; } = 'w';

    /// <summary>
    /// Represents the bitboards for the black pieces.
    /// </summary>
    public Dictionary<char, ulong> BlackPieces { get; } = new(new CaseInsensitiveCharComparer())
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
    public string EnPassantTargetSquare { get; private set; } = "-";

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
    public ulong OccupiedBitBoard { get; private set; }

    /// <summary>
    /// Gets the bitboard representing all squares under attack by white pieces.
    /// This property is updated after each move to reflect the current state of the board.
    /// </summary>
    /// 
    public ulong WhiteAttacks { get; private set; }

    /// <summary>
    /// Represents the bitboards for the white pieces.
    /// </summary>
    public Dictionary<char, ulong> WhitePieces { get; } = new(new CaseInsensitiveCharComparer())
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
            this.EnPassantTargetSquare = enPassantTargetSquare;
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
        ulong targetBitBoard = BitBoards.Files[file] & BitBoards.Ranks[rank];
        if ((this.ActiveColor == Color.Black.ToChar() && (this.WhitePieces['P'] & targetBitBoard) == 0) ||
            (this.ActiveColor == Color.White.ToChar() && (this.BlackPieces['p'] & targetBitBoard) == 0))
        {
            throw new ArgumentException(
                "Invalid en passant target square in FEN string. No pawn to be taken en passant found.",
                nameof(enPassantTargetSquare));
        }

        this.EnPassantTargetSquare = enPassantTargetSquare;
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

    private void UpdateAttacks()
    {
        this.BlackAttacks = this.UpdatePieceAttacks(this.BlackPieces);
        this.WhiteAttacks = this.UpdatePieceAttacks(this.WhitePieces);
    }

    private ulong UpdatePieceAttacks(Dictionary<char, ulong> pieces)
    {
        ulong attacks = 0;
        foreach (KeyValuePair<char, ulong> pieceKvp in pieces.Where(p => p.Value != 0))
        {
            var piece = PieceFactory.GetPiece(pieceKvp.Key);
            ulong bitboard = pieceKvp.Value;
            while (bitboard != 0)
            {
                ulong lsb = bitboard & (~bitboard + 1);
                attacks |= piece.GetPieceAttacks(lsb, this);
                bitboard &= ~lsb;
            }
        }

        return attacks;
    }

    /// <summary>
    /// Updates the board status, including piece bitboards, attacks, and check status.
    /// Optionally toggles the active color and increments the full move number after a move.
    /// </summary>
    /// <param name="afterMove">Indicates whether the update is happening after a move, which
    /// will toggle the active color and may increment the full move number, or after an
    /// artificial setup of the board, which will not.</param>
    public void UpdateBoardStatus()
    {
        this.UpdatePiecesBitBoards();
        this.UpdateOccupiedBitBoard();
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
    /// Updates the occupied bitboard by combining the bitboards of black and white pieces.
    /// This method recalculates the occupied squares on the board by performing a bitwise OR operation
    /// between the bitboards representing the positions of all black and white pieces.
    /// </summary>
    private void UpdateOccupiedBitBoard()
    {
        this.OccupiedBitBoard = this.BlackPiecesBitBoard | this.WhitePiecesBitBoard;
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