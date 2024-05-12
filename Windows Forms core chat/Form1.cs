using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows_Forms_Chat
{
    public partial class Form1 : Form
    {
        TicTacToe ticTacToe = new TicTacToe();
        TCPChatServer server = null;
        TCPChatClient client = null;

        private TextBox UsernameTextBox;
        public Form1()
        {
            InitializeComponent();
            // Initialize the UsernameTextBox
            UsernameTextBox = new TextBox();
            UsernameTextBox.Location = new Point(10, 350);
            UsernameTextBox.Size = new Size(200, 20);
            UsernameTextBox.Name = "UsernameTextBox";
            UsernameTextBox.PlaceholderText = "Enter Username";
            this.Controls.Add(UsernameTextBox); // Add UsernameTextBox to the form's controls
            this.TypeTextBox.KeyDown += new KeyEventHandler(TypeTextBox_KeyDown);
        }
        private bool CanHostOrJoin()
        {
            return server == null && client == null;
        }
        private void HostButton_Click(object sender, EventArgs e)
        {
            if (CanHostOrJoin())
            {
                try
                {
                    int port = int.Parse(MyPortTextBox.Text);
                    server = TCPChatServer.CreateInstance(port, ChatTextBox);
                    server.SetupServer();
                }
                catch (Exception ex)
                {
                    ChatTextBox.Text += "Error: " + ex;
                    ChatTextBox.AppendText(Environment.NewLine);
                }
            }
        }

        private void JoinButton_Click(object sender, EventArgs e)
        {
            if (CanHostOrJoin())
            {
                try
                {
                    int port = int.Parse(MyPortTextBox.Text);
                    int serverPort = int.Parse(serverPortTextBox.Text);
                    string serverIP = ServerIPTextBox.Text;
                    string username = UsernameTextBox.Text;
                    client = TCPChatClient.CreateInstance(port, serverPort, serverIP, ChatTextBox, username);
                    client.ConnectToServer();
                }
                catch (Exception ex)
                {
                    client = null;
                    ChatTextBox.Text += "Error: " + ex.Message;
                    ChatTextBox.AppendText(Environment.NewLine);
                }
            }
        }
        private void UsernameTextBox_TextChanged(object sender, EventArgs e)
        {
            // Enable the JoinButton only if the UsernameTextBox is not empty
            JoinButton.Enabled = !string.IsNullOrEmpty(UsernameTextBox.Text.Trim());
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (client != null)
                client.SendString(TypeTextBox.Text);
            else if (server != null)
                server.SendToAll(TypeTextBox.Text, null);
            TypeTextBox.Clear();
        }

        private void TypeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (client != null)
                    client.SendString(TypeTextBox.Text);
                else if (server != null)
                    server.SendToAll(TypeTextBox.Text, null);
                TypeTextBox.Clear();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UsernameTextBox.TextChanged += UsernameTextBox_TextChanged;
            ticTacToe.Buttons.Add(button1);
            ticTacToe.Buttons.Add(button2);
            ticTacToe.Buttons.Add(button3);
            ticTacToe.Buttons.Add(button4);
            ticTacToe.Buttons.Add(button5);
            ticTacToe.Buttons.Add(button6);
            ticTacToe.Buttons.Add(button7);
            ticTacToe.Buttons.Add(button8);
            ticTacToe.Buttons.Add(button9);
        }

        private void AttemptMove(int i)
        {
            if (ticTacToe.MyTurn)
            {
                bool validMove = ticTacToe.SetTile(i, ticTacToe.PlayerTileType);
                if (validMove)
                {
                    GameState gs = ticTacToe.GetGameState();
                    switch (gs)
                    {
                        case GameState.CrossWins:
                            ChatTextBox.AppendText("X wins!");
                            ticTacToe.ResetBoard();
                            break;
                        case GameState.NaughtWins:
                            ChatTextBox.AppendText("O wins!");
                            ticTacToe.ResetBoard();
                            break;
                        case GameState.Draw:
                            ChatTextBox.AppendText("Draw!");
                            ticTacToe.ResetBoard();
                            break;
                    }
                    ChatTextBox.AppendText(Environment.NewLine);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) { AttemptMove(0); }
        private void button2_Click(object sender, EventArgs e) { AttemptMove(1); }
        private void button3_Click(object sender, EventArgs e) { AttemptMove(2); }
        private void button4_Click(object sender, EventArgs e) { AttemptMove(3); }
        private void button5_Click(object sender, EventArgs e) { AttemptMove(4); }
        private void button6_Click(object sender, EventArgs e) { AttemptMove(5); }
        private void button7_Click(object sender, EventArgs e) { AttemptMove(6); }
        private void button8_Click(object sender, EventArgs e) { AttemptMove(7); }
        private void button9_Click(object sender, EventArgs e) { AttemptMove(8); }
    }
}
