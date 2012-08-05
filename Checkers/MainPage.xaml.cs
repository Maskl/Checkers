using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
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

            OnGameStartQuestions();

            GameManager.Page = this;
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

        private async void NewGameButton_OnClick(object sender, RoutedEventArgs e)
        {
            _shouldStartNewGame = false;
            var dialog = new MessageDialog("Are you sure you want to end this game and start a new one?", "New Game");

            var cmdNo = new UICommand("No", cmd => _shouldStartNewGame = false, 1);
            var cmdYes = new UICommand("Yes", cmd => _shouldStartNewGame = true, 2);

            dialog.Commands.Add(cmdNo);
            dialog.Commands.Add(cmdYes);
            dialog.DefaultCommandIndex = 0;

            await dialog.ShowAsync();

            if (_shouldStartNewGame)
            {
                OnGameStartQuestions();
                GameManager.NewGame();
            }
        }

        private bool _shouldStartNewGame;

        public async void OnGameStartQuestions()
        {
            {
                var dialog = new MessageDialog("Would you like to play with real player or with computer?", "Game type");

                var cmdOpt1 = new UICommand("player vs player", cmd => GameManager.SetPvP(), 1);
                var cmdOpt2 = new UICommand("player vs computer", cmd => GameManager.SetPvAI(), 2);

                dialog.Commands.Add(cmdOpt1);
                dialog.Commands.Add(cmdOpt2);
                dialog.DefaultCommandIndex = 0;

                await dialog.ShowAsync();
            }

            if (GameManager.IsGameVersusAI)
            {
                var dialog = new MessageDialog("Select color of your pieces (player with the darker coloured pieces moves first).", "Color");
                var cmdOpt1 = new UICommand("dark", cmd => GameManager.SetPlayerBlack(), 1);
                var cmdOpt2 = new UICommand("light", cmd => GameManager.SetPlayerWhite(), 2);

                dialog.Commands.Add(cmdOpt1);
                dialog.Commands.Add(cmdOpt2);
                dialog.DefaultCommandIndex = 0;

                await dialog.ShowAsync();
            }
        }

        private async void ColorsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Choose one of options below.", "Color scheme");

            var cmdOpt1 = new UICommand("metro", cmd => GameManager.SetScheme(1), 1);
            var cmdOpt2 = new UICommand("wood", cmd => GameManager.SetScheme(2), 2);
            var cmdOpt3 = new UICommand("contrast", cmd => GameManager.SetScheme(3), 3);

            dialog.Commands.Add(cmdOpt1);
            dialog.Commands.Add(cmdOpt2);
            dialog.Commands.Add(cmdOpt3);
            dialog.DefaultCommandIndex = 0;

            await dialog.ShowAsync();
        }

        private async void HelpButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Variant: English draughts (American checkers, straight checkers).\n\nIf you never played checkers check the rules on the internet or ask somebody :)\n\nSpecific rules for the chosen variant:\n\nPieces (men):\n  - move and capture diagonally and only forward.\n  - crowned when they reach the opposite end.\n  \nKings:\n  - can move and capture also backward.\n  - the same speed as normal pieces (men).\n\nJumping:\n  - mandatory.\n  - chosen sequence does not necessarily have to be the longest one.\n  - any piece can jump a king.\n  \nWin:\n  - by capturing all of the opposing player's pieces.\n  - by leaving the opposing player with no legal moves.\n  \nDraw:\n  - if neither side can force a win. \n  - if a player offers a draw and the opponent accepts. [not avaible in current app version]\n  \nMisc:\n  - The player with the darker coloured pieces moves first.", "Rules");
            await dialog.ShowAsync();
        }
    }
}
