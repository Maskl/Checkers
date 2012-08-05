using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;

namespace Checkers
{
    public class Piece
    {
        public Ellipse Drawable { get; set; }
        public bool IsBlack { get; set; }
        public bool IsKing { get; set; }
        public Field Field { get; set; }
        public Field OldField { get; set; }
        public int AnimStep { get; set; }
        public int Direction { get; set; }
        public int FinishY { get; set; }

        public Piece(bool isBlack, int x, int y)
        {
            IsBlack = isBlack;
            IsKing = false;
            Field = OldField = Board.Fields[y][x];
            AnimStep = 0;
            Direction = isBlack ? -1 : 1;
            FinishY = isBlack ? 0 : 7;
        }

        public void SetPosition(Field field, double? x = null, double? y = null)
        {
            Field = field;

            if (x != null && y != null)
                Drawable.Margin = new Thickness((double)x + Board.PieceMargin, (double)y + Board.PieceMargin, 0, 0);
            else
                Drawable.Margin = new Thickness(field.DisplayX + Board.PieceMargin, field.DisplayY + Board.PieceMargin, 0, 0);
        }
    }
}