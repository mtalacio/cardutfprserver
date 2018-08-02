using GameServer.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using static GameServer.Enums;

namespace GameServer.Network {
    internal class NetworkSocket {
        private TcpListener _serverSocket;
        public static readonly NetworkSocket Instance = new NetworkSocket();
        private static readonly Client[] Clients = new Client[Constants.MAX_PLAYERS]; // Classe criada no tutorial


        public void ServerStart() {

            for (int i = 0; i < 2; i++) {
                Clients[i] = new Client();
            }

            _serverSocket = new TcpListener(IPAddress.Any, 5500);
            _serverSocket.Start();
            _serverSocket.BeginAcceptTcpClient(OnClientConnect, null);
            Console.WriteLine("Server Started");
        }

        private void OnClientConnect(IAsyncResult result) {
            TcpClient client = _serverSocket.EndAcceptTcpClient(result);
            client.NoDelay = false;
            _serverSocket.BeginAcceptTcpClient(OnClientConnect, null);

            for (int i = 0; i < 2; i++) {
                if (Clients[i].Socket1 == null) {
                    Clients[i].Socket1 = client;
                    Clients[i].Index = i;
                    Clients[i].Ip = client.Client.RemoteEndPoint.ToString();
                    Clients[i].Start();
                    Console.WriteLine("Incoming Connection from: " + Clients[i].Ip + "|| Index: " + i);
                    SendWelcomeMessage(i);
                    return;
                }
            }
        }

        public static void SendDataTo(long index, byte[] data) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
            buffer.WriteBytes(data);
            Clients[index].MyStream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
        }

        private static void SendWelcomeMessage(long index) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SWelcome);
            Console.WriteLine("Sending WelcomeMessage to index: " + index);

            buffer.WriteString("Conectado ao servidor!");

            SendDataTo(index, buffer.ToArray());
        }
    }
}
