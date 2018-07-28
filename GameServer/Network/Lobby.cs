using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Network {
    public static class Lobby {

        private static int playersReady = 0;

        public static void PlayerReady(long index) {
            Console.WriteLine("Player Ready: " + index);
            playersReady++;

            if (playersReady >= Constants.MAX_PLAYERS)
                GameEngine.InitializeGame();
        }

    }
}
