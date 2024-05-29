namespace Minerva;


/// <summary>
/// Represents a chess board and its pieces using a set of bitboards.
/// A bitboard is a 64-bit integer where each bit represents a square on the board. In our
/// case, a bit set to 1 means that a piece is present on that square.
///
/// H1 is represented by the most significant bit (MSB) and A8 is represented by the least (LSB).
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
    public const ulong FileA = 0x0101010101010101ul;
    public const ulong FileB = 0x0202020202020202ul;
    public const ulong FileC = 0x0404040404040404ul;
    public const ulong FileD = 0x0808080808080808ul;
    public const ulong FileE = 0x1010101010101010ul;
    public const ulong FileF = 0x2020202020202020ul;
    public const ulong FileG = 0x4040404040404040ul;
    public const ulong FileH = 0x8080808080808080ul;

    public const ulong Rank1 = 0xFF00000000000000ul;
    public const ulong Rank2 = 0x00FF000000000000ul;
    public const ulong Rank3 = 0x0000FF0000000000ul;
    public const ulong Rank4 = 0x000000FF00000000ul;
    public const ulong Rank5 = 0x00000000FF000000ul;
    public const ulong Rank6 = 0x0000000000FF0000ul;
    public const ulong Rank7 = 0x000000000000FF00ul;
    public const ulong Rank8 = 0x00000000000000FFul;

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
    /// This method returns the bitboard for a given square represented by
    /// its standard chess notation.
    /// </summary>
    public ulong Square(string? square)
    {
        if (square == null)  { throw new ArgumentNullException(nameof(square)); }

        if (square.Length != 2)
        {
            throw new ArgumentException("Square notation must be 2 characters long.", nameof(square));
        }

        var file = square[0] - 'a';
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
}
