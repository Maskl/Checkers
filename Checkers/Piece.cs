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

        public Piece(bool isBlack, int x, int y)
        {
            IsBlack = isBlack;
            IsKing = false;
            Field = OldField = Board.Fields[y][x];
            AnimStep = 0;
        }
    }
}