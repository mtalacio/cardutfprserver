using System;
using System.Net.Sockets;

namespace GameServer.Network {
    internal class Client {
        public int Index { private get; set; }
        public string Ip { get; set; }
        public TcpClient Socket1 { get; set; }
        public NetworkStream MyStream { get; private set; }

        private byte[] _readBuff;

        public void Start() {
            Socket1.SendBufferSize = 4096;
            Socket1.ReceiveBufferSize = 4096;
            MyStream = Socket1.GetStream();
            Array.Resize(ref _readBuff, Socket1.ReceiveBufferSize);
            MyStream.BeginRead(_readBuff, 0, Socket1.ReceiveBufferSize, OnReceiveData, null);
        }

        private void OnReceiveData(IAsyncResult result) {
            try {
                int readBytes = MyStream.EndRead(result);
                if (Socket1 == null) {
                    return;
                }
                if (readBytes <= 0) {
                    CloseConnection();
                    return;
                }

                byte[] newBytes = null;
                Array.Resize(ref newBytes, readBytes);
                Buffer.BlockCopy(_readBuff, 0, newBytes, 0, readBytes);

                ServerHandleData.HandleData(Index, newBytes);

                if (Socket1 == null) {
                    return;
                }

                MyStream.BeginRead(_readBuff, 0, Socket1.ReceiveBufferSize, OnReceiveData, null);

            }
            catch (Exception e) {
                CloseConnection();
                Console.WriteLine(e.Message);
            }
        }

        private void CloseConnection() {
            Socket1.Close();
            Socket1 = null;
            Console.WriteLine("Connection of client index: " + Index + " closed and destroyed.");
        }
    }
}
