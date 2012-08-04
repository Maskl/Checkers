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
        static public Color WhitePieceColor { get; set; }
        static public Color BlackPieceColor { get; set; }
        static public int KingImage { get; set; }
        public static double PieceMargin { get; set; }

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
                                         Margin = new Thickness(piece.Field.DisplayX + PieceMargin, piece.Field.DisplayY + PieceMargin, 0, 0),
                                         Height = FieldSize - PieceMargin * 2,
                                         Width = FieldSize - PieceMargin * 2,
                                         StrokeThickness = 0
                                     };

                BoardCanvas.Children.Add(piece.Drawable);
            }
        }
    }
}