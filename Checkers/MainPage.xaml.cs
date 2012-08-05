using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

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
            Board.SelectedFieldColor = Colors.Maroon;
            Board.HighlightedFieldColor = Colors.Green;
            Board.WhitePieceColor = Colors.Yellow;
            Board.BlackPieceColor = Colors.Blue;
            Board.KingImage = 0;
            Board.PieceMargin = Board.FieldSize / 10;

            GameManager.Start();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void BoardCanvasTapped(object sender, TappedRoutedEventArgs e)
        {
            var point = e.GetPosition(boardCanvas);
            Board.Clicked(point.X, point.Y);
        }
    }
}
