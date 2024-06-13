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

namespace Minerva.Extensions;

public static class UlongExtensions
{
    public static ulong Move(this ulong position, MovingDirections direction)
    {
        if(position.TryMove(direction, out ulong newPosition))
        {
            return newPosition;
        }

        throw new InvalidOperationException("The move is off the board.");
    }

    public static ulong MoveDown(this ulong bitboard)
    {
        if (bitboard.TryMoveDown(out ulong resultBitboard))
        {
            return resultBitboard;
        }

        throw new InvalidOperationException("The move is off the board.");
    }

    public static ulong MoveDownLeft(this ulong bitboard)
    {
        if (bitboard.TryMoveDownLeft(out ulong resultBitboard))
        {
            return resultBitboard;
        }

        throw new InvalidOperationException("The move is off the board.");
    }

    public static ulong MoveDownRight(this ulong bitboard)
    {
        if (bitboard.TryMoveDownRight(out ulong resultBitboard))
        {
            return resultBitboard;
        }

        throw new InvalidOperationException("The move is off the board.");
    }

    public static ulong MoveLeft(this ulong bitboard)
    {
        if (bitboard.TryMoveLeft(out ulong resultBitboard))
        {
            return resultBitboard;
        }

        throw new InvalidOperationException("The move is off the board.");
    }

    public static ulong MoveRight(this ulong bitboard)
    {
        if (bitboard.TryMoveRight(out ulong resultBitboard))
        {
            return resultBitboard;
        }

        throw new InvalidOperationException("The move is off the board.");
    }

    public static ulong MoveUp(this ulong bitboard)
    {
        if (bitboard.TryMoveUp(out ulong resultBitboard))
        {
            return resultBitboard;
        }

        throw new InvalidOperationException("The move is off the board.");
    }

    public static ulong MoveUpLeft(this ulong bitboard)
    {
        if (bitboard.TryMoveUpLeft(out ulong resultBitboard))
        {
            return resultBitboard;
        }

        throw new InvalidOperationException("The move is off the board.");
    }

    public static ulong MoveUpRight(this ulong bitboard)
    {
        if (bitboard.TryMoveUpRight(out ulong resultBitboard))
        {
            return resultBitboard;
        }

        throw new InvalidOperationException("The move is off the board.");
    }

    public static bool TryMove(this ulong position, MovingDirections direction, out ulong newPosition)
    {
        newPosition = 0;
        switch (direction)
        {
            case MovingDirections.Down:
                return position.TryMoveDown(out newPosition);
            case MovingDirections.DownLeft:
                return position.TryMoveDownLeft(out newPosition);
            case MovingDirections.DownRight:
                return position.TryMoveDownRight(out newPosition);
            case MovingDirections.Left:
                return position.TryMoveLeft(out newPosition);
            case MovingDirections.Right:
                return position.TryMoveRight(out newPosition);
            case MovingDirections.Up:
                return position.TryMoveUp(out newPosition);
            case MovingDirections.UpLeft:
                return position.TryMoveUpLeft(out newPosition);
            case MovingDirections.UpRight:
                return position.TryMoveUpRight(out newPosition);
            case MovingDirections.UpUpRight:
                if (position.TryMoveUp(out ulong intermediatePosition4) &&
                    intermediatePosition4.TryMoveUpRight(out newPosition))
                {
                    return true;
                }

                return false;
            case MovingDirections.UpUpLeft:
                if (position.TryMoveUp(out ulong intermediatePosition3) &&
                    intermediatePosition3.TryMoveUpLeft(out newPosition))
                {
                    return true;
                }

                return false;
            case MovingDirections.DownDownRight:
                if (position.TryMoveDown(out ulong intermediatePosition2) &&
                    intermediatePosition2.TryMoveDownRight(out newPosition))
                {
                    return true;
                }

                return false;
            case MovingDirections.DownDownLeft:
                if (position.TryMoveDown(out ulong intermediatePosition) &&
                    intermediatePosition.TryMoveDownLeft(out newPosition))
                {
                    return true;
                }

                return false;
            case MovingDirections.UpRightRight:
                if (position.TryMoveUpRight(out ulong intermediatePosition7) &&
                    intermediatePosition7.TryMoveRight(out newPosition))
                {
                    return true;
                }

                return false;
            case MovingDirections.UpLeftLeft:
                if (position.TryMoveUpLeft(out ulong intermediatePosition6) &&
                    intermediatePosition6.TryMoveLeft(out newPosition))
                {
                    return true;
                }

                return false;
            case MovingDirections.DownRightRight:
                if (position.TryMoveDownRight(out ulong intermediatePosition8) &&
                    intermediatePosition8.TryMoveRight(out newPosition))
                {
                    return true;
                }

                return false;
            case MovingDirections.DownLeftLeft:
                if (position.TryMoveLeft(out ulong intermediatePosition5) &&
                    intermediatePosition5.TryMoveDownLeft(out newPosition))
                {
                    return true;
                }

                return false;
            case MovingDirections.None:
            case MovingDirections.Rook:
            case MovingDirections.Bishop:
            case MovingDirections.KingAndQueen:
            default:
                throw new ArgumentException("A move must be in one single direction.", nameof(direction));
        }
    }

    public static bool TryMoveDown(this ulong bitboard, out ulong resultBitboard)
    {
        // Check if the piece is on the 1st rank.
        if ((bitboard & Board.Rank1) != 0ul)
        {
            resultBitboard = 0;
            return false;
        }

        resultBitboard = bitboard >> 8;
        return true;
    }

    public static bool TryMoveDownLeft(this ulong bitboard, out ulong resultBitboard)
    {
        // Check if the piece is on the a file or the 1st rank.
        if ((bitboard & (Board.FileA | Board.Rank1)) != 0ul)
        {
            resultBitboard = 0;
            return false;
        }

        resultBitboard = bitboard >> 7;
        return true;
    }

    public static bool TryMoveDownRight(this ulong bitboard, out ulong resultBitboard)
    {
        // Check if the piece is on the h file or the 1st rank.
        if ((bitboard & (Board.FileH | Board.Rank1)) != 0ul)
        {
            resultBitboard = 0;
            return false;
        }

        resultBitboard = bitboard >> 9;
        return true;
    }

    public static bool TryMoveLeft(this ulong bitboard, out ulong resultBitboard)
    {
        // Check if the piece is on the a file.
        if ((bitboard & Board.FileA) != 0ul)
        {
            resultBitboard = 0;
            return false;
        }

        resultBitboard = bitboard << 1;
        return true;
    }

    public static bool TryMoveRight(this ulong bitboard, out ulong resultBitboard)
    {
        // Check if the piece is on the h file.
        if ((bitboard & Board.FileH) != 0ul)
        {
            resultBitboard = 0;
            return false;
        }

        resultBitboard = bitboard >> 1;
        return true;
    }

    public static bool TryMoveUp(this ulong bitboard, out ulong resultBitboard)
    {
        // Check if the piece is on the 8th rank.
        if ((bitboard & Board.Rank8) != 0ul)
        {
            resultBitboard = 0;
            return false;
        }

        resultBitboard = bitboard << 8;
        return true;
    }

    public static bool TryMoveUpLeft(this ulong bitboard, out ulong resultBitboard)
    {
        // Check if the piece is on the a file or the 8th rank.
        if ((bitboard & (Board.FileA | Board.Rank8)) != 0ul)
        {
            resultBitboard = 0;
            return false;
        }

        resultBitboard = bitboard << 9;
        return true;
    }

    public static bool TryMoveUpRight(this ulong bitboard, out ulong resultBitboard)
    {
        // Check if the piece is on the h file or the 8th rank.
        if ((bitboard & (Board.FileH | Board.Rank8)) != 0ul)
        {
            resultBitboard = 0;
            return false;
        }

        resultBitboard = bitboard << 7;
        return true;
    }
}