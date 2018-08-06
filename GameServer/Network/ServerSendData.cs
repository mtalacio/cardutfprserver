﻿using GameServer.Utils;
using System;

using static GameServer.Enums;

namespace GameServer.Network {

    //TODO: Change the ByteBuffer to using statements

    public static class ServerSendData {

        public static void SendServerMessage(long index, string line) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.MESSAGE);
            buffer.WriteString(line);
            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendLoginResponse(long index, LoginResponse response) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.LOGIN_RESPONSE);
            buffer.WriteInteger((int)response);
            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendCreateCard(long index, long cardId, long serverId, CardPlace place, long boardIndex) {
            Console.WriteLine("Sending CreateCard to: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.CREATE_CARD);

            buffer.WriteLong(cardId);
            buffer.WriteLong(serverId);
            buffer.WriteLong((long)place);
            buffer.WriteLong(boardIndex);

            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendSetTurn(long index, long your) {
            Console.WriteLine("Sending SendSetTurn value = " + your + " to: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SET_TURN);

            buffer.WriteLong(your);

            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendSetTotalMana(long index, long mana) {
            Console.WriteLine("Sending SendSetTotalMana value = " + mana + " to: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SET_TOTAL_MANA);

            buffer.WriteLong(mana);

            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendSetAvailableMana(long index, long mana) {
            Console.WriteLine("Sending SetAvailableMana value = " + mana + " to: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SET_AVAILABLE_MANA);

            buffer.WriteLong(mana);

            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendSetCanPlayCard(long index, long sId, long can) {
            Console.WriteLine("Sending SetCanPlay Card SID = " + sId + " value = " + can + " to: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SET_CAN_PLAY_CARD);

            buffer.WriteLong(sId);
            buffer.WriteLong(can);

            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendSetCanAttack(long index, long sId, long can) {
            Console.WriteLine("Sending SetCanAttack Card SID = " + sId + " value = " + can + " to: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SET_CAN_ATTACK);

            buffer.WriteLong(sId);
            buffer.WriteLong(can);

            NetworkSocket.SendDataTo(index, buffer.ToArray());
        }

        public static void SendStartSelectTarget(long index) {
            Console.WriteLine("Sending StartSelectCard to: " + index);

            using (ByteBuffer buffer = new ByteBuffer()) {
                buffer.WriteLong((long) ServerPackets.START_SELECT_TARGET);

                NetworkSocket.SendDataTo(index, buffer.ToArray());
            }
        }

        public static void SendSetCanTarget(long index, long sId, long can) {
            Console.WriteLine("Sending SetCanTarget Card SID = " + sId + " value = " + can + " to: " + index);

            using (ByteBuffer buffer = new ByteBuffer()) {
                buffer.WriteLong((long)ServerPackets.SET_CAN_BE_TARGET);

                buffer.WriteLong(sId);
                buffer.WriteLong(can);

                NetworkSocket.SendDataTo(index, buffer.ToArray());
            }
        }

        public static void SendEndSelectTarget(long index, long response) {
            Console.WriteLine("Sending EndSelectCard value = " + response + " to: " + index);

            using (ByteBuffer buffer = new ByteBuffer()) {
                buffer.WriteLong((long)ServerPackets.END_SELECT_TARGET);

                buffer.WriteLong(response);

                NetworkSocket.SendDataTo(index, buffer.ToArray());
            }
        }

    }
}
