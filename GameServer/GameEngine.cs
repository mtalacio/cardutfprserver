using GameServer.Game_Objects;
using GameServer.Network;
using GameServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static GameServer.Enums;

namespace GameServer {
    public static class GameEngine {

        private static List<Player> playerList = new List<Player>();
        public static void AddPlayer(Player player) {
            if(playerList.Count > Constants.MAX_PLAYERS) {
                Console.WriteLine("Attempt to add a player beyond MAX_PLAYERS.");
                return;
            }
  
            playerList.Add(player);
        }

        private static List<Card> cardsOnBoard1 = new List<Card>();
        private static List<Card> cardsOnBoard2 = new List<Card>();

        private static List<Card> cardsOnHand1 = new List<Card>();
        private static List<Card> cardsOnHand2 = new List<Card>();

        private static List<Card> cardsOnDeck1;
        private static List<Card> cardsOnDeck2;

        #region Events

        public static void CardPlayed(long playerIndex, int cardId, int boardIndex) {

            if ((int)playerIndex != playerOnTurn)
                throw new IllegalMessageReceivedException("Carta Jogada por jogador não permitido.");

            if (playerOnTurn == 0) { 
                ServerSendData.SendCreateCard(1, cardId, CardPlace.ENEMY_BOARD, boardIndex);
            } else {
                ServerSendData.SendCreateCard(0, cardId, CardPlace.ENEMY_BOARD, boardIndex);
            }
        }

        public static void TurnEnded(long index) {

            if(index != playerOnTurn) {
                throw new IllegalMessageReceivedException("EndTurn recebido por jogador não permitido.");
            }

            if (playerOnTurn == 0) {
                playerOnTurn = 1;
                ServerSendData.SendSetTurn(0, 0);
                ServerSendData.SendSetTurn(1, 1);
            }
            else {
                playerOnTurn = 0;
                ServerSendData.SendSetTurn(0, 1);
                ServerSendData.SendSetTurn(1, 0);
            }
        }

        public static void DrawCardTo(long index, long qtd) {
            //TODO: Decide if cards are server identified or not
            if (index == 0) {
                if (cardsOnDeck1.Count == 0)
                    return;
                for (int i = 0; i < qtd; i++) {
                    Card toDrawn = cardsOnDeck1[0];
                    ServerSendData.SendCreateCard(0, toDrawn.CardId, CardPlace.HAND, -1);
                    cardsOnDeck1.Remove(toDrawn);
                    cardsOnHand1.Add(toDrawn);
                }
            } else {
                if (cardsOnDeck2.Count == 0)
                    return;
                for (int i = 0; i < qtd; i++) {
                    Card toDrawn = cardsOnDeck2[0];
                    ServerSendData.SendCreateCard(1, toDrawn.CardId, CardPlace.HAND, -1);
                    cardsOnDeck2.Remove(toDrawn);
                    cardsOnHand2.Add(toDrawn);
                }
            }
        }

        #endregion

        #region Game State

        private static int playerOnTurn = 0;    

        public static void InitializeGame() {
            playerOnTurn = 0;
            ServerSendData.SendSetTurn(0, 1);
            ServerSendData.SendSetTurn(1, 0);

            //NOTE: Temp code for testing purposes
            cardsOnDeck1 = new List<Card> {
                    new Card(0, 0),
                    new Card(1, 0),
                    new Card(0, 0),
                    new Card(1, 0),
                    new Card(0, 0),
                    new Card(1, 0)
            };

            cardsOnDeck2 = new List<Card> {
                    new Card(0, 1),
                    new Card(1, 1),
                    new Card(0, 1),
                    new Card(1, 1),
                    new Card(0, 1),
                    new Card(1, 1)
            };

        }

        #endregion
    }
}
