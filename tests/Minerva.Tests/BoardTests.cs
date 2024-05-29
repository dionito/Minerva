namespace Minerva.Tests;

[TestClass]
public class BoardTests
{
    [TestMethod]
    [DataRow("a1", Board.Rank1 & Board.FileA, DisplayName = "A1")]
    [DataRow("a8", Board.Rank8 & Board.FileA, DisplayName = "A8")]
    [DataRow("h1", Board.Rank1 & Board.FileH, DisplayName = "H1")]
    [DataRow("h8", Board.Rank8 & Board.FileH, DisplayName = "H8")]
    public void GetSquareReturnsTheRightBitboard(string square, ulong result)
    {
        var board = new Board();
        var bitboard = board.Square(square);
        Assert.AreEqual(result, bitboard);
    }

    [TestMethod]
    public void SquareH1IsRepresentedByTheMostSignicantBit()
    {
        var board = new Board();
        var bitboard = board.Square("h1");
        Assert.AreEqual(0x8000000000000000ul, bitboard);
    }

    [TestMethod]
    public void SquareA8IsRepresentedByTheLeastSignicantBit()
    {
        var board = new Board();
        var bitboard = board.Square("a8");
        Assert.AreEqual(0x0000000000000001ul, bitboard);
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
        var exception = Assert.ThrowsException<ArgumentException>(
            () => board.Square(square),
            "Wrong exception type.");
        Assert.AreEqual(exceptionType, exception.GetType());
        Assert.AreEqual(message, exception.Message);
    }

    [TestMethod]
    public void SquareThrowsArgumentNullExceptionWhenSquareIsNull()
    {
        var board = new Board();
        var exception = Assert.ThrowsException<ArgumentNullException>(
            () => board.Square(null),
            "Wrong exception type.");
        Assert.AreEqual("square", exception.ParamName);
        Assert.AreEqual("Value cannot be null. (Parameter 'square')", exception.Message);
    }
}