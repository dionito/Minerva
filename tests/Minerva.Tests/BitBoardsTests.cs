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

namespace Minerva.Tests;

[TestClass]
public class BitBoardsTests
{
    [TestMethod]
    public void BishopsTests()
    {
        var board = new Board();
        PieceBase bishop = PieceFactory.GetPiece('b', Color.White);
        foreach (KeyValuePair<ulong, ulong> diagonal in BitBoards.Bishop)
        {
            ulong moves = bishop.GetPieceMoves(diagonal.Key, board);
            Assert.AreEqual(moves.ToString("X"), diagonal.Value.ToString("X"));
        }
    }

    [TestMethod]
    public void DiagonalsTests()
    {
        var board = new Board();
        PieceBase bishop = PieceFactory.GetPiece('b', Color.White);
        foreach (KeyValuePair<ulong, ulong> diagonal in BitBoards.Diagonals)
        {
            ulong positions = bishop.GetPieceMoves(diagonal.Key, board) | diagonal.Key;
            Assert.AreEqual(positions.ToString("X"), diagonal.Value.ToString("X"));
        }
    }

    [TestMethod]
    [DataRow("a1", BitBoards.Rank1 & BitBoards.FileA, DisplayName = "a1")]
    [DataRow("a8", BitBoards.Rank8 & BitBoards.FileA, DisplayName = "a8")]
    [DataRow("h1", BitBoards.Rank1 & BitBoards.FileH, DisplayName = "h1")]
    [DataRow("h8", BitBoards.Rank8 & BitBoards.FileH, DisplayName = "h8")]
    public void SquaresBitBoardReturnsTheRightBitboard(string square, ulong result)
    {
        ulong bitboard = BitBoards.Squares[square];
        Assert.AreEqual(result, bitboard);
    }
}