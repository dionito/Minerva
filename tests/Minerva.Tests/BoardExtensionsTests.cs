using Minerva.Extensions;
using Minerva.Pieces;

namespace Minerva.Tests;

public class BoardExtensionsTests
{
    [TestMethod]
    public void AllBitBoardsAreCorrectAfterCreatingGameStartingBoard()
    {
        var board = new Board();
        board.InitializeGameStartingBoard();

        // Bitboards
        Assert.AreEqual(BitBoards.Rank1 | BitBoards.Rank2, board.WhitePiecesBitBoard, "White Pieces.");
        Assert.AreEqual(BitBoards.Rank8 | BitBoards.Rank7, board.BlackPiecesBitBoard, "Black pieces.");
        Assert.AreEqual(
            BitBoards.Rank1 | BitBoards.Rank2 | BitBoards.Rank7 | BitBoards.Rank8,
            board.OccupiedBitBoard,
            "Occupied squares.");
        
        // Black pieces
        Assert.AreEqual(
            BitBoards.Squares["a8"] | BitBoards.Squares["h8"],
            board.BlackPieces['r'],
            "Black rocks");
        Assert.AreEqual(
            BitBoards.Squares["b8"] | BitBoards.Squares["g8"],
            board.BlackPieces['n'],
            "Black knights");
        Assert.AreEqual(
            BitBoards.Squares["c8"] | BitBoards.Squares["f8"],
            board.BlackPieces['b'],
            "Black bishops");
        Assert.AreEqual(BitBoards.Squares["d8"], board.BlackPieces['q'], "Black queen");
        Assert.AreEqual(BitBoards.Squares["e8"], board.BlackPieces['k'], "Black king");
        Assert.AreEqual(
            BitBoards.Squares["a7"] | BitBoards.Squares["b7"] | BitBoards.Squares["c7"] |
            BitBoards.Squares["d7"] | BitBoards.Squares["e7"] | BitBoards.Squares["f7"] |
            BitBoards.Squares["g7"] | BitBoards.Squares["h7"],
            board.BlackPieces['p'],
            "Black pawns");

        // White pieces
        Assert.AreEqual(
            BitBoards.Squares["a1"] | BitBoards.Squares["h1"],
            board.WhitePieces['R'],
            "White rocks");
        Assert.AreEqual(
            BitBoards.Squares["b1"] | BitBoards.Squares["g1"],
            board.WhitePieces['N'],
            "White knights");
        Assert.AreEqual(
            BitBoards.Squares["c1"] | BitBoards.Squares["f1"],
            board.WhitePieces['B'],
            "White bishops");
        Assert.AreEqual(BitBoards.Squares["d1"], board.WhitePieces['Q'], "White queen");
        Assert.AreEqual(BitBoards.Squares["e1"], board.WhitePieces['K'], "White king");
        Assert.AreEqual(
            BitBoards.Squares["a2"] | BitBoards.Squares["b2"] | BitBoards.Squares["c2"] |
            BitBoards.Squares["d2"] | BitBoards.Squares["e2"] | BitBoards.Squares["f2"] |
            BitBoards.Squares["g2"] | BitBoards.Squares["h2"],
            board.WhitePieces['P'],
            "White pawns");
    }

    [TestMethod]
    public void BlackOrEmptyTests()
    {
        var board = new Board();
        Assert.AreEqual(0xFFFFFFFFFFFFFFFF, board.BlackOrEmpty(), "Empty board.");
        board.InitializeGameStartingBoard();
        Assert.AreEqual(
            BitBoards.Rank3 | BitBoards.Rank4 | BitBoards.Rank5 | BitBoards.Rank6 | BitBoards.Rank7 | BitBoards.Rank8,
            board.BlackOrEmpty(),
            "Starting board.");
    }

    [TestMethod]
    [DataRow(Color.White, 'R', DisplayName = "White Rook Attacks")]
    [DataRow(Color.Black, 'r', DisplayName = "Black Rook Attacks")]
    public void CalculateAttacksReturnsCorrectAttacksForRooks(Color color, char pieceType)
    {
        // Arrange
        var board = new Board();
        board.SetPieceAt(BitBoards.Squares["a1"], pieceType);
        board.SetPieceAt(BitBoards.Squares["h8"], pieceType);
        var pieceBase = PieceFactory.GetPiece(pieceType, color);

        // Act
        ulong attacks = board.CalculateDefendedSquaresByPiece(pieceBase);

        // Assert
        // Assuming a method to calculate expected attacks for a rook at a1
        ulong expectedAttacks = (BitBoards.Rank1 | BitBoards.Rank8 | BitBoards.FileA | BitBoards.FileH) &
            ~(BitBoards.Rank1 & BitBoards.FileA | BitBoards.Rank8 & BitBoards.FileH);
        Assert.AreEqual(expectedAttacks, attacks, "Rook attacks did not match expected attacks.");
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
    [DataRow('d', 4, BitBoards.EmptySquare, DisplayName = "Empty square at d4")]
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

        Assert.AreEqual(expectedPiece, board.GetPieceAt(BitBoards.Squares[$"{file}{rank}"]));
    }

    [TestMethod]
    public void ContainsColorPieceReturnsCorrectValues()
    {
        var board = new Board();
        board.InitializeGameStartingBoard();

        Assert.IsTrue(board.SquareContainPieceOfColor(BitBoards.Squares["a8"], 'b'));
        Assert.IsFalse(board.SquareContainPieceOfColor(BitBoards.Squares["a8"], 'w'));
        Assert.IsTrue(board.SquareContainPieceOfColor(BitBoards.Squares["c8"], 'b'));
        Assert.IsFalse(board.SquareContainPieceOfColor(BitBoards.Squares["c8"], 'w'));

        Assert.IsTrue(board.SquareContainPieceOfColor(BitBoards.Squares["a1"], 'w'));
        Assert.IsFalse(board.SquareContainPieceOfColor(BitBoards.Squares["a1"], 'b'));
        Assert.IsTrue(board.SquareContainPieceOfColor(BitBoards.Squares["c1"], 'w'));
        Assert.IsFalse(board.SquareContainPieceOfColor(BitBoards.Squares["c1"], 'b'));
    }

    [TestMethod]
    public void ContainsColorPiecesThrowsExceptionIfColorIsInvalid()
    {
        var board = new Board();
        board.InitializeGameStartingBoard();
        Exception exception =
            Assert.ThrowsException<ArgumentException>(() => board.SquareContainPieceOfColor(BitBoards.Squares["a1"], 'x'));
        Assert.AreEqual("Invalid color: x. Valid colors are 'b' or 'w'. (Parameter 'color')", exception.Message);
    }

    [TestMethod]
    public void GetPieceAtReturnsEmptySquareForCorrectSquares()
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
                    Assert.AreNotEqual(
                        BitBoards.EmptySquare,
                        board.GetPieceAt(BitBoards.Squares[$"{fileChar}{rank}"]),
                        $"Not empty square [{fileChar}{rank}].");
                }
                else
                {
                    Assert.AreEqual(
                        BitBoards.EmptySquare,
                        board.GetPieceAt(BitBoards.Squares[$"{fileChar}{rank}"]),
                        $"Empty square [{fileChar}{rank}].");
                }
            }
        }
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
                    Assert.IsFalse(board.IsEmptySquare(BitBoards.Squares[$"{fileChar}{rank}"]), "Not empty square.");
                }
                else
                {
                    Assert.IsTrue(board.IsEmptySquare(BitBoards.Squares[$"{fileChar}{rank}"]), "Empty square.");
                }
            }
        }
    }

    [TestMethod]
    public void WhiteOrEmptyTests()
    {
        var board = new Board();
        Assert.AreEqual(0xFFFFFFFFFFFFFFFF, board.WhiteOrEmpty(), "Empty board.");
        board.InitializeGameStartingBoard();
        Assert.AreEqual(
            BitBoards.Rank1 | BitBoards.Rank2 | BitBoards.Rank3 | BitBoards.Rank4 | BitBoards.Rank5 | BitBoards.Rank6,
            board.WhiteOrEmpty(),
            "Starting board.");
    }
}