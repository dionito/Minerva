// 
// Copyright (C) 2024 Dioni de la Morena Morales
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

namespace Minerva.Tests;

[TestClass]
public class PieceTests
{
    [TestMethod]
    [DataRow(Color.White, DisplayName = "White")]
    [DataRow(Color.Black, DisplayName = "Black")]
    public void BishopConstructorSetsCorrectPieceTypeAndColor(Color color)
    {
        var bishop = new Bishop(color);
        Assert.AreEqual(PieceType.Bishop, bishop.PieceType);
        Assert.AreEqual(color, bishop.Color);
    }

    [TestMethod]
    public void GetPossibleMovesReturnsCorrectMoves()
    {
        // Arrange
        var bishop = new Bishop(Color.White);
        var board = new Board(); 
        var position = new Square("d4");

        // Act
        var possibleMoves = bishop.GetPossibleMoves(position, board);

        // Assert
        // Assuming a Square class with an override for Equals that compares the square's position
        Assert.IsTrue(possibleMoves.Contains(new Square("c3")));
        Assert.IsTrue(possibleMoves.Contains(new Square("b2")));
        Assert.IsTrue(possibleMoves.Contains(new Square("a1")));
        Assert.IsTrue(possibleMoves.Contains(new Square("e5")));
        Assert.IsTrue(possibleMoves.Contains(new Square("f6")));
        Assert.IsTrue(possibleMoves.Contains(new Square("g7")));
        Assert.IsTrue(possibleMoves.Contains(new Square("h8")));
        Assert.IsTrue(possibleMoves.Contains(new Square("c5")));
        Assert.IsTrue(possibleMoves.Contains(new Square("b6")));
        Assert.IsTrue(possibleMoves.Contains(new Square("a7")));
        Assert.IsTrue(possibleMoves.Contains(new Square("e3")));
        Assert.IsTrue(possibleMoves.Contains(new Square("f2")));
        Assert.IsTrue(possibleMoves.Contains(new Square("g1")));
    }
}
