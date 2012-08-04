using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
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

        public void Deselect()
        {
            Drawable.Fill = new SolidColorBrush((X + Y) % 2 == 1 ? Board.DarkFieldColor : Board.LightFieldColor);
        }

        public void Select()
        {
            Drawable.Fill = new SolidColorBrush(Board.SelectedFieldColor);
        }

        public void Dehighlight()
        {
            Drawable.StrokeThickness = 0;
        }

        public void Highlight()
        {
            Drawable.StrokeThickness = 3;
            Drawable.Stroke = new SolidColorBrush(Board.HighlightedFieldColor);
        }
    }
}