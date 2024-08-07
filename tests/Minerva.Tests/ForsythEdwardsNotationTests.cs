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
public class ForsythEdwardsNotationTests
{
    [TestMethod]
    public void GenerateBoardThrowsArgumentNullExceptionWhenFenIsNull()
    {
        string? nullFen = null;
#pragma warning disable CS8604 // Possible null reference argument.
        Assert.ThrowsException<ArgumentNullException>(() => ForsythEdwardsNotation.GenerateBoard(nullFen));
#pragma warning restore CS8604 // Possible null reference argument.
    }

    [TestMethod]
    [DataRow(
        "8/8/8/8/8/8/8/8 w - - 0 1 extra",
        "Invalid FEN string. FEN should always have 6 parts. (Parameter 'fen')",
        DisplayName = "Extra parts in FEN string")]
    [DataRow(
        "8/8/8/8/8/8/8/8 w",
        "Invalid FEN string. FEN should always have 6 parts. (Parameter 'fen')",
        DisplayName = "Few parts in FEN string")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",
        "Invalid FEN string. Ranks should always have 8 parts. (Parameter 'fen')",
        DisplayName = "Extra parts in in ranks")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",
        "Invalid FEN string. Ranks should always have 8 parts. (Parameter 'fen')",
        DisplayName = "Few parts in in ranks")]
    [DataRow(
        "rn&qkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1",
        "Invalid character in FEN string ranks. (Parameter 'ranks')",
        DisplayName = "Invalid Character in FEN string ranks")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR x KQkq - 0 1",
        "Invalid active color in FEN string. (Parameter 'fen')",
        DisplayName = "Invalid active color")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KXq - 0 1",
        "Invalid castling rights in FEN string. (Parameter 'fen')",
        DisplayName = "Invalid character in castling rights")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkqKQkq - 0 1",
        "Invalid castling rights in FEN string. (Parameter 'fen')",
        DisplayName = "Too many characters in castling rights")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq e2 0 1",
        "Invalid en passant target square in FEN string. (Parameter 'fen')",
        DisplayName = "Invalid en passant target square e2")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq e7 0 1",
        "Invalid en passant target square in FEN string. (Parameter 'fen')",
        DisplayName = "Invalid en passant target square e7")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq i3 0 1",
        "Invalid en passant target square in FEN string. (Parameter 'fen')",
        DisplayName = "Invalid en passant target square i3")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq a9 0 1",
        "Invalid en passant target square in FEN string. (Parameter 'fen')",
        DisplayName = "Invalid en passant target square a9")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq c3 0 1",
        "Invalid en passant target square in FEN string. " +
        "No pawn to be taken en passant found. (Parameter 'enPassantTargetSquare')",
        DisplayName = "No pawn for en passant target square c3")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq c6 0 1",
        "Invalid en passant target square in FEN string. " +
        "No pawn to be taken en passant found. (Parameter 'enPassantTargetSquare')",
        DisplayName = "No pawn for en passant target square c6")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - f 1",
        "Invalid halfmove clock in FEN string. (Parameter 'fen')",
        DisplayName = "Invalid halfmove clock in FEN string")]
    [DataRow(
        "4k/8/8/8/8/8/8/K w - - 51 133",
        "Halfmove clock out of range in FEN string. (Parameter 'fen')",
        DisplayName = "Too large halfmove clock in FEN string")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - -1 1",
        "Halfmove clock out of range in FEN string. (Parameter 'fen')",
        DisplayName = "Too small fullcount number in FEN string")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 f",
        "Invalid fullmove counter in FEN string. (Parameter 'fen')",
        DisplayName = "Invalid fullcount number in FEN string")]
    [DataRow(
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 1 0",
        "Fullmove counter must be 1 or greater. (Parameter 'fen')",
        DisplayName = "Too small fullcount number in FEN string")]
    public void GenerateBoardThrowsArgumentExceptionWhenFenIsInvalid(string fen, string expectedMessage)
    {
        Exception exception =
            Assert.ThrowsException<ArgumentException>(() => ForsythEdwardsNotation.GenerateBoard(fen));
        Assert.AreEqual(expectedMessage, exception.Message);
    }

    [TestMethod]
    [DataRow(
        "8/8/8/4k3/2K2q2/3N4/Qb6/8 b - - 0 1",
        "Invalid FEN string. The side to move can take the opposite king. (Parameter 'fen')",
        DisplayName = "Black is moving and can capture enemy king.")]
    [DataRow(
        "8/8/8/4k3/2K2q2/3N4/Qb6/8 w - - 0 1",
        "Invalid FEN string. The side to move can take the opposite king. (Parameter 'fen')",
        DisplayName = "White is moving and can capture enemy king.")]
    [DataRow(
        "8/8/2k5/5k2/2K5/8/8/8 w - - 0 1",
        "Invalid FEN string. There must be only one king for each side. (Parameter 'fen')",
        DisplayName = "More than one king for black.")]
    [DataRow(
        "8/8/2K5/5k2/2K5/8/8/8 w - - 0 1",
        "Invalid FEN string. There must be only one king for each side. (Parameter 'fen')",
        DisplayName = "More than one king for white.")]
    [DataRow(
        "3K4/8/8/8/5q2/8/Qb6/1N6 w - - 0 1",
        "Invalid FEN string. The kings must be present on the board for both sides. (Parameter 'fen')",
        DisplayName = "No black king on board.")]
    [DataRow(
        "8/8/8/8/5q2/8/Qb6/kN6 w - - 0 1",
        "Invalid FEN string. The kings must be present on the board for both sides. (Parameter 'fen')",
        DisplayName = "No white king on board.")]
    public void TestInvalidFenStrings(string fen, string message)
    {
        Exception exception =
            Assert.ThrowsException<ArgumentException>(() => ForsythEdwardsNotation.GenerateBoard(fen));
        Assert.AreEqual(message, exception.Message);
    }

    [TestMethod]
    public void GenerateBoardCreatesCorrectStartingBoardFromValidFen()
    {
        string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        Board board = ForsythEdwardsNotation.GenerateBoard(fen);

        // Bitboards
        Assert.AreEqual(BitBoards.Rank1 | BitBoards.Rank2, board.WhitePiecesBitBoard, "White Pieces");
        Assert.AreEqual(BitBoards.Rank7 | BitBoards.Rank8, board.BlackPiecesBitBoard, "Black pieces");
        Assert.AreEqual(
            BitBoards.Rank1 | BitBoards.Rank2 | BitBoards.Rank7 | BitBoards.Rank8,
            board.OccupiedBitBoard,
            "Occupied squares");

        // Black pieces
        Assert.AreEqual(
            BitBoards.Squares["a8"] | BitBoards.Squares["h8"],
            board.BlackPieces['r'],
            "Black rocks");
        Assert.AreEqual(
            BitBoards.Squares["b8"] | BitBoards.Squares["g8"],
            board.BlackPieces['n'],
            "Black knights");
        Assert.AreEqual(
            BitBoards.Squares["c8"] | BitBoards.Squares["f8"],
            board.BlackPieces['b'],
            "Black bishops");
        Assert.AreEqual(BitBoards.Squares["d8"], board.BlackPieces['q'], "Black queen");
        Assert.AreEqual(BitBoards.Squares["e8"], board.BlackPieces['k'], "Black king");
        Assert.AreEqual(
            BitBoards.Squares["a7"] | BitBoards.Squares["b7"] | BitBoards.Squares["c7"] |
            BitBoards.Squares["d7"] | BitBoards.Squares["e7"] | BitBoards.Squares["f7"] |
            BitBoards.Squares["g7"] | BitBoards.Squares["h7"],
            board.BlackPieces['p'],
            "Black pawns");

        // White pieces
        Assert.AreEqual(
            BitBoards.Squares["a1"] | BitBoards.Squares["h1"],
            board.WhitePieces['R'],
            "White rocks");
        Assert.AreEqual(
            BitBoards.Squares["b1"] | BitBoards.Squares["g1"],
            board.WhitePieces['N'],
            "White knights");
        Assert.AreEqual(
            BitBoards.Squares["c1"] | BitBoards.Squares["f1"],
            board.WhitePieces['B'],
            "White bishops");
        Assert.AreEqual(BitBoards.Squares["d1"], board.WhitePieces['Q'], "White queen");
        Assert.AreEqual(BitBoards.Squares["e1"], board.WhitePieces['K'], "White king");
        Assert.AreEqual(
            BitBoards.Squares["a2"] | BitBoards.Squares["b2"] | BitBoards.Squares["c2"] |
            BitBoards.Squares["d2"] | BitBoards.Squares["e2"] | BitBoards.Squares["f2"] |
            BitBoards.Squares["g2"] | BitBoards.Squares["h2"],
            board.WhitePieces['P'],
            "White pawns");
        
        // Active color
        Assert.AreEqual('w', board.ActiveColor, "Active color");

        // Castling rights
        Assert.AreEqual("KQkq", board.CastlingRights, "Castling rights");

        // En passant target
        Assert.AreEqual("-", board.EnPassantTargetSquare, "En passant target");

        // Halfmove clock
        Assert.AreEqual(0, board.HalfmoveClock, "Halfmove clock");

        // Fullmove number
        Assert.AreEqual(1, board.FullmoveNumber, "Fullmove number");
    }

    [TestMethod]
    public void GenerateFenThrowsArgumentNullExceptionWhenBoardIsNull()
    {
        Board board = null!;
        Assert.ThrowsException<ArgumentNullException>(() => board.GenerateFen());
    }

    [TestMethod]
    public void GenerateFenReturnsCorrectFenString()
    {
        string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        Board board = ForsythEdwardsNotation.GenerateBoard(fen);
        string boardFen = board.GenerateFen();
        Assert.AreEqual(fen, boardFen);
    }
}