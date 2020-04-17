namespace GameServer {
    public class Player {
        public long Index { get; }
        public string PlayerName { get; }

        public int Mana { get; set; }
        public int ManaRemaining { get; set; }

        public Player(long index, string playerName) {
            Index = index;
            PlayerName = playerName;
        }

    }
}
