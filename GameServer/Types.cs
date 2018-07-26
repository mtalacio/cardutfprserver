using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer {
    class Types {

        public static TempPlayerRec[] tempPlayer = new TempPlayerRec[Constants.MAX_PLAYERS];

        public struct TempPlayerRec {
            public ByteBuffer buffer;
            public int dataBytes;
            public int dataPackets;
        }
    }
}
