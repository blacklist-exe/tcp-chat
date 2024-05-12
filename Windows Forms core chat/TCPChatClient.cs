using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Windows_Forms_Chat
{
    public class TCPChatClient : TCPChatBase
    {
        public Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private const int BUFFER_SIZE = 2048;
        private byte[] buffer = new byte[BUFFER_SIZE];
        private new int port;
        private int serverPort;
        private string serverIP;
        private string username;

        public static TCPChatClient CreateInstance(int port, int serverPort, string serverIP, TextBox chatTextBox, string username)
        {
            if (port > 0 && port < 65535 && serverPort > 0 && serverPort < 65535 && !string.IsNullOrEmpty(serverIP) && chatTextBox != null)
            {
                TCPChatClient tcpClient = new TCPChatClient
                {
                    port = port,
                    serverPort = serverPort,
                    serverIP = serverIP,
                    chatTextBox = chatTextBox,
                    username = username
                };
                return tcpClient;
            }
            return null;
        }

        public void ConnectToServer()
        {
            try
            {
                clientSocket.Bind(new IPEndPoint(IPAddress.Any, port));
                clientSocket.Connect(new IPEndPoint(IPAddress.Parse(serverIP), serverPort));
                AddToChat("Connected to server");
                clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, this);
                SendString($"!username {username}");
            }
            catch (SocketException ex)
            {
                AddToChat($"Error connecting to server: {ex.Message}");
            }
        }

        public void SendString(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            clientSocket.Send(data);
        }

        public void ReceiveCallback(IAsyncResult AR)
        {
            int received;
            try
            {
                received = clientSocket.EndReceive(AR);
            }
            catch (SocketException)
            {
                AddToChat("Disconnected from server");
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);

            AddToChat(text);
            clientSocket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, this);
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
    }
}
