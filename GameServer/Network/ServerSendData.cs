﻿using GameServer.Utils;
using System;

using static GameServer.Enums;

namespace GameServer.Network {
    public static class ServerSendData {

        public static void SendServerMessage(long index, string line) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SMessage);
            buffer.WriteString(line);
            NetworkSocket.SendDataTo(index, buffer.ToArray());
            buffer = null;
        }

        public static void SendLoginResponse(long index, LoginResponse response) {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.SLoginResponse);
            buffer.WriteInteger((int)response);
            NetworkSocket.SendDataTo(index, buffer.ToArray());
            buffer = null;
        }

        public static void SendCreateCard(long index, long cardId, CardPlace place, long boardIndex) {

            Console.WriteLine("Sending CreateCard to: " + index);
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteLong((long)ServerPackets.CreateCard);

            buffer.WriteLong(cardId);
            buffer.WriteLong((long)place);
            buffer.WriteLong(boardIndex);

            NetworkSocket.SendDataTo(index, buffer.ToArray());

            buffer = null;
        }
    }
}