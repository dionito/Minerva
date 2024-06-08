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

namespace Minerva.Tests;

[TestClass]
public class ColorExtensionsTests
{
    const char Black = 'b';
    const char None = ' ';
    const char White = 'w';

    [TestMethod]
    public void TestOppositeColor()
    {
        Assert.AreEqual(Color.White, Color.Black.Opposite());
        Assert.AreEqual(Color.Black, Color.White.Opposite());
        Assert.AreEqual(Color.None, Color.None.Opposite());
    }

    [TestMethod]
    public void TestToChar()
    {
        Assert.AreEqual(Black, Color.Black.ToChar());
        Assert.AreEqual(White, Color.White.ToChar());
        Assert.AreEqual(None, Color.None.ToChar());
    }

    [TestMethod]
    public void TestToColor()
    {
        Assert.AreEqual(Color.Black, Black.ToColor());
        Assert.AreEqual(Color.White, White.ToColor());
        Assert.AreEqual(Color.None, None.ToColor());
    }

    [TestMethod]
    public void TestToColorThrowsExceptionForInvalidColor()
    {
        Exception exception = Assert.ThrowsException<ArgumentOutOfRangeException>(() => 'x'.ToColor());
        Assert.IsTrue(
            exception.Message.Contains("Invalid color (Parameter 'color')"),
            "Wrong message part 1.");
        Assert.IsTrue(
            exception.Message.Contains("Actual value was x."),
            "Wrong message part 2.");
    }
}
