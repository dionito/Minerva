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
    public void GenerateBoardCreatesCorrectStartingBoardFromValidFen()
    {
        string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        Board board = ForsythEdwardsNotation.GenerateBoard(fen);

        // Bitboards
        Assert.AreEqual(Board.Rank1 | Board.Rank2, board.WhitePiecesBitBoard, "White Pieces");
        Assert.AreEqual(Board.Rank7 | Board.Rank8, board.BlackPiecesBitBoard, "Black pieces");
        Assert.AreEqual(
            Board.Rank1 | Board.Rank2 | Board.Rank7 | Board.Rank8,
            board.OccupiedBitBoard,
            "Occupied squares");

        // Black pieces
        Assert.AreEqual(
            board.GetSquareBitBoard("a8") | board.GetSquareBitBoard("h8"),
            board.BlackPieces["r"],
            "Black rocks");
        Assert.AreEqual(
            board.GetSquareBitBoard("b8") | board.GetSquareBitBoard("g8"),
            board.BlackPieces["n"],
            "Black knights");
        Assert.AreEqual(
            board.GetSquareBitBoard("c8") | board.GetSquareBitBoard("f8"),
            board.BlackPieces["b"],
            "Black bishops");
        Assert.AreEqual(board.GetSquareBitBoard("d8"), board.BlackPieces["q"], "Black queen");
        Assert.AreEqual(board.GetSquareBitBoard("e8"), board.BlackPieces["k"], "Black king");
        Assert.AreEqual(
            board.GetSquareBitBoard("a7") | board.GetSquareBitBoard("b7") | board.GetSquareBitBoard("c7") |
            board.GetSquareBitBoard("d7") | board.GetSquareBitBoard("e7") | board.GetSquareBitBoard("f7") |
            board.GetSquareBitBoard("g7") | board.GetSquareBitBoard("h7"),
            board.BlackPieces["p"],
            "Black pawns");

        // White pieces
        Assert.AreEqual(
            board.GetSquareBitBoard("a1") | board.GetSquareBitBoard("h1"),
            board.WhitePieces["R"],
            "White rocks");
        Assert.AreEqual(
            board.GetSquareBitBoard("b1") | board.GetSquareBitBoard("g1"),
            board.WhitePieces["N"],
            "White knights");
        Assert.AreEqual(
            board.GetSquareBitBoard("c1") | board.GetSquareBitBoard("f1"),
            board.WhitePieces["B"],
            "White bishops");
        Assert.AreEqual(board.GetSquareBitBoard("d1"), board.WhitePieces["Q"], "White queen");
        Assert.AreEqual(board.GetSquareBitBoard("e1"), board.WhitePieces["K"], "White king");
        Assert.AreEqual(
            board.GetSquareBitBoard("a2") | board.GetSquareBitBoard("b2") | board.GetSquareBitBoard("c2") |
            board.GetSquareBitBoard("d2") | board.GetSquareBitBoard("e2") | board.GetSquareBitBoard("f2") |
            board.GetSquareBitBoard("g2") | board.GetSquareBitBoard("h2"),
            board.WhitePieces["P"],
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