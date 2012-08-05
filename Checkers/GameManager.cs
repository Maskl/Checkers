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

        static public void Start()
        {
            BlackTurn = true;
            SelectedPiece = null;
            MovableFields = new List<Field>();
            JumpableFields = new List<Field>();

            Board.NewBoard();
        }

        static public void FieldTapped(Field field)
        {
            if (!field.IsUsableInGame())
                return;

            if (JumpableFields.Contains(field))
            {
                SelectedPiece.SetPosition(field);
                return;
            }
           
            if (MovableFields.Contains(field))
            {
                SelectedPiece.SetPosition(field);
                return;
            }

            foreach (var piece in Board.Pieces.Where(piece => piece.Field == field && piece.IsBlack == BlackTurn))
            {
                SelectPiece(piece);
            }
        }

        private static void SelectPiece(Piece piece)
        {
            SelectedPiece = piece;
            Board.SelectField(piece.Field);

            MovableFields.Clear();
            JumpableFields.Clear();

            CheckPossibleMoveOrJump(Board.GetField(piece.Field.X - 1, piece.Field.Y + piece.Direction),
                                    Board.GetField(piece.Field.X - 2, piece.Field.Y + piece.Direction * 2));

            CheckPossibleMoveOrJump(Board.GetField(piece.Field.X + 1, piece.Field.Y + piece.Direction),
                                    Board.GetField(piece.Field.X + 2, piece.Field.Y + piece.Direction * 2));

            if (piece.IsKing)
            {
                CheckPossibleMoveOrJump(Board.GetField(piece.Field.X - 1, piece.Field.Y - piece.Direction),
                                        Board.GetField(piece.Field.X - 2, piece.Field.Y - piece.Direction * 2));

                CheckPossibleMoveOrJump(Board.GetField(piece.Field.X + 1, piece.Field.Y - piece.Direction),
                                        Board.GetField(piece.Field.X + 2, piece.Field.Y - piece.Direction * 2));
            }

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

        private static void CheckPossibleMoveOrJump(Field moveField, Field jumpField)
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
            if (pieceOnJumpField == null && pieceOnMoveField.IsBlack != SelectedPiece.IsBlack)
            {
                JumpableFields.Add(jumpField);
            }
        }
    }
}
