using GameServer.Network;
using GameServer.Utils;
using System;
using System.Linq;
using System.Threading;
using static GameServer.Enums;

namespace GameServer {
    class Program {
        private static Thread threadConsole;
        private static bool consoleRunning;

        static void Main(string[] args) {
            threadConsole = new Thread(new ThreadStart(ConsoleThread));
            threadConsole.Start();
            SetupServer();
        }

        static void SetupServer() {
            for (int i = 0; i < Constants.MAX_PLAYERS; i++) {
                Types.tempPlayer[i] = new Types.TempPlayerRec();
            }

            ServerHandleData.InitMessages();
            NetworkSocket.instance.ServerStart();
            MySQL.MySQLInit();
        }


        private static void ConsoleThread() {
            string line;
            consoleRunning = true;

            while (consoleRunning) {
                line = Console.ReadLine();

                if (String.IsNullOrWhiteSpace(line)) {
                    consoleRunning = false;
                    return;
                }
                else {
                    KickPlayer(0);
                }
            }
        }

        private static void KickPlayer(int index) {
            Console.WriteLine("Kicking Player index: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SKick);
            buffer.WriteLong(index);
            NetworkSocket.SendDataTo(0, buffer.ToArray());
            buffer = null;
        }
    }
}
