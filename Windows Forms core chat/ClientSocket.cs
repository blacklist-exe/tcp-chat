using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Windows_Forms_Chat
{
    public class ClientSocket
    {
        // Socket connection to the client
        public Socket socket;
        // Buffer size for data received
        public const int BUFFER_SIZE = 2048;
        // Buffer for data being received
        public byte[] buffer = new byte[BUFFER_SIZE];
        // Username for the client
        public string username; // Added username property
        public TileType PlayerTileType { get; set; }
        // Additional attributes such as client state, etc., can also be added here
    }
}
