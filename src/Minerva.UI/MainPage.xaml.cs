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
using System.Globalization;

namespace Minerva.UI;

/// <summary>
/// Main page
/// </summary>
public partial class MainPage : ContentPage
{
    const string ChessStartingFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    readonly ImageButton[] allquares;

    readonly Dictionary<RadioButton, PieceBase> defenseRadioButtons;
    readonly Dictionary<RadioButton, PieceBase> moveRadioButtons;

    readonly ImageSource blackBishop = ImageSource.FromFile("blackbishop.png");
    readonly ImageSource blackKing = ImageSource.FromFile("blackking.png");
    readonly ImageSource blackKnight = ImageSource.FromFile("blackknight.png");
    readonly ImageSource blackPawn = ImageSource.FromFile("blackpawn.png");
    readonly ImageSource blackQueen = ImageSource.FromFile("blackqueen.png");
    readonly ImageSource blackRook = ImageSource.FromFile("blackrook.png");

    Board? board;

    Microsoft.Maui.Graphics.Color darkSquareColor = Colors.DarkGreen;

    readonly ImageButton[] darkSquares;

    Microsoft.Maui.Graphics.Color lightSquareColor = Colors.LightGrey;

    readonly ImageButton[] lightSquares;

    readonly ImageSource whiteBishop = ImageSource.FromFile("whitebishop.png");
    readonly ImageSource whiteKing = ImageSource.FromFile("whiteking.png");
    readonly ImageSource whiteKnight = ImageSource.FromFile("whiteknight.png");
    readonly ImageSource whitePawn = ImageSource.FromFile("whitepawn.png");
    readonly ImageSource whiteQueen = ImageSource.FromFile("whitequeen.png");
    readonly ImageSource whiteRook = ImageSource.FromFile("whiterook.png");

    /// <summary>
    /// Creates an instance of <see cref="MainPage"/>
    /// </summary>
    public MainPage()
    {
        this.InitializeComponent();

        this.allquares = new[]
        {
            this.btn1H, this.btn1G, this.btn1F, this.btn1E, this.btn1D, this.btn1C, this.btn1B,this.btn1A, 
            this.btn2H, this.btn2G, this.btn2F, this.btn2E, this.btn2D, this.btn2C, this.btn2B,this.btn2A, 
            this.btn3H, this.btn3G, this.btn3F, this.btn3E, this.btn3D, this.btn3C, this.btn3B,this.btn3A, 
            this.btn4H, this.btn4G, this.btn4F, this.btn4E, this.btn4D, this.btn4C, this.btn4B,this.btn4A, 
            this.btn5H, this.btn5G, this.btn5F, this.btn5E, this.btn5D, this.btn5C, this.btn5B,this.btn5A, 
            this.btn6H, this.btn6G, this.btn6F, this.btn6E, this.btn6D, this.btn6C, this.btn6B,this.btn6A, 
            this.btn7H, this.btn7G, this.btn7F, this.btn7E, this.btn7D, this.btn7C, this.btn7B,this.btn7A, 
            this.btn8H, this.btn8G, this.btn8F, this.btn8E, this.btn8D, this.btn8C, this.btn8B,this.btn8A, 
        };
        this.darkSquares = new[]
        {
            this.btn1A, this.btn1C, this.btn1E, this.btn1G,
            this.btn2B, this.btn2D, this.btn2F, this.btn2H,
            this.btn3A, this.btn3C, this.btn3E, this.btn3G,
            this.btn4B, this.btn4D, this.btn4F, this.btn4H,
            this.btn5A, this.btn5C, this.btn5E, this.btn5G,
            this.btn6B, this.btn6D, this.btn6F, this.btn6H,
            this.btn7A, this.btn7C, this.btn7E, this.btn7G,
            this.btn8B, this.btn8D, this.btn8F, this.btn8H,
        };
        this.lightSquares = new[]
        {
            this.btn1B, this.btn1D, this.btn1F, this.btn1H,
            this.btn2A, this.btn2C, this.btn2E, this.btn2G,
            this.btn3B, this.btn3D, this.btn3F, this.btn3H,
            this.btn4A, this.btn4C, this.btn4E, this.btn4G,
            this.btn5B, this.btn5D, this.btn5F, this.btn5H,
            this.btn6A, this.btn6C, this.btn6E, this.btn6G,
            this.btn7B, this.btn7D, this.btn7F, this.btn7H,
            this.btn8A, this.btn8C, this.btn8E, this.btn8G,
        };
        this.defenseRadioButtons = new Dictionary<RadioButton, PieceBase>
        {
            { this.cbBlackBishopsDefends, PieceFactory.GetPiece(PieceType.Bishop, Color.Black) },
            { this.cbBlackKingDefends, PieceFactory.GetPiece(PieceType.King, Color.Black) },
            { this.cbBlackKnightsDefends, PieceFactory.GetPiece(PieceType.Knight, Color.Black) },
            { this.cbBlackPawnsDefends, PieceFactory.GetPiece(PieceType.Pawn, Color.Black) },
            { this.cbBlackQueensDefends, PieceFactory.GetPiece(PieceType.Queen, Color.Black)},
            { this.cbBlackRooksDefends, PieceFactory.GetPiece(PieceType.Rook, Color.Black) },
            { this.cbWhiteBishopsDefends, PieceFactory.GetPiece(PieceType.Bishop, Color.White) },
            { this.cbWhiteKingDefends, PieceFactory.GetPiece(PieceType.King, Color.White) },
            { this.cbWhiteKnightsDefends, PieceFactory.GetPiece(PieceType.Knight, Color.White) },
            { this.cbWhitePawnsDefends, PieceFactory.GetPiece(PieceType.Pawn, Color.White) },
            { this.cbWhiteQueensDefends, PieceFactory.GetPiece(PieceType.Queen, Color.White) },
            { this.cbWhiteRooksDefends, PieceFactory.GetPiece(PieceType.Rook, Color.White) },
        };
        this.moveRadioButtons = new Dictionary<RadioButton, PieceBase>
        {
            { this.cbBlackBishopsMoves, PieceFactory.GetPiece(PieceType.Bishop, Color.Black) },
            { this.cbBlackKingMoves, PieceFactory.GetPiece(PieceType.King, Color.Black) },
            { this.cbBlackKnightsMoves, PieceFactory.GetPiece(PieceType.Knight, Color.Black) },
            { this.cbBlackPawnsMoves, PieceFactory.GetPiece(PieceType.Pawn, Color.Black) },
            { this.cbBlackQueensMoves, PieceFactory.GetPiece(PieceType.Queen, Color.Black)},
            { this.cbBlackRooksMoves, PieceFactory.GetPiece(PieceType.Rook, Color.Black) },
            { this.cbWhiteBishopsMoves, PieceFactory.GetPiece(PieceType.Bishop, Color.White) },
            { this.cbWhiteKingMoves, PieceFactory.GetPiece(PieceType.King, Color.White) },
            { this.cbWhiteKnightsMoves, PieceFactory.GetPiece(PieceType.Knight, Color.White) },
            { this.cbWhitePawnsMoves, PieceFactory.GetPiece(PieceType.Pawn, Color.White) },
            { this.cbWhiteQueensMoves, PieceFactory.GetPiece(PieceType.Queen, Color.White) },
            { this.cbWhiteRooksMoves, PieceFactory.GetPiece(PieceType.Rook, Color.White) },
        };

        this.SetBoardColors();
    }

    void BtnDarkColor_OnClicked(object? sender, EventArgs e)
    {
        this.darkSquareColor = this.swatch.BackgroundColor;
        SetBoardColors(this.darkSquares, this.darkSquareColor);
    }

    void BtnLightColor_OnClicked(object? sender, EventArgs e)
    {
        this.lightSquareColor = this.swatch.BackgroundColor;
        SetBoardColors(this.lightSquares, this.lightSquareColor);
    }

    void DefendsButtonCheckChanged(object? sender, CheckedChangedEventArgs e)
    {
        this.SetBoardColors();

        if (this.board != null && sender is RadioButton radioButton)
        {
            if (!radioButton.IsChecked)
            {
                return;
            }

            foreach (KeyValuePair<RadioButton, PieceBase> attackRadioButton in this.moveRadioButtons)
            {
                attackRadioButton.Key.IsChecked = false;
            }

            PieceBase piece = this.defenseRadioButtons[radioButton];
            ulong defendedSquares = this.board.GetPieceDefenses(piece);
            if (defendedSquares > 0)
            {
                for (int i = 0; i < 64; i++)
                {
                    ulong bitmask = 1ul << i;
                    if ((bitmask & defendedSquares) != 0)
                    {
                        this.allquares[i].BackgroundColor = Colors.Red;
                    }
                }
            }
        }
    }

    void CleanBoard()
    {
        foreach (ImageButton image in this.allquares)
        {
            image.Source = null;
        }
    }

    void DrawPieces()
    {
        if (this.board == null)
        {
            return;
        }

        for (int i = 0; i < 64; i++)
        {
            ImageButton square = this.allquares[i];
            if (((1ul << i) & this.board.BlackPieces['b']) != 0)
            {
                square.Source = this.blackBishop;
                continue;
            }

            if (((1ul << i) & this.board.BlackPieces['k']) != 0)
            {
                square.Source = this.blackKing;
                continue;
            }

            if (((1ul << i) & this.board.BlackPieces['n']) != 0)
            {
                square.Source = this.blackKnight;
                continue;
            }

            if (((1ul << i) & this.board.BlackPieces['p']) != 0)
            {
                square.Source = this.blackPawn;
                continue;
            }

            if (((1ul << i) & this.board.BlackPieces['q']) != 0)
            {
                square.Source = this.blackQueen;
                continue;
            }

            if (((1ul << i) & this.board.BlackPieces['r']) != 0)
            {
                square.Source = this.blackRook;
                continue;
            }

            if (((1ul << i) & this.board.WhitePieces['b']) != 0)
            {
                square.Source = this.whiteBishop;
                continue;
            }

            if (((1ul << i) & this.board.WhitePieces['k']) != 0)
            {
                square.Source = this.whiteKing;
                continue;
            }

            if (((1ul << i) & this.board.WhitePieces['n']) != 0)
            {
                square.Source = this.whiteKnight;
                continue;
            }

            if (((1ul << i) & this.board.WhitePieces['p']) != 0)
            {
                square.Source = this.whitePawn;
                continue;
            }

            if (((1ul << i) & this.board.WhitePieces['q']) != 0)
            {
                square.Source = this.whiteQueen;
                continue;
            }

            if (((1ul << i) & this.board.WhitePieces['r']) != 0)
            {
                square.Source = this.whiteRook;
            }
        }
    }

    void InitializeBoard(object? sender, EventArgs e)
    {
        this.board = ForsythEdwardsNotation.GenerateBoard(
            string.IsNullOrWhiteSpace(this.txtFen.Text) ? ChessStartingFen : this.txtFen.Text);
        this.CleanBoard();
        this.DrawPieces();
        this.SetBoardData();
    }

    void SetBoardData()
    {
        // black pieces data
        this.lblBlackBishopsHex.Text = this.board?.BlackPieces[PieceType.Bishop.ToChar()].ToString("X16") ?? "0";
        this.lblBlackBishopsBinary.Text =
            this.board != null
                ? Convert.ToString((long)this.board!.BlackPieces[PieceType.Bishop.ToChar()], 2).PadLeft(64, '0')
                : "0";
        this.lblBlackBishopsAttacks.Text = this.board?.GetPieceDefenses(PieceFactory.GetPiece(PieceType.Bishop, Color.Black))
            .ToString("X16") ?? "0";

        this.lblBlackKingHex.Text = this.board?.BlackPieces[PieceType.King.ToChar()].ToString("X16") ?? "0";
        this.lblBlackKingBinary.Text =
            this.board != null
                ? Convert.ToString((long)this.board!.BlackPieces[PieceType.King.ToChar()], 2).PadLeft(64, '0')
                : "0";
        this.lblBlackKingAttacks.Text = this.board?.GetPieceDefenses(PieceFactory.GetPiece(PieceType.King, Color.Black))
            .ToString("X16") ?? "0";


        this.lblBlackKnightsHex.Text = this.board?.BlackPieces[PieceType.Knight.ToChar()].ToString("X16") ?? "0";
        this.lblBlackKnightsBinary.Text =
            this.board != null
                ? Convert.ToString((long)this.board!.BlackPieces[PieceType.Knight.ToChar()], 2).PadLeft(64, '0')
                : "0";
        this.lblBlackKnightsAttacks.Text = this.board?.GetPieceDefenses(PieceFactory.GetPiece(PieceType.Knight, Color.Black))
            .ToString("X16") ?? "0";


        this.lblBlackPawnsHex.Text = this.board?.BlackPieces[PieceType.Pawn.ToChar()].ToString("X16") ?? "0";
        this.lblBlackPawnsBinary.Text =
            this.board != null
                ? Convert.ToString((long)this.board!.BlackPieces[PieceType.Pawn.ToChar()], 2).PadLeft(64, '0')
                : "0";
        this.lblBlackPawnsAttacks.Text = this.board?.GetPieceDefenses(PieceFactory.GetPiece(PieceType.Pawn, Color.Black))
            .ToString("X16") ?? "0";


        this.lblBlackQueensHex.Text = this.board?.BlackPieces[PieceType.Queen.ToChar()].ToString("X16") ?? "0";
        this.lblBlackQueensBinary.Text =
            this.board != null
                ? Convert.ToString((long)this.board!.BlackPieces[PieceType.Queen.ToChar()], 2).PadLeft(64, '0')
                : "0";
        this.lblBlackQueensAttacks.Text = this.board?.GetPieceDefenses(PieceFactory.GetPiece(PieceType.Queen, Color.Black))
            .ToString("X16") ?? "0";


        this.lblBlackRooksBinary.Text =
            this.board != null
                ? Convert.ToString((long)this.board!.BlackPieces[PieceType.Rook.ToChar()], 2).PadLeft(64, '0')
                : "0";
        this.lblBlackRooksHex.Text = this.board?.BlackPieces[PieceType.Rook.ToChar()].ToString("X16") ?? "0";
        this.lblBlackRooksAttacks.Text = this.board?.GetPieceDefenses(PieceFactory.GetPiece(PieceType.Rook, Color.Black))
            .ToString("X16") ?? "0";


        // white pieces data
        this.lblWhiteBishopsHex.Text = this.board?.WhitePieces[PieceType.Bishop.ToChar()].ToString("X16") ?? "0";
        this.lblWhiteBishopsBinary.Text =
            this.board != null
                ? Convert.ToString((long)this.board!.WhitePieces[PieceType.Bishop.ToChar()], 2).PadLeft(64, '0')
                : "0";
        this.lblWhiteBishopsAttacks.Text = this.board?.GetPieceDefenses(PieceFactory.GetPiece(PieceType.Bishop, Color.White))
            .ToString("X16") ?? "0";

        this.lblWhiteKingHex.Text = this.board?.WhitePieces[PieceType.King.ToChar()].ToString("X16") ?? "0";
        this.lblWhiteKingBinary.Text =
            this.board != null
                ? Convert.ToString((long)this.board!.WhitePieces[PieceType.King.ToChar()], 2).PadLeft(64, '0')
                : "0";
        this.lblWhiteKingAttacks.Text = this.board?.GetPieceDefenses(PieceFactory.GetPiece(PieceType.King, Color.White))
            .ToString("X16") ?? "0";

        this.lblWhiteKnightsHex.Text = this.board?.WhitePieces[PieceType.Knight.ToChar()].ToString("X16") ?? "0";
        this.lblWhiteKnightsBinary.Text =
            this.board != null
                ? Convert.ToString((long)this.board!.WhitePieces[PieceType.Knight.ToChar()], 2).PadLeft(64, '0')
                : "0";
        this.lblWhiteKnightsAttacks.Text = this.board?.GetPieceDefenses(PieceFactory.GetPiece(PieceType.Knight, Color.White))
            .ToString("X16") ?? "0";

        this.lblWhitePawnsHex.Text = this.board?.WhitePieces[PieceType.Pawn.ToChar()].ToString("X16") ?? "0";
        this.lblWhitePawnsBinary.Text =
            this.board != null
                ? Convert.ToString((long)this.board!.WhitePieces[PieceType.Pawn.ToChar()], 2).PadLeft(64, '0')
                : "0";
        this.lblWhitePawnsAttacks.Text = this.board?.GetPieceDefenses(PieceFactory.GetPiece(PieceType.Pawn, Color.White))
            .ToString("X16") ?? "0";

        this.lblWhiteQueensHex.Text = this.board?.WhitePieces[PieceType.Queen.ToChar()].ToString("X16") ?? "0";
        this.lblWhiteQueensBinary.Text =
            this.board != null
                ? Convert.ToString((long)this.board!.WhitePieces[PieceType.Queen.ToChar()], 2).PadLeft(64, '0')
                : "0";
        this.lblWhiteQueensAttacks.Text = this.board?.GetPieceDefenses(PieceFactory.GetPiece(PieceType.Queen, Color.White))
            .ToString("X16") ?? "0";

        this.lblWhiteRooksHex.Text = this.board?.WhitePieces[PieceType.Rook.ToChar()].ToString("X16") ?? "0";
        this.lblWhiteRooksBinary.Text =
            this.board != null
                ? Convert.ToString((long)this.board!.WhitePieces[PieceType.Rook.ToChar()], 2).PadLeft(64, '0')
                : "0";
        this.lblWhiteRooksAttacks.Text = this.board?.GetPieceDefenses(PieceFactory.GetPiece(PieceType.Rook, Color.White))
            .ToString("X16") ?? "0";
    }

    void ResetBoard(object? sender, EventArgs e)
    {
        this.board = null;
        this.CleanBoard();
        this.SetBoardData();
    }

    static void SetBoardColors(ImageButton[] squares, Microsoft.Maui.Graphics.Color color)
    {
        foreach (ImageButton square in squares)
        {
            square.BackgroundColor = color;
        }
    }

    void SetBoardColors()
    {
        SetBoardColors(this.lightSquares, this.lightSquareColor);
        SetBoardColors(this.darkSquares, this.darkSquareColor);
    }

    void SquareSelected(object? sender, EventArgs e)
    {
    }

    void UpdateColorSwatch(object? sender, ValueChangedEventArgs e)
    {
        this.lblBlue.Text = ((int)this.sldrBlue.Value).ToString(CultureInfo.InvariantCulture);
        this.lblGreen.Text = ((int)this.sldrGreen.Value).ToString(CultureInfo.InvariantCulture);
        this.lblRed.Text = ((int)this.sldrRed.Value).ToString(CultureInfo.InvariantCulture);
        this.swatch.BackgroundColor = Microsoft.Maui.Graphics.Color.FromRgb(
            this.sldrRed.Value / 255d,
            this.sldrGreen.Value / 255d,
            this.sldrBlue.Value / 255d);
        this.swatch.Background = Microsoft.Maui.Graphics.Color.FromRgb(
            this.sldrRed.Value / 255d,
            this.sldrGreen.Value / 255d,
            this.sldrBlue.Value / 255d);
    }

    private void MovesButtonCheckChanged(object? sender, CheckedChangedEventArgs e)
    {
        this.SetBoardColors();

        if (this.board != null && sender is RadioButton radioButton)
        {
            if (!radioButton.IsChecked)
            {
                return;
            }

            foreach (KeyValuePair<RadioButton, PieceBase> defenseRadioButton in this.defenseRadioButtons)
            {
                defenseRadioButton.Key.IsChecked = false;
            }

            PieceBase piece = this.moveRadioButtons[radioButton];
            ulong legalMoves = this.board.GetPieceMoves(piece);
            if (legalMoves > 0)
            {
                for (int i = 0; i < 64; i++)
                {
                    ulong bitmask = 1ul << i;
                    if ((bitmask & legalMoves) != 0)
                    {
                        this.allquares[i].BackgroundColor = Colors.Cyan;
                    }
                }
            }
        }
    }
}
