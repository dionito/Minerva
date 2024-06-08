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
        Assert.AreEqual(PieceType.King, king.PieceType, "Piece type.");
        Assert.AreEqual(color, king.Color, "Piece color.");
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
    [DataRow(Color.White, DisplayName = "White")]
    [DataRow(Color.Black, DisplayName = "Black")]
    public void KnightConstructorSetsCorrectPieceTypeAndColor(Color color)
    {
        var knight = new Knight(color);
        Assert.AreEqual(PieceType.Knight, knight.PieceType, "Piece type.");
        Assert.AreEqual(color, knight.Color, "Piece color.");
    }

    [TestMethod]
    public void KnightGetPossibleMovesBitBoardReturnsCorrectMovesInEmptyBoard()
    {
        var knight = new Knight(Color.White);
        var board = new Board();
        var position = new Square("d4");

        ulong possibleMoves = knight.GetPossibleMovesBitBoard(position, board);
        ulong exptectedMoves = new Square("b3").BitBoard | new Square("b5").BitBoard |
            new Square("c2").BitBoard | new Square("c6").BitBoard | new Square("e2").BitBoard |
            new Square("e6").BitBoard | new Square("f3").BitBoard | new Square("f5").BitBoard;

        Assert.AreEqual(exptectedMoves, possibleMoves, "Moves missmatch.");

        // test that the knight can't move to the same square
        Assert.IsTrue((position.BitBoard & possibleMoves) == 0, "Can't move to same square");
    }

    [TestMethod]
    [DataRow(Color.White, DisplayName = "White")]
    [DataRow(Color.Black, DisplayName = "Black")]
    public void PawnConstructorSetsCorrectPieceTypeAndColor(Color color)
    {
        var pawn = new Pawn(color);
        Assert.AreEqual(PieceType.Pawn, pawn.PieceType, "Piece type.");
        Assert.AreEqual(color, pawn.Color, "Piece color.");
    }

    [TestMethod]
    public void PawnGetPossibleMovesBitBoardReturnsCorrectMovesInEmptyBoard()
    {
        var pawn = new Pawn(Color.White);
        var board = new Board();
        var position = new Square("d2");

        ulong possibleMoves = pawn.GetPossibleMovesBitBoard(position, board);
        ulong exptectedMoves = new Square("d3").BitBoard | new Square("d4").BitBoard;

        Assert.AreEqual(exptectedMoves, possibleMoves, "Moves missmatch.");

        // test that the pawn can't move to the same square
        Assert.IsTrue((position.BitBoard & possibleMoves) == 0, "Can't move to same square");
    }

    [TestMethod]
    [DataRow(PieceType.Bishop, DisplayName = "Bishop")]
    [DataRow(PieceType.King, DisplayName = "King")]
    [DataRow(PieceType.Knight, DisplayName = "Knight")]
    [DataRow(PieceType.Pawn, DisplayName = "Pawn")]
    [DataRow(PieceType.Queen, DisplayName = "Queen")]
    [DataRow(PieceType.Rook, DisplayName = "Rook")]
    public void PiecesCannotBeCreatedWithoutAColor(PieceType pieceType)
    {
        Exception exception =
            Assert.ThrowsException<ArgumentException>(() => PieceFactory.CreatePiece(pieceType, Color.None));
        Assert.AreEqual($"Invalid piece color: '{Color.None}'. (Parameter 'color')", exception.Message);
    }

    [TestMethod]
    public void PiecesCanOnlyTakeOpponentPieces()
    {
        var scenarios = new[]
        {
            new
            {
                Name = "White queen can take black pieces, but not white ones",
                Fen = "3k4/8/8/8/5K2/8/qB6/Qn6 w - - 0 1",
                Piece = PieceFactory.CreatePiece(PieceType.Queen, Color.White),
                Position = new Square("a1"),
                ExpectedMoves =
                    new Square("a2").BitBoard |
                    new Square("b1").BitBoard,
            },
            new
            {
                Name = "Black queen can take black pieces, but not black ones",
                Fen = "3K4/8/8/8/5k2/8/Qb6/qN6 b - - 0 1",
                Piece = PieceFactory.CreatePiece(PieceType.Queen, Color.Black),
                Position = new Square("a1"),
                ExpectedMoves =
                    new Square("a2").BitBoard |
                    new Square("b1").BitBoard,
            },
            new
            {
                Name = "White king can take black pieces, but not white ones",
                Fen = "3k4/8/8/8/5Q2/8/qB6/Kn6 w - - 0 1",
                Piece = PieceFactory.CreatePiece(PieceType.King, Color.White),
                Position = new Square("a1"),
                ExpectedMoves =
                    new Square("a2").BitBoard |
                    new Square("b1").BitBoard,
            },
            new
            {
                Name = "Black king can take black pieces, but not white ones",
                Fen = "3K4/8/8/8/5q2/8/Qb6/kN6 b - - 0 1",
                Piece = PieceFactory.CreatePiece(PieceType.King, Color.Black),
                Position = new Square("a1"),
                ExpectedMoves =
                    new Square("a2").BitBoard |
                    new Square("b1").BitBoard,
            },
            new
            {
                Name = "White knight can take black pieces, but not white ones",
                Fen = "rnbqkbnr/pppppppp/8/2N5/8/1P1P4/P1P1PPPP/R1BQKBNR w KQkq - 0 1",
                Piece = PieceFactory.CreatePiece(PieceType.Knight, Color.White),
                Position = new Square("c5"),
                ExpectedMoves =
                    new Square("a4").BitBoard |
                    new Square("a6").BitBoard |
                    new Square("b7").BitBoard |
                    new Square("d7").BitBoard |
                    new Square("e4").BitBoard |
                    new Square("e6").BitBoard,
            },
            new
            {
                Name = "Black knight can take white pieces, but not black ones",
                Fen = "r1bqkbnr/pppppppp/8/2n5/8/1P1P4/P1P1PPPP/RNBQKBNR b KQkq - 0 1",
                Piece = PieceFactory.CreatePiece(PieceType.Knight, Color.Black),
                Position = new Square("c5"),
                ExpectedMoves =
                    new Square("a4").BitBoard |
                    new Square("a6").BitBoard |
                    new Square("b3").BitBoard |
                    new Square("d3").BitBoard |
                    new Square("e4").BitBoard |
                    new Square("e6").BitBoard,
            },
            new
            {
                Name = "White pawn can take black pieces, but not white ones",
                Fen = "rnbqkbnr/ppp1pppp/8/8/8/3p1N2/PPPPPPPP/RNBQKB1R w KQkq - 0 1",
                Piece = PieceFactory.CreatePiece(PieceType.Pawn, Color.White),
                Position = new Square("e2"),
                ExpectedMoves =
                    new Square("d3").BitBoard |
                    new Square("e3").BitBoard |
                    new Square("e4").BitBoard,
            },
            new
            {
                Name = "Black pawn can take white pieces, but not black ones",
                Fen = "rnbqkb1r/pppppppp/3P1n2/8/8/8/PPP1PPPP/RNBQKBNR b KQkq - 0 1",
                Piece = PieceFactory.CreatePiece(PieceType.Pawn, Color.Black),
                Position = new Square("e7"),
                ExpectedMoves =
                    new Square("d6").BitBoard |
                    new Square("e6").BitBoard |
                    new Square("e5").BitBoard,
            },
        };

        foreach (var scenario in scenarios)
        {
            Console.WriteLine(scenario.Name);
            Board board = ForsythEdwardsNotation.GenerateBoard(scenario.Fen);
            ulong possibleMoves = scenario.Piece.GetPossibleMovesBitBoard(scenario.Position, board);
            Assert.AreEqual(scenario.ExpectedMoves, possibleMoves);
        }
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