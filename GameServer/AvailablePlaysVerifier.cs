using System;
using System.Collections.Generic;
using GameServer.Game_Objects;
using GameServer.Network;

namespace GameServer {
    internal static class AvailablePlaysVerifier {

        public static void CheckHandPlays(int playerOnTurn, Player player, List<Card> cards) {
            foreach (Card card in cards) {

                if (card.Mana > player.ManaRemaining) {
                    ServerSendData.SendSetCanPlayCard(playerOnTurn, card.ServerId, 0);
                    continue;
                }

                ServerSendData.SendSetCanPlayCard(playerOnTurn, card.ServerId, card.CanPlay() ? 1 : 0);
            }
        }

        public static void CheckBoardPlays(List<Card> cards) {

        }

    }
}
