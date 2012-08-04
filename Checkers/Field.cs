using Windows.UI.Xaml.Shapes;

namespace Checkers
{
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
}