using System;

namespace Windows_Forms_Chat
{
    public class GameSession
    {
        public TicTacToe Game { get; private set; }
        public ClientSocket Player1 { get; private set; }
        public ClientSocket Player2 { get; private set; }
        public ClientSocket CurrentTurn { get; private set; }

        public GameSession(ClientSocket player1, ClientSocket player2)
        {
            Player1 = player1;
            Player2 = player2;
            Game = new TicTacToe();
            CurrentTurn = Player1;

            // Assign tile types to players
            Player1.PlayerTileType = TileType.Cross;
            Player2.PlayerTileType = TileType.Naught;
        }

        public void SwitchTurns()
        {
            CurrentTurn = CurrentTurn == Player1 ? Player2 : Player1;
        }

        public void RemovePlayer(ClientSocket client)
        {
            if (Player1 == client) Player1 = null;
            if (Player2 == client) Player2 = null;
        }

        public bool HasPlayers()
        {
            return Player1 != null || Player2 != null;
        }
    }
}
