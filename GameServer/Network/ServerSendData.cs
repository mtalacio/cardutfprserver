using GameServer.Utils;
using System;

using static GameServer.Enums;

namespace GameServer.Network {
    public static class ServerSendData {

        public static void SendServerMessage(long index, string line) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SMessage);
            buffer.WriteString(line);
            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendLoginResponse(long index, LoginResponse response) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SLoginResponse);
            buffer.WriteInteger((int)response);
            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendCreateCard(long index, long cardId, long serverId, CardPlace place, long boardIndex) {
            Console.WriteLine("Sending CreateCard to: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.CreateCard);

            buffer.WriteLong(cardId);
            buffer.WriteLong(serverId);
            buffer.WriteLong((long)place);
            buffer.WriteLong(boardIndex);

            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendSetTurn(long index, long your) {
            Console.WriteLine("Sending SendSetTurn" + your + " to: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SetTurn);

            buffer.WriteLong(your);

            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendSetTotalMana(long index, long mana) {
            Console.WriteLine("Sending SendSetTotalMana value = " + mana + " to: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SetTotalMana);

            buffer.WriteLong(mana);

            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendSetAvailableMana(long index, long mana) {
            Console.WriteLine("Sending SetAvailableMana value = " + mana + " to: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SetAvailableMana);

            buffer.WriteLong(mana);

            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendSetCanPlayCard(long index, long sId, long can) {
            Console.WriteLine("Sending SetCanPlay Card SID = " + sId + " value = " + can + " to: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SetCanPlayCard);

            buffer.WriteLong(sId);
            buffer.WriteLong(can);

            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendSetCanAttack(long index, long sId, long can) {
            Console.WriteLine("Sending SetCanAttack Card SID = " + sId + " value = " + can + " to: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SetCanAttack);

            buffer.WriteLong(sId);
            buffer.WriteLong(can);

            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

    }
}
