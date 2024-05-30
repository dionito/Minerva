namespace Minerva;


/// <summary>
/// Represents a chess board and its pieces using a set of bitboards.
/// A bitboard is a 64-bit integer where each bit represents a square on the board. In our
/// case, a bit set to 1 means that a piece is present on that square.
///
/// A8 is represented by the most significant bit (MSB) and
/// H1 is represented by the least significant one (LSB)
/// 
/// 8 ▓ ▒ ▓ ▒ ▓ ▒ ▓ ▒ 
/// 7 ▒ ▓ ▒ ▓ ▒ ▓ ▒ ▓ 
/// 6 ▓ ▒ ▓ ▒ ▓ ▒ ▓ ▒ 
/// 5 ▒ ▓ ▒ ▓ ▒ ▓ ▒ ▓ 
/// 4 ▓ ▒ ▓ ▒ ▓ ▒ ▓ ▒ 
/// 3 ▒ ▓ ▒ ▓ ▒ ▓ ▒ ▓ 
/// 2 ▓ ▒ ▓ ▒ ▓ ▒ ▓ ▒ 
/// 1 ▒ ▓ ▒ ▓ ▒ ▓ ▒ ▓ 
///   a b c d e f g h
/// 
/// </summary>
public class Board
{
    public const ulong FileA = 0x8080808080808080ul;
    public const ulong FileB = 0x4040404040404040ul;
    public const ulong FileC = 0x2020202020202020ul;
    public const ulong FileD = 0x1010101010101010ul;
    public const ulong FileE = 0x0808080808080808ul;
    public const ulong FileF = 0x0404040404040404ul;
    public const ulong FileG = 0x0202020202020202ul;
    public const ulong FileH = 0x0101010101010101ul;

    public const ulong Rank1 = 0x00000000000000FFul;
    public const ulong Rank2 = 0x000000000000FF00ul;
    public const ulong Rank3 = 0x0000000000FF0000ul;
    public const ulong Rank4 = 0x00000000FF000000ul;
    public const ulong Rank5 = 0x000000FF00000000ul;
    public const ulong Rank6 = 0x0000FF0000000000ul;
    public const ulong Rank7 = 0x00FF000000000000ul;
    public const ulong Rank8 = 0xFF00000000000000ul;

    /// <summary>
    /// Represents the files of the chess board.
    /// </summary>
    readonly ulong[] files = 
    {
        FileA, FileB, FileC, FileD, FileE, FileF, FileG, FileH,
    };

    /// <summary>
    /// Represents the ranks of the chess board.
    /// </summary>
    readonly ulong[] ranks = 
    {
        Rank1, Rank2, Rank3, Rank4, Rank5, Rank6, Rank7, Rank8,
    };

    /// <summary>
    /// Represents the bitboards for the black pieces.
    /// </summary>
    public Dictionary<string, ulong> BlackPieces { get; } = new()
    {
        { "b", 0ul },
        { "n", 0ul },
        { "r", 0ul },
        { "q", 0ul },
        { "k", 0ul },
        { "p", 0ul },
    };

    /// <summary>
    /// Gets the combined bitboard for all black pieces.
    /// This property performs a bitwise OR operation on the bitboards of all black pieces.
    /// </summary>
    public ulong BlackPiecesBitBoard => this.BlackPieces.Values.Aggregate((a, b) => a | b);

    /// <summary>
    /// Gets the combined bitboard for all pieces on the board.
    /// This property performs a bitwise OR operation on the bitboards of all black and white pieces.
    /// </summary>
    public ulong OccupiedBitBoard => this.BlackPiecesBitBoard | this.WhitePiecesBitBoard;

    /// <summary>
    /// Represents the bitboards for the white pieces.
    /// </summary>
    public Dictionary<string, ulong> WhitePieces { get; } = new()
    {
        { "B", 0ul },
        { "N", 0ul },
        { "R", 0ul },
        { "Q", 0ul },
        { "K", 0ul },
        { "P", 0ul },
    };

    /// <summary>
    /// Gets the combined bitboard for all white pieces.
    /// This property performs a bitwise OR operation on the bitboards of all white pieces.
    /// </summary>
    public ulong WhitePiecesBitBoard => this.WhitePieces.Values.Aggregate((a, b) => a | b);

    /// <summary>
    /// This method returns the bitboard for a given square represented by
    /// its standard chess notation.
    /// </summary>
    /// <param name="square">The square in standard chess notation.</param>
    public ulong GetSquareBitBoard(string? square)
    {
        if (square == null)  { throw new ArgumentNullException(nameof(square)); }

        if (square.Length != 2)
        {
            throw new ArgumentException("Square notation must be 2 characters long.", nameof(square));
        }

        int file = char.ToLower(square[0]) - 'a';
        if (file is < 0 or > 7)
        {
            throw new ArgumentException("Invalid file.", nameof(square));
        }

        var rank = square[1] - '1';
        if (rank is < 0 or > 7)
        {
            throw new ArgumentException("Invalid rank.", nameof(square));
        }

        return this.files[file] & this.ranks[rank];
    }

    /// <summary>
    /// Initializes the chess board to the standard starting position.
    /// This method sets the bitboards for both black and white pieces.
    /// </summary>
    public void InitializeGameStartingBoard()
    {
        // Initialize black pieces
        // Rooks are placed on a8 and h8
        this.BlackPieces["r"] = this.GetSquareBitBoard("a8") | this.GetSquareBitBoard("h8");
        // Knights are placed on b8 and g8
        this.BlackPieces["n"] = this.GetSquareBitBoard("b8") | this.GetSquareBitBoard("g8");
        // Bishops are placed on c8 and f8
        this.BlackPieces["b"] = this.GetSquareBitBoard("c8") | this.GetSquareBitBoard("f8");
        // Queen is placed on d8
        this.BlackPieces["q"] = this.GetSquareBitBoard("d8");
        // King is placed on e8
        this.BlackPieces["k"] = this.GetSquareBitBoard("e8");
        // Pawns are placed on a7 to h7
        this.BlackPieces["p"] = this.GetSquareBitBoard("a7") | this.GetSquareBitBoard("b7") |
            this.GetSquareBitBoard("c7") | this.GetSquareBitBoard("d7") | this.GetSquareBitBoard("e7") |
            this.GetSquareBitBoard("f7") | this.GetSquareBitBoard("g7") | this.GetSquareBitBoard("h7");

        // Initialize white pieces
        // Rooks are placed on a1 and h1
        this.WhitePieces["R"] = this.GetSquareBitBoard("a1") | this.GetSquareBitBoard("h1");
        // Knights are placed on b1 and g1
        this.WhitePieces["N"] = this.GetSquareBitBoard("b1") | this.GetSquareBitBoard("g1");
        // Bishops are placed on c1 and f1
        this.WhitePieces["B"] = this.GetSquareBitBoard("c1") | this.GetSquareBitBoard("f1");
        // Queen is placed on d1
        this.WhitePieces["Q"] = this.GetSquareBitBoard("d1");
        // King is placed on e1
        this.WhitePieces["K"] = this.GetSquareBitBoard("e1");
        // Pawns are placed on a2 to h2
        this.WhitePieces["P"] = this.GetSquareBitBoard("a2") | this.GetSquareBitBoard("b2") |
            this.GetSquareBitBoard("c2") | this.GetSquareBitBoard("d2") | this.GetSquareBitBoard("e2") |
            this.GetSquareBitBoard("f2") | this.GetSquareBitBoard("g2") | this.GetSquareBitBoard("h2");
    }
}
