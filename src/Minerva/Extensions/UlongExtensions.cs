namespace Minerva.Extensions;

/// <summary>
/// Provides extension methods for ulong representing bitboards in a chess game.
/// </summary>
public static class UlongExtensions
{
    private const ulong NotFileA = ~Board.FileA;
    private const ulong NotFileH = ~Board.FileH;
    private const ulong NotRank1 = ~Board.Rank1;
    private const ulong NotRank8 = ~Board.Rank8;

    /// <summary>
    /// Moves a position in the specified direction.
    /// </summary>
    /// <param name="position">The bitboard position to move.</param>
    /// <param name="direction">The direction to move the position.</param>
    /// <returns>The new position after moving.</returns>
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
            _ => throw new InvalidOperationException("The move direction is invalid."),
        };
    }

    /// <summary>
    /// Moves a position one square down.
    /// </summary>
    /// <param name="bitboard">The bitboard position to move.</param>
    /// <returns>The new position after moving down.</returns>
    public static ulong MoveDown(this ulong bitboard) => (bitboard & NotRank1) >> 8;

    /// <summary>
    /// Moves a position one square down and to the left.
    /// </summary>
    /// <param name="bitboard">The bitboard of the position to move.</param>
    /// <returns>The new position after moving down and to the left.</returns>
    public static ulong MoveDownLeft(this ulong bitboard) => (bitboard & NotFileA & NotRank1) >> 7;

    /// <summary>
    /// Moves a position one square down and to the right.
    /// </summary>
    /// <param name="bitboard">The bitboard of the position to move.</param>
    /// <returns>The new position after moving down and to the right.</returns>
    public static ulong MoveDownRight(this ulong bitboard) => (bitboard & NotFileH & NotRank1) >> 9;

    /// <summary>
    /// Moves a position one square to the left.
    /// </summary>
    /// <param name="bitboard">The bitboard of the position to move.</param>
    /// <returns>The new position after moving to the left.</returns>
    public static ulong MoveLeft(this ulong bitboard) => (bitboard & NotFileA) << 1;

    /// <summary>
    /// Moves a position one square to the right.
    /// </summary>
    /// <param name="bitboard">The bitboard of the position to move.</param>
    /// <returns>The new position after moving to the right.</returns>
    public static ulong MoveRight(this ulong bitboard) => (bitboard & NotFileH) >> 1;

    /// <summary>
    /// Moves a position one square up.
    /// </summary>
    /// <param name="bitboard">The bitboard of the position to move.</param>
    /// <returns>The new position after moving up.</returns>
    public static ulong MoveUp(this ulong bitboard) => (bitboard & NotRank8) << 8;

    /// <summary>
    /// Moves a position one square up and to the left.
    /// </summary>
    /// <param name="bitboard">The bitboard of the position to move.</param>
    /// <returns>The new position after moving up and to the left.</returns>
    public static ulong MoveUpLeft(this ulong bitboard) => (bitboard & NotFileA & NotRank8) << 9;

    /// <summary>
    /// Moves a position one square up and to the right.
    /// </summary>
    /// <param name="bitboard">The bitboard of the position to move.</param>
    /// <returns>The new position after moving up and to the right.</returns>
    public static ulong MoveUpRight(this ulong bitboard) => (bitboard & NotFileH & NotRank8) << 7;
}
