using System;
using System.Threading.Tasks;
using Windows.Foundation;
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

        public async void AnimableMove(Point thicknessOld, Point thicknessNew, Piece toDestroy)
        {
            for (var i = 0; i <= 15; ++i)
            {
                var point = new Point((thicknessNew.X * i + thicknessOld.X * (15 - i)) / 15,
                                        (thicknessNew.Y * i + thicknessOld.Y * (15 - i)) / 15);

                Drawable.Margin = new Thickness(point.X, point.Y, 0, 0);
                if (IsKing)
                    KingImage.Margin = Drawable.Margin;

                await Task.Delay(250 / 15);

                if (i == 7 && toDestroy != null)
                    toDestroy.Destroy();
            }
        }

        public void SetPosition(bool animable, Field field, Piece toDestroy, double? x = null, double? y = null)
        {
            var thicknessOld = new Point(Drawable.Margin.Left, Drawable.Margin.Top);
            Point thicknessNew;
            if (x != null && y != null)
                thicknessNew = new Point((double) x + Board.PieceMargin, (double) y + Board.PieceMargin);
            else
                thicknessNew = new Point(field.DisplayX + Board.PieceMargin, field.DisplayY + Board.PieceMargin);

            if (animable)
            {
                AnimableMove(thicknessOld, thicknessNew, toDestroy);
            }
            else
            {
                Drawable.Margin = new Thickness(thicknessNew.X, thicknessNew.Y, 0, 0);
                if (IsKing)
                    KingImage.Margin = Drawable.Margin;
            }

            Field = field;

            if (!IsKing && field.Y == FinishY)
            {
                IsKing = true;
                KingImage = new Image { Source = new BitmapImage(new Uri("ms-appx:///king.png")), Width = Drawable.Width, Height = Drawable.Height };
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

        public void Update()
        {
            if (!IsKing)
                return;

            KingImage = new Image { Source = new BitmapImage(new Uri("ms-appx:///king.png")), Width = Drawable.Width, Height = Drawable.Height };
            Board.BoardCanvas.Children.Add(KingImage);
            KingImage.Margin = Drawable.Margin;
        }
    }
}