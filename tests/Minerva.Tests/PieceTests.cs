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
        PieceBase bishop = PieceFactory.GetPiece(PieceType.Bishop, Color.White);
        Board board = ForsythEdwardsNotation.GenerateBoard("k7/8/8/8/3B4/8/8/7K w - - 0 1"); // Bishop in d4
        string position = "d4";
        ulong bitBoard = BitBoards.Squares[position];

        ulong possibleMoves = bishop.GetPieceMoves(bitBoard, board);
        ulong attacks = bishop.GetPieceAttacks(bitBoard, board);
        ulong exptectedMoves = BitBoards.Squares["c3"] | BitBoards.Squares["b2"] |
            BitBoards.Squares["a1"] | BitBoards.Squares["e5"] | BitBoards.Squares["f6"] |
            BitBoards.Squares["g7"] | BitBoards.Squares["h8"] | BitBoards.Squares["c5"] |
            BitBoards.Squares["b6"] | BitBoards.Squares["a7"] | BitBoards.Squares["e3"] |
            BitBoards.Squares["f2"] | BitBoards.Squares["g1"];

        Assert.AreEqual(exptectedMoves, possibleMoves, "Moves missmatch.");
        Assert.AreEqual(exptectedMoves, attacks, "Attacks missmatch.");

        // test that the bishop can't move to the same square
        Assert.IsTrue((bitBoard & possibleMoves) == 0, "Can't move to same square");
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
        PieceBase king = PieceFactory.GetPiece(PieceType.King, Color.White);
        Board board = ForsythEdwardsNotation.GenerateBoard("k7/8/8/8/3K4/8/8/8 w - - 0 1"); // King in d4
        ulong position = BitBoards.Squares["d4"];

        ulong possibleMoves = king.GetPieceMoves(position, board);
        ulong attacks = king.GetPieceAttacks(position, board);
        ulong expectedMoves = BitBoards.Squares["c3"] | BitBoards.Squares["d3"] |
            BitBoards.Squares["e3"] | BitBoards.Squares["c4"] | BitBoards.Squares["e4"] |
            BitBoards.Squares["c5"] | BitBoards.Squares["d5"] | BitBoards.Squares["e5"];

        Assert.AreEqual(expectedMoves, possibleMoves, "Moves missmatch.");
        Assert.AreEqual(expectedMoves, attacks, "Attacks mismatch.");

        // test that the king can't move to the same square
        Assert.IsTrue((position & possibleMoves) == 0, "Can't move to same square");
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
        var position = BitBoards.Squares["d4"];

        ulong possibleMoves = knight.GetPieceMoves(position, board);
        ulong attacks = knight.GetPieceAttacks(position, board);
        ulong expectedMoves = BitBoards.Squares["b3"] | BitBoards.Squares["b5"] |
            BitBoards.Squares["c2"] | BitBoards.Squares["c6"] | BitBoards.Squares["e2"] |
            BitBoards.Squares["e6"] | BitBoards.Squares["f3"] | BitBoards.Squares["f5"];

        Assert.AreEqual(expectedMoves, possibleMoves, "Moves missmatch.");
        Assert.AreEqual(expectedMoves, attacks, "Attacks mismatch.");

        // test that the knight can't move to the same square
        Assert.IsTrue((position & possibleMoves) == 0, "Can't move to same square");
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
        var position = BitBoards.Squares["d2"];

    ulong attacks = pawn.GetPieceAttacks(position, board);
        ulong expectedMoves = BitBoards.Squares["c3"] | BitBoards.Squares["e3"];

        Assert.AreEqual(expectedMoves, attacks, "Attacks missmatch.");

        // test that the pawn can't move to the same square
        Assert.IsTrue((position & attacks) == 0, "Can't move to same square");
    }

    [TestMethod]
    public void PawnGetPossibleMovesBitBoardReturnsCorrectMovesInEmptyBoard()
    {
        var pawn = PieceFactory.GetPiece(PieceType.Pawn, Color.White);
        var board = ForsythEdwardsNotation.GenerateBoard("k7/8/8/8/8/8/3P4/7K w - - 0 1"); // Knigth in d4
        var position = BitBoards.Squares["d2"];

        ulong possibleMoves = pawn.GetPieceMoves(position, board);
        ulong expectedMoves = BitBoards.Squares["d3"] | BitBoards.Squares["d4"];

        Assert.AreEqual(expectedMoves, possibleMoves, "Moves missmatch.");

        // test that the pawn can't move to the same square
        Assert.IsTrue((position & possibleMoves) == 0, "Can't move to same square");
    }

    [TestMethod]
    [DataRow("Black bishop", DisplayName = "Black Bishop")]
    [DataRow("Black king", DisplayName = "Black King")]
    [DataRow("Black knight", DisplayName = "Black Knight")]
    [DataRow("Black pawn", DisplayName = "Black Pawn")]
    [DataRow("Black queen", DisplayName = "Black Queen")]
    [DataRow("Black rook", DisplayName = "Black Rook")]
    [DataRow("White bishop", DisplayName = "White Bishop")]
    [DataRow("White king", DisplayName = "White King")]
    [DataRow("White knight", DisplayName = "White Knight")]
    [DataRow("White pawn", DisplayName = "White Pawn")]
    [DataRow("White queen", DisplayName = "White Queen")]
    [DataRow("White rook", DisplayName = "White Rook")]
    public void PieceAttacksAreCorrectInInitialBoard(string name)
    {
        var scenarios = new[]
        {
            new
            {
                Name = "Black bishop",
                Piece = PieceFactory.GetPiece(PieceType.Bishop, Color.Black),
                Position = BitBoards.Squares["c8"],
                ExpectedAttacks = BitBoards.Squares["b7"] | BitBoards.Squares["d7"],
            },
            new
            {
                Name = "Black king",
                Piece = PieceFactory.GetPiece(PieceType.King, Color.Black),
                Position = BitBoards.Squares["e8"],
                ExpectedAttacks = BitBoards.Squares["d7"] | BitBoards.Squares["d8"] |
                    BitBoards.Squares["e7"] | BitBoards.Squares["f7"] |
                    BitBoards.Squares["f8"],
            },
            new
            {
                Name = "Black knight",
                Piece = PieceFactory.GetPiece(PieceType.Knight, Color.Black),
                Position = BitBoards.Squares["b8"],
                ExpectedAttacks = BitBoards.Squares["a6"] | 
                    BitBoards.Squares["c6"] |
                    BitBoards.Squares["d7"],
            },
            new
            {
                Name = "Black queen",
                Piece = PieceFactory.GetPiece(PieceType.Queen, Color.Black),
                Position = BitBoards.Squares["d8"],
                ExpectedAttacks = BitBoards.Squares["c7"] | BitBoards.Squares["c8"] |
                    BitBoards.Squares["d7"] | BitBoards.Squares["e7"] |
                    BitBoards.Squares["e8"],
            },
            new
            {
                Name = "Black rook",
                Piece = PieceFactory.GetPiece(PieceType.Rook, Color.Black),
                Position = BitBoards.Squares["a8"],
                ExpectedAttacks = BitBoards.Squares["a7"] | BitBoards.Squares["b8"],
            },
            new
            {
                Name = "File A black pawn",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.Black),
                Position = BitBoards.Squares["a7"],
                ExpectedAttacks = BitBoards.Squares["b6"],
            },
            new
            {
                Name = "File A white pawn",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.White),
                Position = BitBoards.Squares["a2"],
                ExpectedAttacks = BitBoards.Squares["b3"],
            },
            new
            {
                Name = "File B black pawn",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.Black),
                Position = BitBoards.Squares["b7"],
                ExpectedAttacks = BitBoards.Squares["a6"] | BitBoards.Squares["c6"],
            },
            new
            {
                Name = "File B white pawn",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.White),
                Position = BitBoards.Squares["b2"],
                ExpectedAttacks = BitBoards.Squares["a3"] | BitBoards.Squares["c3"],
            },
            new
            {
                Name = "File H black pawn",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.Black),
                Position = BitBoards.Squares["h7"],
                ExpectedAttacks = BitBoards.Squares["g6"],
            },
            new
            {
                Name = "File H white pawn",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.White),
                Position = BitBoards.Squares["h2"],
                ExpectedAttacks = BitBoards.Squares["g3"],
            },
            new
            {
                Name = "White bishop",
                Piece = PieceFactory.GetPiece(PieceType.Bishop, Color.White),
                Position = BitBoards.Squares["c1"],
                ExpectedAttacks = BitBoards.Squares["b2"] | BitBoards.Squares["d2"],
            },
            new
            {
                Name = "White king",
                Piece = PieceFactory.GetPiece(PieceType.King, Color.White),
                Position = BitBoards.Squares["e1"],
                ExpectedAttacks = BitBoards.Squares["d1"] | BitBoards.Squares["d2"] |
                    BitBoards.Squares["e2"] | BitBoards.Squares["f1"] |
                    BitBoards.Squares["f2"],
            },
            new
            {
                Name = "White knight",
                Piece = PieceFactory.GetPiece(PieceType.Knight, Color.White),
                Position = BitBoards.Squares["b1"],
                ExpectedAttacks = BitBoards.Squares["a3"] |
                    BitBoards.Squares["c3"] |
                    BitBoards.Squares["d2"],
            },
            new
            {
                Name = "White queen",
                Piece = PieceFactory.GetPiece(PieceType.Queen, Color.White),
                Position = BitBoards.Squares["d1"],
                ExpectedAttacks = BitBoards.Squares["c1"] | BitBoards.Squares["c2"] |
                    BitBoards.Squares["d2"] | BitBoards.Squares["e1"] |
                    BitBoards.Squares["e2"],
            },
            new
            {
                Name = "White rook",
                Piece = PieceFactory.GetPiece(PieceType.Rook, Color.White),
                Position = BitBoards.Squares["a1"],
                ExpectedAttacks = BitBoards.Squares["a2"] | BitBoards.Squares["b1"],
            },
        };

        foreach (var scenario in scenarios.Where(s=>s.Name.Equals(name)))
        {
            Console.WriteLine($"Starting: {scenario.Name}.");
            Board board = new Board();
            board.InitializeGameStartingBoard();
            ulong attacks = scenario.Piece.GetPieceAttacks(scenario.Position, board);
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
    [DataRow(
        "White queen can take black pieces, but not white ones",
        DisplayName = "White queen can take black pieces, but not white ones")]
    [DataRow(
        "Black queen can take black pieces, but not black ones",
        DisplayName = "Black queen can take black pieces, but not black ones")]
    [DataRow(
        "White king can take black pieces, but not white ones",
        DisplayName = "White king can take black pieces, but not white ones")]
    [DataRow(
        "Black king can take black pieces, but not white ones",
        DisplayName = "Black king can take black pieces, but not white ones")]
    [DataRow(
        "White knight can take black pieces, but not white ones",
        DisplayName = "White knight can take black pieces, but not white ones")]
    [DataRow(
        "Black knight can take white pieces, but not black ones",
        DisplayName = "Black knight can take white pieces, but not black ones")]
    [DataRow(
        "White pawn can take black pieces, but not white ones",
        DisplayName = "White pawn can take black pieces, but not white ones")]
    [DataRow(
        "Black pawn can take white pieces, but not black ones",
        DisplayName = "Black pawn can take white pieces, but not black ones")]
    [DataRow(
        "White pawn can take black pieces, but not white ones, opposite side",
        DisplayName = "White pawn can take black pieces, but not white ones, opposite side")]
    [DataRow(
        "Black pawn can take white pieces, but not black ones, opposite side",
        DisplayName = "Black pawn can take white pieces, but not black ones, opposite side")]
    [DataRow("White pawn can take black pawn en passant", DisplayName = "White pawn can take black pawn en passant")]
    [DataRow("Black pawn can take white pawn en passant", DisplayName = "Black pawn can take white pawn en passant")]
    public void PiecesCanOnlyTakeOpponentPieces(string name)
    {
        var scenarios = new[]
        {
            new
            {
                Name = "White queen can take black pieces, but not white ones",
                Fen = "3k4/8/8/8/5K2/8/qB6/Qn6 w - - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.Queen, Color.White),
                Position = BitBoards.Squares["a1"],
                ExpectedMoves =
                    BitBoards.Squares["a2"] |
                    BitBoards.Squares["b1"],
            },
            new
            {
                Name = "Black queen can take black pieces, but not black ones",
                Fen = "3K4/8/8/8/5k2/8/Qb6/qN6 b - - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.Queen, Color.Black),
                Position = BitBoards.Squares["a1"],
                ExpectedMoves =
                    BitBoards.Squares["a2"] |
                    BitBoards.Squares["b1"],
            },
            new
            {
                Name = "White king can take black pieces, but not white ones",
                Fen = "3k4/8/8/8/5Q2/8/qB6/Kn6 w - - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.King, Color.White),
                Position = BitBoards.Squares["a1"],
                ExpectedMoves =
                    BitBoards.Squares["a2"] |
                    BitBoards.Squares["b1"],
            },
            new
            {
                Name = "Black king can take black pieces, but not white ones",
                Fen = "3K4/8/8/8/5q2/8/Qb6/kN6 b - - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.King, Color.Black),
                Position = BitBoards.Squares["a1"],
                ExpectedMoves =
                    BitBoards.Squares["a2"] |
                    BitBoards.Squares["b1"],
            },
            new
            {
                Name = "White knight can take black pieces, but not white ones",
                Fen = "rnbqkbnr/pppppppp/8/2N5/8/1P1P4/P1P1PPPP/R1BQKBNR w KQkq - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.Knight, Color.White),
                Position = BitBoards.Squares["c5"],
                ExpectedMoves =
                    BitBoards.Squares["a4"] |
                    BitBoards.Squares["a6"] |
                    BitBoards.Squares["b7"] |
                    BitBoards.Squares["d7"] |
                    BitBoards.Squares["e4"] |
                    BitBoards.Squares["e6"],
            },
            new
            {
                Name = "Black knight can take white pieces, but not black ones",
                Fen = "r1bqkbnr/pppppppp/8/2n5/8/1P1P4/P1P1PPPP/RNBQKBNR b KQkq - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.Knight, Color.Black),
                Position = BitBoards.Squares["c5"],
                ExpectedMoves =
                    BitBoards.Squares["a4"] |
                    BitBoards.Squares["a6"] |
                    BitBoards.Squares["b3"] |
                    BitBoards.Squares["d3"] |
                    BitBoards.Squares["e4"] |
                    BitBoards.Squares["e6"],
            },
            new
            {
                Name = "White pawn can take black pieces, but not white ones",
                Fen = "rnbqkbnr/ppp1pppp/8/8/8/3p1N2/PPPPPPPP/RNBQKB1R w KQkq - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.White),
                Position = BitBoards.Squares["e2"],
                ExpectedMoves =
                    BitBoards.Squares["d3"] |
                    BitBoards.Squares["e3"] |
                    BitBoards.Squares["e4"],
            },
            new
            {
                Name = "Black pawn can take white pieces, but not black ones",
                Fen = "rnbqkb1r/pppppppp/3P1n2/8/8/8/PPP1PPPP/RNBQKBNR b KQkq - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.Black),
                Position = BitBoards.Squares["e7"],
                ExpectedMoves =
                    BitBoards.Squares["d6"] |
                    BitBoards.Squares["e6"] |
                    BitBoards.Squares["e5"],
            },
            new
            {
                Name = "White pawn can take black pieces, but not white ones, opposite side",
                Fen = "rnbqkbnr/pppp1ppp/8/8/8/2N1p3/PPPPPPPP/R1BQKBNR w KQkq - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.White),
                Position = BitBoards.Squares["d2"],
                ExpectedMoves =
                    BitBoards.Squares["d3"] |
                    BitBoards.Squares["d4"] |
                    BitBoards.Squares["e3"],
            },
            new
            {
                Name = "Black pawn can take white pieces, but not black ones, opposite side",
                Fen = "r1bqkbnr/pppppppp/2n1P3/8/8/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.Black),
                Position = BitBoards.Squares["d7"],
                ExpectedMoves =
                    BitBoards.Squares["d5"] |
                    BitBoards.Squares["d6"] |
                    BitBoards.Squares["e6"],
            },
            new
            {
                Name = "White pawn can take black pawn en passant",
                Fen = "rnbqkbnr/pppp1ppp/8/3Pp3/4P3/8/PPP2PPP/RNBQKBNR w KQkq e6 0 2",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.White),
                Position = BitBoards.Squares["d5"],
                ExpectedMoves = BitBoards.Squares["e6"] | BitBoards.Squares["d6"],
            },
            new
            {
                Name = "Black pawn can take white pawn en passant",
                Fen = "rnbqkbnr/pp1ppppp/8/8/2pPPP2/8/PPP3PP/RNBQKBNR b KQkq d3 0 3",
                Piece = PieceFactory.GetPiece(PieceType.Pawn, Color.Black),
                Position = BitBoards.Squares["c4"],
                ExpectedMoves = BitBoards.Squares["c3"] | BitBoards.Squares["d3"],
            },
        };

        foreach (var scenario in scenarios.Where(s => s.Name.Equals(name)))
        {
            Console.WriteLine($"Starting: {scenario.Name}.");
            Board board = ForsythEdwardsNotation.GenerateBoard(scenario.Fen);
            ulong possibleMoves = scenario.Piece.GetPieceMoves(scenario.Position, board);
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
        var position = BitBoards.Squares["d4"];

        ulong possibleMoves = queen.GetPieceMoves(position, board);
        ulong attacks = queen.GetPieceAttacks(position, board);
        ulong expectedMoves = (BitBoards.FileD | BitBoards.Rank4 | BitBoards.Squares["c3"] |
            BitBoards.Squares["b2"] | BitBoards.Squares["a1"] | BitBoards.Squares["e5"] |
            BitBoards.Squares["f6"] | BitBoards.Squares["g7"] | BitBoards.Squares["h8"] |
            BitBoards.Squares["c5"] | BitBoards.Squares["b6"] | BitBoards.Squares["a7"] |
            BitBoards.Squares["e3"] | BitBoards.Squares["f2"] | BitBoards.Squares["g1"])
            ^ position;

        Assert.AreEqual(expectedMoves, possibleMoves, "Moves missmatch.");
        Assert.AreEqual(expectedMoves, attacks, "Attacks mismatch.");

        // test that the queen can't move to the same square
        Assert.IsTrue((position & possibleMoves) == 0, "Can't move to same square");
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
        var position = BitBoards.Squares["d4"];

        ulong possibleMoves = rook.GetPieceMoves(position, board);
        ulong attacks = rook.GetPieceAttacks(position, board);
        ulong expectedMoves = (BitBoards.FileD | BitBoards.Rank4) ^ position;

        Assert.AreEqual(expectedMoves, possibleMoves, "Moves mismatch");
        Assert.AreEqual(expectedMoves, attacks, "Attacks mismatch.");

        // test that the rook can't move to the same square
        Assert.IsTrue((position & possibleMoves) == 0, "Can't move to same square");
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
        ulong square = BitBoards.Squares[position];
        Assert.AreEqual(0ul, piece.GetPieceMoves(square, board));
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