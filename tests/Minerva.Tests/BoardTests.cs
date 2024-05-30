namespace Minerva.Tests;

[TestClass]
public class BoardTests
{
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
    [DataRow("a8", 1ul << 63, DisplayName = "a8 - MSB")]
    [DataRow("H1", 1ul, DisplayName = "H1 - LSB")]
    public void MostSignificanAndLeastSignificantBitSqaresAreSetProperly(string square, ulong expectedBitBoard)
    {
        var board = new Board();
        ulong bitboard = board.GetSquareBitBoard(square);
        Assert.AreEqual(expectedBitBoard, bitboard);
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
    public void SquareThrowsTheRightArgumentExceptions(string? square, Type exceptionType, string message)
    {
        var board = new Board();
        ArgumentException exception = Assert.ThrowsException<ArgumentException>(
            () => board.GetSquareBitBoard(square),
            "Wrong exception type.");
        Assert.AreEqual(exceptionType, exception.GetType());
        Assert.AreEqual(message, exception.Message);
    }

    [TestMethod]
    public void SquareThrowsArgumentNullExceptionWhenSquareIsNull()
    {
        var board = new Board();
        ArgumentNullException exception = Assert.ThrowsException<ArgumentNullException>(
            () => board.GetSquareBitBoard(null),
            "Wrong exception type.");
        Assert.AreEqual("square", exception.ParamName);
        Assert.AreEqual("Value cannot be null. (Parameter 'square')", exception.Message);
    }

    [TestMethod]
    public void BlackPiecesBitBoardIsCorrectOnEmptyBoardCreation()
    {
        var board = new Board();
        ulong blackPiecesBitBoard = board.BlackPieces.Values.Aggregate((a, b) => a | b);
        Assert.AreEqual(0ul, blackPiecesBitBoard);
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
        var board = new Board { BlackPieces = { ["p"] = 1ul } };
        ulong blackPiecesBitBoard = board.BlackPieces.Values.Aggregate((a, b) => a | b);
        Assert.AreEqual(1ul, blackPiecesBitBoard);
    }

    [TestMethod]
    public void AllBitBoardsAreCorrectAfterCreatingGameStartingBoard()
    {
        var board = new Board();
        board.InitializeGameStartingBoard();

        // Bitboards
        Assert.AreEqual(0x000000000000FFFFul, board.WhitePiecesBitBoard, "White Pieces.");
        Assert.AreEqual(0xFFFF000000000000ul, board.BlackPiecesBitBoard, "Black pieces.");
        Assert.AreEqual(0xFFFF00000000FFFFul, board.OccupiedBitBoard, "Occupied squares.");
        
        // Black pieces
        Assert.AreEqual(
            board.GetSquareBitBoard("a8") | board.GetSquareBitBoard("h8"),
            board.BlackPieces["r"],
            "Black rocks");
        Assert.AreEqual(
            board.GetSquareBitBoard("b8") | board.GetSquareBitBoard("g8"),
            board.BlackPieces["n"],
            "Black knights");
        Assert.AreEqual(
            board.GetSquareBitBoard("c8") | board.GetSquareBitBoard("f8"),
            board.BlackPieces["b"],
            "Black bishops");
        Assert.AreEqual(board.GetSquareBitBoard("d8"), board.BlackPieces["q"], "Black queen");
        Assert.AreEqual(board.GetSquareBitBoard("e8"), board.BlackPieces["k"], "Black king");
        Assert.AreEqual(
            board.GetSquareBitBoard("a7") | board.GetSquareBitBoard("b7") | board.GetSquareBitBoard("c7") |
            board.GetSquareBitBoard("d7") | board.GetSquareBitBoard("e7") | board.GetSquareBitBoard("f7") |
            board.GetSquareBitBoard("g7") | board.GetSquareBitBoard("h7"),
            board.BlackPieces["p"],
            "Black pawns");

        // White pieces
        Assert.AreEqual(
            board.GetSquareBitBoard("a1") | board.GetSquareBitBoard("h1"),
            board.WhitePieces["R"],
            "White rocks");
        Assert.AreEqual(
            board.GetSquareBitBoard("b1") | board.GetSquareBitBoard("g1"),
            board.WhitePieces["N"],
            "White knights");
        Assert.AreEqual(
            board.GetSquareBitBoard("c1") | board.GetSquareBitBoard("f1"),
            board.WhitePieces["B"],
            "White bishops");
        Assert.AreEqual(board.GetSquareBitBoard("d1"), board.WhitePieces["Q"], "White queen");
        Assert.AreEqual(board.GetSquareBitBoard("e1"), board.WhitePieces["K"], "White king");
        Assert.AreEqual(
            board.GetSquareBitBoard("a2") | board.GetSquareBitBoard("b2") | board.GetSquareBitBoard("c2") |
            board.GetSquareBitBoard("d2") | board.GetSquareBitBoard("e2") | board.GetSquareBitBoard("f2") |
            board.GetSquareBitBoard("g2") | board.GetSquareBitBoard("h2"),
            board.WhitePieces["P"],
            "White pawns");
    }
}