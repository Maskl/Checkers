using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    enum GameState
    {
        SelectPiece,
        Move,
        Jump
    }

    class GameManager
    {
        static public bool BlackTurn;
        static public GameState State;
        public static Field SelectedField;

        static public void Start()
        {
            BlackTurn = true;
            State = GameState.SelectPiece;
            SelectedField = null;

            Board.NewBoard();
        }

        static public void FieldTapped(Field field)
        {
            Board.SelectField(field);
        }
    }
}
