using GameServer.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using static GameServer.Enums;

namespace GameServer.Network {
    class NetworkSocket {
        public TcpListener ServerSocket;
        public static NetworkSocket instance = new NetworkSocket();
        public static Client[] clients = new Client[Constants.MAX_PLAYERS]; // Classe criada no tutorial


        public void ServerStart() {

            for (int i = 0; i < 2; i++) {
                clients[i] = new Client();
            }

            ServerSocket = new TcpListener(IPAddress.Any, 5500);
            ServerSocket.Start();
            ServerSocket.BeginAcceptTcpClient(OnClientConnect, null);
            Console.WriteLine("Server Started");
        }

        private void OnClientConnect(IAsyncResult result) {
            TcpClient client = ServerSocket.EndAcceptTcpClient(result);
            client.NoDelay = false;
            ServerSocket.BeginAcceptTcpClient(OnClientConnect, null);

            for (int i = 0; i < 2; i++) {
                if (clients[i].socket == null) {
                    clients[i].socket = client;
                    clients[i].index = i;
                    clients[i].ip = client.Client.RemoteEndPoint.ToString();
                    clients[i].Start();
                    Console.WriteLine("Incoming Connection from: " + clients[i].ip + "|| Index: " + i);
                    SendWelcomeMessage(i);
                    return;
                }
            }
        }

        public static void SendDataTo(long index, byte[] data) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
            buffer.WriteBytes(data);
            clients[index].myStream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
            buffer = null;
        }

        public static void SendWelcomeMessage(long index) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SWelcome);
            Console.WriteLine("Sending WelcomeMessage to index: " + index);
            buffer.WriteString("Conectado ao servidor!");
            SendDataTo(index, buffer.ToArray());
        }
    }
}
