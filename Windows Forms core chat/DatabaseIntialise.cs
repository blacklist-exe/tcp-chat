using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Windows_Forms_Chat
{
    public class DatabaseManager
    {
        private const string connectionString = "Data Source=ChatDatabase.sqlite;Version=3;";

        // Initialize database and create table if it doesn't exist
        public void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = @"
                CREATE TABLE IF NOT EXISTS Users (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Username TEXT UNIQUE NOT NULL,
                    Wins INTEGER DEFAULT 0,
                    Losses INTEGER DEFAULT 0,
                    Draws INTEGER DEFAULT 0
                );";
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Add or update a user's record
        public void UpdateUserRecord(string username, int wins, int losses, int draws)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var cmd = new SQLiteCommand("INSERT OR REPLACE INTO Users (Username, Wins, Losses, Draws) VALUES (@username, @wins, @losses, @draws)", connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@wins", wins);
                cmd.Parameters.AddWithValue("@losses", losses);
                cmd.Parameters.AddWithValue("@draws", draws);
                cmd.ExecuteNonQuery();
            }
        }

        // Delete a user
        public void DeleteUser(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var cmd = new SQLiteCommand("DELETE FROM Users WHERE Username = @username", connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.ExecuteNonQuery();
            }
        }

        // Retrieve a single user
        public User GetUser(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM Users WHERE Username = @username", connection);
                cmd.Parameters.AddWithValue("@username", username);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Username = reader["Username"].ToString(),
                            Wins = Convert.ToInt32(reader["Wins"]),
                            Losses = Convert.ToInt32(reader["Losses"]),
                            Draws = Convert.ToInt32(reader["Draws"])
                        };
                    }
                }
            }
            return null;
        }

        // List all users
        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM Users", connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Username = reader["Username"].ToString(),
                            Wins = Convert.ToInt32(reader["Wins"]),
                            Losses = Convert.ToInt32(reader["Losses"]),
                            Draws = Convert.ToInt32(reader["Draws"])
                        });
                    }
                }
            }
            return users;
        }

        // List users sorted by score
        public List<User> GetUsersSortedByScore()
        {
            var users = new List<User>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var cmd = new SQLiteCommand("SELECT * FROM Users ORDER BY Wins DESC, Draws DESC, Losses ASC", connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Username = reader["Username"].ToString(),
                            Wins = Convert.ToInt32(reader["Wins"]),
                            Losses = Convert.ToInt32(reader["Losses"]),
                            Draws = Convert.ToInt32(reader["Draws"])
                        });
                    }
                }
            }
            return users;
        }
    }

    // Data model for user information
    public class User
    {
        public string Username { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
    }
}
