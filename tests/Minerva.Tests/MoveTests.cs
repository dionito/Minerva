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
public class MoveTests
{
    [TestMethod]
    public void TestMoveCreation()
    {
        var move = new Move(1, 2);
        Assert.AreEqual(1, move.FileMove);
        Assert.AreEqual(2, move.RankMove);
    }

    [TestMethod]
    [DataRow(
        8,
        0,
        "Specified argument was out of the range of valid values. (Parameter 'fileMove')",
        DisplayName = "Excessive move to the right")]
    [DataRow(
        0,
        8,
        "Specified argument was out of the range of valid values. (Parameter 'rankMove')",
        DisplayName = "Excessive move down")]
    [DataRow(
        -8,
        0,
        "Specified argument was out of the range of valid values. (Parameter 'fileMove')",
        DisplayName = "Excessive move to the left")]
    [DataRow(
        0,
        -8,
        "Specified argument was out of the range of valid values. (Parameter 'rankMove')",
        DisplayName = "Excessive move up")]
    public void TestInvalidMoveCreation(int file, int rank, string message)
    {
        Exception exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Move(file, rank));
        Assert.AreEqual(message, exception.Message);
    }

    [TestMethod]
    public void TestNoMoveThrowsArgumentException()
    {
        Exception exception = Assert.ThrowsException<ArgumentException>(() => new Move(0, 0));
        Assert.AreEqual("A move must be in at least one direction. (Parameter 'fileMove')", exception.Message);
    }

    [TestMethod]
    public void TestMoveAddition()
    {
        var move1 = new Move(1, 2);
        var move2 = new Move(3, 4);
        var result = move1 + move2;

        Assert.AreEqual(4, result.FileMove);
        Assert.AreEqual(6, result.RankMove);
    }

    [TestMethod]
    public void TestMoveMultiplication()
    {
        int scalar = 2;
        int file = 2;
        int rank = 3;
        var move = new Move(file, rank);
        var result = scalar * move;

        Assert.AreEqual(scalar * file, result.FileMove);
        Assert.AreEqual(scalar * rank, result.RankMove);
    }

    [TestMethod]
    public void TestPredefinedMoves()
    {
        Assert.AreEqual(0, Move.Down.FileMove);
        Assert.AreEqual(-1, Move.Down.RankMove);

        Assert.AreEqual(-1, Move.DownLeft.FileMove);
        Assert.AreEqual(-1, Move.DownLeft.RankMove);

        Assert.AreEqual(1, Move.DownRight.FileMove);
        Assert.AreEqual(-1, Move.DownRight.RankMove);

        Assert.AreEqual(-1, Move.Left.FileMove);
        Assert.AreEqual(0, Move.Left.RankMove);

        Assert.AreEqual(1, Move.Right.FileMove);
        Assert.AreEqual(0, Move.Right.RankMove);

        Assert.AreEqual(0, Move.Up.FileMove);
        Assert.AreEqual(1, Move.Up.RankMove);

        Assert.AreEqual(-1, Move.UpLeft.FileMove);
        Assert.AreEqual(1, Move.UpLeft.RankMove);

        Assert.AreEqual(1, Move.UpRight.FileMove);
        Assert.AreEqual(1, Move.UpRight.RankMove);
    }
}
