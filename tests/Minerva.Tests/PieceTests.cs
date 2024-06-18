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
using System.Reflection;

namespace Minerva.Tests;

[TestClass]
public class PieceTests
{
    [TestMethod]
    [DataRow(Color.White, DisplayName = "White")]
    [DataRow(Color.Black, DisplayName = "Black")]
    public void BishopConstructorSetsCorrectPieceTypeAndColor(Color color)
    {
        var bishop = PieceFactory.GetPiece(PieceType.Bishop, color);
        Assert.AreEqual(PieceType.Bishop, bishop.PieceType);
        Assert.AreEqual(color, bishop.Color);
    }

    [TestMethod]
    public void BishopGetPossibleMovesAndAttacksBitBoardsAreTheSameOnEmptyBoard()
    {
        var bishop = PieceFactory.GetPiece(PieceType.Bishop, Color.White);
        var board = ForsythEdwardsNotation.GenerateBoard("k7/8/8/8/3B4/8/8/7K w - - 0 1"); // Bishop in d4
        var position = new Square("d4");

        ulong possibleMoves = bishop.GetPieceMoves(position.BitBoard, board);
        ulong attacks = bishop.GetPieceAttacks(position.BitBoard, board);
        ulong exptectedMoves = new Square("c3").BitBoard | new Square("b2").BitBoard |
            new Square("a1").BitBoard | new Square("e5").BitBoard | new Square("f6").BitBoard |
            new Square("g7").BitBoard | new Square("h8").BitBoard | new Square("c5").BitBoard |
            new Square("b6").BitBoard | new Square("a7").BitBoard | new Square("e3").BitBoard |
            new Square("f2").BitBoard | new Square("g1").BitBoard;

        Assert.AreEqual(exptectedMoves, possibleMoves, "Moves missmatch.");
        Assert.AreEqual(exptectedMoves, attacks, "Attacks missmatch.");

        // test that the bishop can't move to the same square
        Assert.IsTrue((position.BitBoard & possibleMoves) == 0, "Can't move to same square");
    }

    [TestMethod]
    [DataRow(Color.White, DisplayName = "White")]
    [DataRow(Color.Black, DisplayName = "Black")]
    public void KingConstructorSetsCorrectPieceTypeAndColor(Color color)
    {
        var king = PieceFactory.GetPiece(PieceType.King, color);
        Assert.AreEqual(PieceType.King, king.PieceType, "Piece type.");
        Assert.AreEqual(color, king.Color, "Piece color.");
    }

    [TestMethod]
    public void KingGetPossibleMovesAndAttacksBitBoardsAreTheSameOnEmptyBoard()
    {
        var king = PieceFactory.GetPiece(PieceType.King, Color.White);
        var board = ForsythEdwardsNotation.GenerateBoard("k7/8/8/8/3K4/8/8/8 w - - 0 1"); // King in d4
        var position = new Square("d4");

        ulong possibleMoves = king.GetPieceMoves(position.BitBoard, board);
        ulong attacks = king.GetPieceAttacks(position.BitBoard, board);
        ulong expectedMoves = new Square("c3").BitBoard | new Square("d3").BitBoard |
            new Square("e3").BitBoard | new Square("c4").BitBoard | new Square("e4").BitBoard |
            new Square("c5").BitBoard | new Square("d5").BitBoard | new Square("e5").BitBoard;

        Assert.AreEqual(expectedMoves, possibleMoves, "Moves missmatch.");
        Assert.AreEqual(expectedMoves, attacks, "Attacks mismatch.");

        // test that the king can't move to the same square
        Assert.IsTrue((position.BitBoard & possibleMoves) == 0, "Can't move to same square");
    }

    [TestMethod]
    [DataRow(Color.White, DisplayName = "White")]
    [DataRow(Color.Black, DisplayName = "Black")]
    public void KnightConstructorSetsCorrectPieceTypeAndColor(Color color)
    {
        var knight = PieceFactory.GetPiece(PieceType.Knight, color);
        Assert.AreEqual(PieceType.Knight, knight.PieceType, "Piece type.");
        Assert.AreEqual(color, knight.Color, "Piece color.");
    }

    [TestMethod]
    public void KnightGetPossibleMovesAndAttacksBitBoardsAreTheSameOnEmptyBoard()
    {
        var knight = PieceFactory.GetPiece(PieceType.Knight, Color.White);
        var board = ForsythEdwardsNotation.GenerateBoard("k7/8/8/8/3N4/8/8/7K w - - 0 1"); // Knigth in d4
        var position = new Square("d4");

        ulong possibleMoves = knight.GetPieceMoves(position.BitBoard, board);
        ulong attacks = knight.GetPieceAttacks(position.BitBoard, board);
        ulong expectedMoves = new Square("b3").BitBoard | new Square("b5").BitBoard |
            new Square("c2").BitBoard | new Square("c6").BitBoard | new Square("e2").BitBoard |
            new Square("e6").BitBoard | new Square("f3").BitBoard | new Square("f5").BitBoard;

        Assert.AreEqual(expectedMoves, possibleMoves, "Moves missmatch.");
        Assert.AreEqual(expectedMoves, attacks, "Attacks mismatch.");

        // test that the knight can't move to the same square
        Assert.IsTrue((position.BitBoard & possibleMoves) == 0, "Can't move to same square");
    }

    [TestMethod]
    public void NoPieceConstructorSetsCorrectPieceAndTypeColor()
    {
        var none = PieceFactory.GetPiece(PieceType.None, Color.None);
        Assert.AreEqual(PieceType.None, none.PieceType, "Piece type.");
        Assert.AreEqual(Color.None, none.Color, "Color");
    }

    [TestMethod]
    public void NoPieceGetsNoMoves()
    {
        var none = PieceFactory.GetPiece(PieceType.None, Color.None);
        var moves = none.GetPieceMoves(1ul, new Board());
        Assert.AreEqual(0ul, moves, "Moves");
    }

    [TestMethod]
    [DataRow(Color.White, DisplayName = "White")]
    [DataRow(Color.Black, DisplayName = "Black")]
    public void PawnConstructorSetsCorrectPieceTypeAndColor(Color color)
    {
        var pawn = PieceFactory.GetPiece(PieceType.Pawn, color);
        Assert.AreEqual(PieceType.Pawn, pawn.PieceType, "Piece type.");
        Assert.AreEqual(color, pawn.Color, "Piece color.");
    }

    [TestMethod]
    public void PawnGetPossibleAttacksBitBoardReturnsCorrectPositionsInEmptyBoard()
    {
        var pawn = PieceFactory.GetPiece(PieceType.Pawn, Color.White);
        var board = ForsythEdwardsNotation.GenerateBoard("k7/8/8/8/8/8/3P4/7K w - - 0 1"); // Knigth in d4
        var position = new Square("d2");

        ulong attacks = pawn.GetPieceAttacks(position.BitBoard, board);
        ulong expectedMoves = new Square("c3").BitBoard | new Square("e3").BitBoard;

        Assert.AreEqual(expectedMoves, attacks, "Attacks missmatch.");

        // test that the pawn can't move to the same square
        Assert.IsTrue((position.BitBoard & attacks) == 0, "Can't move to same square");
    }

    [TestMethod]
    public void PawnGetPossibleMovesBitBoardReturnsCorrectMovesInEmptyBoard()
    {
        var pawn = PieceFactory.GetPiece(PieceType.Pawn, Color.White);
        var board = ForsythEdwardsNotation.GenerateBoard("k7/8/8/8/8/8/3P4/7K w - - 0 1"); // Knigth in d4
        var position = new Square("d2");

        ulong possibleMoves = pawn.GetPieceMoves(position.BitBoard, board);
        ulong expectedMoves = new Square("d3").BitBoard | new Square("d4").BitBoard;

        Assert.AreEqual(expectedMoves, possibleMoves, "Moves missmatch.");

        // test that the pawn can't move to the same square
        Assert.IsTrue((position.BitBoard & possibleMoves) == 0, "Can't move to same square");
    }

    [TestMethod]
    public void PieceAttacksAreCorrectInInitialBoard()
    {
        var scenarios = new[]
        {
            new
            {
                Name = "Black bishop",
                Piece = PieceFactory.GetPiece(PieceType.Bishop, Color.Black),
                Position = new Square("c8"),
                ExpectedAttacks = new Square("b7").BitBoard | new Square("d7").BitBoard,
            },
            new
            {
                Name = "Black king",
                Piece = PieceFactory.GetPiece(PieceType.King, Color.Black),
                Position = new Square("e8"),
                ExpectedAttacks = new Square("d7").BitBoard | new Square("d8").BitBoard |
                    new Square("e7").BitBoard | new Square("f7").BitBoard |
                    new Square("f8").BitBoard,
            },
            new
            {
                Name = "Black knight",
                Piece = PieceFactory.GetPiece(PieceType.Knight, Color.Black),
                Position = new Square("b8"),
                ExpectedAttacks = new Square("a6").BitBoard | 
                    new Square("c6").BitBoard |
                    new Square("d7").BitBoard,
            },
            new
            {
                Name = "Black queen",
                Piece = PieceFactory.GetPiece(PieceType.Queen, Color.Black),
                Position = new Square("d8"),
                ExpectedAttacks = new Square("c7").BitBoard | new Square("c8").BitBoard |
                    new Square("d7").BitBoard | new Square("e7").BitBoard |
                    new Square("e8").BitBoard,
            },
            new
            {
                Name = "Black rook",
                Piece = PieceFactory.GetPiece(PieceType.Rook, Color.Black),
                Position = new Square("a8"),
                ExpectedAttacks = new Square("a7").BitBoard | new Square("b8").BitBoard,
            },
            new
            {
                Name = "File A black pawn",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.Black),
                Position = new Square("a7"),
                ExpectedAttacks = new Square("b6").BitBoard,
            },
            new
            {
                Name = "File A white pawn",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.White),
                Position = new Square("a2"),
                ExpectedAttacks = new Square("b3").BitBoard,
            },
            new
            {
                Name = "File B black pawn",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.Black),
                Position = new Square("b7"),
                ExpectedAttacks = new Square("a6").BitBoard | new Square("c6").BitBoard,
            },
            new
            {
                Name = "File B white pawn",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.White),
                Position = new Square("b2"),
                ExpectedAttacks = new Square("a3").BitBoard | new Square("c3").BitBoard,
            },
            new
            {
                Name = "File H black pawn",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.Black),
                Position = new Square("h7"),
                ExpectedAttacks = new Square("g6").BitBoard,
            },
            new
            {
                Name = "File H white pawn",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.White),
                Position = new Square("h2"),
                ExpectedAttacks = new Square("g3").BitBoard,
            },
            new
            {
                Name = "White bishop",
                Piece = PieceFactory.GetPiece(PieceType.Bishop, Color.White),
                Position = new Square("c1"),
                ExpectedAttacks = new Square("b2").BitBoard | new Square("d2").BitBoard,
            },
            new
            {
                Name = "White king",
                Piece = PieceFactory.GetPiece(PieceType.King, Color.White),
                Position = new Square("e1"),
                ExpectedAttacks = new Square("d1").BitBoard | new Square("d2").BitBoard |
                    new Square("e2").BitBoard | new Square("f1").BitBoard |
                    new Square("f2").BitBoard,
            },
            new
            {
                Name = "White knight",
                Piece = PieceFactory.GetPiece(PieceType.Knight, Color.White),
                Position = new Square("b1"),
                ExpectedAttacks = new Square("a3").BitBoard |
                    new Square("c3").BitBoard |
                    new Square("d2").BitBoard,
            },
            new
            {
                Name = "White queen",
                Piece = PieceFactory.GetPiece(PieceType.Queen, Color.White),
                Position = new Square("d1"),
                ExpectedAttacks = new Square("c1").BitBoard | new Square("c2").BitBoard |
                    new Square("d2").BitBoard | new Square("e1").BitBoard |
                    new Square("e2").BitBoard,
            },
            new
            {
                Name = "White rook",
                Piece = PieceFactory.GetPiece(PieceType.Rook, Color.White),
                Position = new Square("a1"),
                ExpectedAttacks = new Square("a2").BitBoard | new Square("b1").BitBoard,
            },
        };

        foreach (var scenario in scenarios)
        {
            Console.WriteLine($"Starting: {scenario.Name}.");
            Board board = new Board();
            board.InitializeGameStartingBoard();
            ulong attacks = scenario.Piece.GetPieceAttacks(scenario.Position.BitBoard, board);
            Assert.AreEqual(scenario.ExpectedAttacks, attacks, scenario.Name);
        }
    }

    [TestMethod]
    [DataRow(typeof(Bishop), DisplayName = "Bishop")]
    [DataRow(typeof(King), DisplayName = "King")]
    [DataRow(typeof(Knight), DisplayName = "Knight")]
    [DataRow(typeof(Pawn), DisplayName = "Pawn")]
    [DataRow(typeof(Queen), DisplayName = "Queen")]
    [DataRow(typeof(Rook), DisplayName = "Rook")]
    public void PiecesCannotBeCreatedWithoutAColor(Type pieceType)
    {
        ConstructorInfo? constructor = pieceType.GetConstructor(new[] { typeof(Color) });
        Assert.IsNotNull(constructor, "constructor != null");
        Exception exception =
            Assert.ThrowsException<TargetInvocationException>(() => constructor.Invoke(new object[] { Color.None }));
        Assert.IsInstanceOfType(exception.InnerException, typeof(ArgumentException));
        Assert.AreEqual($"Invalid piece color: '{Color.None}'. (Parameter 'color')", exception.InnerException.Message);
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
                Piece = PieceFactory.GetPiece(PieceType.Queen, Color.White),
                Position = new Square("a1"),
                ExpectedMoves =
                    new Square("a2").BitBoard |
                    new Square("b1").BitBoard,
            },
            new
            {
                Name = "Black queen can take black pieces, but not black ones",
                Fen = "3K4/8/8/8/5k2/8/Qb6/qN6 b - - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.Queen, Color.Black),
                Position = new Square("a1"),
                ExpectedMoves =
                    new Square("a2").BitBoard |
                    new Square("b1").BitBoard,
            },
            new
            {
                Name = "White king can take black pieces, but not white ones",
                Fen = "3k4/8/8/8/5Q2/8/qB6/Kn6 w - - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.King, Color.White),
                Position = new Square("a1"),
                ExpectedMoves =
                    new Square("a2").BitBoard |
                    new Square("b1").BitBoard,
            },
            new
            {
                Name = "Black king can take black pieces, but not white ones",
                Fen = "3K4/8/8/8/5q2/8/Qb6/kN6 b - - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.King, Color.Black),
                Position = new Square("a1"),
                ExpectedMoves =
                    new Square("a2").BitBoard |
                    new Square("b1").BitBoard,
            },
            new
            {
                Name = "White knight can take black pieces, but not white ones",
                Fen = "rnbqkbnr/pppppppp/8/2N5/8/1P1P4/P1P1PPPP/R1BQKBNR w KQkq - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.Knight, Color.White),
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
                Piece = PieceFactory.GetPiece(PieceType.Knight, Color.Black),
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
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.White),
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
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.Black),
                Position = new Square("e7"),
                ExpectedMoves =
                    new Square("d6").BitBoard |
                    new Square("e6").BitBoard |
                    new Square("e5").BitBoard,
            },
            new
            {
                Name = "White pawn can take black pieces, but not white ones, opposite side",
                Fen = "rnbqkbnr/pppp1ppp/8/8/8/2N1p3/PPPPPPPP/R1BQKBNR w KQkq - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.White),
                Position = new Square("d2"),
                ExpectedMoves =
                    new Square("d3").BitBoard |
                    new Square("d4").BitBoard |
                    new Square("e3").BitBoard,
            },
            new
            {
                Name = "Black pawn can take white pieces, but not black ones, opposite side",
                Fen = "r1bqkbnr/pppppppp/2n1P3/8/8/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.Black),
                Position = new Square("d7"),
                ExpectedMoves =
                    new Square("d5").BitBoard |
                    new Square("d6").BitBoard |
                    new Square("e6").BitBoard,
            },
            new
            {
                Name = "White pawn can take black pawn en passant",
                Fen = "rnbqkbnr/pppp1ppp/8/3Pp3/4P3/8/PPP2PPP/RNBQKBNR w KQkq e6 0 2",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.White),
                Position = new Square("d5"),
                ExpectedMoves = new Square("e6").BitBoard | new Square("d6").BitBoard,
            },
            new
            {
                Name = "Black pawn can take white pawn en passant",
                Fen = "rnbqkbnr/pp1ppppp/8/8/2pPPP2/8/PPP3PP/RNBQKBNR b KQkq d3 0 3",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.Black),
                Position = new Square("c4"),
                ExpectedMoves = new Square("c3").BitBoard | new Square("d3").BitBoard,
            },
        };

        foreach (var scenario in scenarios)
        {
            Console.WriteLine($"Starting: {scenario.Name}.");
            Board board = ForsythEdwardsNotation.GenerateBoard(scenario.Fen);
            ulong possibleMoves = scenario.Piece.GetPieceMoves(scenario.Position.BitBoard, board);
            Assert.AreEqual(scenario.ExpectedMoves, possibleMoves, scenario.Name);
        }
    }

    [TestMethod]
    [DataRow(Color.White, DisplayName = "White")]
    [DataRow(Color.Black, DisplayName = "Black")]
    public void QueenConstructorSetsCorrectPieceTypeAndColor(Color color)
    {
        var queen = PieceFactory.GetPiece(PieceType.Queen, color);
        Assert.AreEqual(PieceType.Queen, queen.PieceType);
        Assert.AreEqual(color, queen.Color);
    }

    [TestMethod]
    public void QueenGetPossibleMovesAndAttacksBitBoardsAreTheSameOnEmptyBoard()
    {
        var queen = PieceFactory.GetPiece(PieceType.Queen, Color.White);
        var board = ForsythEdwardsNotation.GenerateBoard("k7/8/8/8/3Q4/8/8/7K w - - 0 1"); // Queen in d4
        var position = new Square("d4");

        ulong possibleMoves = queen.GetPieceMoves(position.BitBoard, board);
        ulong attacks = queen.GetPieceAttacks(position.BitBoard, board);
        ulong expectedMoves = (Board.FileD | Board.Rank4 | new Square("c3").BitBoard |
            new Square("b2").BitBoard | new Square("a1").BitBoard | new Square("e5").BitBoard |
            new Square("f6").BitBoard | new Square("g7").BitBoard | new Square("h8").BitBoard |
            new Square("c5").BitBoard | new Square("b6").BitBoard | new Square("a7").BitBoard |
            new Square("e3").BitBoard | new Square("f2").BitBoard | new Square("g1").BitBoard)
            ^ position.BitBoard;

        Assert.AreEqual(expectedMoves, possibleMoves, "Moves missmatch.");
        Assert.AreEqual(expectedMoves, attacks, "Attacks mismatch.");

        // test that the queen can't move to the same square
        Assert.IsTrue((position.BitBoard & possibleMoves) == 0, "Can't move to same square");
    }

    [TestMethod]
    [DataRow(Color.White, DisplayName = "White")]
    [DataRow(Color.Black, DisplayName = "Black")]
    public void RookConstructorSetsCorrectPieceTypeAndColor(Color color)
    {
        var rook = PieceFactory.GetPiece(PieceType.Rook, color);
        Assert.AreEqual(PieceType.Rook, rook.PieceType);
        Assert.AreEqual(color, rook.Color);
    }

    [TestMethod]
    public void RookGetPossibleMovesAndAttacksBitBoardsAreTheSameOnEmptyBoard()
    {
        var rook = PieceFactory.GetPiece(PieceType.Rook, Color.Black);
        var board = ForsythEdwardsNotation.GenerateBoard("k7/8/8/8/3r4/8/8/7K w - - 0 1"); // rook in d4
        var position = new Square("d4");

        ulong possibleMoves = rook.GetPieceMoves(position.BitBoard, board);
        ulong attacks = rook.GetPieceAttacks(position.BitBoard, board);
        ulong expectedMoves = (Board.FileD | Board.Rank4) ^ position.BitBoard;

        Assert.AreEqual(expectedMoves, possibleMoves, "Moves mismatch");
        Assert.AreEqual(expectedMoves, attacks, "Attacks mismatch.");

        // test that the rook can't move to the same square
        Assert.IsTrue((position.BitBoard & possibleMoves) == 0, "Can't move to same square");
    }

    [TestMethod]
    [DataRow(PieceType.Rook, Color.Black, "a8", DisplayName = "Black Rook")]
    [DataRow(PieceType.Bishop, Color.Black, "c8", DisplayName = "Black Bishop")]
    [DataRow(PieceType.Queen, Color.Black, "d8", DisplayName = "Black Queen")]
    [DataRow(PieceType.King, Color.Black, "e8", DisplayName = "Black King")]
    [DataRow(PieceType.Rook, Color.White, "a1", DisplayName = "White Rook")]
    [DataRow(PieceType.Bishop, Color.White, "c1", DisplayName = "White Bishop")]
    [DataRow(PieceType.Queen, Color.White, "d1", DisplayName = "White Queen")]
    [DataRow(PieceType.King, Color.White, "e1", DisplayName = "White King")]
    public void SomePiecesCannotMoveOnAnInitialBoard(PieceType pieceType, Color pieceColor, string position)
    {
        var board = new Board();
        board.InitializeGameStartingBoard();
        PieceBase piece = PieceFactory.GetPiece(pieceType, pieceColor);
        Square square = new Square(position);
        Assert.AreEqual(0ul, piece.GetPieceMoves(square.BitBoard, board));
    }

    [TestMethod]
    [DataRow(PieceType.Pawn, Color.Black, "p")]
    [DataRow(PieceType.Rook, Color.Black, "r")]
    [DataRow(PieceType.Knight, Color.Black, "n")]
    [DataRow(PieceType.Bishop, Color.Black, "b")]
    [DataRow(PieceType.Queen, Color.Black, "q")]
    [DataRow(PieceType.King, Color.Black, "k")]
    [DataRow(PieceType.Pawn, Color.White, "P")]
    [DataRow(PieceType.Rook, Color.White, "R")]
    [DataRow(PieceType.Knight, Color.White, "N")]
    [DataRow(PieceType.Bishop, Color.White, "B")]
    [DataRow(PieceType.Queen, Color.White, "Q")]
    [DataRow(PieceType.King, Color.White, "K")]
    [DataRow(PieceType.None, Color.None, " ")]
    public void ToStringReturnsCorrectString(PieceType pieceType, Color color, string expected)
    {
        var piece = PieceFactory.GetPiece(pieceType, color);
        Assert.AreEqual(expected, piece.ToString());
    }
}