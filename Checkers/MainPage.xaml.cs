using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Checkers
{

    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            InitializeComponent();

            Board.BoardCanvas = boardCanvas;
            Board.BoardSize = 532;
            Board.FieldSize = Board.BoardSize / 8;
            Board.LightFieldColor = Colors.Wheat;
            Board.DarkFieldColor = Colors.DarkGoldenrod;
            Board.WhitePieceColor = Colors.Yellow;
            Board.BlackPieceColor = Colors.Blue;
            Board.KingImage = 0;

            Board.NewBoard();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }

    public class Field
    {
        public Rectangle Drawable { get; set; }
        public FieldValue Value { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public double DisplayX { get; set; }
        public double DisplayY { get; set; }

        public Field(int x, int y)
        {
            X = x;
            Y = y;
            DisplayX = Board.BoardSize / 8 * x;
            DisplayY = Board.BoardSize / 8 * y;
        }
    }

    public class Piece
    {
        public Ellipse Drawable { get; set; }
        public bool IsBlack { get; set; }
        public bool IsKing { get; set; }
        public Field Field { get; set; }
        public Field OldField { get; set; }
        public int AnimStep { get; set; }

        public Piece(bool isBlack, int x, int y)
        {
            IsBlack = isBlack;
            IsKing = false;
            Field = OldField = Board.Fields[y][x];
            AnimStep = 0;
        }
    }

    public class Board
    {
        static public double BoardSize { get; set; }
        static public double FieldSize { get; set; }
        static public Color LightFieldColor { get; set; }
        static public Color DarkFieldColor { get; set; }
        static public Color WhitePieceColor { get; set; }
        static public Color BlackPieceColor { get; set; }
        static public int KingImage { get; set; }

        static public Canvas BoardCanvas { get; set; }
        static public Field[][] Fields { get; set; }
        static public Piece[] Pieces { get; set; }

        static public void NewBoard()
        {
            Fields = new Field[8][];
            for (var y = 0; y < 8; y++)
            {
                Fields[y] = new Field[8];
                for (var x = 0; x < 8; x++)
                {
                    Fields[y][x] = new Field(x, y);
                }
            }

            Pieces = new Piece[24];
            for (var i = 0; i < 12; ++i)
            {
                Pieces[i] = new Piece(true, (i % 4) * 2 + (i / 4) % 2, (i / 4) + 5);
            }

            for (var i = 12; i < 24; ++i)
            {
                Pieces[i] = new Piece(false, (i % 4) * 2 + (i / 4) % 2, (i / 4) - 3);
            }

            DrawBoard();
        }

        static public void DrawBoard()
        {
            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++)
                {
                    var field = Fields[y][x];
                    field.Drawable = new Rectangle
                    {
                        Fill = new SolidColorBrush((x + y) % 2 == 1 ? DarkFieldColor : LightFieldColor),
                        Margin = new Thickness(field.DisplayX, field.DisplayY, 0, 0),
                        Height = FieldSize,
                        Width = FieldSize,
                        StrokeThickness = 0
                    };

                    BoardCanvas.Children.Add(field.Drawable);
                }
            }

            foreach (var piece in Pieces)
            {
                piece.Drawable = new Ellipse
                {
                    Fill = new SolidColorBrush(piece.IsBlack ? BlackPieceColor : WhitePieceColor),
                    Margin = new Thickness(piece.Field.DisplayX, piece.Field.DisplayY, 0, 0),
                    Height = FieldSize,
                    Width = FieldSize,
                    StrokeThickness = 0
                };

                BoardCanvas.Children.Add(piece.Drawable);
            }
        }
    }

    public enum FieldValue
    {
        None = 0,
        Black = 1,
        White = 2,
        King = 4,
        Selected = 8,
        Hover = 16,
        Possible = 32
    }
}
