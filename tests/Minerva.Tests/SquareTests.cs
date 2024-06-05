// /*
//  * Copyright (C) 2024 dionito
//  *
//  * This program is free software: you can redistribute it and/or modify
//  * it under the terms of the GNU General Public License as published by
//  * the Free Software Foundation, either version 3 of the License, or
//  * (at your option) any later version.
//  *
//  * This program is distributed in the hope that it will be useful,
//  * but WITHOUT ANY WARRANTY; without even the implied warranty of
//  * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  * GNU General Public License for more details.
//  *
//  * You should have received a copy of the GNU General Public License
//  * along with this program.  If not, see <https://www.gnu.org/licenses/>.
//  */

namespace Minerva.Tests;

[TestClass]
public class SquareTests
{
    [TestMethod]
    [DataRow('e', 5, Board.FileE & Board.Rank5, DisplayName = "Middle of the board.")]
    [DataRow('a', 1, Board.FileA & Board.Rank1, DisplayName = "Bottom left corner.")]
    [DataRow('h', 8, Board.FileH & Board.Rank8, DisplayName = "Top right corner.")]
    public void TestSquareConstructorWithValidParameters(char file, int rank, ulong expectedBitBoard)
    {
        var square = new Square(file, rank);
        Assert.AreEqual(file, square.File, "File.");
        Assert.AreEqual(rank, square.Rank, "Rank.");
        Assert.AreEqual(expectedBitBoard, square.BitBoard, "BitBoard.");
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
    public void TestSquareConstructorWithStringParameter()
    {
        var square = new Square("e5");
        Assert.AreEqual('e', square.File);
        Assert.AreEqual(5, square.Rank);
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
    public void TestEquals()
    {
        var square1 = new Square('e', 5);
        var square2 = new Square("e5");
        Assert.IsTrue(square1.Equals(square2));
    }

    [TestMethod]
    public void TestNotEquals()
    {
        var square1 = new Square('e', 5);
        var square2 = new Square("f6");
        Assert.IsFalse(square1.Equals(square2));
    }

    [TestMethod]
    public void TestGetHashCode()
    {
        var square1 = new Square('e', 5);
        var square2 = new Square('e', 5);
        Assert.AreEqual(square1.GetHashCode(), square2.GetHashCode());
    }

    [TestMethod]
    public void TestOperatorEqual()
    {
        var square1 = new Square('e', 5);
        var square2 = new Square("e5");
        Assert.IsTrue(square1 == square2);
    }

    [TestMethod]
    public void TestOperatorNotEqual()
    {
        var square1 = new Square('e', 5);
        var square2 = new Square("f6");
        Assert.IsTrue(square1 != square2);
    }
}
