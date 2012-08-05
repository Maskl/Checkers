using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace Checkers
{
    public class Piece
    {
        public Ellipse Drawable { get; set; }
        public bool IsBlack { get; set; }
        public bool IsKing { get; set; }
        public Image KingImage { get; set; }
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

            if (!IsKing && field.Y == FinishY)
            {
                IsKing = true;
                KingImage = new Image { Source = new BitmapImage(new Uri("ms-appx:///king.png")), Width = Drawable.Width, Height = Drawable.Height, Margin = Drawable.Margin };
                Board.BoardCanvas.Children.Add(KingImage);
            }

            if (IsKing)
                KingImage.Margin = Drawable.Margin;
        }

        public void Destroy()
        {
            Drawable.Visibility = Visibility.Collapsed;
            if (IsKing)
                KingImage.Visibility = Visibility.Collapsed;
        }
    }
}