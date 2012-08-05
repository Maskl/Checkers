using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Checkers
{
    public class Board
    {
        static public double BoardSize { get; set; }
        static public double FieldSize { get; set; }
        static public Color LightFieldColor { get; set; }
        static public Color DarkFieldColor { get; set; }
        static public Color SelectedFieldColor { get; set; }
        static public Color HighlightedFieldColor { get; set; }
        static public Color WhitePieceColor { get; set; }
        static public Color BlackPieceColor { get; set; }
        static public int KingImage { get; set; }
        public static double PieceMargin { get; set; }

        

        static public Canvas BoardCanvas { get; set; }
        static public Field[][] Fields { get; set; }
        static public List<Piece> Pieces { get; set; }

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

            Pieces = new List<Piece>();
            for (var i = 0; i < 12; ++i)
            {
                Pieces.Add(new Piece(true, (i % 4) * 2 + (i / 4) % 2, (i / 4) + 5));
            }

            for (var i = 12; i < 24; ++i)
            {
                Pieces.Add(new Piece(false, (i % 4) * 2 + (i / 4) % 2, (i / 4) - 3));
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
                                         Height = FieldSize - PieceMargin * 2,
                                         Width = FieldSize - PieceMargin * 2,
                                         StrokeThickness = 0
                                     };

                piece.SetPosition(piece.Field);
                BoardCanvas.Children.Add(piece.Drawable);
            }
        }

        public static void SelectField(Field fieldToSelect)
        {
            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++)
                {
                    var field = Fields[y][x];
                    field.Deselect();
                }
            }

            fieldToSelect.Select();
        }

        public static void DehighlightFields()
        {
            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++)
                {
                    var field = Fields[y][x];
                    field.Dehighlight();
                }
            }
        }

        public static void DeselectFields()
        {
            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++)
                {
                    var field = Fields[y][x];
                    field.Deselect();
                }
            }
        }

        static public Field GetField(int x, int y)
        {
            if (x < 0 || x > 7 || y < 0 || y > 7)
                return null;

            return Fields[y][x];
        }

        public static void Clicked(double x, double y)
        {
            var field = Fields[(int)(y / FieldSize)][(int)(x / FieldSize)];
            GameManager.FieldTapped(field);
        }
    }
}