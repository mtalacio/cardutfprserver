using GameServer.Utils;

namespace GameServer.Network {
    internal static class Types {

        public static readonly TempPlayerRec[] TempPlayer = new TempPlayerRec[Constants.MAX_PLAYERS];

        public struct TempPlayerRec {
            public ByteBuffer Buffer { get; set; }
            //public int DataBytes { get; }
            //public int DataPackets { get; }
        }
    }
}
