using GameServer.Utils;

namespace GameServer.Network {
    class Types {

        public static TempPlayerRec[] tempPlayer = new TempPlayerRec[Constants.MAX_PLAYERS];

        public struct TempPlayerRec {
            public ByteBuffer buffer;
            public int dataBytes;
            public int dataPackets;
        }
    }
}
