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
using Minerva.Pieces;
using System.Text.RegularExpressions;

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
    public const char EmptySquare = ' ';

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

    public static readonly Dictionary<ulong, ulong> BishopXRay = new()
    {
        { FileA & Rank1, 0x102040810204000ul },
        { FileA & Rank2, 0x204081020400040ul },
        { FileA & Rank3, 0x408102040004020ul },
        { FileA & Rank4, 0x810204000402010ul },
        { FileA & Rank5, 0x1020400040201008ul },
        { FileA & Rank6, 0x2040004020100804ul },
        { FileA & Rank7, 0x4000402010080402ul },
        { FileA & Rank8, 0x40201008040201ul },
        { FileB & Rank1, 0x102040810A000ul },
        { FileB & Rank2, 0x102040810A000A0ul },
        { FileB & Rank3, 0x2040810A000A010ul },
        { FileB & Rank4, 0x40810A000A01008ul },
        { FileB & Rank5, 0x810A000A0100804ul },
        { FileB & Rank6, 0x10A000A010080402ul },
        { FileB & Rank7, 0xA000A01008040201ul },
        { FileB & Rank8, 0xA0100804020100ul },
        { FileC & Rank1, 0x10204885000ul },
        { FileC & Rank2, 0x1020488500050ul },
        { FileC & Rank3, 0x102048850005088ul },
        { FileC & Rank4, 0x204885000508804ul },
        { FileC & Rank5, 0x488500050880402ul },
        { FileC & Rank6, 0x8850005088040201ul },
        { FileC & Rank7, 0x5000508804020100ul },
        { FileC & Rank8, 0x50880402010000ul },
        { FileD & Rank1, 0x182442800ul },
        { FileD & Rank2, 0x18244280028ul },
        { FileD & Rank3, 0x1824428002844ul },
        { FileD & Rank4, 0x182442800284482ul },
        { FileD & Rank5, 0x8244280028448201ul },
        { FileD & Rank6, 0x4428002844820100ul },
        { FileD & Rank7, 0x2800284482010000ul },
        { FileD & Rank8, 0x28448201000000ul },
        { FileE & Rank1, 0x8041221400ul },
        { FileE & Rank2, 0x804122140014ul },
        { FileE & Rank3, 0x80412214001422ul },
        { FileE & Rank4, 0x8041221400142241ul },
        { FileE & Rank5, 0x4122140014224180ul },
        { FileE & Rank6, 0x2214001422418000ul },
        { FileE & Rank7, 0x1400142241800000ul },
        { FileE & Rank8, 0x14224180000000ul },
        { FileF & Rank1, 0x804020110A00ul },
        { FileF & Rank2, 0x804020110A000Aul },
        { FileF & Rank3, 0x804020110A000A11ul },
        { FileF & Rank4, 0x4020110A000A1120ul },
        { FileF & Rank5, 0x20110A000A112040ul },
        { FileF & Rank6, 0x110A000A11204080ul },
        { FileF & Rank7, 0xA000A1120408000ul },
        { FileF & Rank8, 0xA112040800000ul },
        { FileG & Rank1, 0x80402010080500ul },
        { FileG & Rank2, 0x8040201008050005ul },
        { FileG & Rank3, 0x4020100805000508ul },
        { FileG & Rank4, 0x2010080500050810ul },
        { FileG & Rank5, 0x1008050005081020ul },
        { FileG & Rank6, 0x805000508102040ul },
        { FileG & Rank7, 0x500050810204080ul },
        { FileG & Rank8, 0x5081020408000ul },
        { FileH & Rank1, 0x8040201008040200ul },
        { FileH & Rank2, 0x4020100804020002ul },
        { FileH & Rank3, 0x2010080402000204ul },
        { FileH & Rank4, 0x1008040200020408ul },
        { FileH & Rank5, 0x804020002040810ul },
        { FileH & Rank6, 0x402000204081020ul },
        { FileH & Rank7, 0x200020408102040ul },
        { FileH & Rank8, 0x2040810204080ul },
    };

    public static readonly Dictionary<ulong, ulong> Diagonals = new()
    {
        { FileA & Rank1, 0x102040810204080ul },
        { FileA & Rank2, 0x204081020408040ul },
        { FileA & Rank3, 0x408102040804020ul },
        { FileA & Rank4, 0x810204080402010ul },
        { FileA & Rank5, 0x1020408040201008ul },
        { FileA & Rank6, 0x2040804020100804ul },
        { FileA & Rank7, 0x4080402010080402ul },
        { FileA & Rank8, 0x8040201008040201ul },
        { FileB & Rank1, 0x102040810A040ul },
        { FileB & Rank2, 0x102040810A040A0ul },
        { FileB & Rank3, 0x2040810A040A010ul },
        { FileB & Rank4, 0x40810A040A01008ul },
        { FileB & Rank5, 0x810A040A0100804ul },
        { FileB & Rank6, 0x10A040A010080402ul },
        { FileB & Rank7, 0xA040A01008040201ul },
        { FileB & Rank8, 0x40A0100804020100ul },
        { FileC & Rank1, 0x10204885020ul },
        { FileC & Rank2, 0x1020488502050ul },
        { FileC & Rank3, 0x102048850205088ul },
        { FileC & Rank4, 0x204885020508804ul },
        { FileC & Rank5, 0x488502050880402ul },
        { FileC & Rank6, 0x8850205088040201ul },
        { FileC & Rank7, 0x5020508804020100ul },
        { FileC & Rank8, 0x2050880402010000ul },
        { FileD & Rank1, 0x182442810ul },
        { FileD & Rank2, 0x18244281028ul },
        { FileD & Rank3, 0x1824428102844ul },
        { FileD & Rank4, 0x182442810284482ul },
        { FileD & Rank5, 0x8244281028448201ul },
        { FileD & Rank6, 0x4428102844820100ul },
        { FileD & Rank7, 0x2810284482010000ul },
        { FileD & Rank8, 0x1028448201000000ul },
        { FileE & Rank1, 0x8041221408ul },
        { FileE & Rank2, 0x804122140814ul },
        { FileE & Rank3, 0x80412214081422ul },
        { FileE & Rank4, 0x8041221408142241ul },
        { FileE & Rank5, 0x4122140814224180ul },
        { FileE & Rank6, 0x2214081422418000ul },
        { FileE & Rank7, 0x1408142241800000ul },
        { FileE & Rank8, 0x814224180000000ul },
        { FileF & Rank1, 0x804020110A04ul },
        { FileF & Rank2, 0x804020110A040Aul },
        { FileF & Rank3, 0x804020110A040A11ul },
        { FileF & Rank4, 0x4020110A040A1120ul },
        { FileF & Rank5, 0x20110A040A112040ul },
        { FileF & Rank6, 0x110A040A11204080ul },
        { FileF & Rank7, 0xA040A1120408000ul },
        { FileF & Rank8, 0x40A112040800000ul },
        { FileG & Rank1, 0x80402010080502ul },
        { FileG & Rank2, 0x8040201008050205ul },
        { FileG & Rank3, 0x4020100805020508ul },
        { FileG & Rank4, 0x2010080502050810ul },
        { FileG & Rank5, 0x1008050205081020ul },
        { FileG & Rank6, 0x805020508102040ul },
        { FileG & Rank7, 0x502050810204080ul },
        { FileG & Rank8, 0x205081020408000ul },
        { FileH & Rank1, 0x8040201008040201ul },
        { FileH & Rank2, 0x4020100804020102ul },
        { FileH & Rank3, 0x2010080402010204ul },
        { FileH & Rank4, 0x1008040201020408ul },
        { FileH & Rank5, 0x804020102040810ul },
        { FileH & Rank6, 0x402010204081020ul },
        { FileH & Rank7, 0x201020408102040ul },
        { FileH & Rank8, 0x102040810204080ul },
    };

    public static readonly Dictionary<ulong, ulong> KingXRay = new()
    {
        { FileA & Rank1, 0xC040ul },
        { FileA & Rank2, 0xC040C0ul },
        { FileA & Rank3, 0xC040C000ul },
        { FileA & Rank4, 0xC040C00000ul },
        { FileA & Rank5, 0xC040C0000000ul },
        { FileA & Rank6, 0xC040C000000000ul },
        { FileA & Rank7, 0xC040C00000000000ul },
        { FileA & Rank8, 0x40C0000000000000ul },
        { FileB & Rank1, 0xE0A0ul },
        { FileB & Rank2, 0xE0A0E0ul },
        { FileB & Rank3, 0xE0A0E000ul },
        { FileB & Rank4, 0xE0A0E00000ul },
        { FileB & Rank5, 0xE0A0E0000000ul },
        { FileB & Rank6, 0xE0A0E000000000ul },
        { FileB & Rank7, 0xE0A0E00000000000ul },
        { FileB & Rank8, 0xA0E0000000000000ul },
        { FileC & Rank1, 0x7050ul },
        { FileC & Rank2, 0x705070ul },
        { FileC & Rank3, 0x70507000ul },
        { FileC & Rank4, 0x7050700000ul },
        { FileC & Rank5, 0x705070000000ul },
        { FileC & Rank6, 0x70507000000000ul },
        { FileC & Rank7, 0x7050700000000000ul },
        { FileC & Rank8, 0x5070000000000000ul },
        { FileD & Rank1, 0x3828ul },
        { FileD & Rank2, 0x382838ul },
        { FileD & Rank3, 0x38283800ul },
        { FileD & Rank4, 0x3828380000ul },
        { FileD & Rank5, 0x382838000000ul },
        { FileD & Rank6, 0x38283800000000ul },
        { FileD & Rank7, 0x3828380000000000ul },
        { FileD & Rank8, 0x2838000000000000ul },
        { FileE & Rank1, 0x1C14ul },
        { FileE & Rank2, 0x1C141Cul },
        { FileE & Rank3, 0x1C141C00ul },
        { FileE & Rank4, 0x1C141C0000ul },
        { FileE & Rank5, 0x1C141C000000ul },
        { FileE & Rank6, 0x1C141C00000000 },
        { FileE & Rank7, 0x1C141C0000000000ul },
        { FileE & Rank8, 0x141C000000000000ul },
        { FileF & Rank1, 0xE0Aul },
        { FileF & Rank2, 0xE0A0Eul },
        { FileF & Rank3, 0xE0A0E00ul },
        { FileF & Rank4, 0xE0A0E0000ul },
        { FileF & Rank5, 0xE0A0E000000ul },
        { FileF & Rank6, 0xE0A0E00000000ul },
        { FileF & Rank7, 0xE0A0E0000000000ul },
        { FileF & Rank8, 0xA0E000000000000ul },
        { FileG & Rank1, 0x705ul },
        { FileG & Rank2, 0x70507ul },
        { FileG & Rank3, 0x7050700ul },
        { FileG & Rank4, 0x705070000ul },
        { FileG & Rank5, 0x70507000000ul },
        { FileG & Rank6, 0x7050700000000ul },
        { FileG & Rank7, 0x705070000000000ul },
        { FileG & Rank8, 0x507000000000000ul },
        { FileH & Rank1, 0x302ul },
        { FileH & Rank2, 0x30203ul },
        { FileH & Rank3, 0x3020300ul },
        { FileH & Rank4, 0x302030000ul },
        { FileH & Rank5, 0x30203000000ul },
        { FileH & Rank6, 0x3020300000000 },
        { FileH & Rank7, 0x302030000000000 },
        { FileH & Rank8, 0x203000000000000ul },
    };

    public static readonly Dictionary<ulong, ulong> KnightXRay = new()
    {
        { FileA & Rank1, 0x402000ul },
        { FileA & Rank2, 0x40200020ul },
        { FileA & Rank3, 0x4020002040ul },
        { FileA & Rank4, 0x402000204000ul },
        { FileA & Rank5, 0x40200020400000ul },
        { FileA & Rank6, 0x4020002040000000ul },
        { FileA & Rank7, 0x2000204000000000ul },
        { FileA & Rank8, 0x20400000000000ul },
        { FileB & Rank1, 0xA01000ul },
        { FileB & Rank2, 0xA0100010ul },
        { FileB & Rank3, 0xA0100010A0ul },
        { FileB & Rank4, 0xA0100010A000ul },
        { FileB & Rank5, 0xA0100010A00000ul },
        { FileB & Rank6, 0xA0100010A0000000ul },
        { FileB & Rank7, 0x100010A000000000ul },
        { FileB & Rank8, 0x10A00000000000ul },
        { FileC & Rank1, 0x508800ul },
        { FileC & Rank2, 0x50880088ul },
        { FileC & Rank3, 0x5088008850ul },
        { FileC & Rank4, 0x508800885000ul },
        { FileC & Rank5, 0x50880088500000ul },
        { FileC & Rank6, 0x5088008850000000ul },
        { FileC & Rank7, 0x8800885000000000ul },
        { FileC & Rank8, 0x88500000000000ul },
        { FileD & Rank1, 0x284400ul },
        { FileD & Rank2, 0x28440044ul },
        { FileD & Rank3, 0x2844004428ul },
        { FileD & Rank4, 0x284400442800ul },
        { FileD & Rank5, 0x28440044280000ul },
        { FileD & Rank6, 0x2844004428000000ul },
        { FileD & Rank7, 0x4400442800000000ul },
        { FileD & Rank8, 0x44280000000000ul },
        { FileE & Rank1, 0x142200ul },
        { FileE & Rank2, 0x14220022ul },
        { FileE & Rank3, 0x1422002214ul },
        { FileE & Rank4, 0x142200221400ul },
        { FileE & Rank5, 0x14220022140000ul },
        { FileE & Rank6, 0x1422002214000000ul },
        { FileE & Rank7, 0x2200221400000000ul },
        { FileE & Rank8, 0x22140000000000ul },
        { FileF & Rank1, 0xA1100ul },
        { FileF & Rank2, 0xA110011ul },
        { FileF & Rank3, 0xA1100110Aul },
        { FileF & Rank4, 0xA1100110A00ul },
        { FileF & Rank5, 0xA1100110A0000ul },
        { FileF & Rank6, 0xA1100110A000000ul },
        { FileF & Rank7, 0x1100110A00000000ul },
        { FileF & Rank8, 0x110A0000000000ul },
        { FileG & Rank1, 0x50800ul },
        { FileG & Rank2, 0x5080008ul },
        { FileG & Rank3, 0x508000805ul },
        { FileG & Rank4, 0x50800080500ul },
        { FileG & Rank5, 0x5080008050000ul },
        { FileG & Rank6, 0x508000805000000ul },
        { FileG & Rank7, 0x800080500000000ul },
        { FileG & Rank8, 0x8050000000000ul },
        { FileH & Rank1, 0x20400ul },
        { FileH & Rank2, 0x2040004ul },
        { FileH & Rank3, 0x204000402ul },
        { FileH & Rank4, 0x20400040200ul },
        { FileH & Rank5, 0x2040004020000ul },
        { FileH & Rank6, 0x204000402000000ul },
        { FileH & Rank7, 0x400040200000000ul },
        { FileH & Rank8, 0x4020000000000ul },
    };

    public static readonly Dictionary<ulong, ulong> PawnDefensesXRayBlack = new()
    {
        { FileA & Rank2, 0x40ul },
        { FileA & Rank3, 0x4000ul },
        { FileA & Rank4, 0x400000ul },
        { FileA & Rank5, 0x40000000ul },
        { FileA & Rank6, 0x4000000000ul },
        { FileA & Rank7, 0x400000000000ul },
        { FileB & Rank2, 0xA0ul },
        { FileB & Rank3, 0xA000ul },
        { FileB & Rank4, 0xA00000ul },
        { FileB & Rank5, 0xA0000000ul },
        { FileB & Rank6, 0xA000000000ul },
        { FileB & Rank7, 0xA00000000000ul },
        { FileC & Rank2, 0x50ul },
        { FileC & Rank3, 0x5000ul },
        { FileC & Rank4, 0x500000ul },
        { FileC & Rank5, 0x50000000ul },
        { FileC & Rank6, 0x5000000000ul },
        { FileC & Rank7, 0x500000000000ul },
        { FileD & Rank2, 0x28ul },
        { FileD & Rank3, 0x2800ul },
        { FileD & Rank4, 0x280000ul },
        { FileD & Rank5, 0x28000000ul },
        { FileD & Rank6, 0x2800000000ul },
        { FileD & Rank7, 0x280000000000ul },
        { FileE & Rank2, 0x14ul },
        { FileE & Rank3, 0x1400ul },
        { FileE & Rank4, 0x140000ul },
        { FileE & Rank5, 0x14000000ul },
        { FileE & Rank6, 0x1400000000ul },
        { FileE & Rank7, 0x140000000000ul },
        { FileF & Rank2, 0xAul },
        { FileF & Rank3, 0xA00ul },
        { FileF & Rank4, 0xA0000ul },
        { FileF & Rank5, 0xA000000ul },
        { FileF & Rank6, 0xA00000000ul },
        { FileF & Rank7, 0xA0000000000ul },
        { FileG & Rank2, 0x5ul },
        { FileG & Rank3, 0x500ul },
        { FileG & Rank4, 0x50000ul },
        { FileG & Rank5, 0x5000000ul },
        { FileG & Rank6, 0x500000000ul },
        { FileG & Rank7, 0x50000000000ul },
        { FileH & Rank2, 0x2ul },
        { FileH & Rank3, 0x200ul },
        { FileH & Rank4, 0x20000ul },
        { FileH & Rank5, 0x2000000ul },
        { FileH & Rank6, 0x200000000ul },
        { FileH & Rank7, 0x20000000000ul },
    };

    public static readonly Dictionary<ulong, ulong> PawnDefensesXRayWhite = new()
    {
        { FileA & Rank2, 0x400000ul },
        { FileA & Rank3, 0x40000000ul },
        { FileA & Rank4, 0x4000000000ul },
        { FileA & Rank5, 0x400000000000ul },
        { FileA & Rank6, 0x40000000000000ul },
        { FileA & Rank7, 0x4000000000000000ul },
        { FileB & Rank2, 0xA00000ul },
        { FileB & Rank3, 0xA0000000ul },
        { FileB & Rank4, 0xA000000000ul },
        { FileB & Rank5, 0xA00000000000ul },
        { FileB & Rank6, 0xA0000000000000ul },
        { FileB & Rank7, 0xA000000000000000ul },
        { FileC & Rank2, 0x500000ul },
        { FileC & Rank3, 0x50000000ul },
        { FileC & Rank4, 0x5000000000ul },
        { FileC & Rank5, 0x500000000000ul },
        { FileC & Rank6, 0x50000000000000ul },
        { FileC & Rank7, 0x5000000000000000ul },
        { FileD & Rank2, 0x280000ul },
        { FileD & Rank3, 0x28000000ul },
        { FileD & Rank4, 0x2800000000ul },
        { FileD & Rank5, 0x280000000000ul },
        { FileD & Rank6, 0x28000000000000ul },
        { FileD & Rank7, 0x2800000000000000ul },
        { FileE & Rank2, 0x140000ul },
        { FileE & Rank3, 0x14000000ul },
        { FileE & Rank4, 0x1400000000ul },
        { FileE & Rank5, 0x140000000000ul },
        { FileE & Rank6, 0x14000000000000ul },
        { FileE & Rank7, 0x1400000000000000ul },
        { FileF & Rank2, 0xA0000ul },
        { FileF & Rank3, 0xA000000ul },
        { FileF & Rank4, 0xA00000000ul },
        { FileF & Rank5, 0xA0000000000ul },
        { FileF & Rank6, 0xA000000000000ul },
        { FileF & Rank7, 0xA00000000000000ul },
        { FileG & Rank2, 0x50000ul },
        { FileG & Rank3, 0x5000000ul },
        { FileG & Rank4, 0x500000000ul },
        { FileG & Rank5, 0x50000000000ul },
        { FileG & Rank6, 0x5000000000000ul },
        { FileG & Rank7, 0x500000000000000ul },
        { FileH & Rank2, 0x20000ul },
        { FileH & Rank3, 0x2000000ul },
        { FileH & Rank4, 0x200000000ul },
        { FileH & Rank5, 0x20000000000ul },
        { FileH & Rank6, 0x2000000000000ul },
        { FileH & Rank7, 0x200000000000000ul },
    };

    public static readonly Dictionary<ulong, ulong> PawnMovesXRayBlack = new()
    {
        { FileA & Rank2, 0x80ul },
        { FileA & Rank3, 0x8000ul },
        { FileA & Rank4, 0x800000ul },
        { FileA & Rank5, 0x80000000ul },
        { FileA & Rank6, 0x8000000000ul },
        { FileA & Rank7, 0x808000000000ul },
        { FileB & Rank2, 0x40ul },
        { FileB & Rank3, 0x4000ul },
        { FileB & Rank4, 0x400000ul },
        { FileB & Rank5, 0x40000000ul },
        { FileB & Rank6, 0x4000000000ul },
        { FileB & Rank7, 0x404000000000ul },
        { FileC & Rank2, 0x20ul },
        { FileC & Rank3, 0x2000ul },
        { FileC & Rank4, 0x200000ul },
        { FileC & Rank5, 0x20000000ul },
        { FileC & Rank6, 0x2000000000ul },
        { FileC & Rank7, 0x202000000000ul },
        { FileD & Rank2, 0x10ul },
        { FileD & Rank3, 0x1000ul },
        { FileD & Rank4, 0x100000ul },
        { FileD & Rank5, 0x10000000ul },
        { FileD & Rank6, 0x1000000000ul },
        { FileD & Rank7, 0x101000000000ul },
        { FileE & Rank2, 0x8ul },
        { FileE & Rank3, 0x800ul },
        { FileE & Rank4, 0x80000ul },
        { FileE & Rank5, 0x8000000ul },
        { FileE & Rank6, 0x800000000ul },
        { FileE & Rank7, 0x80800000000ul },
        { FileF & Rank2, 0x4ul },
        { FileF & Rank3, 0x400ul },
        { FileF & Rank4, 0x40000ul },
        { FileF & Rank5, 0x4000000ul },
        { FileF & Rank6, 0x400000000ul },
        { FileF & Rank7, 0x40400000000ul },
        { FileG & Rank2, 0x2ul },
        { FileG & Rank3, 0x200ul },
        { FileG & Rank4, 0x20000ul },
        { FileG & Rank5, 0x2000000ul },
        { FileG & Rank6, 0x200000000ul },
        { FileG & Rank7, 0x20200000000ul },
        { FileH & Rank2, 0x1ul },
        { FileH & Rank3, 0x100ul },
        { FileH & Rank4, 0x10000ul },
        { FileH & Rank5, 0x1000000ul },
        { FileH & Rank6, 0x100000000ul },
        { FileH & Rank7, 0x10100000000ul },
    };

    public static readonly Dictionary<ulong, ulong> PawnMovesXRayWhite = new()
    {
        { FileA & Rank2, 0x80800000ul },
        { FileA & Rank3, 0x80000000ul },
        { FileA & Rank4, 0x8000000000ul },
        { FileA & Rank5, 0x800000000000ul },
        { FileA & Rank6, 0x80000000000000ul },
        { FileA & Rank7, 0x8000000000000000ul },
        { FileB & Rank2, 0x40400000ul },
        { FileB & Rank3, 0x40000000ul },
        { FileB & Rank4, 0x4000000000ul },
        { FileB & Rank5, 0x400000000000ul },
        { FileB & Rank6, 0x40000000000000ul },
        { FileB & Rank7, 0x4000000000000000ul },
        { FileC & Rank2, 0x20200000ul },
        { FileC & Rank3, 0x20000000ul },
        { FileC & Rank4, 0x2000000000ul },
        { FileC & Rank5, 0x200000000000ul },
        { FileC & Rank6, 0x20000000000000ul },
        { FileC & Rank7, 0x2000000000000000ul },
        { FileD & Rank2, 0x10100000ul },
        { FileD & Rank3, 0x10000000ul },
        { FileD & Rank4, 0x1000000000ul },
        { FileD & Rank5, 0x100000000000ul },
        { FileD & Rank6, 0x10000000000000ul },
        { FileD & Rank7, 0x1000000000000000ul },
        { FileE & Rank2, 0x8080000ul },
        { FileE & Rank3, 0x8000000ul },
        { FileE & Rank4, 0x800000000ul },
        { FileE & Rank5, 0x80000000000ul },
        { FileE & Rank6, 0x8000000000000ul },
        { FileE & Rank7, 0x800000000000000ul },
        { FileF & Rank2, 0x4040000ul },
        { FileF & Rank3, 0x4000000ul },
        { FileF & Rank4, 0x400000000ul },
        { FileF & Rank5, 0x40000000000ul },
        { FileF & Rank6, 0x4000000000000ul },
        { FileF & Rank7, 0x400000000000000ul },
        { FileG & Rank2, 0x2020000ul },
        { FileG & Rank3, 0x2000000ul },
        { FileG & Rank4, 0x200000000ul },
        { FileG & Rank5, 0x20000000000ul },
        { FileG & Rank6, 0x2000000000000ul },
        { FileG & Rank7, 0x200000000000000ul },
        { FileH & Rank2, 0x1010000ul },
        { FileH & Rank3, 0x1000000ul },
        { FileH & Rank4, 0x100000000ul },
        { FileH & Rank5, 0x10000000000ul },
        { FileH & Rank6, 0x1000000000000ul },
        { FileH & Rank7, 0x100000000000000ul },
    };

    public static readonly Dictionary<ulong, ulong> QueenXRay = new()
    {
        { FileA & Rank1, 0x8182848890A0C07Ful },
        { FileA & Rank2, 0x82848890A0C07FC0ul },
        { FileA & Rank3, 0x848890A0C07FC0A0ul },
        { FileA & Rank4, 0x8890A0C07FC0A090ul },
        { FileA & Rank5, 0x90A0C07FC0A09088ul },
        { FileA & Rank6, 0xA0C07FC0A0908884ul },
        { FileA & Rank7, 0xC07FC0A090888482ul },
        { FileA & Rank8, 0x7FC0A09088848281ul },
        { FileB & Rank1, 0x404142444850E0BFul },
        { FileB & Rank2, 0x4142444850E0BFE0ul },
        { FileB & Rank3, 0x42444850E0BFE050ul },
        { FileB & Rank4, 0x444850E0BFE05048ul },
        { FileB & Rank5, 0x4850E0BFE0504844ul },
        { FileB & Rank6, 0x50E0BFE050484442ul },
        { FileB & Rank7, 0xE0BFE05048444241ul },
        { FileB & Rank8, 0xBFE0504844424140ul },
        { FileC & Rank1, 0x2020212224A870DFul },
        { FileC & Rank2, 0x20212224A870DF70ul },
        { FileC & Rank3, 0x212224A870DF70A8ul },
        { FileC & Rank4, 0x2224A870DF70A824ul },
        { FileC & Rank5, 0x24A870DF70A82422ul },
        { FileC & Rank6, 0xA870DF70A8242221ul },
        { FileC & Rank7, 0x70DF70A824222120ul },
        { FileC & Rank8, 0xDF70A82422212020ul },
        { FileD & Rank1, 0x10101011925438EFul },
        { FileD & Rank2, 0x101011925438EF38ul },
        { FileD & Rank3, 0x1011925438EF3854ul },
        { FileD & Rank4, 0x11925438EF385492ul },
        { FileD & Rank5, 0x925438EF38549211ul },
        { FileD & Rank6, 0x5438EF3854921110ul },
        { FileD & Rank7, 0x38EF385492111010ul },
        { FileD & Rank8, 0xEF38549211101010ul },
        { FileE & Rank1, 0x8080888492A1CF7ul },
        { FileE & Rank2, 0x80888492A1CF71Cul },
        { FileE & Rank3, 0x888492A1CF71C2Aul },
        { FileE & Rank4, 0x88492A1CF71C2A49ul },
        { FileE & Rank5, 0x492A1CF71C2A4988ul },
        { FileE & Rank6, 0x2A1CF71C2A498808ul },
        { FileE & Rank7, 0x1CF71C2A49880808ul },
        { FileE & Rank8, 0xF71C2A4988080808ul },
        { FileF & Rank1, 0x404844424150EFBul },
        { FileF & Rank2, 0x4844424150EFB0Eul },
        { FileF & Rank3, 0x844424150EFB0E15ul },
        { FileF & Rank4, 0x4424150EFB0E1524ul },
        { FileF & Rank5, 0x24150EFB0E152444ul },
        { FileF & Rank6, 0x150EFB0E15244484ul },
        { FileF & Rank7, 0xEFB0E1524448404ul },
        { FileF & Rank8, 0xFB0E152444840404ul },
        { FileG & Rank1, 0x2824222120A07FDul },
        { FileG & Rank2, 0x824222120A07FD07ul },
        { FileG & Rank3, 0x4222120A07FD070Aul },
        { FileG & Rank4, 0x22120A07FD070A12ul },
        { FileG & Rank5, 0x120A07FD070A1222ul },
        { FileG & Rank6, 0xA07FD070A122242ul },
        { FileG & Rank7, 0x7FD070A12224282ul },
        { FileG & Rank8, 0xFD070A1222428202ul },
        { FileH & Rank1, 0x81412111090503FEul },
        { FileH & Rank2, 0x412111090503FE03ul },
        { FileH & Rank3, 0x2111090503FE0305ul },
        { FileH & Rank4, 0x11090503FE030509ul },
        { FileH & Rank5, 0x90503FE03050911ul },
        { FileH & Rank6, 0x503FE0305091121ul },
        { FileH & Rank7, 0x3FE030509112141ul },
        { FileH & Rank8, 0xFE03050911214181ul },
    };

    public static readonly Dictionary<ulong, ulong> RookXRay = new()
    {
        { FileA & Rank1, 0x808080808080807Ful },
        { FileA & Rank2, 0x8080808080807F80ul },
        { FileA & Rank3, 0x80808080807F8080ul },
        { FileA & Rank4, 0x808080807F808080ul },
        { FileA & Rank5, 0x8080807F80808080ul },
        { FileA & Rank6, 0x80807F8080808080ul },
        { FileA & Rank7, 0x807F808080808080ul },
        { FileA & Rank8, 0x7F80808080808080ul },
        { FileB & Rank1, 0x40404040404040BFul },
        { FileB & Rank2, 0x404040404040BF40ul },
        { FileB & Rank3, 0x4040404040BF4040ul },
        { FileB & Rank4, 0x40404040BF404040ul },
        { FileB & Rank5, 0x404040BF40404040ul },
        { FileB & Rank6, 0x4040BF4040404040ul },
        { FileB & Rank7, 0x40BF404040404040ul },
        { FileB & Rank8, 0xBF40404040404040ul },
        { FileC & Rank1, 0x20202020202020DFul },
        { FileC & Rank2, 0x202020202020DF20ul },
        { FileC & Rank3, 0x2020202020DF2020ul },
        { FileC & Rank4, 0x20202020DF202020ul },
        { FileC & Rank5, 0x202020DF20202020ul },
        { FileC & Rank6, 0x2020DF2020202020ul },
        { FileC & Rank7, 0x20DF202020202020ul },
        { FileC & Rank8, 0xDF20202020202020ul },
        { FileD & Rank1, 0x10101010101010EFul },
        { FileD & Rank2, 0x101010101010EF10ul },
        { FileD & Rank3, 0x1010101010EF1010ul },
        { FileD & Rank4, 0x10101010EF101010ul },
        { FileD & Rank5, 0x101010EF10101010ul },
        { FileD & Rank6, 0x1010EF1010101010ul },
        { FileD & Rank7, 0x10EF101010101010ul },
        { FileD & Rank8, 0xEF10101010101010ul },
        { FileE & Rank1, 0x8080808080808F7ul },
        { FileE & Rank2, 0x80808080808F708ul },
        { FileE & Rank3, 0x808080808F70808ul },
        { FileE & Rank4, 0x8080808F7080808ul },
        { FileE & Rank5, 0x80808F708080808ul },
        { FileE & Rank6, 0x808F70808080808ul },
        { FileE & Rank7, 0x8F7080808080808ul },
        { FileE & Rank8, 0xF708080808080808ul },
        { FileF & Rank1, 0x4040404040404FBul },
        { FileF & Rank2, 0x40404040404FB04ul },
        { FileF & Rank3, 0x404040404FB0404ul },
        { FileF & Rank4, 0x4040404FB040404ul },
        { FileF & Rank5, 0x40404FB04040404ul },
        { FileF & Rank6, 0x404FB0404040404ul },
        { FileF & Rank7, 0x4FB040404040404ul },
        { FileF & Rank8, 0xFB04040404040404ul },
        { FileG & Rank1, 0x2020202020202FDul },
        { FileG & Rank2, 0x20202020202FD02ul },
        { FileG & Rank3, 0x202020202FD0202ul },
        { FileG & Rank4, 0x2020202FD020202ul },
        { FileG & Rank5, 0x20202FD02020202ul },
        { FileG & Rank6, 0x202FD0202020202ul },
        { FileG & Rank7, 0x2FD020202020202ul },
        { FileG & Rank8, 0xFD02020202020202ul },
        { FileH & Rank1, 0x1010101010101FEul },
        { FileH & Rank2, 0x10101010101FE01ul },
        { FileH & Rank3, 0x101010101FE0101ul },
        { FileH & Rank4, 0x1010101FE010101ul },
        { FileH & Rank5, 0x10101FE01010101ul },
        { FileH & Rank6, 0x101FE0101010101ul },
        { FileH & Rank7, 0x1FE010101010101ul },
        { FileH & Rank8, 0xFE01010101010101ul },
    };

    public static readonly Dictionary<string, ulong> Squares = new()
    {
        { "-", 0 },
        { "a1", FileA & Rank1 },
        { "b1", FileB & Rank1 },
        { "c1", FileC & Rank1 },
        { "d1", FileD & Rank1 },
        { "e1", FileE & Rank1 },
        { "f1", FileF & Rank1 },
        { "g1", FileG & Rank1 },
        { "h1", FileH & Rank1 },
        { "a2", FileA & Rank2 },
        { "b2", FileB & Rank2 },
        { "c2", FileC & Rank2 },
        { "d2", FileD & Rank2 },
        { "e2", FileE & Rank2 },
        { "f2", FileF & Rank2 },
        { "g2", FileG & Rank2 },
        { "h2", FileH & Rank2 },
        { "a3", FileA & Rank3 },
        { "b3", FileB & Rank3 },
        { "c3", FileC & Rank3 },
        { "d3", FileD & Rank3 },
        { "e3", FileE & Rank3 },
        { "f3", FileF & Rank3 },
        { "g3", FileG & Rank3 },
        { "h3", FileH & Rank3 },
        { "a4", FileA & Rank4 },
        { "b4", FileB & Rank4 },
        { "c4", FileC & Rank4 },
        { "d4", FileD & Rank4 },
        { "e4", FileE & Rank4 },
        { "f4", FileF & Rank4 },
        { "g4", FileG & Rank4 },
        { "h4", FileH & Rank4 },
        { "a5", FileA & Rank5 },
        { "b5", FileB & Rank5 },
        { "c5", FileC & Rank5 },
        { "d5", FileD & Rank5 },
        { "e5", FileE & Rank5 },
        { "f5", FileF & Rank5 },
        { "g5", FileG & Rank5 },
        { "h5", FileH & Rank5 },
        { "a6", FileA & Rank6 },
        { "b6", FileB & Rank6 },
        { "c6", FileC & Rank6 },
        { "d6", FileD & Rank6 },
        { "e6", FileE & Rank6 },
        { "f6", FileF & Rank6 },
        { "g6", FileG & Rank6 },
        { "h6", FileH & Rank6 },
        { "a7", FileA & Rank7 },
        { "b7", FileB & Rank7 },
        { "c7", FileC & Rank7 },
        { "d7", FileD & Rank7 },
        { "e7", FileE & Rank7 },
        { "f7", FileF & Rank7 },
        { "g7", FileG & Rank7 },
        { "h7", FileH & Rank7 },
        { "a8", FileA & Rank8 },
        { "b8", FileB & Rank8 },
        { "c8", FileC & Rank8 },
        { "d8", FileD & Rank8 },
        { "e8", FileE & Rank8 },
        { "f8", FileF & Rank8 },
        { "g8", FileG & Rank8 },
        { "h8", FileH & Rank8 },
    };
    
    /// <summary>
    /// Represents the files of the chess board.
    /// </summary>
    public static readonly ulong[] Files = { FileA, FileB, FileC, FileD, FileE, FileF, FileG, FileH, };

    /// <summary>
    /// Represents the ranks of the chess board.
    /// </summary>
    public static readonly ulong[] Ranks = { Rank1, Rank2, Rank3, Rank4, Rank5, Rank6, Rank7, Rank8, };

    /// <summary>
    /// Gets the active color, which is the color that has the next move.
    /// </summary>
    /// <value>The active color is either 'w' for white or 'b' for black.</value>
    public char ActiveColor { get; private set; } = 'w';

    /// <summary>
    /// Represents the bitboards for the black pieces.
    /// </summary>
    public Dictionary<char, ulong> BlackPieces { get; } = new(new CaseInsensitiveCharComparer())
    {
        { 'b', 0ul },
        { 'n', 0ul },
        { 'r', 0ul },
        { 'q', 0ul },
        { 'k', 0ul },
        { 'p', 0ul },
    };

    /// <summary>
    /// Gets the bitboard representing all squares under attack by black pieces.
    /// This property is updated after each move to reflect the current state of the board.
    /// </summary>
    public ulong BlackAttacks { get; private set; }

    /// <summary>
    /// Gets the bitboard representing all squares that are either occupied by black pieces or are empty.
    /// This is achieved by inverting the bitboard of white pieces, thus marking all squares not occupied
    /// by white pieces.
    /// </summary>
    public ulong BlackOrEmpty => ~this.WhitePiecesBitBoard;

    /// <summary>
    /// Gets the combined bitboard for all black pieces.
    /// This property performs a bitwise OR operation on the bitboards of all black pieces.
    /// </summary>
    public ulong BlackPiecesBitBoard { get; private set; }

    /// <summary>
    /// Gets or sets the castling rights for both white and black.
    /// The rights are represented as a string with the following possible values:
    /// "K" - White can castle kingside
    /// "Q" - White can castle queenside
    /// "k" - Black can castle kingside
    /// "q" - Black can castle queenside
    /// "-"  - Neither side can castle
    /// </summary>
    public string CastlingRights { get; private set; } = "KQkq";

    /// <summary>
    /// Gets a value indicating whether the current player is in check.
    /// This property is true if the current player's king is under attack by the opponent's pieces,
    /// indicating that the player must make a move to remove the threat to the king.
    /// </summary>
    public bool Check { get; private set; }

    /// <summary>
    /// Gets the en passant target square in Forsyth-Edwards Notation (FEN).
    /// The en passant target square is the square where a pawn can be captured en passant.
    /// This property is represented as a string with the following possible values:
    /// </summary>
    /// <value>
    /// "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3" - for white pawns
    /// "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6" - for black pawns
    /// "-" - if there is no en passant target square
    /// </value>
    public Square EnPassantTargetSquare { get; private set; } = new();

    /// <summary>
    /// Gets the fullmove number.
    /// </summary>
    public int FullmoveNumber { get; private set; }

    /// <summary>
    /// Gets or sets the halfmove clock.
    /// </summary>
    public int HalfmoveClock { get; set; }

    /// <summary>
    /// Gets a value indicating whether the current player can take the opposing king.
    /// This property is used to check for illegal moves where the king is left in check or
    /// to determine checkmate conditions.
    /// </summary>
    public bool IllegalCheck { get; private set; }

    /// <summary>
    /// Gets the combined bitboard for all pieces on the board.
    /// This property performs a bitwise OR operation on the bitboards of all black and white pieces.
    /// </summary>
    public ulong OccupiedBitBoard { get; private set; }

    /// <summary>
    /// Gets the bitboard representing all squares under attack by white pieces.
    /// This property is updated after each move to reflect the current state of the board.
    /// </summary>
    /// 
    public ulong WhiteAttacks { get; private set; }

    /// <summary>
    /// Gets the bitboard representing all squares that are either occupied by white pieces or are empty.
    /// This is achieved by inverting the bitboard of black pieces, thus marking all squares not occupied
    /// by black pieces.
    /// </summary>
    public ulong WhiteOrEmpty => ~this.BlackPiecesBitBoard;

    /// <summary>
    /// Represents the bitboards for the white pieces.
    /// </summary>
    public Dictionary<char, ulong> WhitePieces { get; } = new(new CaseInsensitiveCharComparer())
    {
        { 'B', 0ul },
        { 'N', 0ul },
        { 'R', 0ul },
        { 'Q', 0ul },
        { 'K', 0ul },
        { 'P', 0ul },
    };

    /// <summary>
    /// Gets the combined bitboard for all white pieces.
    /// This property performs a bitwise OR operation on the bitboards of all white pieces.
    /// </summary>
    public ulong WhitePiecesBitBoard { get; private set; }

    public ulong GetPieceDefenses(PieceBase piece)
    {
        Dictionary<char, ulong> pieces;
        Dictionary<ulong, ulong> preCalculatedDefenses;
        if (piece.Color == Color.Black)
        {
            pieces = this.BlackPieces;
            preCalculatedDefenses = PawnDefensesXRayBlack;
        }
        else
        {
            pieces = this.WhitePieces;
            preCalculatedDefenses = PawnDefensesXRayWhite;
        }

        preCalculatedDefenses = piece.PieceType switch
        {
            PieceType.Bishop => BishopXRay,
            PieceType.King => KingXRay,
            PieceType.Knight => KnightXRay,
            PieceType.Pawn => preCalculatedDefenses,
            PieceType.Queen => QueenXRay,
            PieceType.Rook => RookXRay,
            PieceType.None => throw new ArgumentOutOfRangeException(nameof(piece.PieceType), "Invalid piece type."),
            _ => throw new ArgumentOutOfRangeException(nameof(piece.PieceType), "Invalid piece type."),
        };

        ulong piecePositions = pieces[piece.PieceType.ToChar()];

        // find the 1s in the piecePositions bitboard and get the positions defended by the piece
        ulong defenses = 0;
        while (piecePositions != 0)
        {
            ulong position = piecePositions & (ulong)-(long)piecePositions;
            // no filter here, we want to know the positions defended by the piece including where friendly pieces are
            defenses |= preCalculatedDefenses[position];
            piecePositions &= ~position;
        }

        return defenses;
    }

    public ulong GetPieceMoves(PieceBase piece)
    {
        var foo = new Dictionary<ulong, ulong>();
        

        Dictionary<char, ulong> pieces;
        Dictionary<ulong, ulong> preCalculatedMoves;
        ulong filter;
        if (piece.Color == Color.Black)
        {
            pieces = this.BlackPieces;
            preCalculatedMoves = new Dictionary<ulong, ulong>();
            foreach (KeyValuePair<ulong, ulong> keyValuePair in PawnMovesXRayBlack)
            {
                preCalculatedMoves[keyValuePair.Key] = keyValuePair.Value | PawnDefensesXRayBlack[keyValuePair.Key] &
                    (this.WhitePiecesBitBoard | Squares[this.EnPassantTargetSquare.ToString()]);
            }
            filter = this.WhiteOrEmpty;
        }
        else
        {
            pieces = this.WhitePieces;
            preCalculatedMoves = new Dictionary<ulong, ulong>();
            foreach (KeyValuePair<ulong, ulong> keyValuePair in PawnMovesXRayWhite)
            {
                preCalculatedMoves[keyValuePair.Key] = keyValuePair.Value | PawnDefensesXRayWhite[keyValuePair.Key] &
                    (this.BlackPiecesBitBoard | Squares[this.EnPassantTargetSquare.ToString()]);
            }
            filter = this.BlackOrEmpty;
        }

        preCalculatedMoves = piece.PieceType switch
        {
            PieceType.Bishop => BishopXRay,
            PieceType.King => KingXRay,
            PieceType.Knight => KnightXRay,
            PieceType.Pawn => preCalculatedMoves,
            PieceType.Queen => QueenXRay,
            PieceType.Rook => RookXRay,
            PieceType.None => throw new ArgumentOutOfRangeException(nameof(piece.PieceType), "Invalid piece type."),
            _ => throw new ArgumentOutOfRangeException(nameof(piece.PieceType), "Invalid piece type."),
        };

        
        ulong piecePositions = pieces[piece.PieceType.ToChar()];

        // find the 1s in the piecePositions bitboard and get the positions defended by the piece
        ulong defenses = 0;
        while (piecePositions != 0)
        {
            ulong position = piecePositions & (ulong)-(long)piecePositions;
            // we filter here to exclude positions where the piece cannot move
            defenses |= preCalculatedMoves[position] & filter;
            piecePositions &= ~position;
        }

        return defenses;
    }
    /// <summary>
    /// Gets the piece at the specified square on the chess board.
    /// </summary>
    /// <param name="file">The file of the target location. Must be between 1 and 8 inclusive.</param>
    /// <param name="rank">The rank of the target location. Must be between 1 and 8 inclusive.</param>
    /// <returns>The piece at the specified location. ' ' if the square is empty.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the file or rank is out of the valid range.
    /// </exception>
    public char GetPieceAt(int file, int rank)
    {
        if (file is <= 0 or > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(file), "Must be between 1 and 8 inclusive.");
        }

        if (rank is <= 0 or > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(file), "Must be between 1 and 8 inclusive.");
        }

        return this.GetPieceAt(Files[file - 1] & Ranks[rank - 1]);
    }

    /// <summary>
    /// Gets the piece at the specified square on the chess board.
    /// </summary>
    /// <param name="square">The square to check.</param>
    /// <returns>The piece at the specified square.</returns>
    public char GetPieceAt(Square square)
    {
        return this.GetPieceAt(square.BitBoard);
    }

    /// <summary>
    /// Gets the piece at the specified square on the chess board.
    /// </summary>
    /// <param name="square">The square to check in algebraic notation (e.g., "e4").</param>
    /// <returns>The piece at the specified square.</returns>
    public char GetPieceAt(string square)
    {
        return this.GetPieceAt(Squares[square]);
    }

    /// <summary>
    /// Gets the piece at the specified square on the chess board.
    /// </summary>
    /// <param name="square">The bitboard representation of the square to check.</param>
    /// <param name="color">The color of the piece to find. If Color.None, any piece will
    /// be considered.</param>
    /// <returns>The character representing the piece at the given position, or the
    /// <see cref="EmptySquare"/> character (' ') if no piece is found.</returns>
    public char GetPieceAt(ulong square, Color color = Color.None)
    {
        if (color != Color.White && (this.BlackPiecesBitBoard & square) != 0)
        {
            foreach (KeyValuePair<char, ulong> pieceType in this.BlackPieces)
            {
                if ((pieceType.Value & square) != 0)
                {
                    return pieceType.Key;
                }
            }
        }

        if (color != Color.Black && (this.WhitePiecesBitBoard & square) != 0)
        {
            foreach (KeyValuePair<char, ulong> pieceType in this.WhitePieces)
            {
                if ((pieceType.Value & square) != 0)
                {
                    return pieceType.Key;
                }
            }
        }

        return EmptySquare;
    }

    /// <summary>
    /// Initializes the chess board to the standard starting position.
    /// This method sets the bitboards for both black and white pieces.
    /// </summary>
    public void InitializeGameStartingBoard()
    {
        // Initialize black pieces
        // Rooks are placed on a8 and h8
        this.BlackPieces['r'] = Rank8 & FileA | Rank8 & FileH;

        // Knights are placed on b8 and g8
        this.BlackPieces['n'] = Rank8 & FileB | Rank8 & FileG;

        // Bishops are placed on c8 and f8
        this.BlackPieces['b'] = Rank8 & FileC | Rank8 & FileF;

        // Queen is placed on d8
        this.BlackPieces['q'] = Rank8 & FileD;

        // King is placed on e8
        this.BlackPieces['k'] = Rank8 & FileE;

        // Pawns are placed on a7 to h7
        this.BlackPieces['p'] = Rank7;

        // Initialize white pieces
        // Rooks are placed on a1 and h1
        this.WhitePieces['R'] = Rank1 & FileA | Rank1 & FileH;

        // Knights are placed on b1 and g1
        this.WhitePieces['N'] = Rank1 & FileB | Rank1 & FileG;

        // Bishops are placed on c1 and f1
        this.WhitePieces['B'] = Rank1 & FileC | Rank1 & FileF;

        // Queen is placed on d1
        this.WhitePieces['Q'] = Rank1 & FileD;

        // King is placed on e1
        this.WhitePieces['K'] = Rank1 & FileE;

        // Pawns are placed on a2 to h2
        this.WhitePieces['P'] = Rank2;

        this.UpdateBoardStatus();
    }

    /// <summary>
    /// Checks if a square on the board is empty.
    /// </summary>
    /// <param name="file">The file of the square to check. Must be between a and h inclusive.</param>
    /// <param name="rank">The rank of the square to check. Must be between 1 and 8 inclusive.</param>
    /// <returns>True if the square is empty, false otherwise.</returns>
    public bool IsEmptySquare(char file, int rank)
    {
        return this.IsEmptySquare(Squares[$"{file}{rank}"]);
    }

    /// <summary>
    /// Checks if a square on the board is empty.
    /// </summary>
    /// <param name="square">The square to check.</param>
    /// <returns>True if the square is empty, false otherwise.</returns>
    public bool IsEmptySquare(Square square)
    {
        return this.IsEmptySquare(square.BitBoard);
    }

    public bool IsEmptySquare(ulong square)
    {
        return (this.OccupiedBitBoard & square) == 0;
    }

    /// <summary>
    /// Sets the active color for the next move on the chess board.
    /// </summary>
    /// <param name="activeColor">The active color. 'w' for white and 'b' for black.</param>
    /// <exception cref="ArgumentException">Thrown when an invalid color is provided.</exception>
    public void SetActiveColor(char activeColor)
    {
        if (activeColor != 'w' && activeColor != 'b')
        {
            throw new ArgumentException("Invalid active color.", nameof(activeColor));
        }

        this.ActiveColor = activeColor;
    }

    /// <summary>
    /// Sets the castling rights for both white and black.
    /// The rights are represented as a string with the following possible values:
    /// "K" - White can castle kingside
    /// "Q" - White can castle queenside
    /// "k" - Black can castle kingside
    /// "q" - Black can castle queenside
    /// "-"  - Neither side can castle
    /// </summary>
    /// <param name="castlingRights">The string representing the castling rights.</param>
    /// <exception cref="ArgumentException">Thrown when an invalid castling rights string is provided.</exception>
    public void SetCastlingRights(string castlingRights)
    {
        if (castlingRights == "-")
        {
            this.CastlingRights = castlingRights;
            return;
        }

        if (castlingRights.Length > 4 || !Regex.IsMatch(castlingRights, "^[KQkq]+$"))
        {
            throw new ArgumentException("Invalid castling availability in FEN string.", nameof(castlingRights));
        }

        this.CastlingRights = castlingRights;
    }

    /// <summary>
    /// Sets the en passant target square on the chess board.
    /// </summary>
    /// <param name="enPassantTargetSquare">The en passant target square in Forsyth-Edwards Notation (FEN). 
    /// This is the square where a pawn can be captured en passant. 
    /// The value can be one of the following:
    /// "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3" - for white pawns
    /// "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6" - for black pawns
    /// "-" - if there is no en passant target square
    /// </param>
    /// <exception cref="ArgumentException">Thrown when an invalid en passant target square is provided.</exception>
    public void SetEnPassantTargetSquare(string enPassantTargetSquare)
    {
        if (enPassantTargetSquare == "-")
        {
            this.EnPassantTargetSquare = new Square();
            return;
        }

        if (enPassantTargetSquare.Length != 2 || enPassantTargetSquare[0] < 'a' || enPassantTargetSquare[0] > 'h' ||
            (enPassantTargetSquare[1] != '3' && enPassantTargetSquare[1] != '6'))
        {
            throw new ArgumentException(
                "Invalid en passant target square in FEN string.",
                nameof(enPassantTargetSquare));
        }

        // validate there is a pawn in the correct square for the en passant target
        int file = enPassantTargetSquare[0] - 'a';
        int rank = enPassantTargetSquare[1] - '1';
        rank = this.ActiveColor == 'w' ? rank - 1 : rank + 1;
        ulong targetBitBoard = Files[file] & Ranks[rank];
        if ((this.ActiveColor == Color.Black.ToChar() && (this.WhitePieces['P'] & targetBitBoard) == 0) ||
            (this.ActiveColor == Color.White.ToChar() && (this.BlackPieces['p'] & targetBitBoard) == 0))
        {
            throw new ArgumentException(
                "Invalid en passant target square in FEN string. No pawn to be taken en passant found.",
                nameof(enPassantTargetSquare));
        }

        this.EnPassantTargetSquare = new Square(enPassantTargetSquare);
    }

    /// <summary>
    /// Sets the fullmove number. The fullmove number is the number of the full moves in a game.
    /// It starts at 1, and is incremented after a black move.
    /// </summary>
    /// <param name="fullmoveNumber">The fullmove number. Must be 1 or greater.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the fullmove number
    /// is less than 1.</exception>
    public void SetFullmoveNumber(int fullmoveNumber)
    {
        if (fullmoveNumber < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(fullmoveNumber), "Fullmove number must be 1 or greater.");
        }

        this.FullmoveNumber = fullmoveNumber;
    }

    /// <summary>
    /// Sets the halfmove clock.
    /// </summary>
    /// <param name="halfmoveClock">The halfmove clock. Must be between 0 and 50 inclusive.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the halfmove clock
    /// is out of the valid range.</exception>
    public void SetHalfmoveClock(int halfmoveClock)
    {
        if (halfmoveClock is < 0 or > 50)
        {
            throw new ArgumentOutOfRangeException(
                nameof(halfmoveClock),
                "Halfmove clock must be between 0 and 50.");
        }

        this.HalfmoveClock = halfmoveClock;
    }

    /// <summary>
    /// Sets a piece at the specified location on the chess board.
    /// </summary>
    /// <param name="file">The file of the target location. Must be between 1 and 8 inclusive.</param>
    /// <param name="rank">The rank of the target location. Must be between 1 and 8 inclusive.</param>
    /// <param name="piece">The piece to set. Lowercase for black pieces, uppercase for white pieces.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the file or rank is
    /// out of the valid range.</exception>
    /// <exception cref="ArgumentException">Thrown when an invalid piece is provided.</exception>
    public void SetPieceAt(int file, int rank, char piece)
    {
        if (file is <= 0 or > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(file));
        }

        if (rank is <= 0 or > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(rank));
        }

        if (!Regex.IsMatch(piece.ToString(), "^[rnbqkpRNBQKP]+$"))
        {
            throw new ArgumentException("Invalid piece.", nameof(piece));
        }

        ulong bitBoard = Files[file - 1] & Ranks[rank - 1];
        this.SetPieceAt(bitBoard, piece);
    }

    public void SetPieceAt(ulong bitBoard, char piece)
    {
        if (char.IsLower(piece))
        {
            // Clear the bit at the target location for all black pieces
            foreach (var key in this.BlackPieces.Keys.ToList())
            {
                this.BlackPieces[key] &= ~bitBoard;
            }

            // Set the bit at the target location for the specified piece
            this.BlackPieces[piece] |= bitBoard;
        }
        else
        {
            // Clear the bit at the target location for all white pieces
            foreach (var key in this.WhitePieces.Keys.ToList())
            {
                this.WhitePieces[key] &= ~bitBoard;
            }

            // Set the bit at the target location for the specified piece
            this.WhitePieces[piece] |= bitBoard;
        }

        this.UpdateBoardStatus();
    }


    /// <summary>
    /// Checks if a given square contains a piece of a specific color.
    /// </summary>
    /// <param name="file">The file of the square to check. Must be between 'a' and 'h' inclusive.</param>
    /// <param name="rank">The rank of the square to check. Must be between 1 and 8 inclusive.</param>
    /// <param name="color">The color of the piece to check for. 'w' for white and 'b' for black.</param>
    /// <returns>True if the square contains a piece of the specified color, false otherwise.</returns>
    public bool SquareContainPieceOfColor(char file, int rank, Color color)
    {
        return this.SquareContainPieceOfColor(Squares[$"{file}{rank}"], (char)color);
    }

    /// <summary>
    /// Checks if a given square contains a piece of a specific color.
    /// </summary>
    /// <param name="square">The square to check.</param>
    /// <param name="color">The color of the piece to check for. 'w' for white and 'b' for black.</param>
    /// <returns>True if the square contains a piece of the specified color, false otherwise.</returns>
    public bool SquareContainPieceOfColor(Square square, Color color)
    {
        return this.SquareContainPieceOfColor(square, (char)color);
    }

    /// <summary>
    /// Checks if a given square contains a piece of a specific color.
    /// </summary>
    /// <param name="file">The file of the square to check. Must be between 'a' and 'h' inclusive.</param>
    /// <param name="rank">The rank of the square to check. Must be between 1 and 8 inclusive.</param>
    /// <param name="color">The color of the piece to check for. 'w' for white and 'b' for black.</param>
    /// <returns>True if the square contains a piece of the specified color, false otherwise.</returns>
    public bool SquareContainPieceOfColor(char file, int rank, char color)
    {
        return this.SquareContainPieceOfColor(Squares[$"{file}{rank}"], color);
    }

    /// <summary>
    /// Checks if a given square contains a piece of a specific color.
    /// </summary>
    /// <param name="square">The square to check.</param>
    /// <param name="color">The color of the piece to check for. 'w' for white and 'b' for black.</param>
    /// <returns>True if the square contains a piece of the specified color, false otherwise.</returns>
    public bool SquareContainPieceOfColor(Square square, char color)
    {
        return this.SquareContainPieceOfColor(square.BitBoard, color);
    }

    /// <summary>
    /// Checks if a square contains a piece of the specified color.
    /// </summary>
    /// <param name="square">The square to check, represented as a bitboard with a single bit set.</param>
    /// <param name="color">The color to check for. Can be either 'w' for white or 'b' for black.</param>
    /// <returns>true if the square contains a piece of the specified color; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid color is provided.</exception>
    public bool SquareContainPieceOfColor(ulong square, Color color)
    {
        return this.SquareContainPieceOfColor(square, (char)color);
    }

    /// <summary>
    /// Checks if a square contains a piece of the specified color.
    /// </summary>
    /// <param name="square">The square to check, represented as a bitboard with a single bit set.</param>
    /// <param name="color">The color to check for. Can be either 'w' for white or 'b' for black.</param>
    /// <returns>true if the square contains a piece of the specified color; otherwise, false.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid color is provided.</exception>
    public bool SquareContainPieceOfColor(ulong square, char color)
    {
        return color switch
        {
            'w' => (this.WhitePiecesBitBoard & square) != 0,
            'b' => (this.BlackPiecesBitBoard & square) != 0,
            _ => throw new ArgumentException($"Invalid color: {color}. Valid colors are 'b' or 'w'.", nameof(color)),
        };
    }

    private void UpdateAttacks()
    {
        this.BlackAttacks = 0;
        this.WhiteAttacks = 0;

        foreach (KeyValuePair<char, ulong> pieceKvp in this.BlackPieces.Where(p => p.Value != 0))
        {
            var piece = PieceFactory.GetPiece(pieceKvp.Key);
            this.BlackAttacks |= piece.GetPieceAttacks(pieceKvp.Value, this);
        }

        foreach (KeyValuePair<char, ulong> pieceKvp in this.WhitePieces.Where(p => p.Value != 0))
        {
            var piece = PieceFactory.GetPiece(pieceKvp.Key);
            this.WhiteAttacks |= piece.GetPieceAttacks(pieceKvp.Value, this);
        }
    }

    /// <summary>
    /// Updates the board status, including piece bitboards, attacks, and check status.
    /// Optionally toggles the active color and increments the full move number after a move.
    /// </summary>
    /// <param name="afterMove">Indicates whether the update is happening after a move, which
    /// will toggle the active color and may increment the full move number, or after an
    /// artificial setup of the board, which will not.</param>
    public void UpdateBoardStatus(bool afterMove = false)
    {
        if (afterMove)
        {
            if (this.ActiveColor == 'w')
            {
                this.ActiveColor = 'b';
            }
            else
            {
                this.ActiveColor = 'w';
                this.FullmoveNumber++;
            }
        }

        this.UpdatePiecesBitBoards();
        this.UpdateOccupiedBitBoard();
        this.UpdateAttacks();
        this.UpdateCheckAndIllegalCheck();
    }

    /// <summary>
    /// Updates the check and illegal check status based on the current attacks against the kings.
    /// </summary>
    /// <remarks>Illegal check indicates whether the current player can take the opposing king.</remarks>
    void UpdateCheckAndIllegalCheck()
    {
        if (this.ActiveColor == 'w')
        {
            this.Check = (this.BlackAttacks & this.WhitePieces['K']) != 0;
            this.IllegalCheck = (this.WhiteAttacks & this.BlackPieces['k']) != 0;
        }
        else
        {
            this.Check = (this.WhiteAttacks & this.BlackPieces['k']) != 0;
            this.IllegalCheck = (this.BlackAttacks & this.WhitePieces['K']) != 0;
        }
    }

    /// <summary>
    /// Updates the occupied bitboard by combining the bitboards of black and white pieces.
    /// This method recalculates the occupied squares on the board by performing a bitwise OR operation
    /// between the bitboards representing the positions of all black and white pieces.
    /// </summary>
    private void UpdateOccupiedBitBoard()
    {
        this.OccupiedBitBoard = this.BlackPiecesBitBoard | this.WhitePiecesBitBoard;
    }

    /// <summary>
    /// Updates the bitboards representing all pieces for each color by aggregating the bitboards
    /// of individual piece types.
    /// </summary>
    private void UpdatePiecesBitBoards()
    {
        this.BlackPiecesBitBoard = 0;
        this.WhitePiecesBitBoard = 0;

        foreach (KeyValuePair<char, ulong> pieceKvp in this.BlackPieces.Where(p => p.Value != 0))
        {
            this.BlackPiecesBitBoard |= pieceKvp.Value;
        }

        foreach (KeyValuePair<char, ulong> pieceKvp in this.WhitePieces.Where(p => p.Value != 0))
        {
            this.WhitePiecesBitBoard |= pieceKvp.Value;
        }
    }
}