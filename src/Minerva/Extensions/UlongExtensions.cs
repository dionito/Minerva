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
    /// Determines if exactly one bit is set in the bitboard.
    /// </summary>
    /// <param name="bitboard">The bitboard to check.</param>
    /// <returns><c>true</c> if exactly one bit is set; otherwise, <c>false</c>.</returns>
    public static bool IsSingleBitSet(this ulong bitboard)
    {
        return bitboard != 0 && (bitboard & (bitboard - 1)) == 0;
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
}