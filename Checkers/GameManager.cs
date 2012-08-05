using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class GameManager
    {
        static public bool BlackTurn;
        static public Piece SelectedPiece;
        static public List<Field> JumpableFields;
        static public List<Field> MovableFields;
        static public List<Field> SelectableFields;

        static public void Start()
        {
            BlackTurn = true;
            SelectedPiece = null;
            MovableFields = new List<Field>();
            JumpableFields = new List<Field>();
            SelectableFields = new List<Field>();

            Board.NewBoard();
            SelectAllCurrentPlayerPieces();
        }

        static public void FieldTapped(Field field)
        {
            if (!field.IsUsableInGame())
                return;

            if (JumpableFields.Contains(field))
            {
                var enemy = Board.Fields[field.Y + (SelectedPiece.Field.Y - field.Y) / 2][field.X + (SelectedPiece.Field.X - field.X) / 2].GetPieceOnField();
                enemy.Destroy();
                Board.Pieces.Remove(enemy);

                SelectedPiece.SetPosition(field);

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
                SelectedPiece.SetPosition(field);
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
    }
}
