using System.Collections.Generic;
using GameServer.Game_Objects;
using GameServer.Network;
using static GameServer.Enums;

namespace GameServer {
    internal static class AvailablePlaysVerifier {

        public static void CheckHandPlays(int playerOnTurn, Player player, List<Card> cards) {
            foreach (Card card in cards) {
                if (card.Mana > player.ManaRemaining) {
                    ServerSendData.SendSetCanPlayCard(playerOnTurn, card.ServerId, 0);
                    continue;
                }

                if (card.CheckRequirement(PlayRequirement.MINIONS_ON_BOARD)) {
                    if (GameEngine.CardsOnBoard[playerOnTurn].Count == 0) {
                        ServerSendData.SendSetCanPlayCard(playerOnTurn, card.ServerId, 0);
                        continue;
                    }
                }

                if (card.CheckRequirement(PlayRequirement.ENEMIES_ON_BOARD)) {
                    if (GameEngine.CardsOnBoard[playerOnTurn == 0 ? 1 : 0].Count == 0) {
                        ServerSendData.SendSetCanPlayCard(playerOnTurn, card.ServerId, 0);
                        continue;
                    }
                }

                ServerSendData.SendSetCanPlayCard(playerOnTurn, card.ServerId, 1);
            }
        }

        public static void CheckBoardPlays(int playerOnTurn, Player player, List<Card> cards) {
            foreach (Card card in cards) {
                ServerSendData.SendSetCanAttack(playerOnTurn, card.ServerId, card.CanAttack() ? 1 : 0);
            }
        }
    }
}
