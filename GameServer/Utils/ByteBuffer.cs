using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Utils {
    public class ByteBuffer : IDisposable {
        private readonly List<byte> _buffer;
        private byte[] _readBuff;
        private int _readPos;
        private bool _buffUpdate;

        public ByteBuffer() {
            _buffer = new List<byte>();
            _readPos = 0;
        }

        public int GetReadPos() {
            return _readPos;
        }

        public byte[] ToArray() {
            return _buffer.ToArray();
        }

        public int Count() {
            return _buffer.Count;
        }

        public int Lenght() {
            return Count() - _readPos;
        }

        public void Clear() {
            _buffer.Clear();
            _readPos = 0;
        }

        #region Write Functions

        public void WriteByte(byte input) {
            _buffer.Add(input);
            _buffUpdate = true;
        }

        public void WriteBytes(byte[] input) {
            _buffer.AddRange(input);
            _buffUpdate = true;
        }

        public void WriteShort(short input) {
            _buffer.AddRange(BitConverter.GetBytes(input));
            _buffUpdate = true;
        }

        public void WriteInteger(int input) {
            _buffer.AddRange(BitConverter.GetBytes(input));
            _buffUpdate = true;
        }

        public void WriteLong(long input) {
            _buffer.AddRange(BitConverter.GetBytes(input));
            _buffUpdate = true;
        }

        public void WriteFloat(float input) {
            _buffer.AddRange(BitConverter.GetBytes(input));
            _buffUpdate = true;
        }

        public void WriteString(string input) {
            _buffer.AddRange(BitConverter.GetBytes(input.Length));
            _buffer.AddRange(Encoding.ASCII.GetBytes(input));
            _buffUpdate = true;
        }

        #endregion

        #region Read Functions

        public int ReadInteger(bool peek = true) {

            if (_buffer.Count <= _readPos)
                throw new Exception("Byte Buffer is past limit!");

            if (_buffUpdate) {
                _readBuff = _buffer.ToArray();
                _buffUpdate = false;
            }

            int ret = BitConverter.ToInt32(_readBuff, _readPos);
            if (peek & _buffer.Count > _readPos) {
                _readPos += 4;
            }
            return ret;

        }

        public long ReadLong(bool peek = true) {
            if (_buffer.Count <= _readPos)
                throw new Exception("Byte Buffer is past limit!");

            if (_buffUpdate) {
                _readBuff = _buffer.ToArray();
                _buffUpdate = false;
            }

            long ret = BitConverter.ToInt64(_readBuff, _readPos);
            if (peek & _buffer.Count > _readPos) {
                _readPos += 8;
            }
            return ret;

        }

        public string ReadString(bool peek = true) {
            int leng = ReadInteger();
            if (_buffUpdate) {
                _readBuff = _buffer.ToArray();
                _buffUpdate = false;
            }
            string ret = Encoding.ASCII.GetString(_readBuff, _readPos, leng);

            if (!peek || _buffer.Count <= _readPos)
                return ret;

            if (ret.Length > 0) {
                _readPos += leng;
            }

            return ret;
        }

        public byte ReadByte(bool peek = true) {
            if (_buffer.Count <= _readPos)
                throw new Exception("Byte Buffer is past limit!");

            if (_buffUpdate) {
                _readBuff = _buffer.ToArray();
                _buffUpdate = false;
            }

            byte ret = _readBuff[_readPos];
            if (peek & _buffer.Count > _readPos) {
                _readPos += 1;
            }
            return ret;

        }

        public byte[] ReadBytes(int leng, bool peek = true) {
            if (_buffUpdate) {
                _readBuff = _buffer.ToArray();
                _buffUpdate = false;
            }

            byte[] ret = _buffer.GetRange(_readPos, leng).ToArray();
            if (peek) {
                _readPos += leng;
            }
            return ret;
        }

        public float ReadFloat(bool peek = true) {
            if (_buffer.Count <= _readPos)
                throw new Exception("Byte Buffer is past limit!");

            if (_buffUpdate) {
                _readBuff = _buffer.ToArray();
                _buffUpdate = false;
            }

            float ret = BitConverter.ToSingle(_readBuff, _readPos);
            if (peek & _buffer.Count > _readPos) {
                _readPos += 4;
            }
            return ret;

        }

        #endregion

        private bool _disposedValue;
        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing)
                    _buffer.Clear();

                _readPos = 0;
            }
            _disposedValue = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
