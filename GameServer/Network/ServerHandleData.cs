using GameServer.Utils;
using System;
using System.Collections.Generic;
using static GameServer.Enums;

namespace GameServer.Network {
    internal static class ServerHandleData {
        private delegate void Packet(long index, byte[] data);
        private static readonly Dictionary<long, Packet> Packets = new Dictionary<long, Packet>();
        private static long _pLenght;

        public static void InitMessages() {
            Console.WriteLine("Initializing Network Messages...");
            Packets.Add((int)ClientPackets.CNewAccount, Packet_CNewAccount);
            Packets.Add((int)ClientPackets.CLogin, Packet_CLogin);
            Packets.Add((int)ClientPackets.PlayCard, Packet_CardPlayed);
            Packets.Add((int)ClientPackets.EndTurn, Packet_TurnEnded);
            Packets.Add((int)ClientPackets.PlayerReady, Packet_PlayerRead);
            Packets.Add((int)ClientPackets.DrawCards, Packet_DrawCard);
            
        }

        public static void HandleData(long index, byte[] data) {
            var buffer = (byte[])data.Clone();
            if (Types.TempPlayer[index].Buffer == null)
                Types.TempPlayer[index].Buffer = new ByteBuffer();
            Types.TempPlayer[index].Buffer.WriteBytes(buffer);

            if (Types.TempPlayer[index].Buffer.Count() == 0) {
                Types.TempPlayer[index].Buffer.Clear();
                return;
            }

            if (Types.TempPlayer[index].Buffer.Lenght() >= 4) {
                _pLenght = Types.TempPlayer[index].Buffer.ReadLong(false);
                if (_pLenght <= 0) {
                    Types.TempPlayer[index].Buffer.Clear();
                    return;
                }
            }

            while (_pLenght > 0 && _pLenght <= Types.TempPlayer[index].Buffer.Lenght() - 8) {

                if (_pLenght <= Types.TempPlayer[index].Buffer.Lenght() - 8) {
                    Types.TempPlayer[index].Buffer.ReadLong();
                    data = Types.TempPlayer[index].Buffer.ReadBytes((int)_pLenght);
                    HandleDataPackets(index, data);
                }

                _pLenght = 0;

                if (Types.TempPlayer[index].Buffer.Lenght() < 4) continue;

                _pLenght = Types.TempPlayer[index].Buffer.ReadLong(false);

                if (_pLenght >= 0) continue;

                Types.TempPlayer[index].Buffer.Clear();

                return;
            }

            if (_pLenght <= 1) {
                Types.TempPlayer[index].Buffer.Clear();
            }

        }

        private static void HandleDataPackets(long index, byte[] data) {

            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            long packetNum = buffer.ReadLong();

            if (packetNum == 0) return;
            if (Packets.TryGetValue(packetNum, out Packet packet)) {
                packet.Invoke(index, data);
            }
        }

        private static void Packet_CNewAccount(long index, byte[] data) {
            Console.WriteLine("Received request CNewAccount from: " + index);

            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            buffer.ReadLong();
            string username = buffer.ReadString();
            string password = buffer.ReadString();

            try {
                AccountManager.CreateAccount(username, password);
                ServerSendData.SendServerMessage(index, "Account created!");
                Console.WriteLine("Account created.");
            } catch (AccountExistsException) {
                ServerSendData.SendServerMessage(index, "Account already exists!");
                Console.WriteLine("Account already exists.");
            }

        }

        private static void Packet_CLogin(long index, byte[] data) {
            Console.WriteLine("Received request CLogin from: " + index);

            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            buffer.ReadLong();
            string username = buffer.ReadString();
            string password = buffer.ReadString();

            try {
                AccountManager.Login(index, username, password);
                ServerSendData.SendLoginResponse(index, LoginResponse.OK);
                Console.WriteLine("Player logged.");
            } catch (AccountDoesNotExist) {
                ServerSendData.SendLoginResponse(index, LoginResponse.DOES_NOT_EXISTS);
                Console.WriteLine("Account does not exists.");
            }  catch (IncorrectPasswordException) {
                ServerSendData.SendLoginResponse(index, LoginResponse.INCORRECT_PASSWORD);
                Console.WriteLine("Incorrect password.");
            }
        }

        private static void Packet_CardPlayed(long index, byte[] data) {
            Console.WriteLine("Received CardPlayed from: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            buffer.ReadLong();

            int serverId = (int)buffer.ReadLong();

            int boardIndex = (int)buffer.ReadLong();
            Console.WriteLine("Server Id: " + serverId + " BoardIndex: " + boardIndex);

            GameEngine.CardPlayed(index, serverId, boardIndex);
        }

        private static void Packet_TurnEnded(long index, byte[] data) {
            Console.WriteLine("Received TurnEnded from: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            buffer.ReadLong();

            GameEngine.TurnEnded(index);
        }

        private static void Packet_PlayerRead(long index, byte[] data) {
            Console.WriteLine("Received PlayerReady from: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            buffer.ReadLong();

            Lobby.PlayerReady(index);
        }

        private static void Packet_DrawCard(long index, byte[] data) {
            Console.WriteLine("Received DrawCard from: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            buffer.ReadLong();

            long qtd = buffer.ReadLong();

            GameEngine.DrawCardTo(index, qtd);            
        }
    }
}
