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

using Minerva.Pieces;
using System.Diagnostics.CodeAnalysis;

namespace Minerva.Tests;

[ExcludeFromCodeCoverage]
public class PieceFactory
{
    public static PieceBase CreatePiece(PieceType pieceType, Color pieceColor)
    {
        return pieceType switch
        {
            PieceType.Bishop => new Bishop(pieceColor),
            PieceType.Queen => new Queen(pieceColor),
            PieceType.Rook => new Rook(pieceColor),
            PieceType.King => new King(pieceColor),
            PieceType.Knight => new Knight(pieceColor),
            PieceType.Pawn => new Pawn(pieceColor),
            _ => throw new ArgumentException("Invalid piece type."),
        };
    }
}