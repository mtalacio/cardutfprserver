using System;

namespace GameServer.Network {
    public static class Lobby {

        private static int _playersReady;

        public static void PlayerReady(long index) {
            Console.WriteLine("Player Ready: " + index);
            _playersReady++;

            if (_playersReady >= Constants.MAX_PLAYERS)
                GameEngine.InitializeGame();
        }

    }
}
