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

namespace Minerva.Tests;

[TestClass]
public class BoardTests : TestBase
{
    [TestMethod]
    public void BlackPiecesBitBoardIsCorrectOnEmptyBoardCreation()
    {
        var board = new Board();
        ulong blackPiecesBitBoard = board.BlackPieces.Values.Aggregate((a, b) => a | b);
        Assert.AreEqual(0ul, blackPiecesBitBoard);
    }

    [TestMethod]
    public void KingsTests()
    {
        var board = new Board();
        PieceBase king = PieceFactory.GetPiece('k', Color.White);
        foreach (KeyValuePair<ulong, ulong> kingAttacks in BitBoards.King)
        {
            var attacks = king.GetPieceMoves(kingAttacks.Key, board);
            Assert.AreEqual(attacks.ToString("X"), kingAttacks.Value.ToString("X"));
        }
    }

    [TestMethod]
    public void KnightsTests()
    {
        var board = new Board();
        PieceBase knight = PieceFactory.GetPiece('n', Color.White);
        foreach (KeyValuePair<ulong, ulong> knightAttacks in BitBoards.Knight)
        {
            ulong moves = knight.GetPieceMoves(knightAttacks.Key, board);
            Assert.AreEqual(moves.ToString("X"), knightAttacks.Value.ToString("X"));
        }
    }

    [TestMethod]
    public void PawnsAttacksBlackTests()
    {
        var board = new Board();
        PieceBase pawn = PieceFactory.GetPiece('p', Color.Black);
        foreach (KeyValuePair<ulong, ulong> pawnAttacks in BitBoards.PawnDefendedBlack)
        {
            ulong attacks = pawn.GetPieceAttacks(pawnAttacks.Key, board);
            Assert.AreEqual(attacks.ToString("X"), pawnAttacks.Value.ToString("X"));
        }
    }

    [TestMethod]
    public void PawnsAttacksWhiteTests()
    {
        var board = new Board();
        PieceBase pawn = PieceFactory.GetPiece('p', Color.White);
        foreach (KeyValuePair<ulong, ulong> pawnAttacks in BitBoards.PawnDefendedWhite)
        {
            ulong attacks = pawn.GetPieceAttacks(pawnAttacks.Key, board);
            Assert.AreEqual(attacks.ToString("X"), pawnAttacks.Value.ToString("X"));
        }
    }

    [TestMethod]
    public void PawnsMovesBlackTests()
    {
        var board = new Board();
        PieceBase pawn = PieceFactory.GetPiece('p', Color.Black);
        foreach (KeyValuePair<ulong, ulong> pawnAttacks in BitBoards.PawnMovesBlack)
        {
            ulong moves = pawn.GetPieceMoves(pawnAttacks.Key, board);
            Assert.AreEqual(moves.ToString("X"), pawnAttacks.Value.ToString("X"));
        }
    }

    [TestMethod]
    public void PawnsMovesWhiteTests()
    {
        var board = new Board();
        PieceBase pawn = PieceFactory.GetPiece('p', Color.White);
        foreach (KeyValuePair<ulong, ulong> pawnAttacks in BitBoards.PawnMovesWhite)
        {
            ulong moves = pawn.GetPieceMoves(pawnAttacks.Key, board);
            Assert.AreEqual(moves.ToString("X"), pawnAttacks.Value.ToString("X"));
        }
    }

    [TestMethod]
    public void QueensTests()
    {
        var board = new Board();
        PieceBase queen = PieceFactory.GetPiece('q', Color.White);
        foreach (KeyValuePair<ulong, ulong> diagonal in BitBoards.Queen)
        {
            ulong moves = queen.GetPieceMoves(diagonal.Key, board);
            Assert.AreEqual(moves.ToString("X"), diagonal.Value.ToString("X"));
        }
    }

    [TestMethod]
    public void RooksTests()
    {
        var board = new Board();
        PieceBase rook = PieceFactory.GetPiece('r', Color.White);
        foreach (KeyValuePair<ulong, ulong> diagonal in BitBoards.Rook)
        {
            ulong moves = rook.GetPieceMoves(diagonal.Key, board);
            Assert.AreEqual(moves.ToString("X"), diagonal.Value.ToString("X"));
        }
    }

    [TestMethod]
    [DataRow("3K4/8/8/8/5q2/8/Qb6/kN6 b - - 0 1", true)]
    [DataRow("8/8/8/4k3/2K2q2/8/Qb6/1N6 w - - 0 1", true)]
    public void IsCheckTests(string fen, bool expected)
    {
        Board board = ForsythEdwardsNotation.GenerateBoard(fen);
        Assert.AreEqual(expected, board.Check);
    }

    [TestMethod]
    [DataRow("a8", 1ul << 63, DisplayName = "a8 - MSB")]
    [DataRow("h1", 1ul, DisplayName = "H1 - LSB")]
    public void MostSignificanAndLeastSignificantBitSqaresAreSetProperly(string square, ulong expectedBitBoard)
    {
        var board = new Board();
        ulong bitboard = BitBoards.Squares[square];
        Assert.AreEqual(expectedBitBoard, bitboard);
    }

    [TestMethod]
    public void WhitePiecesBitBoardIsCorrectOnEmptyBoardCreation()
    {
        var board = new Board();
        ulong whitePiecesBitBoard = board.WhitePieces.Values.Aggregate((a, b) => a | b);
        Assert.AreEqual(0ul, whitePiecesBitBoard);
    }

    [TestMethod]
    public void OccupiedBitBoardIsCorrectOnEmptyBoardCreation()
    {
        var board = new Board();
        ulong occupiedBitBoard = board.OccupiedBitBoard;
        Assert.AreEqual(0ul, occupiedBitBoard);
    }

    [TestMethod]
    public void BlackPiecesBitBoardIsCorrectAfterSettingBlackPieces()
    {
        var board = new Board { BlackPieces = { ['p'] = 1ul } };
        ulong blackPiecesBitBoard = board.BlackPieces.Values.Aggregate((a, b) => a | b);
        Assert.AreEqual(1ul, blackPiecesBitBoard);
    }

    [TestMethod]
    public void SetActiveColorSetsTheRightColor()
    {
        var board = new Board();

        // Test setting the active color to white
        board.SetActiveColor('w');
        Assert.AreEqual('w', board.ActiveColor);

        // Test setting the active color to black
        board.SetActiveColor('b');
        Assert.AreEqual('b', board.ActiveColor);
    }

    [TestMethod]
    [DataRow('a', DisplayName = "Invalid color a")]
    [DataRow('z', DisplayName = "Invalid color z")]
    [DataRow('W', DisplayName = "Invalid color W")]
    [DataRow('B', DisplayName = "Invalid color B")]
    public void SetActiveColorThrowsExceptionWithInvalidColor(char color)
    {
        var board = new Board();
        Exception exception = Assert.ThrowsException<ArgumentException>(() => board.SetActiveColor(color));
        Assert.AreEqual("Invalid active color. (Parameter 'activeColor')", exception.Message);
    }

    [TestMethod]
    public void SetCastlingRightsSetsTheRightRights()
    {
        var board = new Board();

        // Test setting the castling rights to "KQkq"
        board.SetCastlingRights("KQkq");
        Assert.AreEqual("KQkq", board.CastlingRights);

        // Test setting the castling rights to "-"
        board.SetCastlingRights("-");
        Assert.AreEqual("-", board.CastlingRights);
    }

    [TestMethod]
    [DataRow("KQk", DisplayName = "Valid castling rights KQk")]
    [DataRow("q", DisplayName = "Valid castling rights q")]
    [DataRow("Kk", DisplayName = "Valid castling rights Kk")]
    public void SetCastlingRightsDoesNotThrowWithValidRights(string castlingRights)
    {
        var board = new Board();
        board.SetCastlingRights(castlingRights);
        Assert.AreEqual(castlingRights, board.CastlingRights);
    }

    [TestMethod]
    [DataRow("KQkqx", DisplayName = "Invalid castling rights KQkqx")]
    [DataRow("KQkqK", DisplayName = "Invalid castling rights KQkqK")]
    [DataRow("KQkqkq", DisplayName = "Invalid castling rights KQkqkq")]
    public void SetCastlingRightsThrowsExceptionWithInvalidRights(string castlingRights)
    {
        var board = new Board();
        Assert.ThrowsException<ArgumentException>(() => board.SetCastlingRights(castlingRights));
    }

    [TestMethod]
    public void SetEnPassantTargetSquareSetsTheRightSquare()
    {
        var board = new Board();

        // Test setting the en passant target square to "e3" after black pawn move
        board.SetPieceAt(BitBoards.Squares["e4"], 'P');
        board.SetActiveColor('b');
        board.SetEnPassantTargetSquare("e3");
        Assert.AreEqual("e3", board.EnPassantTargetSquare, "White en passant failed.");

        // Test setting the en passant target square to "e5" after black pawn move
        board.SetPieceAt(BitBoards.Squares["e5"], 'p');
        board.SetActiveColor('w');
        board.SetEnPassantTargetSquare("e6");
        Assert.AreEqual("e6", board.EnPassantTargetSquare, "Black en passant failed.");

        // Test setting the en passant target square to "-"
        board.SetEnPassantTargetSquare("-");
        Assert.AreEqual("-", board.EnPassantTargetSquare);
    }

    [TestMethod]
    [DataRow("i3", DisplayName = "Invalid file i")]
    [DataRow("e9", DisplayName = "Invalid rank 9")]
    [DataRow("e1", DisplayName = "Invalid rank 1")]
    [DataRow("e7", DisplayName = "Invalid rank 7")]
    public void SetEnPassantTargetSquareThrowsExceptionWithInvalidSquare(string square)
    {
        var board = new Board();
        Assert.ThrowsException<ArgumentException>(() => board.SetEnPassantTargetSquare(square));
    }

    [TestMethod]
    public void SetEnPassantTargetSquareThrowsExceptionWhenNoPawnToBeTakenEnPassant()
    {
        var board = new Board();
        board.InitializeGameStartingBoard();
        board.SetActiveColor('w');
        Assert.ThrowsException<ArgumentException>(() => board.SetEnPassantTargetSquare("e3"));
    }

    [TestMethod]
    public void SetFullmoveNumberSetsTheRightNumber()
    {
        var board = new Board();
        board.SetFullmoveNumber(5);
        Assert.AreEqual(5, board.FullmoveNumber);
    }

    [TestMethod]
    [DataRow(1, DisplayName = "Valid fullmove number 1")]
    [DataRow(10, DisplayName = "Valid fullmove number 10")]
    [DataRow(100, DisplayName = "Valid fullmove number 100")]
    public void SetFullmoveNumberDoesNotThrowWithValidNumber(int fullmoveNumber)
    {
        var board = new Board();
        board.SetFullmoveNumber(fullmoveNumber);
        Assert.AreEqual(fullmoveNumber, board.FullmoveNumber);
    }

    [TestMethod]
    [DataRow(0, DisplayName = "Invalid fullmove number 0")]
    [DataRow(-1, DisplayName = "Invalid fullmove number -1")]
    [DataRow(-10, DisplayName = "Invalid fullmove number -10")]
    public void SetFullmoveNumberThrowsExceptionWithInvalidNumber(int fullmoveNumber)
    {
        var board = new Board();
        Exception exception =
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => board.SetFullmoveNumber(fullmoveNumber));
        Assert.AreEqual("Fullmove number must be 1 or greater. (Parameter 'fullmoveNumber')", exception.Message);
    }

    [TestMethod]
    [DataRow(0, false, DisplayName = "Valid halfmove clock 0")]
    [DataRow(25, false, DisplayName = "Valid halfmove clock 25")]
    [DataRow(50, false, DisplayName = "Valid halfmove clock 50")]
    [DataRow(-1, true, DisplayName = "Invalid halfmove clock -1")]
    [DataRow(51, true, DisplayName = "Invalid halfmove clock 51")]
    public void SetHalfmoveClockTests(int halfmoveClock, bool outOfRange)
    {
        var board = new Board();
        if (outOfRange)
        {
            Exception exception =
                Assert.ThrowsException<ArgumentOutOfRangeException>(() => board.SetHalfmoveClock(halfmoveClock));
            Assert.AreEqual("Halfmove clock must be between 0 and 50. (Parameter 'halfmoveClock')", exception.Message);
            return;
        }

        board.SetHalfmoveClock(halfmoveClock);
        Assert.AreEqual(halfmoveClock, board.HalfmoveClock);
    }

    [TestMethod]
    public void SetPieceAtSetsTheRightPieceAtTheRightPlace()
    {
        var board = new Board();

        // Set a black pawn at e5
        board.SetPieceAt(BitBoards.Squares["e5"], 'p');
        Assert.AreEqual(BitBoards.Squares["e5"], board.BlackPieces['p']);

        // Set a white knight at b1
        board.SetPieceAt(BitBoards.Squares["b1"], 'N');
        Assert.AreEqual(BitBoards.Squares["b1"], board.WhitePieces['N']);
    }
}