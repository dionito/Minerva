// Copyright (C) 2024 Dioni de la Morena Morales
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minerva.Pieces;

namespace Minerva.Tests;

[TestClass]
public class PieceTests
{
    [TestMethod]
    [DataRow(Color.White, DisplayName = "White")]
    [DataRow(Color.Black, DisplayName = "Black")]
    public void BishopConstructorSetsCorrectPieceTypeAndColor(Color color)
    {
        var bishop = new Bishop(color);
        Assert.AreEqual(PieceType.Bishop, bishop.PieceType);
        Assert.AreEqual(color, bishop.Color);
    }

    [TestMethod]
    public void BishopGetPossibleMovesBitBoardReturnsCorrectMovesInEmptyBoard()
    {
        var bishop = new Bishop(Color.White);
        var board = new Board(); 
        var position = new Square("d4");

        ulong possibleMoves = bishop.GetPossibleMovesBitBoard(position, board);
        ulong exptectedMoves = new Square("c3").BitBoard | new Square("b2").BitBoard | 
            new Square("a1").BitBoard | new Square("e5").BitBoard | new Square("f6").BitBoard | 
            new Square("g7").BitBoard | new Square("h8").BitBoard | new Square("c5").BitBoard | 
            new Square("b6").BitBoard | new Square("a7").BitBoard | new Square("e3").BitBoard |
            new Square("f2").BitBoard | new Square("g1").BitBoard;

        Assert.AreEqual(exptectedMoves, possibleMoves, "Moves missmatch.");

        // test that the bishop can't move to the same square
        Assert.IsTrue((position.BitBoard & possibleMoves) == 0, "Can't move to same square");
    }

    [TestMethod]
    [DataRow(Color.White, DisplayName = "White")]
    [DataRow(Color.Black, DisplayName = "Black")]
    public void KingConstructorSetsCorrectPieceTypeAndColor(Color color)
    {
        var king = new King(color);
        Assert.AreEqual(PieceType.King, king.PieceType);
        Assert.AreEqual(color, king.Color);
    }

    [TestMethod]
    public void KingGetPossibleMovesBitBoardReturnsCorrectMovesInEmptyBoard()
    {
        var king = new King(Color.White);
        var board = new Board();
        var position = new Square("d4");

        ulong possibleMoves = king.GetPossibleMovesBitBoard(position, board);
        ulong exptectedMoves = new Square("c3").BitBoard | new Square("d3").BitBoard | 
            new Square("e3").BitBoard | new Square("c4").BitBoard | new Square("e4").BitBoard |
            new Square("c5").BitBoard | new Square("d5").BitBoard | new Square("e5").BitBoard;

        Assert.AreEqual(exptectedMoves, possibleMoves, "Moves missmatch.");

        // test that the king can't move to the same square
        Assert.IsTrue((position.BitBoard & possibleMoves) == 0, "Can't move to same square");
    }

    [TestMethod]
    public void PiecesCanTakeOpponentPiecesAndCannotPassThroughThem()
    {
        Board board = ForsythEdwardsNotation.GenerateBoard("3k4/8/8/8/5K2/8/qb6/Qn6 w - - 0 1");
        Queen whiteQueen = new Queen(Color.White);
        var queenMoves = whiteQueen.GetPossibleMovesBitBoard(new Square("a1"), board);
        var expectedQueenMoves = new Square("a2").BitBoard | new Square("b2").BitBoard | new Square("b1").BitBoard;
        Assert.AreEqual(expectedQueenMoves, queenMoves);
    }

    [TestMethod]
    [DataRow(Color.White, DisplayName = "White")]
    [DataRow(Color.Black, DisplayName = "Black")]
    public void QueenConstructorSetsCorrectPieceTypeAndColor(Color color)
    {
        var queen = new Queen(color);
        Assert.AreEqual(PieceType.Queen, queen.PieceType);
        Assert.AreEqual(color, queen.Color);
    }

    [TestMethod]
    public void QueenGetPossibleMovesReturnsCorrectMovesInEmptyBoard()
    {
        var queen = new Queen(Color.White);
        var bishop = new Bishop(Color.White);
        var rook = new Rook(Color.White);
        var board = new Board();
        var position = new Square("d4");

        ulong possibleMoves = queen.GetPossibleMovesBitBoard(position, board);
        ulong expectedPossibleMoves = bishop.GetPossibleMovesBitBoard(position, board) |
            rook.GetPossibleMovesBitBoard(position, board);

        Assert.AreEqual(expectedPossibleMoves, possibleMoves, "Moves missmatch.");

        // test that the queen can't move to the same square
        Assert.IsTrue((position.BitBoard & possibleMoves) == 0, "Can't move to same square");
    }

    [TestMethod]
    [DataRow(Color.White, DisplayName = "White")]
    [DataRow(Color.Black, DisplayName = "Black")]
    public void RookConstructorSetsCorrectPieceTypeAndColor(Color color)
    {
        var rook = new Rook(color);
        Assert.AreEqual(PieceType.Rook, rook.PieceType);
        Assert.AreEqual(color, rook.Color);

    }

    [TestMethod]
    public void RookGetPossibleMovesReturnsCorrectMovesInEmptyBoard()
    {
        var rook = new Rook(Color.Black);
        var board = new Board();
        var position = new Square("d4");

        ulong possibleMoves = rook.GetPossibleMovesBitBoard(position, board);
        ulong expectedPossibleMoves = (Board.FileD | Board.Rank4) ^ position.BitBoard;

        Assert.AreEqual(expectedPossibleMoves, possibleMoves, "Moves mismatch");

        // test that the rook can't move to the same square
        Assert.IsTrue((position.BitBoard & possibleMoves) == 0, "Can't move to same square");
    }

    [TestMethod]
    [DataRow(PieceType.Rook, Color.White, "a1", DisplayName = "White Rook")]
    [DataRow(PieceType.Bishop, Color.White, "b1", DisplayName = "White Bishop")]
    [DataRow(PieceType.Queen, Color.White, "d1", DisplayName = "White Queen")]
    [DataRow(PieceType.King, Color.White, "e1", DisplayName = "White King")]
    [DataRow(PieceType.Rook, Color.Black, "a8", DisplayName = "Black Rook")]
    [DataRow(PieceType.Bishop, Color.Black, "b8", DisplayName = "Black Bishop")]
    [DataRow(PieceType.Queen, Color.Black, "d8", DisplayName = "Black Queen")]
    [DataRow(PieceType.King, Color.Black, "e8", DisplayName = "Black King")]
    public void SomePiecesCannotMoveOnAnInitialBoard(PieceType pieceType, Color pieceColor, string position)
    {
        var board = new Board();
        board.InitializeGameStartingBoard();
        PieceBase piece = PieceFactory.CreatePiece(pieceType, pieceColor);
        Square square = new Square(position);
        Assert.AreEqual(0ul, piece.GetPossibleMovesBitBoard(square, board));
    }
}