using System.Collections.Generic;
using GameServer.Game_Objects;
using GameServer.Network;
using static GameServer.Enums;

namespace GameServer {
    internal static class AvailablePlaysVerifier {

        public static void CheckHandPlays(int playerIndex, Player player, List<Card> cards) {
            foreach (Card card in cards) {
                if (card.Mana > player.ManaRemaining) {
                    ServerSendData.SendSetCanPlayCard(playerIndex, card.ServerId, 0);
                    continue;
                }

                if (card.CheckRequirement(PlayRequirement.MINIONS_ON_BOARD)) {
                    if (GameEngine.CardsOnBoard[playerIndex].Count == 0) {
                        ServerSendData.SendSetCanPlayCard(playerIndex, card.ServerId, 0);
                        continue;
                    }
                }

                if (card.CheckRequirement(PlayRequirement.ENEMIES_ON_BOARD)) {
                    if (GameEngine.CardsOnBoard[playerIndex == 0 ? 1 : 0].Count == 0) {
                        ServerSendData.SendSetCanPlayCard(playerIndex, card.ServerId, 0);
                        continue;
                    }
                }

                ServerSendData.SendSetCanPlayCard(playerIndex, card.ServerId, 1);
            }
        }

        public static void CheckBoardPlays(int playerIndex, List<Card> cards) {
            foreach (Card card in cards) {
                ServerSendData.SendSetCanAttack(playerIndex, card.ServerId, card.CanAttack() ? 1 : 0);
            }
        }

        public static void CheckAttackTargets(int playerIndex, List<Card> cards) {
            // Check for taunts
            bool hasTaunt = false;
            foreach (Card card in cards) {
                if (!card.IsTaunt) continue;

                hasTaunt = true;
                break;
            }

            if (hasTaunt) {
                foreach (Card card in cards) {
                    ServerSendData.SendSetCanTarget(playerIndex, card.ServerId, card.IsTaunt ? 1 : 0);
                }
            }
            else {
                foreach (Card card in cards) {
                    ServerSendData.SendSetCanTarget(playerIndex, card.ServerId, 1);
                }
            }
        }
    }
}
