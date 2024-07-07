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
using System.Text;

namespace Minerva.Tests;

[TestClass]
public class BoardTests : TestBase
{
    [TestMethod]
    public void BishopsTests()
    {
        var board = new Board();
        PieceBase bishop = PieceFactory.GetPiece('b', Color.White);
        foreach (KeyValuePair<ulong, ulong> diagonal in Board.BishopXRay)
        {
            ulong moves = bishop.GetPieceMoves(diagonal.Key, board);
            Assert.AreEqual(moves.ToString("X"), diagonal.Value.ToString("X"));
        }
    }

    [TestMethod]
    public void BlackOrEmptyTests()
    {
        var board = new Board();
        Assert.AreEqual(0xFFFFFFFFFFFFFFFF, board.BlackOrEmpty, "Empty board.");
        board.InitializeGameStartingBoard();
        Assert.AreEqual(
            Board.Rank3 | Board.Rank4 | Board.Rank5 | Board.Rank6 | Board.Rank7 | Board.Rank8,
            board.BlackOrEmpty,
            "Starting board.");
    }

    [TestMethod]
    public void WhiteOrEmptyTests()
    {
        var board = new Board();
        Assert.AreEqual(0xFFFFFFFFFFFFFFFF, board.WhiteOrEmpty, "Empty board.");
        board.InitializeGameStartingBoard();
        Assert.AreEqual(
            Board.Rank1 | Board.Rank2 | Board.Rank3 | Board.Rank4 | Board.Rank5 | Board.Rank6,
            board.WhiteOrEmpty,
            "Starting board.");
    }

    [TestMethod]
    public void DiagonalsTests()
    {
        var board = new Board();
        PieceBase bishop = PieceFactory.GetPiece('b', Color.White);
        foreach (KeyValuePair<ulong, ulong> diagonal in Board.Diagonals)
        {
            ulong positions = bishop.GetPieceMoves(diagonal.Key, board) | diagonal.Key;
            Assert.AreEqual(positions.ToString("X"), diagonal.Value.ToString("X"));
        }
    }

    [TestMethod]
    public void BlackPiecesBitBoardIsCorrectOnEmptyBoardCreation()
    {
        var board = new Board();
        ulong blackPiecesBitBoard = board.BlackPieces.Values.Aggregate((a, b) => a | b);
        Assert.AreEqual(0ul, blackPiecesBitBoard);
    }

    [TestMethod]
    [DataRow("a1", Board.Rank1 & Board.FileA, DisplayName = "a1")]
    [DataRow("a8", Board.Rank8 & Board.FileA, DisplayName = "a8")]
    [DataRow("h1", Board.Rank1 & Board.FileH, DisplayName = "h1")]
    [DataRow("h8", Board.Rank8 & Board.FileH, DisplayName = "h8")]
    [DataRow("A1", Board.Rank1 & Board.FileA, DisplayName = "A1")]
    [DataRow("A8", Board.Rank8 & Board.FileA, DisplayName = "A8")]
    [DataRow("H1", Board.Rank1 & Board.FileH, DisplayName = "H1")]
    [DataRow("H8", Board.Rank8 & Board.FileH, DisplayName = "H8")]
    public void GetSquareReturnsTheRightBitboard(string square, ulong result)
    {
        var board = new Board();
        ulong bitboard = board.GetSquareBitBoard(square);
        Assert.AreEqual(result, bitboard);
    }

    [TestMethod]
    [DataRow(
        "ac3",
        typeof(ArgumentException),
        "Square notation must be 2 characters long. (Parameter 'square')",
        DisplayName = "Wrong lenght square")]
    [DataRow(
        "j3",
        typeof(ArgumentException),
        "Invalid file. (Parameter 'square')",
        DisplayName = "Invalid File")]
    [DataRow(
        "a0",
        typeof(ArgumentException),
        "Invalid rank. (Parameter 'square')",
        DisplayName = "Invalid Rank")]
    public void GetSquareBitBoardThrowsTheRightArgumentExceptions(string square, Type exceptionType, string message)
    {
        var board = new Board();
        ArgumentException exception = Assert.ThrowsException<ArgumentException>(
            () => board.GetSquareBitBoard(square),
            "Wrong exception type.");
        Assert.AreEqual(exceptionType, exception.GetType());
        Assert.AreEqual(message, exception.Message);
    }

    [TestMethod]
    public void KingsTests()
    {
        var board = new Board();
        PieceBase king = PieceFactory.GetPiece('k', Color.White);
        foreach (KeyValuePair<ulong, ulong> kingAttacks in Board.KingXRay)
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
        foreach (KeyValuePair<ulong, ulong> knightAttacks in Board.KnightXRay)
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
        foreach (KeyValuePair<ulong, ulong> pawnAttacks in Board.PawnDefensesXRayBlack)
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
        foreach (KeyValuePair<ulong, ulong> pawnAttacks in Board.PawnDefensesXRayWhite)
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
        foreach (KeyValuePair<ulong, ulong> pawnAttacks in Board.PawnMovesXRayBlack)
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
        foreach (KeyValuePair<ulong, ulong> pawnAttacks in Board.PawnMovesXRayWhite)
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
        foreach (KeyValuePair<ulong, ulong> diagonal in Board.QueenXRay)
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
        foreach (KeyValuePair<ulong, ulong> diagonal in Board.RookXRay)
        {
            ulong moves = rook.GetPieceMoves(diagonal.Key, board);
            Assert.AreEqual(moves.ToString("X"), diagonal.Value.ToString("X"));
        }
    }

    [TestMethod]
    public void GetSquareBitBoardThrowsArgumentNullExceptionWhenSquareIsNull()
    {
        var board = new Board();
        string? nullSquare = null;
        #pragma warning disable CS8604 // Possible null reference argument.
        ArgumentNullException exception = Assert.ThrowsException<ArgumentNullException>(
            () => board.GetSquareBitBoard(nullSquare),
            "Wrong exception type.");
        #pragma warning restore CS8604 // Possible null reference argument.
        Assert.AreEqual("square", exception.ParamName);
        Assert.AreEqual("Value cannot be null. (Parameter 'square')", exception.Message);
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
    [DataRow("H1", 1ul, DisplayName = "H1 - LSB")]
    public void MostSignificanAndLeastSignificantBitSqaresAreSetProperly(string square, ulong expectedBitBoard)
    {
        var board = new Board();
        ulong bitboard = board.GetSquareBitBoard(square);
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
    public void AllBitBoardsAreCorrectAfterCreatingGameStartingBoard()
    {
        var board = new Board();
        board.InitializeGameStartingBoard();

        // Bitboards
        Assert.AreEqual(Board.Rank1 | Board.Rank2, board.WhitePiecesBitBoard, "White Pieces.");
        Assert.AreEqual(Board.Rank8 | Board.Rank7, board.BlackPiecesBitBoard, "Black pieces.");
        Assert.AreEqual(
            Board.Rank1 | Board.Rank2 | Board.Rank7 | Board.Rank8,
            board.OccupiedBitBoard,
            "Occupied squares.");
        
        // Black pieces
        Assert.AreEqual(
            board.GetSquareBitBoard("a8") | board.GetSquareBitBoard("h8"),
            board.BlackPieces['r'],
            "Black rocks");
        Assert.AreEqual(
            board.GetSquareBitBoard("b8") | board.GetSquareBitBoard("g8"),
            board.BlackPieces['n'],
            "Black knights");
        Assert.AreEqual(
            board.GetSquareBitBoard("c8") | board.GetSquareBitBoard("f8"),
            board.BlackPieces['b'],
            "Black bishops");
        Assert.AreEqual(board.GetSquareBitBoard("d8"), board.BlackPieces['q'], "Black queen");
        Assert.AreEqual(board.GetSquareBitBoard("e8"), board.BlackPieces['k'], "Black king");
        Assert.AreEqual(
            board.GetSquareBitBoard("a7") | board.GetSquareBitBoard("b7") | board.GetSquareBitBoard("c7") |
            board.GetSquareBitBoard("d7") | board.GetSquareBitBoard("e7") | board.GetSquareBitBoard("f7") |
            board.GetSquareBitBoard("g7") | board.GetSquareBitBoard("h7"),
            board.BlackPieces['p'],
            "Black pawns");

        // White pieces
        Assert.AreEqual(
            board.GetSquareBitBoard("a1") | board.GetSquareBitBoard("h1"),
            board.WhitePieces['R'],
            "White rocks");
        Assert.AreEqual(
            board.GetSquareBitBoard("b1") | board.GetSquareBitBoard("g1"),
            board.WhitePieces['N'],
            "White knights");
        Assert.AreEqual(
            board.GetSquareBitBoard("c1") | board.GetSquareBitBoard("f1"),
            board.WhitePieces['B'],
            "White bishops");
        Assert.AreEqual(board.GetSquareBitBoard("d1"), board.WhitePieces['Q'], "White queen");
        Assert.AreEqual(board.GetSquareBitBoard("e1"), board.WhitePieces['K'], "White king");
        Assert.AreEqual(
            board.GetSquareBitBoard("a2") | board.GetSquareBitBoard("b2") | board.GetSquareBitBoard("c2") |
            board.GetSquareBitBoard("d2") | board.GetSquareBitBoard("e2") | board.GetSquareBitBoard("f2") |
            board.GetSquareBitBoard("g2") | board.GetSquareBitBoard("h2"),
            board.WhitePieces['P'],
            "White pawns");
    }

    [TestMethod]
    [DataRow('a', 8, 'r', DisplayName = "Black rook at a8")]
    [DataRow('b', 8, 'n', DisplayName = "Black knight at b8")]
    [DataRow('c', 8, 'b', DisplayName = "Black bishop at c8")]
    [DataRow('d', 8, 'q', DisplayName = "Black queen at d8")]
    [DataRow('e', 8, 'k', DisplayName = "Black king at e8")]
    [DataRow('f', 8, 'b', DisplayName = "Black bishop at f8")]
    [DataRow('g', 8, 'n', DisplayName = "Black knight at g8")]
    [DataRow('h', 8, 'r', DisplayName = "Black rook at h8")]
    [DataRow('d', 4, Board.EmptySquare, DisplayName = "Empty square at d4")]
    [DataRow('a', 7, 'p', DisplayName = "Black pawn at a7")]
    [DataRow('a', 2, 'P', DisplayName = "White pawn at a2")]
    [DataRow('a', 1, 'R', DisplayName = "White rook at a1")]
    [DataRow('b', 1, 'N', DisplayName = "White knight at b1")]
    [DataRow('c', 1, 'B', DisplayName = "White bishop at c1")]
    [DataRow('d', 1, 'Q', DisplayName = "White queen at d1")]
    [DataRow('e', 1, 'K', DisplayName = "White king at e1")]
    [DataRow('f', 1, 'B', DisplayName = "White bishop at f1")]
    [DataRow('g', 1, 'N', DisplayName = "White knight at g1")]
    [DataRow('h', 1, 'R', DisplayName = "White rook at h1")]
    public void GetPieceAtReturnsCorrectPiece(char file, int rank, char expectedPiece)
    {
        var board = new Board();
        board.InitializeGameStartingBoard();

        Assert.AreEqual(expectedPiece, board.GetPieceAt(file - 'a' + 1, rank));
        Assert.AreEqual(expectedPiece, board.GetPieceAt($"{file}{rank}"));
        Assert.AreEqual(expectedPiece, board.GetPieceAt(new Square(file, rank)));
        // no need to test bitboard overload, as it is called from all the above tested methods
    }

    [TestMethod]
    public void ContainsColorPieceReturnsCorrectValues()
    {
        var board = new Board();
        board.InitializeGameStartingBoard();

        Assert.IsTrue(board.SquareContainPieceOfColor(new Square("a8"), Color.Black));
        Assert.IsFalse(board.SquareContainPieceOfColor(new Square("a8"), Color.White));
        Assert.IsTrue(board.SquareContainPieceOfColor('b', 7, Color.Black));
        Assert.IsFalse(board.SquareContainPieceOfColor('b', 7, Color.White));
        Assert.IsTrue(board.SquareContainPieceOfColor(new Square("c8"), 'b'));
        Assert.IsFalse(board.SquareContainPieceOfColor(new Square("c8"), 'w'));
        Assert.IsTrue(board.SquareContainPieceOfColor('d', 7, 'b'));
        Assert.IsFalse(board.SquareContainPieceOfColor('d', 7, 'w'));

        Assert.IsTrue(board.SquareContainPieceOfColor(new Square("a1"), Color.White));
        Assert.IsFalse(board.SquareContainPieceOfColor(new Square("a1"), Color.Black));
        Assert.IsTrue(board.SquareContainPieceOfColor('b', 2, Color.White));
        Assert.IsFalse(board.SquareContainPieceOfColor('b', 2, Color.Black));
        Assert.IsTrue(board.SquareContainPieceOfColor(new Square("c1"), 'w'));
        Assert.IsFalse(board.SquareContainPieceOfColor(new Square("c1"), 'b'));
        Assert.IsTrue(board.SquareContainPieceOfColor('d', 2, 'w'));
        Assert.IsFalse(board.SquareContainPieceOfColor('d', 2, 'b'));
    }

    [TestMethod]
    public void ContainsColorPiecesThrowsExceptionIfColorIsInvalid()
    {
        var board = new Board();
        board.InitializeGameStartingBoard();
        Exception exception =
            Assert.ThrowsException<ArgumentException>(() => board.SquareContainPieceOfColor(new Square("a1"), 'x'));
        Assert.AreEqual("Invalid color: x. Valid colors are 'b' or 'w'. (Parameter 'color')", exception.Message);
    }

    [TestMethod]
    public void GetPieceAtReturnsEmptyForEmptySquare()
    {
        var board = new Board();
        board.InitializeGameStartingBoard();

        Assert.AreEqual(Board.EmptySquare, board.GetPieceAt(4, 4)); // Empty square at d4
        Assert.AreEqual(Board.EmptySquare, board.GetPieceAt(5, 5)); // Empty square at e5
    }

    [TestMethod]
    public void GetPieceAtThrowsExceptionForInvalidFile()
    {
        var board = new Board();
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => board.GetPieceAt(0, 1)); // Invalid file
    }

    [TestMethod]
    public void GetPieceAtThrowsExceptionForInvalidRank()
    {
        var board = new Board();
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => board.GetPieceAt(1, 0)); // Invalid rank
    }

    [TestMethod]
    public void IsEmptySquareReturnsCorrectValues()
    {
        var board = new Board();
        board.InitializeGameStartingBoard();

        for (int file = 1; file <= 8; file++)
        {
            for (int rank = 1; rank <= 8; rank++)
            {
                char fileChar = (char)('a' + file - 1); // Convert file from int to char
                if (rank is < 3 or > 6)
                {
                    Assert.IsFalse(board.IsEmptySquare(fileChar, rank), "Not empty square.");
                }
                else
                {
                    Assert.IsTrue(board.IsEmptySquare(new Square(fileChar, rank)), "Empty square.");
                }
            }
        }
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
        board.SetPieceAt(5, 4, 'P'); // set white pawn to e4
        board.SetActiveColor('b');
        board.SetEnPassantTargetSquare("e3");
        Assert.AreEqual("e3", board.EnPassantTargetSquare.ToString(), "White en passant failed.");

        // Test setting the en passant target square to "e5" after black pawn move
        board.SetPieceAt(5,5, 'p');
        board.SetActiveColor('w');
        board.SetEnPassantTargetSquare("e6");
        Assert.AreEqual("e6", board.EnPassantTargetSquare.ToString(), "Black en passant failed.");

        // Test setting the en passant target square to "-"
        board.SetEnPassantTargetSquare("-");
        Assert.AreEqual("-", board.EnPassantTargetSquare.ToString());
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
        board.SetPieceAt(5, 5, 'p');
        Assert.AreEqual(board.GetSquareBitBoard("e5"), board.BlackPieces['p']);

        // Set a white knight at b1
        board.SetPieceAt(2, 1, 'N');
        Assert.AreEqual(board.GetSquareBitBoard("b1"), board.WhitePieces['N']);
    }

    [TestMethod]
    [DataRow(0, 1, 'p', DisplayName = "File to low")]
    [DataRow(9, 1, 'p', DisplayName = "File to high")]
    [DataRow(1, 0, 'p', DisplayName = "Row to low")]
    [DataRow(1, 9, 'p', DisplayName = "Row to High")]
    public void SetPieceAtThrowsHighWithInvalidFileOrRank(int file, int rank, char piece)
    {
        var board = new Board();
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => board.SetPieceAt(file, rank, piece));
    }

    [TestMethod]
    [DataRow(1, 1, 'x', DisplayName = "Invalid black piece.")]
    [DataRow(1, 1, 'X', DisplayName = "Invalid white piece.")]
    public void SetPieceAtThrowsExceptionWithInvalidPiece(int file, int rank, char piece)
    {
        var board = new Board();
        Assert.ThrowsException<ArgumentException>(() => board.SetPieceAt(file, rank, piece));
    }

    [TestMethod]
    [DataRow(Color.White, 'R', DisplayName = "White Rook Attacks")]
    [DataRow(Color.Black, 'r', DisplayName = "Black Rook Attacks")]
    public void GetPieceAttacksReturnsCorrectAttacksForRooks(Color color, char pieceType)
    {
        // Arrange
        var board = new Board();
        board.SetPieceAt(1, 1, pieceType); // Place rook at a1
        board.SetPieceAt(8, 8, pieceType); // Place rook at h8
        var pieceBase = PieceFactory.GetPiece(pieceType, color);

        // Act
        ulong attacks = board.GetPieceDefenses(pieceBase);

        // Assert
        // Assuming a method to calculate expected attacks for a rook at a1
        ulong expectedAttacks = (Board.Rank1 | Board.Rank8 | Board.FileA | Board.FileH) &
            ~(Board.Rank1 & Board.FileA | Board.Rank8 & Board.FileH);
        Assert.AreEqual(expectedAttacks, attacks, "Rook attacks did not match expected attacks.");
    }
}