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
/// Defines the possible moving directions for a chess piece on the board.
/// </summary>
[Flags]
public enum MovingDirections
{
    /// <summary>
    /// Represents no movement.
    /// </summary>
    None = 0,

    /// <summary>
    /// Represents movement upwards on the board (e.g. rank 1 to rank 2).
    /// </summary>
    Up = 1,

    /// <summary>
    /// Represents movement downwards on the board (e.g. rank 8 to rank 7).
    /// </summary>
    Down = 2,

    /// <summary>
    /// Represents movement to the right on the board (e.g. file a to file b).
    /// </summary>
    Right = 4,

    /// <summary>
    /// Represents movement to the left on the board (e.g. file h to file g).
    /// </summary>
    Left = 8,

    /// <summary>
    /// Represents movement diagonally upwards to the left on the board.
    /// </summary>
    UpLeft = 16,

    /// <summary>
    /// Represents movement diagonally upwards to the right on the board.
    /// </summary>
    UpRight = 32,

    /// <summary>
    /// Represents movement diagonally downwards to the left on the board.
    /// </summary>
    DownLeft = 64,

    /// <summary>
    /// Represents movement diagonally downwards to the right on the board.
    /// </summary>
    DownRight = 128,

    /// <summary>
    /// Represents a knight move (L shaped) two squares up and one to the right on the chess board.
    /// </summary>
    UpUpRight = 256,

    /// <summary>
    /// Represents a knight move (L shaped) two squares up and one to the left on the chess board.
    /// </summary>
    UpUpLeft = 512,

    /// <summary>
    /// Represents a knight move (L shaped) two squares down and one to the right on the chess board.
    /// </summary>
    DownDownRight = 1024,

    /// <summary>
    /// Represents a knight move (L shaped) two squares down and one to the left on the chess board.
    /// </summary>
    DownDownLeft = 2048,

    /// <summary>
    /// Represents a knight move (L shaped) two squares to the right and one up on the chess board.
    /// </summary>
    UpRightRight = 4096,

    /// <summary>
    /// Represents a knight move (L shaped) two squares to the left and one up on the chess board.
    /// </summary>
    UpLeftLeft = 8192,

    /// <summary>
    /// Represents a knight move (L shaped) two squares to the right and one down on the chess board.
    /// </summary>
    DownRightRight = 16384,

    /// <summary>
    /// Represents a knight move (L shaped) two squares to the left and one down on the chess board.
    /// </summary>
    DownLeftLeft = 32768,

    /// <summary>
    /// Represents all straight movement directions (up, down, right, left) on the chess board.
    /// </summary>
    Rook = Up | Down | Right | Left,

    /// <summary>
    /// Represents all diagonal movement directions (up-left, up-right, down-left, down-right) on the chess board.
    /// </summary>
    Bishop = UpLeft | UpRight | DownLeft | DownRight,

    /// <summary>
    /// Represents all possible movement directions on the chess board.
    /// </summary>
    KingAndQueen = Rook | Bishop,
}