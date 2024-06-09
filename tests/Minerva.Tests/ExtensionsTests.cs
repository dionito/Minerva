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
public class ExtensionsTests
{
    const char Black = 'b';
    const char None = ' ';
    const char White = 'w';

    [TestMethod]
    public void ColorOpposite()
    {
        Assert.AreEqual(Color.White, Color.Black.Opposite());
        Assert.AreEqual(Color.Black, Color.White.Opposite());
        Assert.AreEqual(Color.None, Color.None.Opposite());
    }

    [TestMethod]
    public void ColorToCharHappyPath()
    {
        Assert.AreEqual(Black, Color.Black.ToChar());
        Assert.AreEqual(White, Color.White.ToChar());
        Assert.AreEqual(None, Color.None.ToChar());
    }

    [TestMethod]
    public void ToColorHappyPath()
    {
        Assert.AreEqual(Color.Black, Black.ToColor());
        Assert.AreEqual(Color.White, White.ToColor());
        Assert.AreEqual(Color.None, None.ToColor());
    }

    [TestMethod]
    public void ColorFromCharThrowsExceptionForInvalidInput()
    {
        Exception exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => 'x'.ToColor());
        Assert.IsTrue(
            exception.Message.Contains("Invalid color (Parameter 'color')"),
            "Wrong message part 1.");
        Assert.IsTrue(
            exception.Message.Contains("Actual value was x."),
            "Wrong message part 2.");
    }

    [TestClass]
    public class PieceTypeExtensionsTests
    {
        [TestMethod]
        [DataRow('p', PieceType.Pawn, DisplayName = "Pawn")]
        [DataRow('R', PieceType.Rook, DisplayName = "Rook")]
        [DataRow('n', PieceType.Knight, DisplayName = "Knight")]
        [DataRow('B', PieceType.Bishop, DisplayName = "Bishop")]
        [DataRow('q', PieceType.Queen, DisplayName = "Queen")]
        [DataRow('K', PieceType.King, DisplayName = "King")]
        [DataRow(' ', PieceType.None, DisplayName = "None")]
        public void ToPieceTypeHappyPath(char input, PieceType expected)
        {
            var result = input.ToPieceType();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [DataRow('o', DisplayName = "Invalid Piece")]
        public void PieceTypeFromCharThrowsExceptionForInvalidInput(char input)
        {
            Exception exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => input.ToPieceType());
            Assert.AreEqual(
                $"Invalid piece type. (Parameter 'pieceType')\r\nActual value was {input}.",
                exception.Message,
                "Exception message.");
        }

        [TestMethod]
        [DataRow(PieceType.Pawn, 'p', DisplayName = "Pawn")]
        [DataRow(PieceType.Rook, 'r', DisplayName = "Rook")]
        [DataRow(PieceType.Knight, 'n', DisplayName = "Knight")]
        [DataRow(PieceType.Bishop, 'b', DisplayName = "Bishop")]
        [DataRow(PieceType.Queen, 'q', DisplayName = "Queen")]
        [DataRow(PieceType.King, 'k', DisplayName = "King")]
        [DataRow(PieceType.None, ' ', DisplayName = "None")]
        public void ToChar_ReturnsCorrectChar(PieceType input, char expected)
        {
            var result = input.ToChar();
            Assert.AreEqual(expected, result);
        }
    }

}
