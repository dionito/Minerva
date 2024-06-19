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

namespace Minerva.Tests;

[TestClass]
public class UlongExtensions
{
    [TestMethod]
    [DataRow(0b1UL, new int[] { 0 }, DisplayName = "Single bit at LSB")]
    [DataRow(0b10000UL, new int[] { 4 }, DisplayName = "Single bit")]
    [DataRow(0b10101010UL, new int[] { 1, 3, 5, 7 }, DisplayName = "Multiple bits")]
    [DataRow(0UL, new int[] { }, DisplayName = "No bits set")]
    public void BitsSetReturnsCorrectIndices(ulong bitboard, int[] expectedIndices)
    {
        var indices = bitboard.BitsSet();
        CollectionAssert.AreEqual(expectedIndices, indices);
    }

    [TestMethod]
    [DataRow(0b1UL, 0, 0UL, DisplayName = "Clear LSB")]
    [DataRow(0b10000UL, 4, 0UL, DisplayName = "Clear single bit")]
    [DataRow(0b10101010UL, 3, 0b10100010UL, DisplayName = "Clear bit from multiple")]
    public void ClearBitReturnsCorrectBitboard(ulong bitboard, int index, ulong expected)
    {
        var result = bitboard.ClearBit(index);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow(0UL, true, DisplayName = "Empty bitboard")]
    [DataRow(0b1UL, false, DisplayName = "Non-empty bitboard")]
    public void IsEmptyReturnsCorrectValue(ulong bitboard, bool expected)
    {
        var result = bitboard.IsEmpty();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow(0UL, false, DisplayName = "Empty bitboard")]
    [DataRow(0b1UL, true, DisplayName = "Single bit set")]
    [DataRow(0b11UL, false, DisplayName = "Multiple bits set")]
    public void IsSingleBitSetReturnsCorrectValue(ulong bitboard, bool expected)
    {
        var result = bitboard.IsSingleBitSet();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [DataRow(0b1UL, 0, true, DisplayName = "Single bit set at LSB")]
    [DataRow(0b10000UL, 4, true, DisplayName = "Single bit set")]
    [DataRow(0b10101010UL, -1, false, DisplayName = "Multiple bits set")]
    [DataRow(0UL, -1, false, DisplayName = "No bits set")]
    public void IsSingleBitSetOutIndexReturnsCorrectValueAndIndex(
        ulong bitboard,
        int expectedIndex,
        bool expectedResult)
    {
        var result = bitboard.IsSingleBitSet(out int index);

        Assert.AreEqual(expectedResult, result);
        Assert.AreEqual(expectedIndex, index);
    }

    [TestMethod]
    [DataRow(0b1UL, 0b1UL, 0, DisplayName = "Identical bitboards")]
    [DataRow(0b10101010UL, 0b01010101UL, 8, DisplayName = "Completely different bitboards")]
    [DataRow(0b10101010UL, 0b10101011UL, 1, DisplayName = "Single bit (LSB) different bitboards")]
    [DataRow(0b10101010UL, 0b10101010UL, 0, DisplayName = "Same bitboards")]
    public void HammingDistanceReturnsCorrectValue(ulong bitboard, ulong other, int expected)
    {
        var result = bitboard.HammingDistance(other);
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SetBit()
    {
        for (int i = 0; i < 64; i++)
        {
            ulong value = 0;
            value = value.SetBit(i);
            Assert.AreEqual(1UL << i, value);
        }
    }

    [TestMethod]
    public void ToggleBit()
    {
        for (int i = 0; i < 64; i++)
        {
            ulong value = ulong.MaxValue;
            ulong value1 = value.ToggleBit(i);
            ulong value2 = value.ToggleBits(1ul << i);
            ulong value3 = value.ToggleBits(new[] { i });
            Assert.AreEqual(value ^ (1UL << i), value1, "Toggle bit wrong.");
            Assert.AreEqual(value1, value2, "Toggle bits with mask wrong.");
            Assert.AreEqual(value2, value3, "Toggle bits with array wrong.");
        }
    }
}