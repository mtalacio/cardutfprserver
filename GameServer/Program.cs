using GameServer.Network;
using GameServer.Utils;
using System;
using System.Threading;
using static GameServer.Enums;

namespace GameServer {
    internal static class Program {
        private static Thread _threadConsole;
        private static bool _consoleRunning;

        private static void Main(string[] args) {
            _threadConsole = new Thread(ConsoleThread);
            _threadConsole.Start();
            SetupServer();
        }

        private static void SetupServer() {
            for (int i = 0; i < Constants.MAX_PLAYERS; i++) {
                Types.TempPlayer[i] = new Types.TempPlayerRec();
            }

            ServerHandleData.InitMessages();
            NetworkSocket.Instance.ServerStart();
            MySql.MySqlInit();
        }


        private static void ConsoleThread() {
            _consoleRunning = true;

            while (_consoleRunning) {
                string line = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(line)) continue;

                _consoleRunning = false;
                return;
            }
        }

        private static void KickPlayer(int index) {
            Console.WriteLine("Kicking Player index: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SKick);
            buffer.WriteLong(index);
            NetworkSocket.SendDataTo(0, buffer.ToArray());
        }
    }
}
