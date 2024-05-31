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
    public void SetPieceAtSetsTheRightPieceAtTheRightPlace()
    {
        var board = new Board();

        // Set a black pawn at e5
        board.SetPieceAt(5, 5, 'p');
        Assert.AreEqual(board.GetSquareBitBoard("e5"), board.BlackPieces["p"]);

        // Set a white knight at b1
        board.SetPieceAt(2, 1, 'N');
        Assert.AreEqual(board.GetSquareBitBoard("b1"), board.WhitePieces["N"]);
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

}