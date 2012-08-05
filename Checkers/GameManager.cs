using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;

namespace Checkers
{
    class GameManager
    {
        public static MainPage Page { get; set; }
        static public bool BlackTurn { get; set; }
        static public Piece SelectedPiece { get; set; }
        static public List<Field> JumpableFields { get; set; }
        static public List<Field> MovableFields { get; set; }
        static public List<Field> SelectableFields { get; set; }
        static public bool IsGameVersusAI { get; set; }
        static public bool IsPlayerBlack { get; set; }
        static public int DrawIterator { get; set; }

        static public void Start()
        {
            BlackTurn = true;
            SelectedPiece = null;
            DrawIterator = 0;
            MovableFields = new List<Field>();
            JumpableFields = new List<Field>();
            SelectableFields = new List<Field>();

            Board.NewBoard();

            LoadAndApplySelectedScheme();

            SelectAllCurrentPlayerPieces();
            if (IsGameVersusAI && IsPlayerBlack != BlackTurn)
                AITurn();
        }

        static public void FieldTapped(Field field)
        {
            if (!field.IsUsableInGame())
                return;

            if (JumpableFields.Contains(field))
            {
                var enemy = Board.Fields[field.Y + (SelectedPiece.Field.Y - field.Y) / 2][field.X + (SelectedPiece.Field.X - field.X) / 2].GetPieceOnField();
                SelectedPiece.SetPosition(true, field, enemy);
                Board.Pieces.Remove(enemy);
                DrawIterator = 0;

                MovableFields.Clear();
                JumpableFields.Clear();
                CheckAllPossibleMoveAndJumpsForPiece(SelectedPiece, SelectedPiece.IsBlack);
                if (JumpableFields.Count == 0)
                {
                    NextTurn();
                    return;
                }

                Board.DehighlightFields();
                foreach (var jumpableField in JumpableFields)
                {
                    jumpableField.Highlight();
                }

                Board.SelectField(SelectedPiece.Field);
                return;
            }

            if (MovableFields.Contains(field) && JumpableFields.Count == 0)
            {
                SelectedPiece.SetPosition(true, field, null);
                NextTurn();
                return;
            }

            foreach (var piece in Board.Pieces.Where(piece => piece.Field == field && piece.IsBlack == BlackTurn && SelectableFields.Contains(piece.Field)))
            {
                SelectPiece(piece);
            }
        }

        static private void NextTurn()
        {
            Board.DeselectFields();
            Board.DehighlightFields();
            SelectedPiece = null;
            JumpableFields.Clear();
            MovableFields.Clear();
            BlackTurn = !BlackTurn;
            SelectAllCurrentPlayerPieces();
            if (SelectableFields.Count == 0)
            {
                EndOfGame();
                return;
            }

            if (Board.Pieces.Any(piece => !piece.IsKing))
                DrawIterator = 0;
            else
                DrawIterator++;

            if (DrawIterator >= 30)
            {
                DrawGame();
                return;
            }

            if (IsGameVersusAI && IsPlayerBlack != BlackTurn)
                AITurn();
        }

        public static async void Sleep(int ms)
        {
           // await Task.Delay(ms);
             new System.Threading.ManualResetEvent(false).WaitOne(ms);
        }

        private static async void AITurn()
        {
            await Task.Delay(200);

            var rand = new Random();
            FieldTapped(SelectableFields[rand.Next(SelectableFields.Count)]);
            if (JumpableFields.Count > 0)
            {
                while (IsPlayerBlack != BlackTurn)
                {
                    await Task.Delay(500);
                    FieldTapped(JumpableFields[rand.Next(JumpableFields.Count)]);
                }
            }
            else
            {
                await Task.Delay(500);
                FieldTapped(MovableFields[rand.Next(MovableFields.Count)]);
            }
        }

        private static async void DrawGame()
        {
            var dialog = new MessageDialog("Probably nobody will win this game.", "Draw!");
            await dialog.ShowAsync();

            Page.OnGameStartQuestions();
        }

        private static async void EndOfGame()
        {
            string msg;
            string tit;
            if (IsGameVersusAI)
            {
                if (BlackTurn != IsPlayerBlack)
                {
                    msg = "Congratulation, you won with computer!";
                    tit = "Winner!";
                }
                else
                {
                    msg = "Computer won that game. Try again.";
                    tit = "Loser!";
                }
            }
            else
            {
                if (BlackTurn)
                {
                    msg = "Player who played lightener pieces won. Congratulation!";
                    tit = "Light pieces won!";
                }
                else
                {
                    msg = "Player who played darkener pieces won. Congratulation!";
                    tit = "Dark pieces won!";
                }
            }

            var dialog = new MessageDialog(msg, tit);
            await dialog.ShowAsync();

            Page.OnGameStartQuestions();
        }

        private static void SelectAllCurrentPlayerPieces()
        {
            SelectableFields.Clear();

            var fieldsFromWhichCanMove = new List<Field>();
            var fieldsFromWhichCanJump = new List<Field>();

            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++)
                {
                    var field = Board.Fields[y][x];
                    var piece = field.GetPieceOnField();
                    if (piece == null || piece.IsBlack != BlackTurn)
                        continue;

                    MovableFields.Clear();
                    JumpableFields.Clear();
                    CheckAllPossibleMoveAndJumpsForPiece(piece, piece.IsBlack);
                    if (JumpableFields.Count > 0)
                        fieldsFromWhichCanJump.Add(field); 
                    if (MovableFields.Count > 0)
                        fieldsFromWhichCanMove.Add(field);
                }
            }

            MovableFields.Clear();
            JumpableFields.Clear();

            if (fieldsFromWhichCanJump.Count > 0)
            {
                foreach (var fieldFromWhichCanJump in fieldsFromWhichCanJump)
                {
                    fieldFromWhichCanJump.Highlight();
                    SelectableFields.Add(fieldFromWhichCanJump);
                }
                return;
            }

            if (fieldsFromWhichCanMove.Count > 0)
            {
                foreach (var fieldFromWhichCanMove in fieldsFromWhichCanMove)
                {
                    fieldFromWhichCanMove.Highlight();
                    SelectableFields.Add(fieldFromWhichCanMove);
                }
            }
        }

        static private void SelectPiece(Piece piece)
        {
            SelectedPiece = piece;
            Board.SelectField(piece.Field);

            MovableFields.Clear();
            JumpableFields.Clear();

            CheckAllPossibleMoveAndJumpsForPiece(piece, SelectedPiece.IsBlack);

            Board.DehighlightFields();
            if (JumpableFields.Count > 0)
            {
                foreach (var jumpableField in JumpableFields)
                {
                    jumpableField.Highlight();
                }
            }
            else
            {
                foreach (var movableFields in MovableFields)
                {
                    movableFields.Highlight();
                }
            }
        }

        private static void CheckAllPossibleMoveAndJumpsForPiece(Piece piece, bool isBlack)
        {
            CheckPossibleMoveOrJump(Board.GetField(piece.Field.X - 1, piece.Field.Y + piece.Direction),
                                    Board.GetField(piece.Field.X - 2, piece.Field.Y + piece.Direction * 2), isBlack);

            CheckPossibleMoveOrJump(Board.GetField(piece.Field.X + 1, piece.Field.Y + piece.Direction),
                                    Board.GetField(piece.Field.X + 2, piece.Field.Y + piece.Direction * 2), isBlack);

            if (piece.IsKing)
            {
                CheckPossibleMoveOrJump(Board.GetField(piece.Field.X - 1, piece.Field.Y - piece.Direction),
                                        Board.GetField(piece.Field.X - 2, piece.Field.Y - piece.Direction * 2), isBlack);

                CheckPossibleMoveOrJump(Board.GetField(piece.Field.X + 1, piece.Field.Y - piece.Direction),
                                        Board.GetField(piece.Field.X + 2, piece.Field.Y - piece.Direction * 2), isBlack);
            }
        }

        private static void CheckPossibleMoveOrJump(Field moveField, Field jumpField, bool isBlack)
        {
            if (moveField == null)
                return;

            var pieceOnMoveField = moveField.GetPieceOnField();
            if (pieceOnMoveField == null)
            {
                MovableFields.Add(moveField);
                return;
            }

            if (jumpField == null)
                return;

            var pieceOnJumpField = jumpField.GetPieceOnField();
            if (pieceOnJumpField == null && pieceOnMoveField.IsBlack != isBlack)
            {
                JumpableFields.Add(jumpField);
            }
        }

        public static void SetPvP()
        {
            IsGameVersusAI = false;
        }

        public static void SetPvAI()
        {
            IsGameVersusAI = true;
        }

        public static void SetPlayerBlack()
        {
            IsPlayerBlack = true;
        }

        public static void SetPlayerWhite()
        {
            IsPlayerBlack = false;
        }

        static public void SetScheme(int scheme)
        {
            SaveSelectedScheme(scheme);
            Page.ApplyScheme(scheme);
        }

        static public async void SaveSelectedScheme(int scheme)
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.CreateFileAsync("scheme.xml", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, scheme.ToString());
            }
            catch (Exception)
            {
            }
        }

        static public async void LoadAndApplySelectedScheme()
        {
            var schemeInt = 0;
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.GetFileAsync("scheme.xml");
                var scheme = await FileIO.ReadTextAsync(file);
                schemeInt = Convert.ToInt32(scheme);
            }
            catch (Exception)
            {
            }

            Page.ApplyScheme(schemeInt);
        }
    }
}
