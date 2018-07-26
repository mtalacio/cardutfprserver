using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Utils {
    public class ByteBuffer : IDisposable {
        private List<byte> buffer;
        private byte[] readBuff;
        private int readPos;
        private bool buffUpdate = false;

        public ByteBuffer() {
            buffer = new List<byte>();
            readPos = 0;
        }

        public int GetReadPos() {
            return readPos;
        }

        public byte[] ToArray() {
            return buffer.ToArray();
        }

        public int Count() {
            return buffer.Count;
        }

        public int Lenght() {
            return Count() - readPos;
        }

        public void Clear() {
            buffer.Clear();
            readPos = 0;
        }

        #region Write Functions

        public void WriteByte(byte input) {
            buffer.Add(input);
            buffUpdate = true;
        }

        public void WriteBytes(byte[] input) {
            buffer.AddRange(input);
            buffUpdate = true;
        }

        public void WriteShort(short input) {
            buffer.AddRange(BitConverter.GetBytes(input));
            buffUpdate = true;
        }

        public void WriteInteger(int input) {
            buffer.AddRange(BitConverter.GetBytes(input));
            buffUpdate = true;
        }

        public void WriteLong(long input) {
            buffer.AddRange(BitConverter.GetBytes(input));
            buffUpdate = true;
        }

        public void WriteFloat(float input) {
            buffer.AddRange(BitConverter.GetBytes(input));
            buffUpdate = true;
        }

        public void WriteString(string input) {
            buffer.AddRange(BitConverter.GetBytes(input.Length));
            buffer.AddRange(Encoding.ASCII.GetBytes(input));
            buffUpdate = true;
        }

        #endregion

        #region Read Functions

        public int ReadInteger(bool Peek = true) {
            if (buffer.Count > readPos) {
                if (buffUpdate) {
                    readBuff = buffer.ToArray();
                    buffUpdate = false;
                }

                int ret = BitConverter.ToInt32(readBuff, readPos);
                if (Peek & buffer.Count > readPos) {
                    readPos += 4;
                }
                return ret;
            }
            else {
                throw new Exception("Byte Buffer is past limit!");
            }
        }

        public long ReadLong(bool Peek = true) {
            if (buffer.Count > readPos) {
                if (buffUpdate) {
                    readBuff = buffer.ToArray();
                    buffUpdate = false;
                }

                long ret = BitConverter.ToInt64(readBuff, readPos);
                if (Peek & buffer.Count > readPos) {
                    readPos += 8;
                }
                return ret;
            }
            else {
                throw new Exception("Byte Buffer is past limit!");
            }
        }

        public string ReadString(bool Peek = true) {
            int leng = ReadInteger(true);
            if (buffUpdate) {
                readBuff = buffer.ToArray();
                buffUpdate = false;
            }
            string ret = Encoding.ASCII.GetString(readBuff, readPos, leng);
            if (Peek && buffer.Count > readPos) {
                if (ret.Length > 0) {
                    readPos += leng;
                }
            }

            return ret;
        }

        public byte ReadByte(bool Peek = true) {
            if (buffer.Count > readPos) {
                if (buffUpdate) {
                    readBuff = buffer.ToArray();
                    buffUpdate = false;
                }

                byte ret = readBuff[readPos];
                if (Peek & buffer.Count > readPos) {
                    readPos += 1;
                }
                return ret;
            }
            else {
                throw new Exception("Byte Buffer is past limit!");
            }
        }

        public byte[] ReadBytes(int leng, bool Peek = true) {
            if (buffUpdate) {
                readBuff = buffer.ToArray();
                buffUpdate = false;
            }

            byte[] ret = buffer.GetRange(readPos, leng).ToArray();
            if (Peek) {
                readPos += leng;
            }
            return ret;
        }

        public float ReadFloat(bool Peek = true) {
            if (buffer.Count > readPos) {
                if (buffUpdate) {
                    readBuff = buffer.ToArray();
                    buffUpdate = false;
                }

                float ret = BitConverter.ToSingle(readBuff, readPos);
                if (Peek & buffer.Count > readPos) {
                    readPos += 4;
                }
                return ret;
            }
            else {
                throw new Exception("Byte Buffer is past limit!");
            }
        }

        #endregion

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing)
                    buffer.Clear();

                readPos = 0;
            }
            disposedValue = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
