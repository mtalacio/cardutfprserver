using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
