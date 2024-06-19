using System.Numerics;

namespace Minerva.Extensions;

/// <summary>
/// Provides extension methods for ulong representing bitboards in a chess game.
/// </summary>
public static class UlongExtensions
{
    private const ulong NotFileA = ~Board.FileA;
    private const ulong NotFileB = ~Board.FileB;
    private const ulong NotFileG = ~Board.FileG;
    private const ulong NotFileH = ~Board.FileH;
    private const ulong NotRank1 = ~Board.Rank1;
    private const ulong NotRank2 = ~Board.Rank2;
    private const ulong NotRank7 = ~Board.Rank7;
    private const ulong NotRank8 = ~Board.Rank8;

    /// <summary>
    /// Clears a bit at the specified index in the bitboard.
    /// </summary>
    /// <param name="bitboard">The bitboard to modify.</param>
    /// <param name="index">The zero-based index of the bit to clear.</param>
    /// <returns>A new bitboard with the specified bit cleared.</returns>
    public static ulong ClearBit(this ulong bitboard, int index) => bitboard & ~(1UL << index);

    /// <summary>
    /// Determines if the bitboard is empty (i.e., all bits are unset).
    /// </summary>
    /// <param name="bitboard">The bitboard to check.</param>
    /// <returns><c>true</c> if the bitboard is empty; otherwise, <c>false</c>.</returns>
    public static bool IsEmpty(this ulong bitboard) => bitboard == 0;

    /// <summary>
    /// Determines if exactly one bit is set in the bitboard.
    /// </summary>
    /// <param name="bitboard">The bitboard to check.</param>
    /// <returns><c>true</c> if exactly one bit is set; otherwise, <c>false</c>.</returns>
    public static bool IsSingleBitSet(this ulong bitboard)
    {
        return bitboard != 0 && (bitboard & (bitboard - 1)) == 0;
    }

    /// <summary>
    /// Determines if exactly one bit is set in the bitboard and outputs the index of the set bit.
    /// </summary>
    /// <param name="bitboard">The bitboard to check.</param>
    /// <param name="index">When this method returns, contains the zero-based index of the set bit if exactly one bit is set; otherwise, -1.</param>
    /// <returns><c>true</c> if exactly one bit is set; otherwise, <c>false</c>.</returns>
    public static bool IsSingleBitSet(this ulong bitboard, out int index)
    {
        index = -1;
        if (bitboard == 0 || (bitboard & (bitboard - 1)) != 0)
        {
            return false;
        }

        index = BitOperations.TrailingZeroCount(bitboard);
        return true;
    }

    /// <summary>
    /// Calculates the Hamming distance between two bitboards.
    /// </summary>
    /// <param name="bitboard">The first bitboard.</param>
    /// <param name="other">The second bitboard to compare.</param>
    /// <returns>The Hamming distance between the two bitboards.</returns>
    public static int HammingDistance(this ulong bitboard, ulong other) => (bitboard ^ other).PopCount();

    /// <summary>
    /// Moves to a position in the specified direction.
    /// </summary>
    /// <param name="position">The bitboard position to move.</param>
    /// <param name="direction">The direction to move the position.</param>
    /// <returns>The new position after moving, or zero if the move is not valid.</returns>
    /// <exception cref="InvalidOperationException">Thrown when an invalid move direction is specified.</exception>
    public static ulong Move(this ulong position, MovingDirections direction)
    {
        return direction switch
        {
            MovingDirections.Down => MoveDown(position),
            MovingDirections.DownLeft => MoveDownLeft(position),
            MovingDirections.DownRight => MoveDownRight(position),
            MovingDirections.Left => MoveLeft(position),
            MovingDirections.Right => MoveRight(position),
            MovingDirections.Up => MoveUp(position),
            MovingDirections.UpLeft => MoveUpLeft(position),
            MovingDirections.UpRight => MoveUpRight(position),
            MovingDirections.Up | MovingDirections.UpRight => MoveUpUpRight(position),
            MovingDirections.Up | MovingDirections.UpLeft => MoveUpUpLeft(position),
            MovingDirections.Down | MovingDirections.DownRight => MoveDownDownRight(position),
            MovingDirections.Down | MovingDirections.DownLeft => MoveDownDownLeft(position),
            MovingDirections.Right | MovingDirections.UpRight => MoveRightUpRight(position),
            MovingDirections.Right | MovingDirections.DownRight => MoveRightDownRight(position),
            MovingDirections.Left | MovingDirections.UpLeft => MoveLeftUpLeft(position),
            MovingDirections.Left | MovingDirections.DownLeft => MoveLeftDownLeft(position),
            _ => throw new InvalidOperationException("The move direction is invalid."),
        };
    }

    /// <summary>
    /// Moves to a position one square down.
    /// </summary>
    /// <param name="position">The bitboard position to move.</param>
    /// <returns>The new position after moving down, or zero if the move is not valid.</returns>
    public static ulong MoveDown(this ulong position) => (position & NotRank1) >> 8;

    /// <summary>
    /// Moves to a position two squares down and one square to the left (L shaped).
    /// </summary>
    /// <param name="position">The bitboard position to move.</param>
    /// <returns>The new position after moving down twice and to the left once,
    /// or zero if the move is not valid.</returns>
    private static ulong MoveDownDownLeft(ulong position) => (position & NotRank1 & NotRank2 & NotFileA) >> 15;

    /// <summary>
    /// Moves to a position two squares down and one square to the right (L shaped).
    /// </summary>
    /// <param name="position">The bitboard position to move.</param>
    /// <returns>The new position after moving down twice and to the right once,
    /// or zero if the move is not valid.</returns>
    private static ulong MoveDownDownRight(ulong position) => (position & NotRank1 & NotRank2 & NotFileH) >> 17;

    /// <summary>
    /// Moves to a position one square down and to the left.
    /// </summary>
    /// <param name="position">The bitboard of the position to move.</param>
    /// <returns>The new position after moving down and to the left,
    /// or zero if the move is not valid.</returns>
    public static ulong MoveDownLeft(this ulong position) => (position & NotFileA & NotRank1) >> 7;

    /// <summary>
    /// Moves to a position one square down and to the right.
    /// </summary>
    /// <param name="position">The bitboard of the position to move.</param>
    /// <returns>The new position after moving down and to the right,
    /// or zero if the move is not valid.</returns>
    public static ulong MoveDownRight(this ulong position) => (position & NotFileH & NotRank1) >> 9;

    /// <summary>
    /// Moves to a position one square to the left.
    /// </summary>
    /// <param name="position">The bitboard of the position to move.</param>
    /// <returns>The new position after moving to the left,
    /// or zero if the move is not valid.</returns>
    public static ulong MoveLeft(this ulong position) => (position & NotFileA) << 1;

    /// <summary>
    /// Moves to a position one square to the left and then one square down (L shaped).
    /// </summary>
    /// <param name="position">The bitboard position to move.</param>
    /// <returns>The new position after moving left once and down once,
    /// or zero if the move is not valid.</returns>
    private static ulong MoveLeftDownLeft(ulong position) => (position & NotFileA & NotFileB & NotRank1) >> 6;

    /// <summary>
    /// Moves to a position one square to the left and then one square up (L shaped).
    /// </summary>
    /// <param name="position">The bitboard position to move.</param>
    /// <returns>The new position after moving left once and up once,
    /// or zero if the move is not valid.</returns>
    private static ulong MoveLeftUpLeft(ulong position) => (position & NotFileA & NotFileB & NotRank8) << 10;

    /// <summary>
    /// Moves to a position one square to the right.
    /// </summary>
    /// <param name="position">The bitboard of the position to move.</param>
    /// <returns>The new position after moving to the right,
    /// or zero if the move is not valid.</returns>
    public static ulong MoveRight(this ulong position) => (position & NotFileH) >> 1;

    /// <summary>
    /// Moves to a position one square to the right and then one square down (L shaped).
    /// </summary>
    /// <param name="position">The bitboard position to move.</param>
    /// <returns>The new position after moving right once and down once,
    /// or zero if the move is not valid.</returns>
    private static ulong MoveRightDownRight(ulong position) => (position & NotFileH & NotFileG & NotRank1) >> 10;

    /// <summary>
    /// Moves to a position one square to the right and then one square up (L shaped).
    /// </summary>
    /// <param name="position">The bitboard position to move.</param>
    /// <returns>The new position after moving right once and up once,
    /// or zero if the move is not valid.</returns>
    private static ulong MoveRightUpRight(ulong position) => (position & NotFileH & NotFileG & NotRank8) << 6;

    /// <summary>
    /// Moves to a position one square up.
    /// </summary>
    /// <param name="position">The bitboard of the position to move.</param>
    /// <returns>The new position after moving up,
    /// or zero if the move is not valid.</returns>
    public static ulong MoveUp(this ulong position) => (position & NotRank8) << 8;

    /// <summary>
    /// Moves to a position one square up and to the left.
    /// </summary>
    /// <param name="position">The bitboard of the position to move.</param>
    /// <returns>The new position after moving up and to the left,
    /// or zero if the move is not valid.</returns>
    public static ulong MoveUpLeft(this ulong position) => (position & NotFileA & NotRank8) << 9;

    /// <summary>
    /// Moves to a position one square up and to the right.
    /// </summary>
    /// <param name="position">The bitboard of the position to move.</param>
    /// <returns>The new position after moving up and to the right,
    /// or zero if the move is not valid.</returns>
    public static ulong MoveUpRight(this ulong position) => (position & NotFileH & NotRank8) << 7;

    /// <summary>
    /// Moves to a position two squares up and one square to the left (L shaped).
    /// </summary>
    /// <param name="position">The bitboard position to move.</param>
    /// <returns>The new position after moving up twice and to the left once,
    /// or zero if the move is not valid..</returns>
    private static ulong MoveUpUpLeft(ulong position) => (position & NotRank8 & NotRank7 & NotFileA) << 17;

    /// <summary>
    /// Moves to a position two squares up and one square to the right (L shaped).
    /// </summary>
    /// <param name="position">The bitboard position to move.</param>
    /// <returns>The new position after moving up twice and to the right once,
    /// or zero if the move is not valid.</returns>
    private static ulong MoveUpUpRight(ulong position) => (position & NotRank8 & NotRank7 & NotFileH) << 15;

    /// <summary>
    /// Counts the number of set bits (1s) in the bitboard.
    /// </summary>
    /// <param name="bitboard">The bitboard to count set bits in.</param>
    /// <returns>The number of set bits in the bitboard.</returns>
    public static int PopCount(this ulong bitboard) => BitOperations.PopCount(bitboard);

    /// <summary>
    /// Sets a bit at the specified index.
    /// </summary>
    /// <param name="bitboard">The bitboard to modify.</param>
    /// <param name="index">The zero-based index of the bit to set.</param>
    /// <returns>A new bitboard with the specified bit set.</returns>
    public static ulong SetBit(this ulong bitboard, int index) => bitboard | (1UL << index);

    /// <summary>
    /// Toggles a bit at the specified index.
    /// </summary>
    /// <param name="bitboard">The bitboard to modify.</param>
    /// <param name="index">The zero-based index of the bit to toggle.</param>
    /// <returns>A new bitboard with the specified bit toggled.</returns>
    public static ulong ToggleBit(this ulong bitboard, int index) => bitboard ^ (1UL << index);

    /// <summary>
    /// Toggles bits at the specified indices.
    /// </summary>
    /// <param name="bitboard">The bitboard to modify.</param>
    /// <param name="indices">An array of zero-based indices of the bits to toggle.</param>
    /// <returns>A new bitboard with the specified bits toggled.</returns>
    public static ulong ToggleBits(this ulong bitboard, params int[] indices)
    {
        ulong mask = 0;
        foreach (int index in indices)
        {
            mask |= 1UL << index;
        }
        return bitboard ^ mask;
    }

    /// <summary>
    /// Toggles bits according to a mask.
    /// </summary>
    /// <param name="bitboard">The bitboard to modify.</param>
    /// <param name="mask">The mask indicating which bits to toggle.</param>
    /// <returns>A new bitboard with bits toggled according to the mask.</returns>
    public static ulong ToggleBits(this ulong bitboard, ulong mask) => bitboard ^ mask;
}