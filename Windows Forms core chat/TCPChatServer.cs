using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Windows_Forms_Chat
{
    public class TCPChatServer : TCPChatBase
    {
        public Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public List<ClientSocket> clientSockets = new List<ClientSocket>();
        public Dictionary<ClientSocket, ClientState> clientStates = new Dictionary<ClientSocket, ClientState>();
        public Dictionary<ClientSocket, GameSession> clientGames = new Dictionary<ClientSocket, GameSession>();
        public HashSet<string> activeUsernames = new HashSet<string>(); // Declare the activeUsernames container
        private ModeratorManager moderatorManager;
        private DatabaseManager dbManager = new DatabaseManager();
        private TextBox chatTextBox;
        
        public enum ClientState
        {
            Login,
            Chatting,
            Playing
        }

        public static TCPChatServer CreateInstance(int port, TextBox chatTextBox)
        {
            TCPChatServer tcp = new TCPChatServer();
            tcp.port = port;
            tcp.chatTextBox = chatTextBox;
            tcp.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Set the socket option to reuse the address to avoid "Only one usage of each socket address is normally permitted" error
            tcp.serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            return tcp;
        }
        public void SetupServer()
        {
            try
            {
                serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                serverSocket.Listen(0);
                serverSocket.BeginAccept(AcceptCallback, null);
                AddToChat("Server setup complete\n");
            }
            catch (SocketException ex)
            {
                AddToChat("Socket exception: " + ex.Message);
            }
            catch (Exception ex)
            {
                AddToChat("Error setting up server: " + ex.Message);
            }
        }


        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                Socket clientSocket = serverSocket.EndAccept(AR);
                ClientSocket newClientSocket = new ClientSocket
                {
                    socket = clientSocket
                };
                clientSockets.Add(newClientSocket);
                clientStates[newClientSocket] = ClientState.Login;

                clientSocket.BeginReceive(newClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, newClientSocket);
                chatTextBox.Invoke(new Action(() => {
                    chatTextBox.AppendText("Client connected: " + clientSocket.RemoteEndPoint.ToString() + "\n");
                }));

                serverSocket.BeginAccept(AcceptCallback, null);
            }
            catch (ObjectDisposedException)
            {
                return; // Ignore this error which signals that the server has been closed
            }
            catch (Exception e)
            {
                chatTextBox.Invoke(new Action(() => {
                    chatTextBox.AppendText("Error accepting client: " + e.Message + "\n");
                }));
            }
        }

        public void ReceiveCallback(IAsyncResult AR)
        {
            ClientSocket currentClientSocket = (ClientSocket)AR.AsyncState;
            int received;

            try
            {
                received = currentClientSocket.socket.EndReceive(AR);
            }
            catch (SocketException)
            {
                AddToChat("Client forcefully disconnected");
                HandleClientExit(currentClientSocket);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(currentClientSocket.buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);

            if (clientStates.ContainsKey(currentClientSocket))
            {
                switch (clientStates[currentClientSocket])
                {
                    case ClientState.Login:
                        HandleLogin(currentClientSocket, text);
                        break;

                    case ClientState.Chatting:
                        HandleChatCommands(currentClientSocket, text);
                        break;

                    case ClientState.Playing:
                        HandleGameCommands(currentClientSocket, text);
                        break;
                }
            }
            else
            {
                AddToChat("Client state not found for: " + currentClientSocket.username);
            }

            currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
        }

        private void HandleLogin(ClientSocket client, string text)
        {
            if (text.StartsWith("!username "))
            {
                string desiredUsername = text.Split(' ')[1].Trim();
                HandleUsernameSetting(client, desiredUsername);
                clientStates[client] = ClientState.Chatting;
                SendString(client, "You are now logged in and can chat.");
                SendToAll($"Server: {desiredUsername} has joined the chat.", null);
            }
        }
        // Manages moderator information, interfacing with a SQLite database to persist data.
        public class ModeratorManager
        {
            private HashSet<string> moderators; // Set to keep track of all moderators in memory for fast access.
            private string connectionString; // Connection string to access the SQLite database.

            // Constructor that initializes the moderator manager with the database path.
            public ModeratorManager(string dbPath)
            {
                moderators = new HashSet<string>();
                connectionString = $"Data Source={dbPath};Version=3;";
                LoadModerators(); // Load existing moderators from the database into memory.
            }

            // Loads moderators from the database and initializes the table if it doesn't exist.
            private void LoadModerators()
            {
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    // Ensure the Moderators table exists.
                    string query = "CREATE TABLE IF NOT EXISTS Moderators (Username TEXT PRIMARY KEY);";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Load all existing moderator usernames into the HashSet.
                    query = "SELECT Username FROM Moderators;";
                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            moderators.Add(reader.GetString(0));
                        }
                    }
                }
            }

            // Adds a new moderator and updates the database.
            public bool AddModerator(string username)
            {
                // Add the username to the in-memory set.
                if (!moderators.Add(username))
                    return false;

                // Update the database with the new moderator.
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Moderators (Username) VALUES (@Username);";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SQLiteException e)
                        {
                            // Handle cases like duplicate entries
                            Console.WriteLine("Error adding moderator to the database: " + e.Message);
                            return false;
                        }
                    }
                }
                return true;
            }

            // Removes a moderator from both the in-memory set and the database.
            public bool RemoveModerator(string username)
            {
                // Remove the username from the in-memory set.
                if (!moderators.Remove(username))
                    return false;

                // Remove the moderator from the database.
                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Moderators WHERE Username = @Username;";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }

            // Checks if a specified username is a moderator.
            public bool IsModerator(string username)
            {
                return moderators.Contains(username);
            }
        }
        private void HandleModerationCommands(ClientSocket client, string text)
        {
            // Split the incoming text to parse commands and subcommands
            var parts = text.Split(' ');

            // Check if the first part of the command is '!mod' and if it's issued by the server
            if (parts[0].ToLower() == "!mod" && client.username == "Server")
            {
                // Ensure there are enough parts for the command to be valid
                if (parts.Length >= 3)
                {
                    var subCommand = parts[1];  // This can be 'add' or 'remove'
                    var username = parts[2];   // The username to add or remove as moderator

                    // Handle the different types of subcommands
                    switch (subCommand.ToLower())
                    {
                        case "add":
                            // Try to add a moderator and inform the server user of the outcome
                            if (moderatorManager.AddModerator(username))
                            {
                                // Optionally save changes to disk or handle state changes
                                SendString(client, $"{username} added as a moderator.");
                            }
                            else
                            {
                                SendString(client, $"Failed to add {username} as a moderator.");
                            }
                            break;

                        case "remove":
                            // Try to remove a moderator and inform the server user of the outcome
                            if (moderatorManager.RemoveModerator(username))
                            {
                                // Optionally save changes to disk or handle state changes
                                SendString(client, $"{username} removed as a moderator.");
                            }
                            else
                            {
                                SendString(client, $"Failed to remove {username} as a moderator.");
                            }
                            break;

                        default:
                            // If the subcommand is neither 'add' nor 'remove'
                            SendString(client, "Invalid mod command. Use 'add' or 'remove'.");
                            break;
                    }
                }
                else
                {
                    // Provide usage instructions if the command format is incorrect
                    SendString(client, "Usage: !mod [add/remove] [username]");
                }
            }
            else
            {
                SendString(client, "You do not have permission to perform this action.");
            }
        }
        private void HandleChatCommands(ClientSocket client, string text)
        {
            if (text.StartsWith("!join"))
            {
                // Check for an existing game session or create a new one
                GameSession session = clientGames.Values.FirstOrDefault(s => s.Player1 != null && s.Player2 == null);

                if (session == null)
                {
                    // Create a new game session
                    session = new GameSession { Player1 = client, CurrentTurn = client };
                    clientGames[client] = session;
                    SendString(client, "You are player 1. Waiting for another player.");
                }
                else
                {
                    // Add client as Player 2 to the existing session
                    session.Player2 = client;
                    clientGames[client] = session;
                    SendString(client, "You are player 2. Game starts now!");

                    // Notify both players that the game has started
                    SendString(session.Player1, "Game starts now! Your opponent is " + session.Player2.username);
                    SendString(session.Player2, "Game starts now! Your opponent is " + session.Player1.username);

                    // Update client states
                    clientStates[session.Player1] = ClientState.Playing;
                    clientStates[session.Player2] = ClientState.Playing;

                    // Inform the current player it's their turn
                    SendString(session.CurrentTurn, "It's your turn to move.");
                }
            }
            else if (text.StartsWith("!who"))
            {
                string userList = string.Join(", ", clientSockets.Select(c => string.IsNullOrEmpty(c.username) ? "Anonymous" : c.username));
                SendString(client, $"Connected users: {userList}");
            }
            else if (text.StartsWith("!listusers"))
            {
                var users = dbManager.GetAllUsers();
                string userList = string.Join(", ", users.Select(u => $"{u.Username} - Wins: {u.Wins}, Losses: {u.Losses}, Draws: {u.Draws}"));
                SendString(client, $"Users: {userList}");
            }
            else if (text.StartsWith("!scores"))
            {
                var users = dbManager.GetUsersSortedByScore();
                string sortedUsers = string.Join(", ", users.Select(u => $"{u.Username}: {u.Wins}W-{u.Losses}L-{u.Draws}D"));
                SendString(client, $"Scores: {sortedUsers}");
            }
        }

        private void HandleGameCommands(ClientSocket client, string text)
        {
            if (text.StartsWith("!move "))
            {
                string moveData = text.Substring(6);
                int position;
                if (int.TryParse(moveData, out position) && position >= 0 && position <= 8)
                {
                    GameSession session = GetGameSession(client);

                    if (session != null && session.CurrentTurn == client)
                    {
                        if (session.Game.IsValidMove(position, client.PlayerTileType))
                        {
                            session.Game.SetTile(position, client.PlayerTileType);

                            GameState state = session.Game.GetGameState();
                            string boardUpdate = session.Game.GridToString();
                            SendToAll($"Update: {boardUpdate}", null);

                            switch (state)
                            {
                                case GameState.CrossWins:
                                case GameState.NaughtWins:
                                case GameState.Draw:
                                    EndGame(session, state);
                                    break;
                                default:
                                    NotifyNextPlayer(session);
                                    break;
                            }
                        }
                        else
                        {
                            SendString(client, "Invalid move or not your turn.");
                        }
                    }
                }
                else
                {
                    SendString(client, "Invalid move. Please choose a valid position (0-8).");
                }
            }
            else if (text.StartsWith("!status"))
            {
                GameSession session = GetGameSession(client);
                if (session != null)
                {
                    string status = session.Game.GridToString();
                    SendString(client, $"Current Board: {status}");
                }
                else
                {
                    SendString(client, "You are not currently in a game.");
                }
            }
            else if (text.StartsWith("!quit"))
            {
                GameSession session = GetGameSession(client);
                if (session != null)
                {
                    RemoveFromGameSession(client);
                    SendString(client, "You have left the game.");
                    SendToAll($"Server: {client.username} has quit the game.", client);
                }
            }
        }

        private GameSession GetGameSession(ClientSocket client)
        {
            if (clientGames.TryGetValue(client, out var session))
                return session;

            return clientGames.Values.FirstOrDefault(s => s.Player1 == client || s.Player2 == client);
        }

        private void NotifyNextPlayer(GameSession session)
        {
            if (session.CurrentTurn == session.Player1)
            {
                session.CurrentTurn = session.Player2;
            }
            else
            {
                session.CurrentTurn = session.Player1;
            }

            SendString(session.CurrentTurn, "It's your turn to move.");
        }

        private void EndGame(GameSession session, GameState state)
        {
            // Announce the winner and reset the game
            string winner = "", loser = "";
            switch (state)
            {
                case GameState.CrossWins:
                    winner = session.Player1.username;
                    loser = session.Player2.username;
                    SendToAll("Player X wins!", null);
                    break;
                case GameState.NaughtWins:
                    winner = session.Player2.username;
                    loser = session.Player1.username;
                    SendToAll("Player O wins!", null);
                    break;
                case GameState.Draw:
                    SendToAll("It's a draw!", null);
                    break;
            }

            // Update database records
            if (state != GameState.Draw)
            {
                dbManager.UpdateUserRecord(winner, 1, 0, 0); // Increment wins for the winner
                dbManager.UpdateUserRecord(loser, 0, 1, 0); // Increment losses for the loser
            }
            else
            {
                dbManager.UpdateUserRecord(session.Player1.username, 0, 0, 1); // Increment draws
                dbManager.UpdateUserRecord(session.Player2.username, 0, 0, 1); // Increment draws
            }

            // Reset players' states
            clientStates[session.Player1] = ClientState.Chatting;
            clientStates[session.Player2] = ClientState.Chatting;

            // Remove the game session
            RemoveFromGameSession(session);
        }

        private void RemoveFromGameSession(GameSession session)
        {
            if (session.Player1 != null)
            {
                clientGames.Remove(session.Player1);
                clientStates[session.Player1] = ClientState.Chatting;
            }

            if (session.Player2 != null)
            {
                clientGames.Remove(session.Player2);
                clientStates[session.Player2] = ClientState.Chatting;
            }
        }

        private void RemoveFromGameSession(ClientSocket client)
        {
            GameSession session = GetGameSession(client);
            if (session != null)
            {
                if (session.Player1 == client)
                    session.Player1 = null;
                else if (session.Player2 == client)
                    session.Player2 = null;

                if (session.Player1 == null && session.Player2 == null)
                    clientGames.Remove(client);
            }
        }

        private void HandleClientExit(ClientSocket clientSocket)
        {
            RemoveFromGameSession(clientSocket);
            clientSocket.socket.Shutdown(SocketShutdown.Both);
            clientSocket.socket.Close();
            clientSockets.Remove(clientSocket);
            if (!string.IsNullOrEmpty(clientSocket.username))
            {
                activeUsernames.Remove(clientSocket.username);
                AddToChat("Client disconnected");
            }
        }

        private void HandleUsernameSetting(ClientSocket clientSocket, string desiredUsername)
        {
            if (desiredUsername.Equals("Server", StringComparison.OrdinalIgnoreCase))
            {
                SendString(clientSocket, "That is a restricted Username. Please try a different one.");
                return;
            }

            if (activeUsernames.Contains(desiredUsername))
            {
                SendString(clientSocket, "Username is already in use. Please try a different one.");
            }
            else
            {
                activeUsernames.Add(desiredUsername);
                clientSocket.username = desiredUsername;
                clientStates[clientSocket] = ClientState.Chatting;
                SendString(clientSocket, "Username set successfully.");
                SendToAll($"Server: {desiredUsername} has joined the chat.", clientSocket);
            }
        }

        new public void SendToAll(string message, ClientSocket from = null)
        {
            string senderName = from != null && !string.IsNullOrEmpty(from.username) ? from.username : "Server";
            string fullMessage = $"{senderName}: {message}";

            // Log to the server's chatTextBox
            AddToChat(fullMessage);

            // Convert message to byte array
            byte[] data = Encoding.ASCII.GetBytes(fullMessage);

            // Send to all connected clients
            foreach (ClientSocket client in clientSockets)
            {
                if (from == null || !from.socket.Equals(client.socket)) // Ensure not sending to the sender if necessary
                {
                    try
                    {
                        client.socket.Send(data);
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine($"Error sending message to {client.username}: {ex.Message}");
                    }
                }
            }
        }

        new private void SendString(ClientSocket client, string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            client.socket.Send(data);
        }

        new private void AddToChat(string message)
        {
            if (chatTextBox.InvokeRequired)
            {
                chatTextBox.Invoke(new MethodInvoker(delegate { AddToChat(message); }));
            }
            else
            {
                chatTextBox.AppendText(message + Environment.NewLine);
            }
        }

        public class GameSession
        {
            public ClientSocket Player1 { get; set; }
            public ClientSocket Player2 { get; set; }
            public ClientSocket CurrentTurn { get; set; }
            public TicTacToe Game { get; set; } = new TicTacToe();
        }
    }
}
