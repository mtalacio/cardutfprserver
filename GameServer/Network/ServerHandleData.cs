using GameServer.Utils;
using System;
using System.Collections.Generic;
using static GameServer.Enums;

namespace GameServer.Network {
    class ServerHandleData {
        private delegate void Packet_(long index, byte[] data);
        private static Dictionary<long, Packet_> packets = new Dictionary<long, Packet_>();
        private static long pLenght;

        public static void InitMessages() {
            Console.WriteLine("Initializing Network Messages...");
            packets.Add((int)ClientPackets.CNewAccount, Packet_CNewAccount);
            packets.Add((int)ClientPackets.CLogin, Packet_CLogin);
            packets.Add((int)ClientPackets.PlayCard, Packet_CardPlayed);
        }

        public static void HandleData(long index, byte[] data) {
            byte[] buffer;
            buffer = (byte[])data.Clone();
            if (Types.tempPlayer[index].buffer == null)
                Types.tempPlayer[index].buffer = new ByteBuffer();
            Types.tempPlayer[index].buffer.WriteBytes(buffer);

            if (Types.tempPlayer[index].buffer.Count() == 0) {
                Types.tempPlayer[index].buffer.Clear();
                return;
            }

            if (Types.tempPlayer[index].buffer.Lenght() >= 4) {
                pLenght = Types.tempPlayer[index].buffer.ReadLong(false);
                if (pLenght <= 0) {
                    Types.tempPlayer[index].buffer.Clear();
                    return;
                }
            }

            while (pLenght > 0 && pLenght <= Types.tempPlayer[index].buffer.Lenght() - 8) {

                if (pLenght <= Types.tempPlayer[index].buffer.Lenght() - 8) {
                    Types.tempPlayer[index].buffer.ReadLong();
                    data = Types.tempPlayer[index].buffer.ReadBytes((int)pLenght);
                    HandleDataPackets(index, data);
                }

                pLenght = 0;

                if (Types.tempPlayer[index].buffer.Lenght() >= 4) {
                    pLenght = Types.tempPlayer[index].buffer.ReadLong(false);
                    if (pLenght < 0) {
                        Types.tempPlayer[index].buffer.Clear();
                        return;
                    }
                }
            }

            if (pLenght <= 1) {
                Types.tempPlayer[index].buffer.Clear();
            }

        }

        public static void HandleDataPackets(long index, byte[] data) {
            long packetNum;
            ByteBuffer buffer;

            buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            packetNum = buffer.ReadLong();
            buffer = null;

            if (packetNum == 0) return;
            if (packets.TryGetValue(packetNum, out Packet_ packet)) {
                packet.Invoke(index, data);
            }
        }

        private static void Packet_CNewAccount(long index, byte[] data) {
            Console.WriteLine("Received request CNewAccount from: " + index);

            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            long packetNum = buffer.ReadLong();
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
            long packetNum = buffer.ReadLong();
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
            Console.WriteLine("Received: CardPlayed from:" + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            long packetNum = buffer.ReadLong();

            int cardId = (int)buffer.ReadLong();
            int boardIndex = (int)buffer.ReadLong();
            Console.WriteLine("Card Id: " + cardId + " BoardIndex: " + boardIndex);

            GameEngine.CardPlayed(cardId, boardIndex);

            buffer = null;
        }
    }
}
