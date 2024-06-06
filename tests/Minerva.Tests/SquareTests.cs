// 
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
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// 

namespace Minerva.Tests;

[TestClass]
public class SquareTests
{
    [TestMethod]
    [DataRow('e', 5, Board.FileE & Board.Rank5, DisplayName = "Middle of the board.")]
    [DataRow('a', 1, Board.FileA & Board.Rank1, DisplayName = "Bottom left corner.")]
    [DataRow('h', 8, Board.FileH & Board.Rank8, DisplayName = "Top right corner.")]
    public void TestSquareConstructorsWithValidParametersEqualOperatorEqualAndGetHashCode(
        char file,
        int rank,
        ulong bitBoard)
    {
        var square = new Square(file, rank);
        var bitBoardSquare = new Square(bitBoard);
        var stringSquare = new Square($"{file}{rank}");

        Assert.AreEqual(square.File, bitBoardSquare.File, "Files do not match.");
        Assert.AreEqual(square.Rank, bitBoardSquare.Rank, "Ranks do not match.");
        Assert.AreEqual(square.BitBoard, bitBoardSquare.BitBoard, "BitBoards do not match.");
        Assert.AreEqual(square.File, stringSquare.File, "Files do not match (string).");
        Assert.AreEqual(square.Rank, stringSquare.Rank, "Ranks do not match (string).");
        Assert.AreEqual(square.BitBoard, stringSquare.BitBoard, "BitBoards do not match (string).");

        Assert.AreEqual(file, square.File, "File.");
        Assert.AreEqual(rank, square.Rank, "Rank.");
        Assert.AreEqual(bitBoard, square.BitBoard, "BitBoard.");

        Assert.AreEqual(square, bitBoardSquare, "Equal bitboard square");
        Assert.AreEqual(square, stringSquare, "Equal string square");
        Assert.IsTrue(square == bitBoardSquare, "Operator Equal bitboard square");
        Assert.IsTrue(square == stringSquare, "Operator Equal string square");

        Assert.AreEqual(square.GetHashCode(), bitBoardSquare.GetHashCode(), "HashCode bitboard square.");
        Assert.AreEqual(square.GetHashCode(), stringSquare.GetHashCode(), "HashCode string square.");
    }

    [TestMethod]
    [DataRow(0UL, DisplayName = "BitBoard is zero.")]
    [DataRow(3UL, DisplayName = "BitBoard has more than one bit set.")]
    public void TestSquareBitBoardConstructorWithInvalidBitBoards(ulong bitBoard)
    {
        var exception = Assert.ThrowsException<ArgumentException>(() => new Square(bitBoard));
        Assert.AreEqual(
            $"{nameof(bitBoard)} must have a single bit set. (Parameter 'bitBoard')",
            exception.Message,
            "Wrong message.");
    }

    [TestMethod]
    [DataRow(
        'Z',
        5,
        "Specified argument was out of the range of valid values. (Parameter 'file')",
        DisplayName = "Invalid file.")]
    [DataRow(
        'i',
        5,
        "Specified argument was out of the range of valid values. (Parameter 'file')",
        DisplayName = "Invalid file.")]
    [DataRow(
        'c',
        -1,
        "Specified argument was out of the range of valid values. (Parameter 'rank')",
        DisplayName = "Invalid rank.")]
    [DataRow(
        'c',
        9,
        "Specified argument was out of the range of valid values. (Parameter 'rank')",
        DisplayName = "Invalid rank.")]
    public void TestSquareWithInvalidValues(char file, int rank, string message)
    {
        Exception exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Square(file, rank));
        Assert.AreEqual(message, exception.Message, "Wrong message.");
    }

    [TestMethod]
    [DataRow(
        "Z1",
        "Specified argument was out of the range of valid values. (Parameter 'file')",
        DisplayName = "Invalid file, too low")]
    [DataRow(
        "I1",
        "Specified argument was out of the range of valid values. (Parameter 'file')",
        DisplayName = "Invalid file, too high")]
    [DataRow(
        "a0",
        "Specified argument was out of the range of valid values. (Parameter 'rank')",
        DisplayName = "Invalid rank, too low")]
    [DataRow(
        "a9",
        "Specified argument was out of the range of valid values. (Parameter 'rank')",
        DisplayName = "Invalid rank, too high")]
    public void TestSquareConstructorWithInvalidStringParameter(string square, string message)
    {
        Exception exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Square(square));
        Assert.AreEqual(message, exception.Message, "Wrong message.");
    }

    [TestMethod]
    [DataRow(
        "e",
        "Square must be two characters long. (Parameter 'square')",
        DisplayName = "Square too short")]
    [DataRow(
        "e",
        "Square must be two characters long. (Parameter 'square')",
        DisplayName = "Square too long")]
    public void TestSquareConstructorWithInvalidStringParameterLenght(string square, string message)
    {
        Exception exception = Assert.ThrowsException<ArgumentException>(() => new Square(square));
        Assert.AreEqual(message, exception.Message, "Wrong message.");
    }

    [TestMethod]
    public void TestToString()
    {
        var square = new Square('e', 5);
        Assert.AreEqual("e5", square.ToString());
    }

    [TestMethod]
    public void TestNotEquals()
    {
        var square1 = new Square('e', 5);
        var square2 = new Square("h1");
        Assert.IsFalse(square1.Equals(square2));
    }

    [TestMethod]
    public void TestOperatorNotEqual()
    {
        var square1 = new Square('e', 5);
        var square2 = new Square("f6");
        Assert.IsTrue(square1 != square2);
    }

    [TestMethod]
    [DataRow('e', 5, 1, 1, 'f', 6, DisplayName = "Move one square up and to the right.")]
    [DataRow('e', 5, -1, -1, 'd', 4, DisplayName = "Move one square down and to the left.")]
    [DataRow('e', 5, 1, -1, 'f', 4, DisplayName = "Move one square down and to the right.")]
    [DataRow('e', 5, -1, 1, 'd', 6, DisplayName = "Move one square up and to the left.")]
    [DataRow('e', 5, -1, 0, 'd', 5, DisplayName = "Move one square to the left.")]
    [DataRow('e', 5, 0, -1, 'e', 4, DisplayName = "Move one square down.")]
    [DataRow('e', 5, 1, 0, 'f', 5, DisplayName = "Move one square to the right.")]
    [DataRow('e', 5, 0, 1, 'e', 6, DisplayName = "Move one square up.")]
    [DataRow('a', 1, 0, 7, 'a', 8, DisplayName = "Move seven square up.")]
    [DataRow('a', 1, 7, 0, 'h', 1, DisplayName = "Move seven square to the right.")]
    [DataRow('a', 8, 0, -7, 'a', 1, DisplayName = "Move seven square down.")]
    [DataRow('h', 1, -7, 0, 'a', 1, DisplayName = "Move seven square to the left.")]
    public void TestMove(
        char fileFrom,
        int rankFrom,
        int fileMove,
        int rankMove,
        char expectedFile,
        int expectedRank)
    {
        var square = new Square(fileFrom, rankFrom);
        var move = new Move(fileMove, rankMove);
        var newSquare = square.Move(move);
        Assert.AreEqual(expectedFile, newSquare.File);
        Assert.AreEqual(expectedRank, newSquare.Rank);
    }

    [TestMethod]
    [DataRow('a', 1, 0, -1, DisplayName = "Move out through bottom.")]
    [DataRow('a', 1, -1, 0, DisplayName = "Move out through left.")]
    [DataRow('h', 8, 0, 1, DisplayName = "Move out through top.")]
    [DataRow('h', 8, 1, 0, DisplayName = "Move out through right.")]
    public void TestMoveOffBoard(
        char fileFrom,
        int rankFrom,
        int fileMove,
        int rankMove)
    {
        var square = new Square(fileFrom, rankFrom);
        var move = new Move(fileMove, rankMove);
        Assert.ThrowsException<InvalidOperationException>(() => square.Move(move));
    }

    [TestMethod]
    [DataRow('e', 5, 1, 1, true, DisplayName = "Try Move one square up and to the right.")]
    [DataRow('e', 5, -1, -1, true, DisplayName = "Try Move one square down and to the left.")]
    [DataRow('e', 5, 1, -1, true, DisplayName = "Try Move one square down and to the right.")]
    [DataRow('e', 5, -1, 1, true, DisplayName = "Try Move one square up and to the left.")]
    [DataRow('e', 5, -1, 0, true, DisplayName = "Try Move one square to the left.")]
    [DataRow('e', 5, 0, -1, true, DisplayName = "Try Move one square down.")]
    [DataRow('e', 5, 1, 0, true, DisplayName = "Try Move one square to the right.")]
    [DataRow('e', 5, 0, 1, true, DisplayName = "Try Move one square up.")]
    [DataRow('a', 1, 0, 7, true, DisplayName = "Try Move seven square up.")]
    [DataRow('a', 1, 7, 0, true, DisplayName = "Try Move seven square to the right.")]
    [DataRow('a', 8, 0, -7, true, DisplayName = "Try Move seven square down.")]
    [DataRow('h', 1, -7, 0, true, DisplayName = "Try Move seven square to the left.")]
    [DataRow('a', 1, 0, -1, false, DisplayName = "Try Move out through bottom.")]
    [DataRow('a', 1, -1, 0, false, DisplayName = "Try Move out through left.")]
    [DataRow('h', 8, 0, 1, false, DisplayName = "Try Move out through top.")]
    [DataRow('h', 8, 1, 0, false, DisplayName = "Try Move out through right.")]
    public void TestTryMove(
        char fileFrom,
        int rankFrom,
        int fileMove,
        int rankMove,
        bool expectedResult)

    {
        var square = new Square(fileFrom, rankFrom);
        var move = new Move(fileMove, rankMove);
        Assert.AreEqual(expectedResult, square.TryMove(move, out Square newSquare), "Result.");
    }
}