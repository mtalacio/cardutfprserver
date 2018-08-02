namespace GameServer {
    public class Player {
        public long Index { get; }
        public string PlayerName { get; }

        public Player(long index, string playerName) {
            Index = index;
            PlayerName = playerName;
        }

    }
}
