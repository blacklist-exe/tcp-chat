using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Windows_Forms_Chat
{
    public enum TileType
    {
        Blank, Cross, Naught
    }

    public enum GameState
    {
        Playing, Draw, CrossWins, NaughtWins
    }

    public class TicTacToe
    {
        public bool MyTurn { get; set; } = false;  // Initial state, should be set by the server
        public TileType PlayerTileType { get; set; } = TileType.Blank;
        public List<Button> Buttons { get; set; } = new List<Button>();  // Managing buttons directly
        public TileType[] Grid { get; private set; } = new TileType[9];
        private List<ClientSocket> players = new List<ClientSocket>();
        public TicTacToe()
        {
            ResetBoard(); // Initialize the board state
        }
        public void RemovePlayer(ClientSocket client)
        {
            players.Remove(client);
            // Additional logic to handle the game state when a player leaves
        }

        public bool IsEmpty()
        {
            return !players.Any();
        }
        public bool SetTile(int index, TileType tileType)
        {
            if (Grid[index] == TileType.Blank)
            {
                Grid[index] = tileType;
                UpdateButton(index, tileType);
                return true;
            }
            return false;
        }

        private void UpdateButton(int index, TileType tileType)
        {
            if (index < Buttons.Count)
            {
                Buttons[index].Text = TileTypeToString(tileType);
                Buttons[index].Enabled = false; // Disable the button after setting to prevent further clicks
            }
        }

        public GameState GetGameState()
        {
            // Check for a win or draw
            if (CheckForWin(TileType.Cross))
                return GameState.CrossWins;
            else if (CheckForWin(TileType.Naught))
                return GameState.NaughtWins;
            else if (CheckForDraw())
                return GameState.Draw;

            return GameState.Playing;
        }
        public class GameSession
        {
            public TicTacToe Game { get; set; }
            public ClientSocket PlayerOne { get; set; }
            public ClientSocket PlayerTwo { get; set; }
            public ClientSocket CurrentTurn { get; set; }

            public GameSession(ClientSocket playerOne, ClientSocket playerTwo)
            {
                Game = new TicTacToe();
                PlayerOne = playerOne;
                PlayerTwo = playerTwo;
                CurrentTurn = playerOne; // Assume player one starts
            }

            public void SwitchTurns()
            {
                CurrentTurn = CurrentTurn == PlayerOne ? PlayerTwo : PlayerOne;
            }
        }

        private bool CheckForWin(TileType t)
        {
            // Check horizontal, vertical, and diagonal lines for a win
            return (CheckLine(0, 1, 2, t) || CheckLine(3, 4, 5, t) || CheckLine(6, 7, 8, t) ||
                    CheckLine(0, 3, 6, t) || CheckLine(1, 4, 7, t) || CheckLine(2, 5, 8, t) ||
                    CheckLine(0, 4, 8, t) || CheckLine(2, 4, 6, t));
        }

        private bool CheckLine(int a, int b, int c, TileType t)
        {
            return Grid[a] == t && Grid[b] == t && Grid[c] == t;
        }

        private bool CheckForDraw()
        {
            // If all fields are filled and no one has won, it's a draw
            for (int i = 0; i < 9; i++)
            {
                if (Grid[i] == TileType.Blank)
                    return false;
            }
            return true;
        }

        public void ResetBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                Grid[i] = TileType.Blank;
                if (i < Buttons.Count)
                {
                    Buttons[i].Text = "";
                    Buttons[i].Enabled = true; // Re-enable the button for new games
                }
            }
        }
        public string GridToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var tile in Grid)
            {
                sb.Append(TileTypeToString(tile));
            }
            return sb.ToString();
        }
        public bool IsValidMove(int index, TileType tileType)
        {
            if (Grid[index] == TileType.Blank)
            {
                Grid[index] = tileType;
                return true;
            }
            return false;
        }
        public static string TileTypeToString(TileType t)
        {
            switch (t)
            {
                case TileType.Cross: return "X";
                case TileType.Naught: return "O";
                default: return "";
            }
        }
    }
}
